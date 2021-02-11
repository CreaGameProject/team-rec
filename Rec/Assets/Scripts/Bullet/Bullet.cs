using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet
{
<<<<<<< HEAD
    public float velocity = 0f;
    protected Rigidbody rb;
=======

    public float velocity = 0f;
    protected Rigidbody rb;

>>>>>>> upstream/bulletpool
    protected GameObject bulletObj;
    //[System.NonSerialized] 
    public GameObject bulletParticle;
    [ColorUsage(true, true), SerializeField] private Color particleColor;


    public virtual void Start(GameObject bulletObject){
        bulletObj = bulletObject;
        rb = bulletObj.GetComponent<Rigidbody>();
        rb.AddForce(bulletObj.transform.up * 50f * velocity);
        bulletParticle.GetComponent<Renderer>().material.SetColor("_EmissionColor",particleColor);
        //Destroy(this.gameObject, 5f);
    }


<<<<<<< HEAD
=======

>>>>>>> upstream/bulletpool
    public virtual void FixedUpdate() {
        rb.velocity = rb.velocity.normalized * velocity;
    }

}
