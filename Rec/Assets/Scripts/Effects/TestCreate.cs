using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCreate : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject cylinder;
    public GameObject bullet;
    private float time = 0f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        transform.Rotate(new Vector3(0, 0, 360 / 5) * Time.deltaTime, Space.World);

        if(time >= 1f){
            Instantiate(bullet,cylinder.transform.position, cylinder.transform.rotation);
            time = 0;
        }
    }
}
