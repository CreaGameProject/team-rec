using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class LaserPool : SingletonMonoBehaviour<LaserPool>
{
    [SerializeField] private GameObject prefab;
    private Laser _laser;
    private Transform _root;

    private List<GameObject> _poolObjectsList;

    protected override void Awake()
    {
        base.Awake();

        GameObject obj = prefab;
        int count = 10;
        CreateLaserPool(obj,count);
    }

    private void CreateLaserPool(GameObject obj, int count)
    {
        var poolObj = obj;
        _poolObjectsList = new List<GameObject>();
        for (int i = 0; i < count; i++)
        {
            var newObj = CreateNewObject();
            newObj.transform.SetParent(transform);
            newObj.SetActive(false);
            _poolObjectsList.Add(newObj);
        }
    }

    public GameObject GetInstance()
    {
        var obj = GetUnusedLaser();
        obj.SetActive(true);
        //obj.GetComponent<BulletObject>().SetBullet(bullet);
        return obj;
    }

    private GameObject GetUnusedLaser() => _poolObjectsList.FirstOrDefault(t => t.activeSelf == false);

    private GameObject CreateNewObject()
    {
        var newObj = Instantiate(prefab);
        newObj.name = prefab.name + (_poolObjectsList.Count + 1);

        return newObj;
    }

    public void Destroy(GameObject obj)
    {
        if (obj.activeSelf) obj.SetActive(false);
    }
    
    public void ShotLaser(Vector3 launchPos, Vector3 endPos, float timer, float attack)
    {
        var laser = GetInstance();
        LaserObject laserObjectClass = laser.GetComponent<LaserObject>();
        laserObjectClass.ShotLaser(launchPos, endPos, timer, attack);
    }
    
    public void ShotLaser(Vector3 launchPos, Vector3 direction, float length,float timer, float attack)
    {
        var laser = GetInstance();
        LaserObject laserObjectClass = laser.GetComponent<LaserObject>();
        laserObjectClass.ShotLaser(launchPos, direction, length, timer, attack);
    }

    
}