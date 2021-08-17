using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet
{

    //Bulletのステータスはすべてこちらで管理する。

    /// <summary>
    /// 弾の名前
    /// </summary>
    public virtual string Name { get; set; }

    ///<summary>
    ///弾の速度
    ///</summary>
    public float Velocity;

    ///<summary>
    ///弾の攻撃力
    ///</summary>
    public int AttackPoint;

    /// <summary>
    /// 弾の飛ばす方角
    /// </summary>
    public Vector3 Direction;
    
    public Force Force { get; private set; } 
    
    protected Rigidbody rb;

    protected GameObject _bulletObject;
    //[System.NonSerialized] 
    [ColorUsage(true, true), SerializeField] private Color particleColor;
    public virtual void Start(GameObject bulletObject){
        _bulletObject = bulletObject;
        rb = _bulletObject.GetComponent<Rigidbody>();
        rb.AddForce(Direction * 50f * Velocity);
        //Destroy(this.gameObject, 5f);
    }
    public virtual void FixedUpdate() {
        rb.velocity = rb.velocity.normalized * Velocity;
    }
}
