using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 弾のオブジェクト
/// </summary>
public class BulletObject : MonoBehaviour
{

    /// <summary>
    /// 弾の勢力（敵・見方など）を表現する
    /// </summary>
    public Force Force { get; private set; }
    
    /// <summary>
    /// ぶつかったときにダメージがあるか
    /// </summary>
    public bool DealDamage { get; }

    /// <summary>
    /// 弾の特性を表すBulletインスタンスをセットする
    /// </summary>
    /// <param name="bullet"></param>
    public void SetBullet(Bullet bullet)
    {

    }

    private void OnTriggerEnter(Collider other)
    {

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
