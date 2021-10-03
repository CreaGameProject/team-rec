using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Core.Enemy.TaskBased
{
    // AddComponentからキャラクターにタスクをアタッチするためのコード   
    [AddComponentMenu("EnemyTask/LunaHoming")]
    // AIデザインのためにコンポーネントとしてアタッチされるクラス
    public class LunaHomingTask : EnemyTaskComponent
    {
        [SerializeField, Label("速さ")] private float velocity;
        [ColorUsage(true, true), SerializeField] private Color homingColor;
        [SerializeField] float homingStrength = 3.0f;
        
        private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

        private Transform playerTf;
        // Taskクラスを生成して返す
        public override IEnemyTask ToEnemyTask()
        {
            playerTf = GameObject.FindGameObjectWithTag("Player").transform;
            return new LunaHoming(velocity, homingStrength, homingColor, playerTf,EmissionColor);
        }

        // キャラクターに渡され実行されるタスクのクラス
        private class LunaHoming : IEnemyTask
        {
            private float velocity;
            private float homingStrength;
            private Color homingColor;
            private Transform playerTf;
            private readonly int EmissionColor;


            // コンストラクタ 引数は必要に応じて追加してください
            public LunaHoming(float velocity, float homingStrength, 
                Color homingColor, Transform playerTf, int EmiccionColor)
            {
                this.velocity = velocity;
                this.homingStrength = homingStrength;
                this.homingColor = homingColor;
                this.playerTf = playerTf;
                this.EmissionColor = EmiccionColor;
            }

            // ゲーム中に呼び出されるタスク実行のメソッド
            // 引数enemyは行動主体
            public IEnumerator Call(TaskBasedEnemy enemy)
            {
                enemy.TriggerAnimation("attack");
                Homing homing = new Homing();
                homing.Name = "Homing";
                homing.Velocity = velocity;
                homing.AttackPoint = 20;
                homing.HomingStrength = homingStrength;
                homing.Direction = enemy.transform.up;
                homing.Target = playerTf.gameObject;
                GameObject enemyBullet = BulletPool.Instance.GetInstance(homing);
                enemyBullet.GetComponent<BulletObject>().setForce(Force.Enemy);
                enemyBullet.transform.position = enemy.transform.position + Vector3.up * 1.8f;
                GameObject effect = enemyBullet.transform.GetChild(0).gameObject;
                effect.GetComponent<Renderer>().material.SetColor(EmissionColor, homingColor);
                yield break;
            }

            // タスクの複製を行うメソッド
            // 意図したものを除いて 複製元と複製先が同じ参照を持たないように注意してください
            public IEnemyTask Copy()
            {
                return new LunaHoming(velocity, homingStrength, homingColor, playerTf ,EmissionColor);
            }
        }
    }
}
