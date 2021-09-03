using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLogTask : EnemyTaskComponent
{
    [SerializeField] private float time;
    [SerializeField] private string message;

    public override IEnemyTask ToEnemyTask()
    {
        return new TestLog(time, message);
    }


    // task implementation
    class TestLog : IEnemyTask
    {
        private float time;
        private string message;

        public TestLog(float time, string message)
        {
            this.message = message;
            this.time = time;
        }

        public float Time => time;
        public void Call(TaskBasedEnemy enemy)
        {
            Debug.Log("[time:" + Time + "|name:" + enemy.gameObject.name + "] "+message);
        }

        public IEnemyTask Copy()
        {
            return new TestLog(time, message);
        }
    }
}
