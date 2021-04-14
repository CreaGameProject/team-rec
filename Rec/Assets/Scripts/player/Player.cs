using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] BulletPool bulletPool;

    private int life;
    private float laserGauge =500f;

    private float angle;

    Camera mainCamera;
    Vector3 mousePos = new Vector3 (0, 0, 0);

    [SerializeField] Texture2D pointTexture;


    float moveXMax = 3f;
    float moveYMin = 1f;
    float moveYMax = 2.5f;


    float homingRange = 1f;
    bool homingShot = false;

    [SerializeField] GameObject testObject;

    [Header("調整可")]
    /// <summary>
    /// 移動速度
    /// </summary>
    [SerializeField] private float speed;

    [ColorUsage(true, true), SerializeField] private Color _straightColor;
    [ColorUsage(true, true), SerializeField] private Color _homingColor;

    public float moveForceMultiplier;

    // 水平移動時に機首を左右に向けるトルク
    public float yawTorqueMagnitude = 30.0f;

    // 垂直移動時に機首を上下に向けるトルク
    public float pitchTorqueMagnitude = 60.0f;

    // 水平移動時に機体を左右に傾けるトルク
    public float rollTorqueMagnitude = 30.0f;

    // バネのように姿勢を元に戻すトルク
    public float restoringTorqueMagnitude = 100.0f;

    private Vector3 Player_pos;
    private new Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        life = 3;
        laserGauge = 500;
        angle = 1f / 180f * Mathf.PI;

        mainCamera = Camera.main;
        Cursor.SetCursor(pointTexture, new Vector2(pointTexture.width/2 , pointTexture.height/2), CursorMode.ForceSoftware);
        
    }

    // Update is called once per frame
    void Update()
    {
        getInput();
    }

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.angularDrag = 20.0f;
    }

    void getInput()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(new Vector3(-0.1f*Mathf.Cos(transform.rotation.y * angle),0, -0.1f * Mathf.Sin(transform.rotation.y * angle)) * speed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(new Vector3(0.1f*Mathf.Cos(transform.rotation.y * angle), 0, 0.1f * Mathf.Sin(transform.rotation.y * angle)) * speed * Time.deltaTime);
        }
        
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(0, 0.1f * speed * Time.deltaTime, 0);           
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(0, -0.1f * speed * Time.deltaTime, 0, 0);
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
            homingRange += 0.01f;

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
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 20f;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        Debug.DrawRay(ray.origin,ray.direction*100,Color.green,5,false);

        if (Physics.Raycast(Camera.main.transform.position, (mousePos3D - Camera.main.transform.position), out hit, 100f))
        {
            //Debug.Log(hit.collider.gameObject.name);
            //Debug.Log("衝突位置 : "+hit.point);

            //ここでストレートを打つ
            //hit.collider.gameObjectでぶつかったオブジェクトのことを指す
            Vector3 array = (hit.point - Camera.main.transform.position).normalized;
            
            Straight straight = new Straight();
            straight.Velocity = 10f; // 仮の値
            straight.Direction = array;
            GameObject bullet = bulletPool.GetInstance(straight);
            bullet.transform.position = this.transform.position;
            GameObject effect = bullet.transform.GetChild(0).gameObject;
            effect.GetComponent<Renderer>().material.SetColor("_EmissionColor", _straightColor);
            bullet.GetComponent<BulletObject>().Force = Force.Player;
        }
    }

    void openTarget()
    {
        
        mousePos = Input.mousePosition;
        mousePos.z = 10f;
        mousePos = mainCamera.ScreenToWorldPoint(mousePos);

        RaycastHit[] hits = Physics.BoxCastAll(mousePos, Vector3.one*homingRange, (mousePos-Camera.main.transform.position), Quaternion.identity, 100f, LayerMask.GetMask("Default"));


        if (homingShot == true)
        {
            foreach (RaycastHit hit in hits)
            {
                
                if (laserGauge >=50)
                {
                    Debug.Log(hit.collider.gameObject.name);
                    laserGauge -= 50;

                    //ここでホーミングを打つ(つまり単発を高速レートで打つ感じ)
                    //hit.collider.gameObjectでぶつかったオブジェクトのことを指す
                    Homing homing = new Homing();
                    homing.Velocity = 10f; // 仮の値
                    homing.HomingStrength = 10f; // 仮の値
                    homing.Direction = transform.forward;
                    homing.Target = hit.collider.gameObject;
                    GameObject bullet = bulletPool.GetInstance(homing);
                    GameObject effect = bullet.transform.GetChild(0).gameObject;
                    effect.GetComponent<Renderer>().material.SetColor("_EmissionColor", _homingColor);
                    bullet.GetComponent<BulletObject>().Force = Force.Player;
                    bullet.transform.position = this.transform.position;
                }
            }

            homingRange = 1f;
            homingShot = false;

            Debug.Log(laserGauge);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(mousePos, Vector3.one * homingRange);
    }

    public void increaseLaserGauge(float num)
    {
        laserGauge +=num;
    }

    public void decreaseLife()
    {
        life--;

        if (life <= 0)
        {
            OnDeath();
        }
    }

    /// <summary>
    /// 死亡時の処理
    /// </summary>
    private void OnDeath()
    {
        Debug.Log("死");
    }

    /// <summary>
    /// 衝突判定
    /// </summary>
    /// <param name="other">相手のゲームオブジェクト</param>
    private void OnTriggerEnter(Collider other)
    {
        // 相手がBulletObjectｺﾝﾎﾟｰﾈﾝﾄを持っているかを検知する
        if (other.gameObject.GetComponent<BulletObject>())
        {
            BulletObject bulletObject = other.gameObject.GetComponent<BulletObject>();
            // 弾が敵の勢力だった場合
            if (bulletObject.Force == Force.Enemy)
            {
                BulletPool.Instance.Destroy(other.gameObject);
                decreaseLife();
            }
        }
    }
    
    void FixedUpdate()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        // xとyにspeedを掛ける
        rigidbody.AddForce(x * speed, y * speed, 0);

        Vector3 moveVector = Vector3.zero;

        rigidbody.AddForce(moveForceMultiplier * (moveVector - rigidbody.velocity));

        // プレイヤーの入力に応じて姿勢をひねろうとするトルク
        Vector3 rotationTorque = new Vector3(-y * pitchTorqueMagnitude, x * yawTorqueMagnitude, -x * rollTorqueMagnitude);

        // 現在の姿勢のずれに比例した大きさで逆方向にひねろうとするトルク
        Vector3 right = transform.right;
        Vector3 up = transform.up;
        Vector3 forward = transform.forward;
        Vector3 restoringTorque = new Vector3(forward.y - up.z, right.z - forward.x, up.x - right.y) * restoringTorqueMagnitude;

        // 機体にトルクを加える
        rigidbody.AddTorque(rotationTorque + restoringTorque);
    }
}
