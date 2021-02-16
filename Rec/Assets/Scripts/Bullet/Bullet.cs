using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet
{
    ///<summary>
    ///弾の速度
    ///</summary>
    public float velocity;

    ///<summary>
    ///弾の攻撃力
    ///</summary>
    public float attackPoint;
    
    protected Rigidbody rb;

    protected GameObject bulletObj;
    //[System.NonSerialized] 
    [ColorUsage(true, true), SerializeField] private Color particleColor;
    public virtual void Start(GameObject bulletObject){
        bulletObj = bulletObject;
        rb = bulletObj.GetComponent<Rigidbody>();
        rb.AddForce(bulletObj.transform.up * 50f * velocity);
        //Destroy(this.gameObject, 5f);
    }
    public virtual void FixedUpdate() {
        rb.velocity = rb.velocity.normalized * velocity;
    }
}
