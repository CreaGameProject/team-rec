using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonLoopEnemyEvent : IStageEvent
{
    private GameObject _enemy;
    private Vector3 _position;
    private float _time;
    private EnemyType _enemyType;
    private EnemyMove _enemyMove;


    public float Time
    {
        get { return _time; }
        set { _time = value; }
    }

    public SummonLoopEnemyEvent(GameObject enemy, EnemyType enemyType, EnemyMove enemyMove, Vector3 position, float time)
    {
        _enemy = enemy;
        _enemyType = enemyType;
        _enemyMove = enemyMove;
        _position = position;
        Time = time;
    }

    public void Call()
    {
        // Instantiate
        GameObject enemyInstance = GameObject.Instantiate(_enemy, _position, Quaternion.identity);
        // Get component
        Enemy enemyClass = enemyInstance.GetComponent<Enemy>();
        // Set parameters
        enemyClass.enemyType = _enemyType;
        enemyClass.enemyMove = _enemyMove;

        // Debug.Log("Called");
    }
}

