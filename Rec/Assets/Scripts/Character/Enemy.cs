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

public class Enemy : MonoBehaviour
{
    /// <summary>
    /// ぶつかったときプレイヤーにダメージを与えるか
    /// </summary>
    public bool dealDamage { get; private set; }//ぶつかったらダメージを与えるそうです
    
    /// <summary>
    /// このキャラクターの勢力
    /// </summary>
    public Force force { get; private set; }
    
    public int hp { get; protected set; }

    protected Transform playerTf;

    private int gaugePoint = 50;

    
    [System.NonSerialized] int lifePoint = 60;
    

    /// <summary>
    /// このキャラクターにダメージを与える
    /// </summary>
    /// <param name="damage">与えるダメージ量</param>
    protected　virtual void Damage(int damage)
    {
        // 「プレイヤーの弾」ー攻撃→「敵」
        hp -= damage;

        // Hpが0以下
        if (hp <= 0)
        {
            Kill();
        }
    }

    /// <summary>
    /// このキャラクターを消去する
    /// </summary>
    protected virtual void Kill()
    {
        gaugePoint = 50;
        playerTf.gameObject.GetComponent<Player>().increaseLaserGauge(gaugePoint);
        Destroy(this.gameObject);
    }

    protected virtual void Awake(){
        playerTf = GameObject.FindGameObjectWithTag("Player").transform;
    }

    

    protected virtual void Update(){

    }

    // Update is called once per frame
    protected virtual void FixedUpdate()
    {

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
