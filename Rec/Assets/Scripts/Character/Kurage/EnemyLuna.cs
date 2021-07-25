using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLuna : LoopEnemy
{
    [Header("攻撃の頻度に関する設定")]
    /// <summary>
    /// 敵の攻撃する速さ(回/s)
    /// </summary>
    [SerializeField] protected float attackRate = 3f;
    /// <summary>
    /// [通常攻撃の敵専用]バースト時に放つ弾の個数
    /// </summary>
    [SerializeField] private int burstCount;
    /// <summary>
    /// [通常攻撃の敵専用]バーストの弾と弾の間隔(発/s)
    /// </summary>
    [SerializeField] float burstTime = 0.1f;

    [SerializeField] float homingStrength = 3.0f;

    Coroutine coroutine;

    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

    [Header("色調整")]
    [SerializeField] float intensity = 2.0f;
    [ColorUsage(true, true), SerializeField] private Color homingColor;

    [SerializeField] private KurageAnimationController animationController;

    protected override void Awake()
    {
        base.Awake();

        homingColor *= intensity;
    }

    private void Start()
    {
        coroutine = StartCoroutine(Fire());
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
        StopCoroutine(coroutine);
        animationController.OnDie();
    }

    protected override void Damage(int damage)
    {
        base.Damage(damage);
    }

    protected override IEnumerator Fire()
    {
        yield return new WaitForSeconds(attackRate);
        while (true)
        {
            animationController.PlayAttackAnimation();

            yield return new WaitForSeconds(0.9f);//変数置いてやる必要があるかも

            Homing homing = new Homing();
            homing.Velocity = 3f; // 仮の値
            homing.AttackPoint = 20;
            homing.HomingStrength = homingStrength; // 仮の値
            homing.Direction = transform.up; // 上方向
            homing.Target = playerTf.gameObject;
            GameObject enemyBullet = BulletPool.Instance.GetInstance(homing);
            enemyBullet.GetComponent<BulletObject>().Force = Force.Enemy;
            enemyBullet.transform.position = this.transform.position + Vector3.up * 1.8f;
            GameObject effect = enemyBullet.transform.GetChild(0).gameObject;
            effect.GetComponent<Renderer>().material.SetColor(EmissionColor, homingColor);

            // クールダウン
            yield return new WaitForSeconds(attackRate);
        }
    }
}
