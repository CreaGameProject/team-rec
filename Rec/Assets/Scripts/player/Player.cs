using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


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
    /// レーザー弾の最大ゲージ量
    /// </summary>
    private const float maxLaserGauge = 200f;

    /// <summary>
    /// レーザー弾のゲージ量
    /// </summary>
    private float laserGauge;

    private float laserGaugeBuffer;

    private float angle;

    Camera mainCamera;
    Vector3 mousePos = new Vector3(0, 0, 0);
    private Image HP_img;
    private Image HP_img_red;
    private Image Laser_img;

    [SerializeField] Texture2D pointTexture;
    [SerializeField] private List<Enemy> lockedEnemyList = new List<Enemy>();


    // 各最大移動量
    float moveXMax = 1.5f;
    float moveYMin = 0.5f;
    float moveYMax = 1.6f;

    float homingRange = 1f;
    bool homingShot = false;

    [SerializeField] GameObject gameOverObj;
    [SerializeField] private GameObject pauseWindow;

    [Header("調整可")]
    /// <summary>
    /// 移動速度
    /// </summary>
    [SerializeField]
    private float speed;

    [ColorUsage(true, true), SerializeField]
    private Color _straightColor;

    [ColorUsage(true, true), SerializeField]
    private Color _homingColor;

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

    void Awake()
    {
        rigidbody = body.GetComponent<Rigidbody>();
        rigidbody.angularDrag = 20.0f;
    }

    // Start is called before the first frame update
    void Start()
    {
        Life = MaxLife;
        laserGauge = maxLaserGauge;
        angle = 1f / 180f * Mathf.PI;

        mainCamera = Camera.main;
        HP_img = GameObject.Find("Canvas/Gauge/HP/MaskDefault").GetComponent<Image>();
        HP_img_red = GameObject.Find("Canvas/Gauge/HP/MaskRed").GetComponent<Image>();
        Laser_img = GameObject.Find("Canvas/Gauge/Laser/Mask").GetComponent<Image>();
        pointTexture = ResizeTexture(pointTexture, 64, 64);
        Cursor.SetCursor(pointTexture, new Vector2(pointTexture.width / 2, pointTexture.height / 2),
            CursorMode.ForceSoftware);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isStopped)
        {
            GetControlInput();
        }

        getOtherInput();
        TargetRenderedCheck();
        //UnityEngine.PlayerLoop.FixedUpdate();
    }


    // ReSharper disable Unity.PerformanceAnalysis
    /// <summary>
    /// 移動・攻撃入力の管理
    /// </summary>
    private void GetControlInput()
    {
        // 移動入力
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(new Vector3(-0.1f * Mathf.Cos(transform.rotation.y * angle), 0,
                -0.1f * Mathf.Sin(transform.rotation.y * angle)) * (speed * Time.deltaTime));
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(new Vector3(0.1f * Mathf.Cos(transform.rotation.y * angle), 0,
                0.1f * Mathf.Sin(transform.rotation.y * angle)) * (speed * Time.deltaTime));
        }

        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(0, 0.1f * speed * Time.deltaTime, 0);
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(0, -0.1f * speed * Time.deltaTime, 0, 0);
        }

        var localPosition = transform.localPosition;
        localPosition = new Vector3(Mathf.Clamp(localPosition.x, -moveXMax, moveXMax)
            , Mathf.Clamp(localPosition.y, -moveYMin, moveYMax)
            , Mathf.Clamp(localPosition.z, -moveXMax, moveXMax));
        transform.localPosition = localPosition;

        // 攻撃入力
        if (Input.GetMouseButtonUp(0))
        {
            SendRay();
        }

        if (Input.GetMouseButtonDown(1))
        {
            laserGaugeBuffer = laserGauge;
        }

        if (Input.GetMouseButton(1))
        {
            homingRange += (100.0f) / 10.0f * Time.deltaTime;

            if (homingRange > 100)
            {
                homingRange = 100;
            }

            LockOnTarget();
        }

        if (Input.GetMouseButtonUp(1))
        {
            //Debug.Log("GetMouseButtonUp(1)");
            homingShot = true;
            homingRange = 0.0f;

            HomingShot();
            ReleaseTarget();
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
            decreaseLife(20);
        }
    }

    /// <summary>
    /// ストレート
    /// </summary>
    void SendRay()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 20f;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePosition);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * 100, Color.green, 5, false);

        if (Physics.Raycast(Camera.main.transform.position, (mousePos3D - Camera.main.transform.position), out hit,
            100f))
        {
            //Debug.Log(hit.collider.gameObject.name);
            //Debug.Log("衝突位置 : "+hit.point);

            //ここでストレートを打つ
            //hit.collider.gameObjectでぶつかったオブジェクトのことを指す
            Vector3 array = (hit.point - Camera.main.transform.position).normalized;

            Straight straight = new Straight();
            straight.Name = "Straight";
            straight.Velocity = 10f; // 仮の値
            straight.AttackPoint = 1; // 仮の値
            straight.Direction = array;
            GameObject bullet = bulletPool.GetInstance(straight);
            bullet.transform.position = this.transform.position;
            GameObject effect = bullet.transform.GetChild(0).gameObject;
            effect.GetComponent<Renderer>().material.SetColor(EmissionColor, _straightColor);
            bullet.GetComponent<BulletObject>().setForce(Force.Player);
        }
    }

    void LockOnTarget()
    {
        mousePos = Input.mousePosition;
        mousePos = mainCamera.ScreenToWorldPoint(mousePos);

        if (!(Camera.main is null))
        {
            RaycastHit[] hits = Physics.BoxCastAll(mousePos, Vector3.one * homingRange,
                (mousePos - Camera.main.transform.position), Quaternion.identity, 0f, LayerMask.GetMask("Default"));

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.gameObject.CompareTag("Enemy"))
                {
                    Enemy enemyClass = hit.collider.gameObject.GetComponent<Enemy>();
                    if (enemyClass.GetIsRendered() && enemyClass.GetHP() > 0)
                    {
                        if (!lockedEnemyList.Contains(enemyClass))
                        {
                            Debug.Log("Lockon: !lockedEnemyList.Contains(enemyClass)"); //TODO:ここに到達していない
                            if (laserGaugeBuffer >= 20)
                            {
                                laserGaugeBuffer -= 20;
                                lockedEnemyList.Add(enemyClass);
                                LockOnMarker.Instance.LockOnEnemy(enemyClass.gameObject);
                            }
                        }
                    }
                }
            }
        }
    }

    private void HomingShot()
    {
        List<Enemy> lockedEnemyListBuffer = lockedEnemyList;

        foreach (var enemyClass in lockedEnemyListBuffer)
        {
            Debug.Log("Enemy Name:" + enemyClass.gameObject.name);
            if (laserGauge >= 20)
            {
                laserGauge -= 20;
                Debug.Log("laserGauge:" + laserGauge);
                //ここでホーミングを打つ(つまり単発を高速レートで打つ感じ)
                //hit.collider.gameObjectでぶつかったオブジェクトのことを指す
                Homing homing = new Homing();
                homing.Name = "Homing";
                homing.Velocity = 5f; // 仮の値
                homing.HomingStrength = 12f;
                homing.AttackPoint = 1; // 仮の値
                homing.Direction = transform.forward;
                homing.Target = enemyClass.gameObject;
                GameObject bullet = bulletPool.GetInstance(homing);
                GameObject effect = bullet.transform.GetChild(0).gameObject;
                effect.GetComponent<Renderer>().material.SetColor(EmissionColor, _homingColor);
                bullet.GetComponent<BulletObject>().setForce(Force.Player);
                bullet.transform.position = this.transform.position;

                LockOnMarker.Instance.ReleaseCursor(enemyClass.gameObject);
                //lockedEnemyList.Remove(enemyClass);
            }
        }
        
        lockedEnemyList.Clear();
    }

    private void TargetRenderedCheck()
    {
        List<Enemy> lockedEnemyListBuffer = lockedEnemyList;
        foreach (var enemyClass in lockedEnemyListBuffer)
        {
            if (!(enemyClass.GetIsRendered() && enemyClass.GetHP() > 0))
            {
                lockedEnemyList.Remove(enemyClass);
                LockOnMarker.Instance.ReleaseCursor(enemyClass.gameObject);
            }
        }
    }

    private void ReleaseTarget()
    {
        lockedEnemyList = new List<Enemy>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(mousePos, Vector3.one * homingRange);
    }

    public void increaseLaserGauge(float num)
    {
        laserGauge += num;
        Laser_img.DOFillAmount(laserGauge / maxLaserGauge, 0.2f).SetEase(Ease.OutCirc).SetUpdate(true);
    }

    public void decreaseLife(int damagePoint)
    {
        Life -= damagePoint;
        HP_img.DOFillAmount((float)Life / MaxLife, 0.2f).SetEase(Ease.OutCirc).SetUpdate(true);
        HP_img_red.DOFillAmount((float)Life / MaxLife, 0.6f).SetEase(Ease.InSine).SetUpdate(true);

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
        CanvasGroup cg = GameObject.Find("Canvas/Gauge").GetComponent<CanvasGroup>();
        cg.DOFade(0, 2f).SetUpdate(true);

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
        else if (other.gameObject.GetComponent<LadybugsBullet>())
        {
            var ladybugsBullet = other.GetComponent<LadybugsBullet>();
            ladybugsBullet.Explosion();
            decreaseLife(LadybugsBullet.AttackPoint);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<LaserObject>())
        {
            LaserObject laserObject = other.gameObject.GetComponent<LaserObject>();
            if (!laserObject.canDealDamage) return;
            Debug.Log("HIT");

            //LaserPool.Instance.Destroy(other.gameObject);
            decreaseLife((int)laserObject.attackPoint);
            laserObject.isHit = true;
            laserObject.canDealDamage = false;
        }
    }

    public float roll_decay;
    public float pitch_decay;
    public float roll_max;
    public float pitch_max;
    public float roll_growth;
    public float pitch_growth;
    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");


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


    static Texture2D ResizeTexture(Texture2D srcTexture, int newWidth, int newHeight)
    {
        var resizedTexture = new Texture2D(newWidth, newHeight);
        Graphics.ConvertTexture(srcTexture, resizedTexture);
        return resizedTexture;
    }
}