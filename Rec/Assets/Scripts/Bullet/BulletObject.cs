using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弾のオブジェクト
/// </summary>
public class BulletObject : MonoBehaviour
{
    
    /// <summary>
    /// 弾の勢力（敵・味方など）を表現する
    /// </summary>
    public Force Force { get; private set; } 
    
    /// <summary>
    /// ぶつかったときにダメージがあるか
    /// </summary>
    public bool DealDamage { get; } 

    // Bulletクラス型の変数
    private Bullet bulletclass;

    /// <summary>
    /// 弾の特性を表すBulletインスタンスをセットする
    /// </summary>
    /// <param name="bullet"></param>
    public void SetBullet(Bullet bullet)
    {
        bulletclass = bullet;
    }

    private void OnTriggerEnter(Collider other)
    {

    }
    
    // Start is called before the first frame update
    void Start()
    {
        // bullet.csのStart(this.gameObject)を呼び出す
        bulletclass.Start(this.gameObject);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // bullet.csのFixedUpdate()を呼び出す
        bulletclass.FixedUpdate();
    }
}
