using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;


namespace Core.Enemy.TaskBased
{
    /*
     * 考慮すべきポイント
     * - キャラクターの移動開始点cp0は制御点に含まれない -> 経路関数の事前生成が困難
     * - 上記を踏まえたうえで、ComponentからTaskに何を渡せばよいか
     *   - 制御点座標 C(={cp1, cp2, ..., cpN})
     *   - 経路生成関数 f_trajectory: {cp0} + C -> ([0, 1] -> (x, y, z))
     *     - 曲線の種類・パラメータのみに依る
     *     - 関数自体はstaticで定義(本質的にはインスタンス変数の情報を使わないはず)
     *   - 速度変換関数 f_velocity: [0, 1] -> [0, 1]
     *
     * このクラスの必要性
     * - 制御点移動と連続移動、曲線移動の需要をごっちゃにして考えていないか
     * - 本当に作るべきものは何か　そもそもそれぞれが必要な場面は何なのか
     * - 必要なケース
     *   - 制御点移動
     *     - 移動先を視覚的に明確にしたい（数値指定移動だと、直感的にどこまで移動するかわからない）
     *   - 連続移動
     *     - 長時間にわたって画面にいるキャラクタの移動の管理を楽に
     *     - 一つの速度曲線を複数の移動に対して割り当てられる（複数の移動がある場合もシームレスに）
     *   - 曲線移動
     *     - 移動方法にバラエティを持たす
     *
     * - 改築案
     *   - 制御点を用いる移動アーキテクチャを基底クラスにして、連続移動・曲線移動を派生クラスとして実装する
     *
     * - 基本方針
     *   - これまで作ってきたもの、これから作るものと必ず併用できるように
     *   - 具体的なユースケースを用意しておく
     *
     * - 現在の需要
     *   - プレイヤーと一緒にステージを移動するボス
     *     - 制御点移動 + 連続移動 + 曲線移動
     *     - 全部必要やんけ
     *     - これまでは、全てを一つのタスクでやる設定だった。
     *     - これを別のユースケースも考慮したうえで上手く分割することはできないか
     *
     * 結論:これは抽象クラスにする
     * 制御点を用いた曲線・直線移動を後に実装する
     * エディタ拡張は後で考える
     *   Component操作時の制御点自動生成・消去
     * 移動方法は内分点移動ではなく、差分による相対移動へ
     * 相対移動による累積誤差は、修正するパラメータを追加するか検討中
     */
    
    // [AddComponentMenu("EnemyTask/MoveByControlPoints")]
    public abstract class MoveByControlPoints : EnemyTaskComponent
    {
        // 基底クラスでは制御点はシリアライズしない
        [SerializeField] protected float durationTime = 2;
        // [SerializeField] protected List<Transform> controlPoints;
        // [SerializeField] public Transform target;
        [SerializeField, Label("start (optional)")] public Transform start;
        // [SerializeField] protected TrajectoryCurve trajectoryCurve;
        [SerializeField] protected VelocityCurve velocityCurve;
        [SerializeField] protected float errorCorrectCoefficient = 0;
        
        // protected enum TrajectoryCurve
        // {
        //     // 線形, エルミート, 2次ベジエ, 3次ベジエ, スプライン, Bスプライン
        //     Linear, Hermite, QuadraticBezier, CubicBezier, Spline, BSpline
        // }

        protected enum VelocityCurve
        {
            Linear, InCubic, OutCubic, InOutCubic
        }
        
        // 曲線の種類により、始点終点を含めた制御点（座標列）の与え方が変わる？
        protected abstract Func<IEnumerable<Vector3>, float, Vector3> GenerateTrajectory();

        // // ↑わざわざ関数を返される必要ある? f(instance, t) → (x, y, z) というメソッドでいいような
        // // いや、よくない。TaskクラスにMonoBehaviour持たせるわけにはいかん。
        // // cpを基底クラスを通さずにやるならTaskComponentの派生クラス内でTaskクラスも継承しないといけないけど、めんどいし今よりも汚い構造になりそう。
        // protected abstract Vector3 Trajectory(float t);

