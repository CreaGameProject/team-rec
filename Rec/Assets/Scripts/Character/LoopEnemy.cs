using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    /// <summary>
    /// 敵キャラクターの召喚後の挙動
    /// </summary>
public enum EnemyMove{
    None,
    North,
    Northeast,
    East,
    Southeast,
    South,
    Southwest,
    West,
    Northwest
}

public abstract class LoopEnemy : Enemy
{
    [Header("敵の移動に関する設定")]
    public EnemyMove enemyMove = EnemyMove.None;
    [SerializeField] float movePower;
    [SerializeField] float maxSpeed;

    [SerializeField] [Range(0.0f, 1.0f)] float dependency;
    [SerializeField] float stopLength;

    // 敵の移動に関する変数
    Rigidbody _rb;
    Transform _naviTf;
    Vector3 _forceDir;

    protected override void Kill(BulletObject bulletObject)
    {
        base.Kill(bulletObject);
    }

    protected override void Awake()
    {
        base.Awake();

        _rb = GetComponent<Rigidbody>();
        _naviTf = playerTf.parent.transform;

        switch(enemyMove){
            case EnemyMove.North:
                _forceDir = Vector3.forward;
                break;
            case EnemyMove.Northeast:
                _forceDir = Vector3.forward + Vector3.right;
                break;
            case EnemyMove.East:
                _forceDir = Vector3.right;
                break;
            case EnemyMove.Southeast:
                _forceDir = -Vector3.forward + Vector3.right;
                break;
            case EnemyMove.South:
                _forceDir = -Vector3.forward;
                break;
            case EnemyMove.Southwest:
                _forceDir = -Vector3.forward - Vector3.right;
                break;
            case EnemyMove.West:
                _forceDir = -Vector3.right;
                break;
            case EnemyMove.Northwest:
                _forceDir = Vector3.forward - Vector3.right;
                break;
        }
    }

    protected override void Update()
    {
        base.Update();
        transform.LookAt(playerTf.position);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    
        Move();

        if(_rb.velocity.magnitude > maxSpeed){
            _rb.velocity = _rb.velocity.normalized * maxSpeed;
        }
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            BulletObject bulletObject =  other.gameObject.GetComponent<BulletObject>();
            Force colForce = bulletObject.Force;
            if (colForce == Force.Player)
            {
                Debug.Log("攻撃力" + bulletObject.bulletclass.AttackPoint + "のPlayerの弾が当たりました");
                Damage(bulletObject.bulletclass.AttackPoint, bulletObject);
            }
        }
    }

    protected virtual void Move(){
        float lengthToPlayer = (transform.position - playerTf.position).magnitude;
        if(lengthToPlayer > stopLength){
            _forceDir = GetMoveDir(_forceDir, dependency);
            _rb.AddForce(_forceDir * movePower);
        }
    }

    protected Vector3 GetMoveDir(Vector3 enemyMoveDir, float dependency){
        enemyMoveDir = enemyMoveDir.normalized;
        Vector3 dirToNavi = (_naviTf.position - transform.position).normalized;
        Vector3 dir = (enemyMoveDir * (1 - dependency) + dirToNavi * dependency).normalized;
        return dir;
    }

    protected abstract IEnumerator Fire();
}
