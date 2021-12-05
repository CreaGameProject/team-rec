using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class LaserObject : MonoBehaviour
{
    private GameObject laserHead;
    [SerializeField] private Component laserParticles;
    private bool _isLaser = false;
    private Vector3 _launchPos;
    private Vector3 _direction;
    private float _length;
    
    public Force force { get; private set; }
    public float attackPoint { get; private set; }

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
        if(_isLaser)OnRay();
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
        attackPoint = attack;

        _isLaser = true;
    }

    public void ShotLaser(Vector3 launchPos, Vector3 direction, float length,float timer, float attack)
    {
        _launchPos = launchPos;
        _length = length;
        _direction = direction.normalized;
        attackPoint = attack;

        _isLaser = true;
    }

    private void OnRay()
    {
        RaycastHit hit;
        Ray ray = new Ray(_launchPos, _direction);
        if (Physics.Raycast(ray, out hit, _length))
        {
            Debug.Log(hit.collider.gameObject.transform.position);
        }
        
        Debug.DrawRay(ray.origin, ray.direction * _length, Color.green, 5);
    }
}