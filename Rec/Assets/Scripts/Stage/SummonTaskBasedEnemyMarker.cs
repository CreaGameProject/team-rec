using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.Enemy.TaskBased;
using UnityEngine;

namespace Core.Stage
{
    /// <summary>
    /// インスペクタなどから得た情報を用いてSummonTaskBasedEnemyEventオブジェクトを生成する。
    /// </summary>
    public class SummonTaskBasedEnemyMarker : StageEventMarker
    {
        [SerializeField] private float time;
        [SerializeField] private GameObject original;
        [Header("if task holder is none, it refer to children.")] [SerializeField] private TaskHolder taskHolder;
    
        public override IStageEvent ToStageEvent()
        {
            var holder = taskHolder;
            if (holder == null)
            {
                holder = GetComponentInChildren<TaskHolder>();
            }

            var e = new SummonTaskBasedEnemyEvent(original, holder.CollectTasks(), transform.position, transform.rotation, time);
        
            return e;
        }
        class SummonTaskBasedEnemyEvent : IStageEvent
        {
            private GameObject original;
            private List<IEnemyTask> tasks;
            private Vector3 position;
            private Quaternion quaternion;
            private float time;

            public SummonTaskBasedEnemyEvent(GameObject enemyObject, IEnumerable<IEnemyTask> tasks, Vector3 position, Quaternion quaternion, float time)
            {
                this.original = enemyObject;
                this.tasks = tasks.ToList();
                this.position = position;
                this.quaternion = quaternion;
                this.time = time;
            }

            public void Call()
            {
                var clone = GameObject.Instantiate(this.original, this.position, this.quaternion);
                var component = clone.GetComponent<TaskBasedEnemy>();
                component.SetTasks(tasks);
            }

            public float Time
            {
                get
                {
                    return time;
                }
                set
                {
                    this.time = value;
                }
            }
        }
    }
}