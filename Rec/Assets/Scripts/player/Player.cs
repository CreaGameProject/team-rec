using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] BulletPool bulletPool;

    /// <summary>
    /// 体力の最大値
    /// </summary>
    public static int MaxLife = 200;

    /// <summary>
    /// 現在の体力値
    /// </summary>
    public static int Life;

    /// <summary>
    /// レーザー弾のゲージ量
    /// </summary>
    private float laserGauge = 200f;

    private float angle;

    Camera mainCamera;
    Vector3 mousePos = new Vector3 (0, 0, 0);

    [SerializeField] Texture2D pointTexture;

    // 各最大移動量
    float moveXMax = 3f;
    float moveYMin = 1f;
    float moveYMax = 2.5f;

    float homingRange = 1f;
    bool homingShot = false;

    [SerializeField] GameObject gameOverObj;
    [SerializeField] private GameObject pauseWindow;

    [Header("調整可")]
    /// <summary>
    /// 移動速度
    /// </summary>
    [SerializeField] private float speed;

    [ColorUsage(true, true), SerializeField] private Color _straightColor;
    [ColorUsage(true, true), SerializeField] private Color _homingColor;

    /// <summary>
    /// 機体
    /// </summary>
    [SerializeField] GameObject body;

    

    private Vector3 Player_pos;
    private new Rigidbody rigidbody;

    /// <summary>
    /// 操作入力不可かどうか
    /// </summary>
    private bool isStopped = false;

    /// <summary>
    /// ポーズ可能かどうか
    /// </summary>
    private bool canPause = true;


    // Start is called before the first frame update
    void Start()
    {
        Life = MaxLife;
        laserGauge = 200;
        angle = 1f / 180f * Mathf.PI;

        mainCamera = Camera.main;
        Cursor.SetCursor(pointTexture, new Vector2(pointTexture.width/2 , pointTexture.height/2), CursorMode.ForceSoftware);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isStopped)
        {
            getControlInput();
        }

        getOtherInput();

        //UnityEngine.PlayerLoop.FixedUpdate();

    }

    void Awake()
    {
        rigidbody = body.GetComponent<Rigidbody>();
        rigidbody.angularDrag = 20.0f;
    }


    /// <summary>
    /// 移動・攻撃入力の管理
    /// </summary>
    void getControlInput()
    {
        // 移動入力
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

        // 攻撃入力
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


    /// <summary>
    /// 移動・攻撃以外の入力を管理
    /// </summary>
    void getOtherInput()
    {
        // 各機能入力
        if (((Input.GetKeyDown(KeyCode.P)) || (Input.GetKeyDown(KeyCode.Escape))) && (canPause))
        {
            // ポーズ切り替え
            var tr = WindowTransitionData.Transition;
            if ((tr != WindowTransition.Pause) && (tr != WindowTransition.Option))
            {
                // ポーズ起動
                WindowTransitionData.Transition = WindowTransition.Pause;
                isStopped = true;
                pauseWindow.SetActive(true);
                Time.timeScale = 0.0f;
            }
            else
            {
                // ポーズ解除
                if (tr != WindowTransition.Option)
                {
                    WindowTransitionData.Transition = WindowTransition.InGame;
                    Time.timeScale = 1.0f;
                    isStopped = false;
                    pauseWindow.SetActive(false);
                }                
            }
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            // てすと、あとでけす
            decreaseLife(50);
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
            straight.AttackPoint = 1; // 仮の値
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
                
                if (laserGauge >=20)
                {
                    Debug.Log(hit.collider.gameObject.name);
                    laserGauge -= 20;

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

    public void decreaseLife(int damagePoint)
    {
        Life -= damagePoint;

        if (Life <= 0)
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

        // ゲームオーバーの画面を表示する
        WindowTransitionData.Transition = WindowTransition.GameOver;
        isStopped = true;
        canPause = false;
        Time.timeScale = 0.1f;
        gameOverObj.SetActive(true);
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
                decreaseLife(bulletObject.bulletclass.AttackPoint);
            }
        }
    }
    public float roll_decay;
    public float pitch_decay;
    public float roll_max;
    public float pitch_max;
    public float roll_growth;
    public float pitch_growth;
    
    
    void FixedUpdate()
    {

        int x = -(Input.GetKey(KeyCode.A) ? 1 : 0) + (Input.GetKey(KeyCode.D) ? 1 : 0);
        int y = -(Input.GetKey(KeyCode.S) ? 1 : 0) + (Input.GetKey(KeyCode.W) ? 1 : 0);
        var ang = body.transform.localEulerAngles;
        var ang_x = ang.x > 180 ? ang.x - 360 : ang.x;
        var ang_z = ang.z > 180 ? ang.z - 360 : ang.z;
        var roll = ang_z + roll_growth * -x;
        var pitch = ang_x + pitch_growth * -y;
        roll = roll * (1 - roll_decay * (1 - Mathf.Abs(x)));
        pitch = pitch * (1 - pitch_decay * (1 - Mathf.Abs(y)));
        roll = Mathf.Clamp(roll, -roll_max, roll_max);
        pitch = Mathf.Clamp(pitch, -pitch_max, pitch_max);
        body.transform.localEulerAngles = new Vector3(pitch, ang.y, roll);

    }
    
}
