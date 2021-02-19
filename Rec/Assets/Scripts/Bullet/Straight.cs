using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Straight : Bullet
{
    public override void Start(GameObject bulletObject)
    {
        base.Start(bulletObject);//<-順番に気をつけて
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
