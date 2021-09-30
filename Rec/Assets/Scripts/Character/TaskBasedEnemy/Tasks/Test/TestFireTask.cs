using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Enemy.TaskBased
{
    [AddComponentMenu("EnemyTask/TestFire")]
    public class TestFireTask : EnemyTaskComponent
    {
    public override IEnemyTask ToEnemyTask()
    {
        return new Task();
    }

    class Task : IEnemyTask
    {
        public Task()
        {

        }

        public IEnumerator Call(TaskBasedEnemy enemy)
        {
            throw new System.NotImplementedException();
        }

        public IEnemyTask Copy()
        {
            return new Task();
        }
    }
    }
}
