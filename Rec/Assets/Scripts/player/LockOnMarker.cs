using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using DG.Tweening;

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
        GameObject cursorObj = GetUnusedCursor();
        cursorObj.gameObject.SetActive(true);
        
        cursorObj.gameObject.transform.DOScale(1, 0.3f);
        Image cImage = cursorObj.GetComponent<Image>();
        var c = cImage.color;
        c.a = 0.0f;
        cImage.color = c;
        DOTween.ToAlpha(
            ()=> cImage.color,
            color => cImage.color = color,
            1.0f, // 目標値
            0.3f // 所要時間
        );
        
        int index = Array.IndexOf(cursors, cursorObj);
        enemies[index] = enemyObj;

        Vector2 position = RectTransformUtility.WorldToScreenPoint(Camera.main, enemyObj.transform.position);
        cursorObj.transform.position = new Vector3(position.x, position.y, 0f);
    }

    public void ReleaseCursor(GameObject enemyObj)
    {
        int index = Array.IndexOf(enemies, enemyObj);
        enemies[index] = null;
        cursors[index].gameObject.SetActive(false);
        cursors[index].gameObject.transform.localScale *= 5;
    }
}