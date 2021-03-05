using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonEnemyEvent : IStageEvent
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

    public SummonEnemyEvent(GameObject enemy, EnemyType enemyType, EnemyMove enemyMove, Vector3 position, float time)
    {
        _enemy = enemy;
        _enemyType = enemyType;
        _enemyMove = enemyMove;
        _position = position;
        _time = time;
    }

    public void Call()
    {
        GameObject enemyInstance = GameObject.Instantiate(_enemy, _position, Quaternion.identity);
        //enemyInstance.GetComponent<Enemy>().enemyType = _enemyType;　private setのため、できない
        //enemyInstance.GetComponent<Enemy>().enemyMove = _enemyMove;  privateのためできない
    }
}

