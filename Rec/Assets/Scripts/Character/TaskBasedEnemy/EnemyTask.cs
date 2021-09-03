using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public interface IEnemyTask
{
    float Time { get; }
    void Call(TaskBasedEnemy enemy);
    IEnemyTask Copy();
}

public abstract class EnemyTaskComponent : MonoBehaviour
{
    public abstract IEnemyTask ToEnemyTask();
}

public static class TaskUtils
{
    public static List<IEnemyTask> CopyTasks(this IEnumerable<IEnemyTask> originalTasks)
    {
        return originalTasks.Select(x=>x.Copy()).ToList();
    }
}
