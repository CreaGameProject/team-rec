using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// task-based enemy を召喚するためのstage event
/// </summary>
public class SummonTaskBasedEnemyEvent : IStageEvent
{
    private GameObject original;
    private List<IEnemyTask> tasks;
    private Vector3 position;
    private Quaternion quaternion;
    private float time;
    
    public SummonTaskBasedEnemyEvent(GameObject enemyObject, IEnumerable<IEnemyTask> tasks, Vector3 position, Quaternion quaternion, float time)
    {
        this.original = enemyObject;
        this.tasks = tasks.ToList();
        this.position = position;
        this.quaternion = quaternion;
        this.time = time;
    }
    
    public void Call()
    {
        var clone = GameObject.Instantiate(this.original, this.position, this.quaternion);
        var component = clone.GetComponent<TaskBasedEnemy>();
        component.SetTasks(tasks);
    }

    public float Time
    {
        get
        {
            return time;
        }
        set
        {
            this.time = value;
        }
    }
}
