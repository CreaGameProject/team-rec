using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBulletGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    public BulletPool bulletPool;
    GameObject bullet;
    GameObject bulletParticle;

    [ColorUsage(true, true), SerializeField] private Color straightColor;
    [ColorUsage(true, true), SerializeField] private Color homingColor;
    [ColorUsage(true, true)] private Color particleColor;
    [ColorUsage(true, true)] private Color randomHDR;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            bullet = bulletPool.GetInstance(new Homing());
            particleColor = homingColor;
            Homing homing = bullet.GetComponent<BulletObject>().bulletclass as Homing;
            homing.velocity = 6f;
            homing.homingStrength = 1f;
            homing.target = GameObject.Find("Target");
            Generate();
        }else if(Input.GetKeyDown(KeyCode.LeftShift)){
            bullet = bulletPool.GetInstance(new Straight());
            Straight straight = bullet.GetComponent<BulletObject>().bulletclass as Straight;
            straight.velocity = 12f + Random.Range(-4f, 4f);
            particleColor = straightColor;
            Generate();
        }
        
    }

    void Generate(){
        float x = Random.Range(-7f, 7f);
        float y = Random.Range(-7f, 7f);
        float z = Random.Range(-7f, 7f);

        bullet.transform.position = new Vector3(x, y, 20 + z);
        bullet.transform.rotation = transform.rotation;
        bulletParticle = bullet.transform.GetChild(0).gameObject;
        

        //randomHDR = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
        bulletParticle.GetComponent<Renderer>().material.SetColor("_EmissionColor", (particleColor + randomHDR * 2f));

    }
}
