using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Enemy.TaskBased
{
    // AddComponentからキャラクターにタスクをアタッチするためのコード   
    [AddComponentMenu("EnemyTask/RandomBranchTask")]
    // AIデザインのためにコンポーネントとしてアタッチされるクラス
    public class RandomBranchTask : EnemyTaskComponent
    {

        [SerializeField, Label("分岐先のタスクリスト")] private List<WeightTaskHolder> weightTaskHolders;
        
        // Taskクラスを生成して返す
        public override IEnemyTask ToEnemyTask()
        {
            return new Task();
        }
        
        // 重みを含んだ分岐タスクのクラス
        [System.Serializable]
        private class WeightTaskHolder
        {
            [SerializeField] private TaskHolder taskHolder;
            [SerializeField, Label("分岐倍率")] private float weight = 1;
            [SerializeField, Label("分岐先のTaskを並列実行")] private bool asynchronous;
            [SerializeField, Label("ループ回数")] private int numLoops = 1;
        }

        // キャラクターに渡され実行されるタスクのクラス
        private class Task : IEnemyTask
        {
            // コンストラクタ 引数は必要に応じて追加してください
            public Task()
            {
            
            }

            // ゲーム中に呼び出されるタスク実行のメソッド
            // 引数enemyは行動主体
            public IEnumerator Call(TaskBasedEnemy enemy)
            {
                throw new System.NotImplementedException();
            }

            // タスクの複製を行うメソッド
            // 意図したものを除いて 複製元と複製先が同じ参照を持たないように注意してください
            public IEnemyTask Copy()
            {
                return new Task();
            }
        }
    }
}
