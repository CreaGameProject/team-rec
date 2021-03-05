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
    public Force Force{get; set;}
    
    /// <summary>
    /// ぶつかったときにダメージがあるか
    /// </summary>
    public bool DealDamage { get; }

    /// <summary>
    /// Bulletクラス型の変数
    /// </summary>
    [HideInInspector] public Bullet bulletclass;

    /// <summary>
    /// 弾の特性を表すBulletインスタンスをセットする
    /// </summary>
    /// <param name="bullet"></param>
    public void SetBullet(Bullet bullet)
    {
        bulletclass = bullet;

        StartCoroutine(TimeDestroy());

        // bullet.csのStart(this.gameObject)を呼び出す
        bulletclass.Start(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {

    }    

    // Update is called once per frame
    void FixedUpdate()
    {
        // bullet.csのFixedUpdate()を呼び出す
        bulletclass.FixedUpdate();
    }

    /// <summary>
    /// 時間経過で消える
    /// </summary>
    /// <returns></returns>
    private IEnumerator TimeDestroy()
    {
        yield return new WaitForSeconds(8);
        BulletPool.Instance.Destroy(this.gameObject);
    }
}
