using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class LaserObject : MonoBehaviour
{
    private const float DamageDelay = 0.4f;

    private GameObject laserHead;
    [SerializeField] private Component laserParticles;
    private bool _isLaser = false;
    private Vector3 _launchPos;
    private Vector3 _direction;
    private float _length;

    private float _duration;
    private float _time;
    
    public Force force { get; private set; }
    public float attackPoint { get; private set; }
    public bool canDealDamage { get; set; }
    public bool isHit { get; set; }

    private void Awake()
    {
        laserHead = new GameObject("RazerHead");
        laserHead.transform.SetParent(this.gameObject.transform);
        laserParticles = GetComponentInChildren(typeof(ParticleSystem));
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        _time += Time.deltaTime;

        if (_time >= _duration) StartCoroutine(StopLaser());

        if (_time >= DamageDelay)
        {
            if (isHit) return;
            canDealDamage = true;
        }
    }

    private void OnEnable()
    {
        _time = 0f;
        isHit = false;
        canDealDamage = false;
    }

    private void OnDisable()
    {
        laserHead.transform.position = this.transform.position;
    }

    public void SetForce(Force f)
    {
        force = f;
    }

    public void ShotLaser(Vector3 launchPos, Vector3 endPos, float timer, float attack)
    {
        _launchPos = launchPos;
        
        Vector3 dir = endPos - launchPos;
        _length = Vector3.Magnitude(dir);
        _direction = dir.normalized;
        _duration = timer;
        attackPoint = attack;

        transform.position = launchPos;
        transform.up = dir;
        _isLaser = true;
    }

    public void ShotLaser(Vector3 launchPos, Vector3 direction, float length,float timer, float attack)
    {
        _launchPos = launchPos;
        _length = length;
        _direction = direction.normalized;
        _duration = timer;
        attackPoint = attack;

        transform.position = launchPos;
        transform.up = direction;
        _isLaser = true;
    }

    public IEnumerator StopLaser()
    {
        laserParticles.transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
        laserParticles.transform.GetChild(1).GetChild(0).GetComponent<ParticleSystem>().Stop();
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}