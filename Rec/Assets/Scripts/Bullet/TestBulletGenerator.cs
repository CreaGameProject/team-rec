using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBulletGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    public BulletPool BulletPool;
    private GameObject _bullet;
    GameObject bulletParticle;

    [ColorUsage(true, true), SerializeField] private Color _straightColor;
    [ColorUsage(true, true), SerializeField] private Color _homingColor;
    [ColorUsage(true, true)] private Color particleColor;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            _bullet = BulletPool.GetInstance(new Homing());
            Homing homing = _bullet.GetComponent<BulletObject>().bulletclass as Homing;
            homing.Velocity = 6f;
            homing.HomingStrength = 1f;
            homing.Target = GameObject.Find("Target");
            homing.AttackPoint = 10f;
            homing.Direction = new Vector3(0, 0, 1);
            particleColor = _homingColor;
            Generate();
        }else if(Input.GetKeyDown(KeyCode.LeftShift)){
            _bullet = BulletPool.GetInstance(new Straight());
            Straight straight = _bullet.GetComponent<BulletObject>().bulletclass as Straight;
            straight.Velocity = 12f + Random.Range(-4f, 4f);
            straight.AttackPoint = 10f;
            straight.Direction = Vector3.Normalize(GameObject.Find("Target").transform.position - this.transform.position);
            particleColor = _straightColor;
            Generate();
        }
        
    }

    void Generate(){


        _bullet.transform.position = transform.position;

        bulletParticle = _bullet.transform.GetChild(0).gameObject;
        

        //randomHDR = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
        bulletParticle.GetComponent<Renderer>().material.SetColor("_EmissionColor", particleColor);

    }
}
