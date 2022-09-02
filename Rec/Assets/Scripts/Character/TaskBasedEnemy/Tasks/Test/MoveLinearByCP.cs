using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.Enemy.TaskBased
{
    // AddComponentからキャラクターにタスクをアタッチするためのコード   
    [AddComponentMenu("EnemyTask/MoveLinearByCP")]
    // AIデザインのためにコンポーネントとしてアタッチされるクラス
    public class MoveLinearByCP: MoveByControlPoints
    {
        private List<GameObject> handledCp;
        
        protected override Func<IEnumerable<Vector3>, float, Vector3> GenerateTrajectory()
        {
            Vector3 Fn(IEnumerable<Vector3> cps, float t)
            {
                var cpsArray = cps.ToArray();
                var dists = new float[cpsArray.Length];
                
                // 各制御点の累積距離を測る。 dists[n] -> 0番目からn番目までの制御点の経路長。
                dists[0] = 0.0f;
                for (int i = 1; i < dists.Length; i++)
                {
                    dists[i] = Vector3.Magnitude(cpsArray[i] - cpsArray[i-1]) + dists[i-1];
                }

                // [0,1]にスケーリングし、tがどの制御点間にあるか求める。idx=n -> n-1番目とn番目の制御点の間
                var sumDists = dists[dists.Length - 1];
                int idx = dists.Length - 1;
                for (int i = dists.Length - 1; i > 0; i--)
                {
                    dists[i] /= sumDists;
                    if (t < dists[i])
                        idx = i;
                }

                // tが属する区間(idx)の端点となる制御点をtで内分した点をもとめて返す
                var m = t - dists[idx-1];
                var n = dists[idx] - t;

                // Debug.Log(idx);
                // Debug.Log("t " + t);
                // Debug.Log("cps[0]" + cpsArray[0]);
                // Debug.Log("cps[1]" + cpsArray[1]);
                // Debug.Log("0 " + dists[0]);
                // Debug.Log("1 " + dists[1]);
                // Debug.Log("2 " + dists[2]);
                // Debug.Log("3 " + dists[3]);
                // Debug.Log("m" + m);
                // Debug.Log("n" + n);
                return (cpsArray[idx] * m + cpsArray[idx-1] * n) / (m + n);
            }

            return Fn;
        }

        /*
         * やりたいこと
         * - 制御点を自動生成
         *   - Resetメソッドで生成
         * - (できたら)コンポーネント削除時にコントロールポイント削除
         *   - OnDestroyでできるかと思ったけどダメだった
         * - 制御点をGizmoで可視化
         *   - OnDrawGizmosでできた
         * - 移動経路をGizmoで可視化
         *   - 曲線の表示方法は派生クラスに依存
         *   - 各派生クラスで表示を行う処理を記述する必要がある。
         * 
         * 改変策
         * - 基底クラスのパラメータ：制御点群→ターゲット座標
         *   - 中間点・制御点は派生クラスで定義
         * - 始点指定のパラメータ追加
         *   - 設定された時に軌跡の表示を行う。
         */
        public void Reset()
        {
            if (handledCp != null) return;

            var o = gameObject;
            var defaultCp = new GameObject("cp (Linear: target)")
            {
                transform =
                {
                    position = o.transform.position,
                    parent = o.transform
                }
            };
            defaultCp.AddComponent<ControlPointForMoveTasks>();
            
            if (this.controlPoints == null)
            {
                this.controlPoints = new List<Transform>();
            }
            this.controlPoints.Add(defaultCp.transform);
            
            handledCp = new List<GameObject>()
            {
                defaultCp
            };
        }

    }
}