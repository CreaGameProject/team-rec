using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BulletObjectをプーリングする
/// </summary>
public class BulletPool : SingletonMonoBehaviour<BulletPool>
{
    /// <summary>
    /// BulletObjectインスタンスをプールから取り出す
    /// </summary>
    /// <param name="bullet">生成する弾のデータ</param>
    /// <returns></returns>
    public GameObject GetInstance(Bullet bullet)
    {
        return null;
    }

    /// <summary>
    /// 使い終わった弾をプールに返す
    /// </summary>
    /// <param name="obj"></param>
    public void Destroy(GameObject obj)
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
