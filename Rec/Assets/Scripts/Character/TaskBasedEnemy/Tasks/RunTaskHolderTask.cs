using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Core.Enemy.TaskBased
{
    /// <summary>
    /// 指定したTaskHolderに格納されたタスクを実行するタスク
    /// </summary>
    [AddComponentMenu("EnemyTask/RunTaskHolder")]
    public class RunTaskHolderTask : EnemyTaskComponent
    {
        [SerializeField] private TaskHolder taskHolder;
        [SerializeField, Label("以降のTaskを並列実行")] private bool asynchronous;
        [SerializeField, Label("ループ回数")] private int numLoops = 1;

        public override IEnemyTask ToEnemyTask()
        {
            if (taskHolder == null)
            {
                Debug.LogError("[RunTaskHolderTask] task holder is null");
            }

            return new RunTaskHolder(taskHolder.CollectTasks(), asynchronous, numLoops);
        }

        class RunTaskHolder : IEnemyTask
        {
            private List<IEnemyTask> tasks;
            private bool asynchronous;
            private int numLoops;

            public RunTaskHolder(IEnumerable<IEnemyTask> tasks, bool asynchronous, int numLoops)
            {
                this.tasks = tasks.ToList();
                this.asynchronous = asynchronous;
                this.numLoops = numLoops;
            }

            private IEnumerator Run(TaskBasedEnemy enemy)
            {
                for (int i = 0; i < numLoops; i++)
                {
                    var loopTasks = tasks.CopyTasks();

                    //tasksはTimeでソートされていることを前提としている。
                    foreach (var task in loopTasks)
                    {
                        yield return task.Call(enemy);
                    }
                }
            }

            public IEnumerator Call(TaskBasedEnemy enemy)
            {
                if (asynchronous)
                {
                    enemy.StartCoroutine(Run(enemy));
                }
                else
                {
                    yield return Run(enemy);
                }
            }

            public IEnemyTask Copy()
            {
                return new RunTaskHolder(tasks.CopyTasks(), asynchronous, numLoops);
            }
        }

    }
}
