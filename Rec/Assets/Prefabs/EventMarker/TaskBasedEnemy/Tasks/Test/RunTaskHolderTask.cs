using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class RunTaskHolderTask : EnemyTaskComponent
{
    [SerializeField] private TaskHolder taskHolder;
    [SerializeField] private float startTime;
    [SerializeField] private int numLoops = 1;
    [SerializeField] private float oneLoopDuration;

    public override IEnemyTask ToEnemyTask()
    {
        if (taskHolder == null)
        {
            Debug.LogError("[RunTaskHolderTask] task holder is null");
        }

        return new RunTaskHolder(startTime, taskHolder.CollectTasks(), numLoops, oneLoopDuration);
    }


    class RunTaskHolder : IEnemyTask
    {
        public float Time { get; }

        private float startTime;
        private List<IEnemyTask> tasks;
        private int numLoops;
        private float oneLoopDuration;
        
        public RunTaskHolder(float startTime, IEnumerable<IEnemyTask> tasks, int numLoops, float oneLoopDuration)
        {
            this.startTime = startTime;
            this.tasks = tasks.ToList();
            this.numLoops = numLoops;
            this.oneLoopDuration = oneLoopDuration;
        }

        private IEnumerator Run(TaskBasedEnemy enemy)
        {
            for (int i = 0; i < numLoops; i++)
            {
                float time = 0;
                var loopTasks = tasks.CopyTasks();
                
                while (enemy != null && time <= oneLoopDuration)
                {
                    time += UnityEngine.Time.deltaTime;

                    //tasksはTimeでソートされていることを前提としている。
                    foreach (var x in loopTasks.TakeWhile(x => x.Time < time))
                    {
                        x.Call(enemy);
                    }

                    loopTasks = loopTasks.SkipWhile(x => x.Time < time).ToList();
                    time += UnityEngine.Time.deltaTime;
                    yield return null;
                }
            }
        }
        
        public void Call(TaskBasedEnemy enemy)
        {
            enemy.StartCoroutine(Run(enemy));
        }

        public IEnemyTask Copy()
        {
            return new RunTaskHolder(startTime, tasks.CopyTasks(), numLoops, oneLoopDuration);
        }
    }

}
