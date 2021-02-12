using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Homing : Bullet
{
    public GameObject target;
    public float homingStrength;

    public override void Start(GameObject bulletObject)
    {
        //target = GameObject.Find("Target");//Target名は後で書き換えます。
        base.Start(bulletObject);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        rb.AddForce((target.transform.position - bulletObj.gameObject.transform.position) * homingStrength);
    }
}
