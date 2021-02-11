using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Homing : Bullet
{
    [SerializeField] private GameObject target;
    [SerializeField] private float homingStrength;

    public override void Start(GameObject bulletObject)
    {
        base.Start(bulletObject);
        target = GameObject.Find("Target");//Target名は後で書き換えます。
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        rb.AddForce((target.transform.position - bulletObj.gameObject.transform.position) * homingStrength);
    }
}
