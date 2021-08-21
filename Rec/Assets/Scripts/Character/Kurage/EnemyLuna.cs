using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLuna : LoopEnemy
{
    [Header("攻撃の頻度に関する設定")]
    [SerializeField] protected float attackRate = 3f;
    [SerializeField] private int burstCount;
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
            homing.HomingStrength = homingStrength;
            homing.Direction = transform.up; // 上方向
            homing.Target = playerTf.gameObject;
            GameObject enemyBullet = BulletPool.Instance.GetInstance(homing);
            enemyBullet.GetComponent<BulletObject>().setForce(Force.Enemy);
            enemyBullet.transform.position = this.transform.position + Vector3.up * 1.8f;
            GameObject effect = enemyBullet.transform.GetChild(0).gameObject;
            effect.GetComponent<Renderer>().material.SetColor(EmissionColor, homingColor);

            // クールダウン
            yield return new WaitForSeconds(attackRate);
        }
    }
}
