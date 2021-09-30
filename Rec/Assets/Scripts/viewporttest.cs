using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class viewporttest : MonoBehaviour
{
    [SerializeField] private float near;
    [SerializeField] private float far;
    [SerializeField] private float fovy;
    [SerializeField] private float aspect; // w/h
    
    [SerializeField] private GameObject obj1;

    private Camera camera;
    private Vector3 cv;

    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        cv = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        var vector3 = transform.position;
        if (cv != vector3)
        {
            var vp = vector3 / 2.0f + new Vector3(0.5f, 0.5f, camera.nearClipPlane);
            var wp = camera.ViewportToWorldPoint(vp);
            obj1.transform.position = wp;
            cv = vector3;
        }
    }
}
