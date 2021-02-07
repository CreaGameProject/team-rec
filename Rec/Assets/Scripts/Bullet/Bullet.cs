using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float velocity = 0f;
    protected Rigidbody rb;
    //[System.NonSerialized] 
    public GameObject bulletParticle;
    [ColorUsage(true, true), SerializeField] private Color particleColor;


    protected virtual void Start(){
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.up * 50f * velocity);
        bulletParticle.GetComponent<Renderer>().material.SetColor("_EmissionColor",particleColor);
        //Destroy(this.gameObject, 5f);
    }

    protected virtual void FixedUpdate() {
        rb.velocity = rb.velocity.normalized * velocity;
    }

    
}
