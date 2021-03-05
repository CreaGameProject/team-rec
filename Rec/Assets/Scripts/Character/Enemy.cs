using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キャラクターの勢力を表す列挙子
/// </summary>
public enum Force
{
    None, Player, Enemy
}

/// <summary>
/// 敵の攻撃タイプを表す列挙子
/// </summary>
public enum EnemyType
{
    None, Normal, Homing
}

/// <summary>
/// 敵キャラクターの召喚後の挙動
/// </summary>
public enum EnemyMove
{
    None,
    Front,
    FrontToBack,
    Right,
    RightToFront,
    Left,
    LeftToFront,
    Back
}

/// <summary>
/// 敵キャラクターを表すクラス
/// </summary>
public class Enemy : MonoBehaviour
{
    /// <summary>
    /// ぶつかったときプレイヤーにダメージを与えるか
    /// </summary>
    public bool DealDamage { get; private set; }
    
    /// <summary>
    /// このキャラクターの勢力
    /// </summary>
    public Force Force { get; private set; }
    
    /// <summary>
    /// キャラクターのHP
    /// </summary>
    public int Hp { get; private set; }

    /// <summary>
    /// このキャラクターの攻撃タイプ
    /// </summary>
    public EnemyType enemyType = EnemyType.None;

    /// <summary>
    /// Playerの座標
    /// </summary>
    private Vector3 player_pos;

    /// <summary>
    /// StageNavigationの座標
    /// </summary>
    private Vector3 navi_pos;

    /// <summary>
    /// Playerの１フレーム前の座標（移動に使用）
    /// </summary>
    private Vector3 before_pos;

    /// <summary>
    /// このキャラクターが生成された時の座標
    /// </summary>
    private Vector3 origin_pos;

    /// <summary>
    /// このキャラクターのRigidbodyを所持
    /// </summary>
    private Rigidbody rig;

    /// <summary>
    /// 左右からの敵キャラ出現時、そこから移動する長さ
    /// </summary>
    private float randomMovement;

    /// <summary>
    /// キルされたときに増えるプレイヤーのゲージポイント
    /// </summary>
    private int gaugePoint = 50;

    [Header("他スクリプトの指定")]

    /// <summary>
    /// Playerのゲームオブジェクト
    /// </summary>
    public GameObject player;

    /// <summary>
    /// StageNavigation(Playerの親)オブジェクト
    /// </summary>
    public GameObject stageNavi;

    /// <summary>
    /// BulletPool型の変数
    /// </summary>
    public BulletPool bulletPool;

    [Header("最大HPの設定")]
 
    /// <summary>
    /// Normalの敵の最大Hp
    /// </summary>
    [SerializeField] int normalLife = 60;

    /// <summary>
    /// Homingの敵の最大Hp
    /// </summary>
    [SerializeField] int homingLife = 30;

    [Header("攻撃の頻度に関する設定")]

    /// <summary>
    /// Normalの敵の攻撃する速さ(回/s)
    /// </summary>
    [SerializeField] float normalRate = 1.5f;

    /// <summary>
    /// Homingの敵の攻撃する速さ(回/s)
    /// </summary>
    [SerializeField] float homingRate = 2.0f;

    /// <summary>
    /// [通常攻撃の敵専用]バーストの弾と弾の間隔(発/s)
    /// </summary>
    [SerializeField] float burstTime = 0.1f;

    /// <summary>
    /// [通常攻撃の敵専用]バースト時に放つ弾の個数
    /// </summary>
    [SerializeField] int burstcount = 3;

    [Header("敵の移動に関する設定")]

    /// <summary>
    /// 敵キャラクターの召喚後の挙動
    /// </summary>
    public EnemyMove enemyMove = EnemyMove.None;

    /// <summary>
    /// 敵キャラクターの移動力
    /// </summary>
    [SerializeField] float movePower = 10.0f;

    /// <summary>
    /// 敵キャラクターの最高移動速度
    /// </summary>
    [SerializeField] float maxSpeed = 30.0f;

    /// <summary>
    /// 敵キャラクターが加速を止めるプレイヤーと敵キャラクターの距離
    /// </summary>
    [SerializeField] float stopLength = 30.0f;

    /// <summary>
    /// 敵キャラクターのプレイヤーの移動に対する依存性
    /// </summary>
    [SerializeField] float dependency = 100.0f;

    /// <summary>
    /// このキャラクターにダメージを与える
    /// </summary>
    /// <param name="damage">与えるダメージ量</param>
    private void Damage(int damage)
    {
        // 「プレイヤーの弾」ー攻撃→「敵」
        Hp -= damage;

        // Hpが0以下
        if (Hp <= 0)
        {
            Kill();
        }
    }

    /// <summary>
    /// このキャラクターを消去する
    /// </summary>
    private void Kill()
    {
        gaugePoint = 50;
        player.GetComponent<Player>().increaseLaserGauge(gaugePoint);
        Destroy(this.gameObject);
    }

