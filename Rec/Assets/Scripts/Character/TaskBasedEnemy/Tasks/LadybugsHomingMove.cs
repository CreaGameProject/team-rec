using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using NUnit.Framework.Constraints;
using UnityEngine;

namespace Core.Enemy.TaskBased
{
    // AddComponentからキャラクターにタスクをアタッチするためのコード   
    [AddComponentMenu("EnemyTask/LadybugsHomingMove")]
    // AIデザインのためにコンポーネントとしてアタッチされるクラス
    public class LadybugsHomingMove : EnemyTaskComponent
    {
        [SerializeField, Label("速さ")] private float velocity;
        private Transform playerTf;
        protected Rigidbody rb;

        protected GameObject _enemy;
        // Taskクラスを生成して返す
        public override IEnemyTask ToEnemyTask()
        {
            //rb = GetComponent<Rigidbody>();
            playerTf = GameObject.FindGameObjectWithTag("Player").transform;
            return new HomingMove(velocity, playerTf,rb,_enemy);
        }

        // キャラクターに渡され実行されるタスクのクラス
        private class HomingMove : IEnemyTask
        {
            private float velocity;
            private float homingStrength;
            private Transform playerTf;
            protected Rigidbody rb;
            protected GameObject _enemy;
            // コンストラクタ 引数は必要に応じて追加してください
            public HomingMove(float velocity, Transform playerTf, Rigidbody rb,GameObject _enemy)
            {
                this.velocity = velocity;
                this.homingStrength = homingStrength;
                this.playerTf = playerTf;
                this.rb = rb;
                this._enemy = _enemy;
            }

            // ゲーム中に呼び出されるタスク実行のメソッド
            // 引数enemyは行動主体
            public IEnumerator Call(TaskBasedEnemy enemy)
            {
                _enemy = enemy.gameObject;
                rb = _enemy.GetComponent<Rigidbody>();
                //rb.velocity = rb.velocity.normalized * velocity;
                //rb.AddForce(playerTf.transform.position * 50f * velocity);
                while(playerTf != null)
                {
                    //rb.AddForce((playerTf.transform.position - _enemy.transform.position).normalized * homingStrength);
                    rb.velocity = (playerTf.transform.position - _enemy.transform.position).normalized * velocity;
                    //rb.AddForce((playerTf.transform.position - _enemy.transform.position).normalized * homingStrength);
                    yield return new WaitForSeconds(0.02f);
                }

                yield break;
            }

            // タスクの複製を行うメソッド
            // 意図したものを除いて 複製元と複製先が同じ参照を持たないように注意してください
            public IEnemyTask Copy()
            {
                return new HomingMove(velocity, playerTf, rb, _enemy);
            }
        }
    }
}
