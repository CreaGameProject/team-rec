using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyPulmo : LoopEnemy
{
    
    
    [Header("攻撃の頻度に関する設定")]
    /// <summary>
    /// 敵の攻撃する速さ(回/s)
    /// </summary>
    [SerializeField] protected float attackRate = 1.5f;
    /// <summary>
    /// [通常攻撃の敵専用]バースト時に放つ弾の個数
    /// </summary>
    [SerializeField] private int burstCount;
    /// <summary>
    /// [通常攻撃の敵専用]バーストの弾と弾の間隔(発/s)
    /// </summary>
    [SerializeField] float burstTime = 0.1f;
    
    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");
    
    [Header("色調整")]
    [SerializeField] float intensity = 2.0f;
    [ColorUsage(true, true), SerializeField] private Color straightColor;

    protected override void Awake()
    {
        base.Awake();
        
        // 弾の色を指定する
        straightColor = new Color(45f / 255f, 217f / 255f, 146f / 255f, 1) * intensity;
    }

    private void Start()
    {
        StartCoroutine(Fire());
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void Move()
    {
        base.Move();
    }

    protected override void Kill()
    {
        base.Kill();
    }

    protected override void Damage(int damage)
    {
        base.Damage(damage);
    }

    protected override IEnumerator Fire()
    {
        while (true)
        {
            for (var i = 0; i < burstCount; i++)
            {
                Straight straight = new Straight();
                straight.Velocity = 10f; // 仮の値
                straight.AttackPoint = 10;
                Vector3 position = transform.position;
                Vector3 dir = (playerTf.position - position).normalized;
                //straight.Direction = -transform.forward; // 前方向
                straight.Direction = dir; // プレイヤーの方向
                GameObject enemyBullet = BulletPool.Instance.GetInstance(straight);
                enemyBullet.GetComponent<BulletObject>().Force = Force.Enemy;
                enemyBullet.transform.position = position;
                GameObject effect = enemyBullet.transform.GetChild(0).gameObject;
                effect.GetComponent<Renderer>().material.SetColor(EmissionColor, straightColor);
                yield return new WaitForSeconds(burstTime);
            }

            // クールダウン
            yield return new WaitForSeconds(attackRate);
        }
    }
}
