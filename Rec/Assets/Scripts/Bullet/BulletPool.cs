using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BulletObjectをプーリングする
/// </summary>
public class BulletPool : SingletonMonoBehaviour<BulletPool>
{
    /// <summary>
    /// プールに使う弾のリスト
    /// </summary>
    private List<GameObject> _poolObjList;

    /// <summary>
    /// プールの中にあるゲームオブジェクトの元
    /// </summary>
    private GameObject _poolObj;

    /// <summary>
    /// オブジェクトプールの作成
    /// </summary>
    private void CreateBulletPool(GameObject obj, int maxCount)
    {
        _poolObj = obj;
        _poolObjList = new List<GameObject>();
        // maxCount個のオブジェクトを作る
        for (int i = 0; i < maxCount; i++)
        {
            var newObj = CreateNewObject();
            newObj.SetActive(false);
            _poolObjList.Add(newObj);
        }
    }

    // Bulletの引数にHomingかStraightかを判断できるものを入れておく

    /// <summary>
    /// BulletObjectインスタンスをプールから取り出す
    /// </summary>
    /// <param name="bullet">生成する弾のデータ</param>
    /// <returns></returns>
    public GameObject GetInstance(Bullet bullet) 
    {
        foreach (var obj in _poolObjList)
        {
            if (obj.activeSelf == false)
            {
                obj.SetActive(true);
                obj.GetComponent<BulletObject>().SetBullet(bullet);
                return obj;
            }
        }

        // プール内のオブジェクトが全て使用中だった場合、新しくプールに生成する。
        var newObj = CreateNewObject();
        newObj.SetActive(true);
        newObj.GetComponent<BulletObject>().SetBullet(bullet);
        _poolObjList.Add(newObj);

        return newObj;
    }

    /// <summary>
    /// 使い終わった弾をプールに返す
    /// </summary>
    /// <param name="obj"></param>
    public void Destroy(GameObject obj)
    {
        if (obj.activeSelf == true)
        {
            obj.SetActive(false);
        }
    }

    /// <summary>
    /// 新しいオブジェクトを生成する
    /// </summary>
    private GameObject CreateNewObject() 
    {
        var newObj = Instantiate(_poolObj);
        newObj.name = _poolObj.name + (_poolObjList.Count + 1);

        return newObj;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        // 仮の値
        GameObject obj = null;
        int maxCount = 20;

        // オブジェクトプールを作成する
        CreateBulletPool(obj, maxCount);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
