using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.Enemy.TaskBased
{
    // AddComponentからキャラクターにタスクをアタッチするためのコード   
    [AddComponentMenu("EnemyTask/RandomBranchTask")]
    // AIデザインのためにコンポーネントとしてアタッチされるクラス
    public class RandomBranchTask : EnemyTaskComponent
    {
        [Header("分岐先のタスクリスト")]
        [SerializeField] private List<WeightTaskHolder> weightTaskHolders;
        
        // Taskクラスを生成して返す
        public override IEnemyTask ToEnemyTask()
        {
            // Listをweightに応じて分岐し、どれを使用するか決める。それをTask()に渡す。
            float allWeight = weightTaskHolders.Select(x => x.weight).Sum();
            if (allWeight <= 0f)
            {
                throw new Exception("[RandomBranchTask] Weight value cannot be less than 0.");
            }
            
            float randomValue = UnityEngine.Random.Range(0, allWeight);
            var task = GetTask(randomValue);
            
            return new Task(task.taskHolder.CollectTasks(), task.asynchronous, task.numLoops);
        }

        // randomValueに応じてどのタスクを使うか決める
        private WeightTaskHolder GetTask(float randomValue, float currentValue = 0.0f, int index = 0)
        {
            var maxValue = currentValue + weightTaskHolders[index].weight;
            if (randomValue <= maxValue)
            {
                return weightTaskHolders[index];
            }
            else
            {
                return GetTask(randomValue, maxValue, ++index);
            }
        }
        
        // 重みを含んだ分岐タスクのクラス
        [System.Serializable]
        private class WeightTaskHolder
        {
            public TaskHolder taskHolder;
            public float weight = 1;
            [Label("分岐先のTaskを並列実行")] public bool asynchronous;
            [Label("ループ回数")] public int numLoops = 1;
        }

        // キャラクターに渡され実行されるタスクのクラス
        private class Task : IEnemyTask
        {
            private readonly List<IEnemyTask> _tasks;
            private readonly bool _asynchronous;
            private readonly int _numLoops;
            
            // コンストラクタ 引数は必要に応じて追加してください
            public Task(IEnumerable<IEnemyTask> tasks, bool asynchronous, int numLoops)
            {
                _tasks = tasks.ToList();
                _asynchronous = asynchronous;
                _numLoops = numLoops;
            }
            
            private IEnumerator Run(TaskBasedEnemy enemy)
            {
                for (int i = 0; i < _numLoops; i++)
                {
                    var loopTasks = _tasks.CopyTasks();

                    //tasksはTimeでソートされていることを前提としている。
                    foreach (var task in loopTasks)
                    {
                        yield return task.Call(enemy);
                    }
                }
            }

            // ゲーム中に呼び出されるタスク実行のメソッド
            // 引数enemyは行動主体
            public IEnumerator Call(TaskBasedEnemy enemy)
            {
                if (_asynchronous)
                {
                    enemy.StartCoroutine(Run(enemy));
                }
                else
                {
                    yield return Run(enemy);
                }
            }

            // タスクの複製を行うメソッド
            // 意図したものを除いて 複製元と複製先が同じ参照を持たないように注意してください
            public IEnemyTask Copy()
            {
                return new Task(_tasks, _asynchronous, _numLoops);
            }
        }
    }
}
