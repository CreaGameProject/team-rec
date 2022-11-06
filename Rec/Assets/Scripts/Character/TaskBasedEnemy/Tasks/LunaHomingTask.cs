using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;


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
        [SerializeField, Label("角度：x")] private float rotationX;
        [SerializeField, Label("角度：y")] private float rotationY;
        [SerializeField, Label("角度：z")] private float rotationZ;
        [SerializeField, Label("発射場所：x")] private float positionX;
        [SerializeField, Label("発射場所：y")] private float positionY;
        [SerializeField, Label("発射場所：z")] private float positionZ;
        [SerializeField] private int animationIndex = 0;
        
        private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

        private Transform playerTf;
        // Taskクラスを生成して返す
        public override IEnemyTask ToEnemyTask()
        {
            playerTf = GameObject.FindGameObjectWithTag("Player").transform;
            return new LunaHoming(velocity, homingStrength, homingColor, playerTf,EmissionColor, 
                rotationX, rotationY, rotationZ, positionX, positionY, positionZ, animationIndex);
        }

        // キャラクターに渡され実行されるタスクのクラス
        private class LunaHoming : IEnemyTask
        {
            private float velocity;
            private float homingStrength;
            private Color homingColor;
            private Transform playerTf;
            private readonly int EmissionColor;
            private float rotationX;
            private float rotationY;
            private float rotationZ;
            private float positionX;
            private float positionY;
            private float positionZ;
            private int animationIndex;


            // コンストラクタ 引数は必要に応じて追加してください
            public LunaHoming(float velocity, float homingStrength, 
                Color homingColor, Transform playerTf, int EmiccionColor,
                float rotationX, float rotationY, float rotationZ,
                float positionX, float positionY, float positionZ, int animationIndex)
            {
                this.velocity = velocity;
                this.homingStrength = homingStrength;
                this.homingColor = homingColor;
                this.playerTf = playerTf;
                this.EmissionColor = EmiccionColor;
                this.rotationX = rotationX;
                this.rotationY = rotationY;
                this.rotationZ = rotationZ;
                this.positionX = positionX;
                this.positionY = positionY;
                this.positionZ = positionZ;
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
                enemy.TriggerAnimation("attack");
                Homing homing = new Homing();
                homing.Name = "Homing";
                homing.Velocity = velocity;
                homing.AttackPoint = 20;
                homing.HomingStrength = homingStrength;
                homing.Direction = Quaternion.Euler(rotationX, rotationY, rotationZ) * enemy.transform.up;
                homing.Target = playerTf.gameObject;
                GameObject enemyBullet = BulletPool.Instance.GetInstance(homing);
                enemyBullet.GetComponent<BulletObject>().setForce(Force.Enemy);
                enemyBullet.transform.position = enemy.transform.position + Vector3.up * positionY +Vector3.right * positionX + Vector3.forward * positionZ;
                GameObject effect = enemyBullet.transform.GetChild(0).gameObject;
                effect.GetComponent<Renderer>().material.SetColor(EmissionColor, homingColor);
                yield break;
            }

            // タスクの複製を行うメソッド
            // 意図したものを除いて 複製元と複製先が同じ参照を持たないように注意してください
            public IEnemyTask Copy()
            {
                return new LunaHoming(velocity, homingStrength, homingColor, playerTf ,EmissionColor,
                    rotationX,rotationY,rotationZ, positionX,positionY, positionZ, animationIndex);
            }
        }
    }
}
