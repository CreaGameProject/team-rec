using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class LockOnMarker : SingletonMonoBehaviour<LockOnMarker>
{
    private GameObject canvas;
    [SerializeField] private GameObject cursor;
    private GameObject[] cursors;
    private GameObject[] enemies;
    [SerializeField] private Player playerScript;
    private int maxCursor = 20;

    private void Awake()
    {
        base.Awake();
        cursors = new GameObject[maxCursor];
        enemies = new GameObject[maxCursor];
    }

    private void Start()
    {
        Player player = playerScript.GetComponent<Player>();
        canvas = transform.root.gameObject;
        for (int i = 0; i < cursors.Length; i++)
        {
            cursors[i] = Instantiate(cursor, canvas.transform, false);
            ;
            cursors[i].gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        for (int i = 0; i < cursors.Length; i++)
        {
            if (cursors[i].activeSelf && enemies != null)
            {
                if (enemies[i] != null)
                {
                    Vector2 position =
                        RectTransformUtility.WorldToScreenPoint(Camera.main, enemies[i].transform.position);
                    cursors[i].transform.position = new Vector3(position.x, position.y, 0f);
                }
            }
        }
            
        
    }

    private GameObject GetUnusedCursor() => cursors.FirstOrDefault(t => t.activeSelf == false);

    private int GetUnusedEnemyArrayIndex() => enemies.Select((p, i) => new {Name = p, Index = i}).First(p => false).Index;

    public void LockOnEnemy(GameObject enemyObj)
    {
        GameObject c = GetUnusedCursor();
        c.gameObject.SetActive(true);
        int index = Array.IndexOf(cursors, c);
        enemies[index] = enemyObj;
        
        
        Vector2 position = RectTransformUtility.WorldToScreenPoint(Camera.main, enemyObj.transform.position);
        c.transform.position = new Vector3(position.x, position.y, 0f);
    }

    public void ReleaseCursor(GameObject enemyObj)
    {
        int index = Array.IndexOf(enemies, enemyObj);
        enemies[index] = null;
        cursors[index].gameObject.SetActive(false);
    }
}