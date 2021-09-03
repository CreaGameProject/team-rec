using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Stage;
using UnityEngine;

/// <summary>
/// インスペクタなどから得た情報を用いてSummonTaskBasedEnemyEventオブジェクトを生成する。
/// </summary>
public class SummonTaskBasedEnemyMarker : StageEventMarker
{
    [SerializeField] private float time;
    [SerializeField] private GameObject original;
    [Header("if task holder is none, it refer to children.")] [SerializeField] private TaskHolder taskHolder;
    
    public override IStageEvent ToStageEvent()
    {
        var holder = taskHolder;
        if (holder == null)
        {
            holder = GetComponentInChildren<TaskHolder>();
        }

        var e = new SummonTaskBasedEnemyEvent(original, holder.CollectTasks(), transform.position, transform.rotation, time);
        
        return e;
    }
}