        // 定義域[0, 1]に対して値域[0, 1]のベロシティカーブf_velocityを作る
        private Func<float, float> GenerateVelocityCurve(VelocityCurve c)
        {
            Func<float, float> fn;

            switch (c)
            {
                case VelocityCurve.Linear:
                    fn = (float x) => x;
                    break;
                case VelocityCurve.InCubic:
                    fn = (float x) => Mathf.Pow(x, 3);
                    break;
                case VelocityCurve.OutCubic:
                    fn = (float x) => 1 - Mathf.Pow(1 - x, 3);
                    break;
                case VelocityCurve.InOutCubic:
                    fn = (float x) => x < 0.5 
                        ? 4 * Mathf.Pow(x, 3) 
                        : 1 - Mathf.Pow(-2 * x + 2, 3) / 2;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return fn;
        }
        
        // 制御点列を得る
        protected abstract IEnumerable<Vector3> ControlPoints { get; }

        // 目的地点を得る
        public abstract Transform TargetPoint { get; }

        public override IEnemyTask ToEnemyTask()
        {
            var controlPointsPos = this.ControlPoints;
            var velocityFunc = GenerateVelocityCurve(velocityCurve);
            var trajectory =  GenerateTrajectory();
            
            return new Task(this.durationTime, controlPointsPos, velocityFunc, trajectory, errorCorrectCoefficient);
        }

        private class Task : IEnemyTask
        {
            private List<Vector3> controlPoints;
            private float durationTime = 2;
            private Func<float, float> velocityFunc;
            private Func<IEnumerable<Vector3>, float, Vector3> trajectory;
            private float errorCorrectCoefficient;
            
            public Task(float durationTime,
                IEnumerable<Vector3> controlPoints,
                Func<float, float> velocityFunc,
                Func<IEnumerable<Vector3>, float, Vector3> trajectory,
                float errorCorrectCoefficient)
            {
                this.controlPoints = controlPoints.ToList();
                this.durationTime = durationTime;
                this.velocityFunc = velocityFunc;
                this.trajectory = trajectory;
                this.errorCorrectCoefficient = errorCorrectCoefficient;
            }

            // 正確な計算上の位置と、実際の差分計算で得られた位置とを補間する
            private Vector3 CorrectError(Vector3 x, Vector3 y)
            {
                return (1 - this.errorCorrectCoefficient) * x + this.errorCorrectCoefficient * y;
            }
            
            public IEnumerator Call(TaskBasedEnemy enemy)
            {
                var transform = enemy.transform;
                var cps = new List<Vector3> { transform.position };
                cps.AddRange(controlPoints);

                foreach (var cp in cps)
                {
                    Debug.Log(cp);
                }
                
                float t = 0;
                float dt = UnityEngine.Time.deltaTime;
                Vector3 prevPos = trajectory(cps, t);
                while (t < durationTime)
                {
                    // 敵座標取り出し
                    var pos = transform.position;
                    
                    // t+dtの座標計算 -> tの座標との差分に変換してから加算
                    t += dt;
                    var nextPos = trajectory(cps, velocityFunc(t / durationTime));
                    var df = nextPos - prevPos;
                    pos += df;
                    dt = UnityEngine.Time.deltaTime;
                    
                    prevPos += df;

                    // 丸め誤差補正
                    pos = CorrectError(pos, nextPos);
                    
                    transform.position = pos;
                    yield return null;
                }
                
                yield break;
            }

            public IEnemyTask Copy()
            {
                return new Task(
                    this.durationTime,
                    this.controlPoints,
                    this.velocityFunc,
                    this.trajectory,
                    this.errorCorrectCoefficient);
            }
        }
    }
}
