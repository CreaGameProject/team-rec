using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Enemy.TaskBased
{
    // AddComponentからキャラクターにタスクをアタッチするためのコード   
    [AddComponentMenu("EnemyTask/LookPlayerTask")]
    // AIデザインのためにコンポーネントとしてアタッチされるクラス
    public class LookPlayerTask : EnemyTaskComponent
    {

        private Transform _playerTransform;

        // Taskクラスを生成して返す
        public override IEnemyTask ToEnemyTask()
        {
            _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            return new Task(_playerTransform);
        }

        // キャラクターに渡され実行されるタスクのクラス
        private class Task : IEnemyTask
        {

            private Transform _playerTransform;
            
            // コンストラクタ 引数は必要に応じて追加してください
            public Task(Transform playerTr)
            {
                this._playerTransform = playerTr;
            }

            // ゲーム中に呼び出されるタスク実行のメソッド
            // 引数enemyは行動主体
            public IEnumerator Call(TaskBasedEnemy enemy)
            {
                enemy.transform.forward = enemy.transform.position - _playerTransform.position;
                yield return null;
            }

            // タスクの複製を行うメソッド
            // 意図したものを除いて 複製元と複製先が同じ参照を持たないように注意してください
            public IEnemyTask Copy()
            {
                return new Task(_playerTransform);
            }
        }
    }
}
