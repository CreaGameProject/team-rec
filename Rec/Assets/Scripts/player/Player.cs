using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private int life;
    private float laserGauge;

    private float angle;

    Camera mainCamera;
    Vector3 mousePos = new Vector3 (0, 0, 0);

    [SerializeField] Texture2D pointTexture;

    float moveXMax = 11f;
    float moveYMin = 4f;
    float moveYMax = 6f;

    float homingRange = 1f;
    bool homingShot = false;

    [SerializeField] GameObject testObject;


    // Start is called before the first frame update
    void Start()
    {
        life = 3;
        laserGauge = 5;
        angle = 1f / 180f * Mathf.PI;

        mainCamera = Camera.main;
        Cursor.SetCursor(pointTexture, new Vector2(pointTexture.width/2 , pointTexture.height/2), CursorMode.ForceSoftware);
    }

    // Update is called once per frame
    void Update()
    {
        getInput();

        //mousePos = Input.mousePosition;
        //mousePos.z = 10f;
        //mousePos = mainCamera.ScreenToWorldPoint(mousePos);
        //testObject.transform.position = mousePos;

        //Debug.Log("x:"+mousePos.x+"y:"+mousePos.y+"z:"+mousePos.z);
    }

    void getInput()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(-0.01f*Mathf.Cos(transform.rotation.y * angle),0, -0.01f * Mathf.Sin(transform.rotation.y * angle));
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(0.01f*Mathf.Cos(transform.rotation.y * angle), 0, 0.01f * Mathf.Sin(transform.rotation.y * angle));
        }
        
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(0, 0.01f, 0);           
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(0, -0.01f, 0, 0);
        }

        transform.localPosition = new Vector3(Mathf.Clamp(transform.localPosition.x,-moveXMax,moveXMax)
                                    ,Mathf.Clamp(transform.localPosition.y,-moveYMin,moveYMax)
                                    ,Mathf.Clamp(transform.localPosition.z,-moveXMax,moveXMax));

        if (Input.GetMouseButtonUp(0))
        {
            sendRay();
        }
        else if (Input.GetMouseButtonUp(1))
        {
           
            Debug.Log("右up");
            homingShot = true;
            openTarget();
        }
        else if (Input.GetMouseButton(1))
        {
            homingRange += 0.05f;

            if (homingRange >10)
            {
                homingRange = 10;
            }

            Debug.Log("右");
            openTarget();
        }
    }

    void sendRay()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Debug.Log(hit.collider.gameObject.name);
            Debug.Log(hit.collider.gameObject.transform.position);
        }
    }

    void openTarget()
    {
        
        mousePos = Input.mousePosition;
        mousePos.z = 10f;
        mousePos = mainCamera.ScreenToWorldPoint(mousePos);

        RaycastHit[] hits = Physics.BoxCastAll(mousePos, Vector3.one*homingRange, Camera.main.transform.forward, Quaternion.identity, 100f, LayerMask.GetMask("Default"));

        if (homingShot == true)
        {
            foreach (RaycastHit hit in hits)
            {
                Debug.Log(hit.collider.gameObject.name);
            }

            homingRange = 1f;
            homingShot = false;
        }
    }

    public void increaseLaserGauge()
    {
        laserGauge++;
    }

    public void decreaseLife()
    {
        life--;
    }
}
