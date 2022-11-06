using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Enemy.TaskBased
{
    // AddComponentからキャラクターにタスクをアタッチするためのコード   
    [AddComponentMenu("EnemyTask/ShotStraightTask")]
    // AIデザインのためにコンポーネントとしてアタッチされるクラス
    public class ShotStraightTask : EnemyTaskComponent
    {
        [SerializeField, Label("発射速度")] float Speed = 10f;
        [SerializeField] private int animationIndex = 0;

        const float intensity = 2.0f;
        [ColorUsage(true, true)] private Color straightColor;

        private Transform playerTf;

        // Taskクラスを生成して返す
        public override IEnemyTask ToEnemyTask()
        {
            playerTf = GameObject.FindGameObjectWithTag("Player").transform;
            straightColor = new Color(45f / 255f, 217f / 255f, 146f / 255f, 1) * intensity;
            return new Task(playerTf, straightColor, Speed, animationIndex);
        }

        // キャラクターに渡され実行されるタスクのクラス
        private class Task : IEnemyTask
        {
            private float speed;

            private Transform playerTf;
            private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");
            private Color straightColor;
            private int animationIndex;

            // コンストラクタ 引数は必要に応じて追加してください
            public Task(Transform playerTf, Color straightColor, float speed, int animationIndex)
            {
                this.speed = speed;
                this.playerTf = playerTf;
                this.straightColor = straightColor;
                this.animationIndex = animationIndex;
            }

            // ゲーム中に呼び出されるタスク実行のメソッド
            // 引数enemyは行動主体
            public IEnumerator Call(TaskBasedEnemy enemy)
            {
                // アニメーション処理
                IAnimatable animatable = enemy.transform.GetChild(0).GetComponent<IAnimatable>();
                animatable.OnAttack(animationIndex);
                
                // 音再生処理
                SystemSoundManager.Instance.PlaySE("Alpha_EnemyShot");
                
                // タスク処理
                Straight straight = new Straight();
                straight.Name = "Straight";
                straight.Velocity = speed;
                straight.AttackPoint = 10; //仮の値
                Vector3 position = enemy.transform.GetChild(0).position;
                Vector3 dir = (playerTf.position - position).normalized;
                straight.Direction = dir; // プレイヤーの方向
                GameObject enemyBullet = BulletPool.Instance.GetInstance(straight);
                enemyBullet.GetComponent<BulletObject>().setForce(Force.Enemy);
                enemyBullet.transform.position = position;
                GameObject effect = enemyBullet.transform.GetChild(0).gameObject;
                effect.GetComponent<Renderer>().material.SetColor(EmissionColor, straightColor);
                yield break;
            }

            // タスクの複製を行うメソッド
            // 意図したものを除いて 複製元と複製先が同じ参照を持たないように注意してください
            public IEnemyTask Copy()
            {
                return new Task(playerTf, straightColor, speed, animationIndex);
            }
        }
    }
}
