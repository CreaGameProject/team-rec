using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Homing : Bullet
{
    [SerializeField] private GameObject target;
    [SerializeField] private float homingStrength;

    protected override void Start()
    {
        base.Start();
        target = GameObject.Find("Target");
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        rb.AddForce((target.transform.position - this.transform.position) * homingStrength);
    }
}
