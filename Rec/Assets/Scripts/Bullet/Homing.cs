using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Homing : Bullet
{

   
    ///<summary>
    ///ターゲットのゲームオブジェクト
    ///</summary>
    public GameObject Target;

    ///<summary>
    ///ホーミングの強さ
    ///</summary>
    public float HomingStrength;

    public override void Start(GameObject bulletObject)
    {
        base.Start(bulletObject);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        rb.AddForce((Target.transform.position - _bulletObject.gameObject.transform.position) * HomingStrength);
    }

}
