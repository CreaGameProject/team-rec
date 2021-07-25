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
    public Force Force;
    
    /// <summary>
    /// ぶつかったときにダメージがあるか
    /// </summary>
    public bool DealDamage { get; }

    /// <summary>
    /// Bulletクラス型の変数
    /// </summary>
    public Bullet bulletclass;

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
        yield return new WaitForSeconds(6);
        BulletPool.Instance.Destroy(this.gameObject);
    }

    /// <summary>
    /// 無効化された時の処理
    /// </summary>
    private void OnDisable()
    {
        Rigidbody rig = this.GetComponent<Rigidbody>();
        rig.velocity = Vector3.zero;
        rig.angularVelocity = Vector3.zero;
    }
}
