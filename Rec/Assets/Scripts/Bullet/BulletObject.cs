using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弾のオブジェクト
/// </summary>
public class BulletObject : MonoBehaviour
{
    public Force Force;
    public bool DealDamage { get; }
    public Bullet bulletclass;
    private ParticleSystem[] particles;
    public ParticleSystem playerLazer;
    public ParticleSystem enemyStage1;

    private void Awake()
    {
        particles = new ParticleSystem[] {playerLazer, enemyStage1};
    }

    public void setForce(Force force)
    {
        Force = force;
        switch (Force)
        {
            case Force.Enemy:
                enemyStage1.gameObject.SetActive(true);
                enemyStage1.Play();
                break;
            case Force.Player:
                playerLazer.gameObject.SetActive(true);
                playerLazer.Play();
                break;
        }
    }

    public void SetBullet(Bullet bullet)
    {
        bulletclass = bullet;

        StartCoroutine(TimeDestroy());

        // bullet.csのStart(this.gameObject)を呼び出す
        bulletclass.Start(this.gameObject);

        // うーんこの書き方直したい
        
    }

    void FixedUpdate()
    {
        // bullet.csのFixedUpdate()を呼び出す
        bulletclass.FixedUpdate();
    }

    private IEnumerator TimeDestroy()
    {
        yield return new WaitForSeconds(6); //6秒は仮
        BulletPool.Instance.Destroy(this.gameObject);
    }

    private void OnDisable()
    {
        Rigidbody rig = this.GetComponent<Rigidbody>();
        rig.velocity = Vector3.zero;
        rig.angularVelocity = Vector3.zero;
        foreach (var particle in particles)
        {
            particle.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Force == Force.Player)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                BulletPool.Instance.Destroy(this.gameObject);
            }
        }
        else
        {
            if (other.gameObject.CompareTag("Player"))
            {
                BulletPool.Instance.Destroy(this.gameObject);
            }
        }
    }
}