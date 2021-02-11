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
    /// このキャラクターにダメージを与える
    /// </summary>
    /// <param name="damage">与えるダメージ量</param>
    private void Damage(int damage)
    {
        Hp -= damage;
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
        Destroy(this.gameObject);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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
