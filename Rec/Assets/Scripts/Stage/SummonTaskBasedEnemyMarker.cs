using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.Enemy.Navigator;
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
        [Header("if path generator is none, it refer to children.")] [SerializeField] private PathGenerator pathGenerator;
    
        public override IStageEvent ToStageEvent()
        {
            var tasks = (taskHolder != null ? taskHolder : GetComponentInChildren<TaskHolder>()).CollectTasks();
            var position = transform.position;
            var rotation = transform.rotation;
            var wayPoints = (pathGenerator != null ?  pathGenerator: GetComponentInChildren<PathGenerator>()).CollectWayPoints(transform.position);
        
            return new SummonTaskBasedEnemyEvent(original, tasks, wayPoints, position, rotation, time);
        }

        class SummonTaskBasedEnemyEvent : IStageEvent
        {
            private GameObject original;
            private List<IEnemyTask> tasks;
            private List<WayPoint> wayPoints;
            private Vector3 position;
            private Quaternion quaternion;
            private float time;

            public SummonTaskBasedEnemyEvent(GameObject enemyObject, IEnumerable<IEnemyTask> tasks, IEnumerable<WayPoint> wayPoints, Vector3 position, Quaternion quaternion, float time)
            {
                this.original = enemyObject;
                this.tasks = tasks.ToList();
                this.wayPoints = wayPoints.ToList();
                this.position = position;
                this.quaternion = quaternion;
                this.time = time;
            }

            public void Call()
            {
                var clone = GameObject.Instantiate(this.original, this.position, this.quaternion);
                
                var enemy = clone.GetComponent<TaskBasedEnemy>();
                enemy.SetTasks(tasks);

                var navigator = clone.GetComponent<EnemyNavigator>();
                navigator.SetPath(wayPoints);
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