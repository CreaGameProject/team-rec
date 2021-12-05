using System.Collections;
using System.Collections.Generic;
using Core.Enemy.Navigator;
using Core.Stage;
using UnityEngine;
using UnityEditor;
using UnityEditor.PackageManager.UI;

namespace Core.Enemy.TaskBased
{
    public class TaskEditor
    {
        [MenuItem("GameObject/TaskBasedEnemy/EnemyMarker", priority = 21)]
        public static void EnemyMarker()
        {
            var marker = new GameObject("NewTaskBasedEnemyMarker");
            var holder = new GameObject("TaskHolder");
            var pathGenerator = new GameObject("PathGenerator");

            var markerComp = marker.AddComponent<SummonTaskBasedEnemyMarker>();
            var holderComp = holder.AddComponent<TaskHolder>();
            var pathGeneratorComp = pathGenerator.AddComponent<PathGenerator>();
            
            holder.transform.SetParent(marker.transform);
            pathGenerator.transform.SetParent(marker.transform);
        }

        [MenuItem("GameObject/TaskBasedEnemy/TaskHolder", priority = 21)]
        public static void TaskHolder()
        {
            var holder = new GameObject("TaskHolder");
            holder.AddComponent<TaskHolder>();
            holder.transform.SetParent(UnityEditor.Selection.activeTransform);
        }

        [MenuItem("Assets/Create/TaskBasedEnemy/NewTask")]
        public static void CreateNewEnemyTask()
        {
            var templatePath = "Assets/Editor/TemplateTask.txt";
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, "NewEnemyTask.cs");
        }
    }
}