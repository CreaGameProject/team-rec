using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLuna : LoopEnemy
{
    protected override void Awake()
    {
        base.Awake();
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
        throw new System.NotImplementedException();
    }
}
