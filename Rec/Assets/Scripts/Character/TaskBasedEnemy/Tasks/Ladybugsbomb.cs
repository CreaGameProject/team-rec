using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Enemy.TaskBased
{
    // AddComponentからキャラクターにタスクをアタッチするためのコード   
    [AddComponentMenu("EnemyTask/Ladybugsbomb")]
    // AIデザインのためにコンポーネントとしてアタッチされるクラス
    public class Ladybugsbomb : EnemyTaskComponent
    {
        //[SerializeField] private KurageAnimationController animationContoller;
        private Transform playerTf;

        [SerializeField, Label("爆発開始距離")] private float Within_range;
        protected GameObject _enemy;
        // Taskクラスを生成して返す
        public override IEnemyTask ToEnemyTask()
        {
            playerTf = GameObject.FindGameObjectWithTag("Player").transform;
            return new HomingBakusatu(playerTf,Within_range,_enemy);
        }

        // キャラクターに渡され実行されるタスクのクラス
        private class HomingBakusatu : IEnemyTask
        {
            //private KurageAnimationController animationController;

            private Transform playerTf;

            private float Within_range;
            protected GameObject _enemy;
            // コンストラクタ 引数は必要に応じて追加してください
            public HomingBakusatu( Transform playerTf,float Within_range,GameObject _enemy)
            {
                //this.animationController = animationController;
                this.playerTf = playerTf;
                this.Within_range = Within_range;
                this._enemy = _enemy;
            }

            // ゲーム中に呼び出されるタスク実行のメソッド
            // 引数enemyは行動主体
            public IEnumerator Call(TaskBasedEnemy enemy)
            {
                if (playerTf != null)
                {
                    Debug.Log("a");
                }
                _enemy = enemy.gameObject;
                float distance2 = Vector3.Distance(_enemy.transform.position, playerTf.transform.position);
                Debug.Log(distance2);
                while (playerTf != null)
                {
                    float distance = Vector3.Distance(_enemy.transform.position, playerTf.transform.position);
                    Debug.Log(distance);
                    if (distance < Within_range)
                    {
                        Debug.Log("bakusatu!");
                    }
                    yield return new WaitForSeconds(0.02f);
                }
            }

            // タスクの複製を行うメソッド
            // 意図したものを除いて 複製元と複製先が同じ参照を持たないように注意してください
            public IEnemyTask Copy()
            {
                return new HomingBakusatu(playerTf, Within_range,_enemy);
            }
        }
    }
}
