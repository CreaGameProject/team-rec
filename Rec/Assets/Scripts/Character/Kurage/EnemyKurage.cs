using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKurage : Enemy
{
    /*
    [Header("最大HPの設定")]
    /// <summary>
    /// Normalの敵の最大Hp
    /// </summary>
    [SerializeField] int normalLife = 60;
    /// <summary>
    /// Homingの敵の最大Hp
    /// </summary>
    [SerializeField] int homingLife = 30;

    [Header("攻撃の頻度に関する設定")]

    /// <summary>
    /// Normalの敵の攻撃する速さ(回/s)
    /// </summary>
    [SerializeField] protected float normalRate = 1.5f;

    /// <summary>
    /// Homingの敵の攻撃する速さ(回/s)
    /// </summary>
    [SerializeField] protected float homingRate = 2.0f;

    /// <summary>
    /// [通常攻撃の敵専用]バーストの弾と弾の間隔(発/s)
    /// </summary>
    [SerializeField] float burstTime = 0.1f;

    /// <summary>
    /// [通常攻撃の敵専用]バースト時に放つ弾の個数
    /// </summary>
    [SerializeField] int burstcount = 3;
    
    [Header("色調整")]
    [SerializeField] float intensity = 2.0f;
    [ColorUsage(true, true), SerializeField] private Color _straightColor;
    [ColorUsage(true, true), SerializeField] private Color _homingColor;


    protected override void Start() {
        // キャラクターの体力を設定する
        if (enemyType == EnemyType.Normal)
        {
            Hp = normalLife;
        }
        else if (enemyType == EnemyType.Homing)
        {
            Hp = homingLife;
        }

        StartCoroutine(Fire());
         // 弾の色を指定する
        _straightColor = new Color(45f / 255f, 217f / 255f, 146f / 255f, 1) * intensity;
        _homingColor = new Color(82f / 255f, 210f / 255f, 254f / 255f, 1) * intensity;
        base.Start();
        
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        // 後で消す
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 仮指定
            enemyType = EnemyType.Normal;
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }


    protected virtual IEnumerator Fire()
    {
        while (true)
        {            
            if (enemyType == EnemyType.Normal)
            {             
                for (var i = 0; i < burstcount; i++)
                {
                    Straight straight = new Straight();
                    straight.Velocity = 10f; // 仮の値
                    straight.AttackPoint = 10;
                    Vector3 dir = (player.transform.position - this.transform.position).normalized;
                    //straight.Direction = -transform.forward; // 前方向
                    straight.Direction = dir; // プレイヤーの方向
                    GameObject enemyBullet = bulletPool.GetInstance(straight);
                    enemyBullet.GetComponent<BulletObject>().Force = Force.Enemy;
                    enemyBullet.transform.position = this.transform.position;
                    GameObject effect = enemyBullet.transform.GetChild(0).gameObject;
                    effect.GetComponent<Renderer>().material.SetColor("_EmissionColor", _straightColor);
                    yield return new WaitForSeconds(burstTime);
                }
                // クールダウン
                yield return new WaitForSeconds(normalRate);
            }
            else if (enemyType == EnemyType.Homing)
            {
                Homing homing = new Homing();
                homing.Velocity = 6f; // 仮の値
                homing.AttackPoint = 20;
                homing.HomingStrength = 6f; // 仮の値
                homing.Direction = transform.up; // 上方向
                homing.Target = player;
                GameObject enemyBullet = bulletPool.GetInstance(homing);
                enemyBullet.GetComponent<BulletObject>().Force = Force.Enemy;
                enemyBullet.transform.position = this.transform.position;
                GameObject effect = enemyBullet.transform.GetChild(0).gameObject;
                effect.GetComponent<Renderer>().material.SetColor("_EmissionColor", _homingColor);

                // クールダウン
                yield return new WaitForSeconds(homingRate);
            }
            else
            {
                yield return new WaitForSeconds(0.1f);
                Debug.Log("存在しない弾種が指定");
            }
        }
    }*/
}