    /// <summary>
    /// 敵の移動を行う
    /// </summary>
    private void Move()
    {
        // 参考「パンツァードラグーン　ツヴァイ　セガサターン」

        // プレイヤーと同じ進行方向へ移動する。
        navi_pos = stageNavi.transform.position;
        player_pos = player.transform.position;
        Vector3 direction = (navi_pos - before_pos);
        this.transform.Translate(direction / 2);
        rig.AddForce(direction * Time.deltaTime * dependency);
        before_pos = stageNavi.transform.position;

        // 敵の挙動
        switch (enemyMove)
        {
            case EnemyMove.Front:
                if (Vector3.Distance(player_pos, this.transform.position) >= stopLength)
                {
                    rig.AddForce(new Vector3(0, 0, -1) * Time.deltaTime * movePower);
                }
                break;

            case EnemyMove.FrontToBack:
                // 切り返し（転回開始地点）z座標
                float reverseLine = -20;
                // 最終停止ライン z座標
                float stopLine = -10;
                if (this.transform.position.z - player_pos.z <= reverseLine)
                {
                    rig.AddForce(new Vector3(0, 0, 1) * Time.deltaTime * movePower);
                }
                else if ((rig.velocity.z <= 0) && (this.transform.position.z - player_pos.z > stopLine))
                {
                    rig.AddForce(new Vector3(0, 0, -1) * Time.deltaTime * movePower);
                }
                break;

            case EnemyMove.Right:
                if (Vector3.Distance(origin_pos, this.transform.position) <= randomMovement)
                {
                    rig.AddForce(new Vector3(-0.4f, 0, 1) * Time.deltaTime * movePower);
                }
                break;

            case EnemyMove.RightToFront:
                if (Vector3.Distance(origin_pos, this.transform.position) <= randomMovement * 6)
                {
                    rig.AddForce(new Vector3(-1, 0, 1) * Time.deltaTime * movePower);
                }
                break;

            case EnemyMove.Left:
                if (Vector3.Distance(origin_pos, this.transform.position) <= randomMovement)
                {
                    rig.AddForce(new Vector3(0.4f, 0, 1) * Time.deltaTime * movePower);
                }
                break;

            case EnemyMove.LeftToFront:
                if (Vector3.Distance(origin_pos, this.transform.position) <= randomMovement * 6)
                {
                    rig.AddForce(new Vector3(1, 0, 1) * Time.deltaTime * movePower);
                }
                break;

            case EnemyMove.Back:
                if (Vector3.Distance(player_pos, this.transform.position) >= stopLength)
                {
                    rig.AddForce(new Vector3(0, 0, 1) * Time.deltaTime * movePower);
                }
                break;

            default:
                break;
        }
    }

    /// <summary>
    /// 敵にプレイヤーの弾が当たったときに衝突判定を行う
    /// </summary>
    /// <param name="other">衝突した相手のゲームオブジェクト</param>
    private void OnTriggerEnter(Collider other)
    {
        // 対象がBulletObjectコンポーネントを持っているかを検知する
        if (other.gameObject.GetComponent<BulletObject>())
        {
            // 衝突した弾がプレイヤーの弾だった場合
            BulletObject bulletobject = other.gameObject.GetComponent<BulletObject>();
            if (bulletobject.Force == Force.Player)
            {
                int damage = bulletobject.bulletclass.AttackPoint;
                //int damage = 15;
                Damage(damage);
            }
        }        
    }

    /// <summary>
    /// キャラクターが攻撃を行い、またクールダウン処理を行う。
    /// </summary>
    /// <returns></returns>
    private IEnumerator Fire()
    {
        while (true)
        {            
            if (enemyType == EnemyType.Normal)
            {             
                for (var i = 0; i < burstcount; i++)
                {
                    Straight straight = new Straight();
                    straight.Velocity = 10f; // 仮の値
                    straight.Direction = -transform.forward; // 前方向
                    GameObject enemyBullet = bulletPool.GetInstance(straight);
                    enemyBullet.GetComponent<BulletObject>().Force = Force.Enemy;
                    enemyBullet.transform.position = this.transform.position;
                    yield return new WaitForSeconds(burstTime);
                }
                // クールダウン
                yield return new WaitForSeconds(normalRate);
            }
            else if (enemyType == EnemyType.Homing)
            {
                Homing homing = new Homing();
                homing.Velocity = 6f; // 仮の値
                homing.HomingStrength = 6f; // 仮の値
                homing.Direction = transform.up; // 上方向
                homing.Target = player;
                GameObject enemyBullet = bulletPool.GetInstance(homing);
                enemyBullet.GetComponent<BulletObject>().Force = Force.Enemy;
                enemyBullet.transform.position = this.transform.position;

                // クールダウン
                yield return new WaitForSeconds(homingRate);
            }
            else
            {
                yield return new WaitForSeconds(0.1f);
                Debug.Log("存在しない弾種が指定");
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // キャラクターの体力を設定する
        if (enemyType == EnemyType.Normal)
        {
            Hp = normalLife;
        }
        else if (enemyType == EnemyType.Homing)
        {
            Hp = homingLife;
        }

        // Rigidbodyを設定する
        rig = this.gameObject.GetComponent<Rigidbody>();

        // ランダム量指定
        randomMovement = Random.Range(2.0f, 6.0f);

        // キャラクターの初期位置を取得
        origin_pos = this.transform.position;

        // Playerの初期１フレーム前座標を指定
        before_pos = stageNavi.transform.position;

        StartCoroutine(Fire());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();

        // 後で消す
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 仮指定
            enemyType = EnemyType.Homing;
        }
    }
        
}

/// <summary>
/// 敵の行動を表現する
/// </summary>
public interface IEnemyTask
{
    /// <summary>
    /// 行動の呼び出し
    /// </summary>
    /// <param name="enemy">行動する敵キャラクター</param>
    void Call(Enemy enemy);
}
