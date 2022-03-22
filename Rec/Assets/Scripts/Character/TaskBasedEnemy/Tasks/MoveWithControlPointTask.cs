using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Enemy.TaskBased
{
    // AddComponentからキャラクターにタスクをアタッチするためのコード   
    [AddComponentMenu("EnemyTask/MoveWithControlPointTask")]
    // AIデザインのためにコンポーネントとしてアタッチされるクラス
    public class MoveWithControlPointTask : EnemyTaskComponent
    {
        [SerializeField, Label("制御点の数")] private int controlPointCount = 5;
        [SerializeField, Label("中心点を制御点に含める")] private bool hasCenter = false;
        [Space(20)]
        [SerializeField, Label("移動速度")] private float moveSpeed = 10.0f;
        [SerializeField, Label("移動距離")] private float moveDistance = 2.0f;
        [SerializeField, Label("移動に所要する時間")] private float moveFrequency = 1.0f;
        [SerializeField, Label("移動回数")] private int moveCount = 4;
        
        // Taskクラスを生成して返す
        public override IEnemyTask ToEnemyTask()
        {
            var playerTr = GameObject.FindGameObjectWithTag("Player").transform;
            return new Task(controlPointCount, hasCenter, moveSpeed, moveDistance, moveFrequency, moveCount, playerTr);
        }

        // キャラクターに渡され実行されるタスクのクラス
        private class Task : IEnemyTask
        {
            private readonly int _controlPointCount;
            private readonly bool _hasCenter;
            private readonly float _moveSpeed;
            private readonly float _moveDistance;
            private readonly float _moveFrequency;
            private readonly int _moveCount;
            private readonly Transform _playerTransform;
            
            // コンストラクタ 引数は必要に応じて追加してください
            public Task(int controlPointCount, bool hasCenter, float moveSpeed, float moveDistance, float moveFrequency, int moveCount, Transform playerTr)
            {
                this._controlPointCount = controlPointCount;
                this._hasCenter = hasCenter;
                this._moveSpeed = moveSpeed;
                this._moveDistance = moveDistance;
                this._moveFrequency = moveFrequency;
                this._moveCount = moveCount;
                this._playerTransform = playerTr;
            }

            // ゲーム中に呼び出されるタスク実行のメソッド
            // 引数enemyは行動主体
            public IEnumerator Call(TaskBasedEnemy enemy)
            {
                var enemyPos = enemy.transform.position;
                var direction = (_playerTransform.position - enemyPos).normalized;
                var controlPoints = GetControlPoint(enemyPos, direction, _controlPointCount, _hasCenter, _moveDistance);
                
                throw new System.NotImplementedException();
            }

            // タスクの複製を行うメソッド
            // 意図したものを除いて 複製元と複製先が同じ参照を持たないように注意してください
            public IEnemyTask Copy()
            {
                return new Task(_controlPointCount, _hasCenter, _moveSpeed, _moveDistance, _moveFrequency, _moveCount, _playerTransform);
            }

            /// <summary>
            /// 制御点を取得する
            /// </summary>
            /// <param name="origin">原点</param>
            /// <param name="direction">プレイヤーの方向</param>
            /// <param name="controlPointCount">制御点の数</param>
            /// <param name="hasCenter">中心点を制御点として数えるか</param>
            /// <param name="moveDistance">移動距離</param>
            /// <returns></returns>
            private Vector3[] GetControlPoint(Vector3 origin, Vector3 direction, int controlPointCount, bool hasCenter, float moveDistance)
            {
                // ベースとなる配列の作成
                var originPoints = new Vector3[controlPointCount];
                if (hasCenter)
                {
                    // 中心点を使用する場合は制御点を１つ減らす
                    originPoints[controlPointCount - 1] = Vector3.zero;
                    controlPointCount--;
                }
                originPoints[0] = Vector3.right * moveDistance;
                for (var i = 1; i < controlPointCount; i++)
                {
                    var rad = 2 * Mathf.PI * i / controlPointCount;
                    var pos = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0);
                    originPoints[i] = pos * moveDistance;
                }

                // 配列のプレイヤー方向への変換
                return originPoints
                    .Select(point => point + origin)
                    .Select(point => Vector3.ProjectOnPlane(point, direction))
                    .ToArray();
            }
        }
    }
}
