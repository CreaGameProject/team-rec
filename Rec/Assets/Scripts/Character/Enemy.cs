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

    protected Transform playerTf;

    [SerializeField] protected int hp;
    [SerializeField] protected int gaugePoint;

    /// <summary>
    /// このキャラクターにダメージを与える
    /// </summary>
    /// <param name="damage">与えるダメージ量</param>
    protected　virtual void Damage(int damage, BulletObject bulletObject)
    {
        // 「プレイヤーの弾」ー攻撃→「敵」
        hp -= damage;

        // Hpが0以下
        if (hp <= 0)
        {
            Kill(bulletObject);
        }
    }

    /// <summary>
    /// このキャラクターを消去する
    /// </summary>
    protected virtual void Kill(BulletObject bulletObject)
    {
        playerTf.gameObject.GetComponent<Player>().increaseLaserGauge(gaugePoint);

        // スコア加点
        if (bulletObject.bulletclass.Name == "Straight")
        {
            Score.NormalKills++;
        }
        else if (bulletObject.bulletclass.Name == "Homing")
        {
            Score.HomingKills++;
        }
        else
        {
            Debug.LogError("指定外の名前が選択されています。 -> " + bulletObject.bulletclass.Name);
        }

        Invoke("ThisDestroy", 3f);//3秒は仮の値
    }

    void ThisDestroy()
    {
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
