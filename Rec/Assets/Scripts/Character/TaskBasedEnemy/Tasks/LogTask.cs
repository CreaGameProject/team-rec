using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Enemy.TaskBased
{
    /// <summary>
    /// コンソールにログを出力するデバッグ用タスク
    /// </summary>
    [AddComponentMenu("EnemyTask/Log")]
    public class LogTask : EnemyTaskComponent
    {
        [SerializeField] private string message;

        public override IEnemyTask ToEnemyTask()
        {
            return new Log(message);
        }

        // task implementation
        class Log : IEnemyTask
        {
            private string message;

            public Log(string message)
            {
                this.message = message;
            }

            public IEnumerator Call(TaskBasedEnemy enemy)
            {
                Debug.Log("[name:" + enemy.gameObject.name + "] " + message);
                yield break;
            }

            public IEnemyTask Copy()
            {
                return new Log(message);
            }
        }
    }
}
