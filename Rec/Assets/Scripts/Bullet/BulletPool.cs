using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// BulletObjectをプーリングする
/// </summary>
public class BulletPool : SingletonMonoBehaviour<BulletPool>
{
    /// <summary>
    /// プールの元オブジェクト
    /// </summary>
    public GameObject defaultObject;
    
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
            newObj.transform.SetParent(transform);
            newObj.SetActive(false);
            _poolObjList.Add(newObj);
        }
    }

    /// <summary>
    /// BulletObjectインスタンスをプールから取り出す
    /// </summary>
    /// <param name="bullet">生成する弾のデータ</param>
    /// <returns></returns>
    public GameObject GetInstance(Bullet bullet) 
    {
        Debug.Log(_poolObjList);
        foreach (var obj in _poolObjList)
        {
            //Debug.Log("foreach");
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
        var newObj = Instantiate(defaultObject);
        newObj.name = defaultObject.name + (_poolObjList.Count + 1);

        return newObj;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        // 仮の値

        GameObject obj = defaultObject;

        int maxCount = 20;

        // オブジェクトプールを作成する
        CreateBulletPool(obj, maxCount);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
