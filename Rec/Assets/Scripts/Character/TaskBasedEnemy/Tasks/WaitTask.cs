using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Enemy.TaskBased
{
    [AddComponentMenu("EnemyTask/Wait")]
    public class WaitTask : EnemyTaskComponent
    {
        [SerializeField, Label("秒数")] private float seconds;

        public override IEnemyTask ToEnemyTask()
        {
            return new Wait(seconds);
        }

        class Wait : IEnemyTask
        {
            private float seconds;

            public Wait(float seconds)
            {
                this.seconds = seconds;
            }

            public IEnumerator Call(TaskBasedEnemy enemy)
            {
                yield return new WaitForSeconds(seconds);
            }

            public IEnemyTask Copy()
            {
                return new Wait(seconds);
            }
        }
    }

}