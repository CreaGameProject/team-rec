using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Stage;
using UnityEngine;

public class SummonEventMarker : StageEventMarker
{
    [SerializeField] private float time;
    [SerializeField] private GameObject enemyObj;
    [SerializeField] private EnemyType enemyType;
    [SerializeField] private EnemyMove enemyMove;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override IStageEvent ToStageEvent()
    {
        return new SummonEnemyEvent(enemyObj, enemyType, enemyMove, transform.position, time);
    }
}
