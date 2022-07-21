using System.Collections.Generic;
using System.Linq;
using Core.Enemy.TaskBased;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Stage
{
    /// <summary>
    /// SummonTaskBasedEnemyMarkerの最終地点版。この敵を全部倒すとクリアに向かう。
    /// </summary>
    public class SummonTaskBasedBossMarker : StageEventMarker
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

            var e = new SummonTaskBasedBossEvent(original, holder.CollectTasks(), transform.position, transform.rotation, time);
        
            return e;
        }

        class SummonTaskBasedBossEvent : IStageEvent
        {
            private readonly GameObject _original;
            private readonly List<IEnemyTask> _tasks;
            private readonly Vector3 _position;
            private readonly Quaternion _quaternion;

            public SummonTaskBasedBossEvent(GameObject enemyObject, IEnumerable<IEnemyTask> tasks,
                Vector3 position, Quaternion quaternion, float time)
            {
                _original = enemyObject;
                _tasks = tasks.ToList();
                _position = position;
                _quaternion = quaternion;
                Time = time;
            }
            
            public void Call()
            {
                var clone = Instantiate(_original, _position, _quaternion);
                var component = clone.GetComponent<TaskBasedEnemy>();
                component.SetTasks(_tasks);
                BossEnemyCounter.AddEnemyCount(1);
                clone.AddComponent<DestroyObserver>().onDestroyed
                    .AddListener(BossEnemyCounter.DecreaseEnemy);
            }

            public float Time { get; set; }

            /// <summary>
            /// 敵オブジェクトがDestroyされることを検知
            /// </summary>
            private class DestroyObserver : MonoBehaviour
            {
                public UnityEvent onDestroyed = new UnityEvent();

                private void OnDestroy()
                {
                    onDestroyed.Invoke();
                }
            }
        }
    }
}