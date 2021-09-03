using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// タスクホルダー(タスクコンポーネントをアタッチするためのオブジェクト)には必ずアタッチされる。
/// 同オブジェクトにアタッチされたタスクコンポーネントをとりまとめ、タスクのリストを生成する。
/// </summary>
public class TaskHolder : MonoBehaviour
{
    public List<IEnemyTask> CollectTasks()
    {
        var tasks = GetComponents<EnemyTaskComponent>().Select(x => x.ToEnemyTask()).ToList();
        tasks.Sort((a, b) => (int)(a.Time - b.Time));
        return tasks;
    }
}
