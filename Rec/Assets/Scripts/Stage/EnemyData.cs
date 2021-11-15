using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : MonoBehaviour
{
    public GameObject enemyObj;
    //public EnemyType enemyType;
    public EnemyMove enemyMove;
    [System.NonSerialized] public Vector3 position;
    public float summonTime;

    private void Awake()
    {
        position = this.transform.position;
    }
}
