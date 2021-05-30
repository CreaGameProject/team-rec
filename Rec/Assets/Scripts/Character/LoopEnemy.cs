using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵キャラクターの召喚後の挙動
/// </summary>
public enum EnemyMove{
    NONE,
    NORTH,
    NORTHEAST,
    EAST,
    SOUTHEAST,
    SOUTH,
    SOUTHWEST,
    WEST,
    NORTHWEST
}

public abstract class LoopEnemy : Enemy
{
     [Header("敵の移動に関する設定")]
    [SerializeField] EnemyMove enemyMove = EnemyMove.NONE;
    [SerializeField] float movePower;
    [SerializeField] float maxSpeed;
    
    [SerializeField] [Range(0.0f, 1.0f)] float dependency;
    [SerializeField] float stopLength;

// 敵の移動に関する変数
    Rigidbody rb;
    Transform naviTf;
    Vector3 forceDir;



    protected override void Awake()
    {
        base.Awake();

        rb = GetComponent<Rigidbody>();
        naviTf = playerTf.parent.transform;

        switch(enemyMove){
            case EnemyMove.NORTH:
            forceDir = Vector3.forward;
            break;
            case EnemyMove.NORTHEAST:
            forceDir = Vector3.forward + Vector3.right;
            break;
            case EnemyMove.EAST:
            forceDir = Vector3.right;
            break;
            case EnemyMove.SOUTHEAST:
            forceDir = -Vector3.forward + Vector3.right;
            break;
            case EnemyMove.SOUTH:
            forceDir = -Vector3.forward;
            break;
            case EnemyMove.SOUTHWEST:
            forceDir = -Vector3.forward - Vector3.right;
            break;
            case EnemyMove.WEST:
            forceDir = -Vector3.right;
            break;
            case EnemyMove.NORTHWEST:
            forceDir = Vector3.forward - Vector3.right;
            break;
        }
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        
        Move();

        if(rb.velocity.magnitude > maxSpeed){
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }

    protected virtual void Move(){
        float lengthToPlayer = (transform.position - playerTf.position).magnitude;
        if(lengthToPlayer > stopLength){
            forceDir = GetMoveDir(forceDir, dependency);
            rb.AddForce(forceDir * movePower);
        }
    }

    protected Vector3 GetMoveDir(Vector3 _enemyMoveDir, float _dependency){
        _enemyMoveDir = _enemyMoveDir.normalized;
        Vector3 dirToNavi = (naviTf.position - transform.position).normalized;
        Vector3 dir = (_enemyMoveDir * (1 - _dependency) + dirToNavi * _dependency).normalized;
        return dir;
    }

    protected abstract IEnumerator Fire();
}
