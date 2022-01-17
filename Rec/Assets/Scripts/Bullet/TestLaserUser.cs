using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLaserUser : MonoBehaviour
{
    public LaserPool laserPool;

    private void Start()
    {
        laserPool = LaserPool.Instance;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            laserPool.ShotLaser(this.transform.position, transform.up, 10.0f, 10.0f, 1.0f);
            laserPool.ShotLaser(this.transform.position,this.transform.position+transform.up,10.0f, 1.0f);
        }
    }
}