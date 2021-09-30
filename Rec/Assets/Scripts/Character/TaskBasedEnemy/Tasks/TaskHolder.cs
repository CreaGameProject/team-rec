using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.Enemy.TaskBased
{
    /// <summary>
    /// タスクホルダー(タスクコンポーネントをアタッチするためのオブジェクト)には必ずアタッチされる。
    /// 同オブジェクトにアタッチされたタスクコンポーネントをとりまとめ、タスクのリストを生成する。
    /// </summary>
    public class TaskHolder : MonoBehaviour
    {
        public List<IEnemyTask> CollectTasks()
        {
            return GetComponents<EnemyTaskComponent>().Select(x => x.ToEnemyTask()).ToList();
        }
    }
}
