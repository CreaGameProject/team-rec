using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Core.Enemy.Navigator
{
    public class NavigatorEditor
    {
        [MenuItem("GameObject/EnemyNavigator/LinearWay", priority = 21)] public static void LinearWay() => CreateWayPoint<LinearWay>();

        private static void CreateWayPoint<T>() where T: WayPointComponent
        {
            var wayPoint = new GameObject(typeof(T).Name);
            wayPoint.AddComponent<T>();
            wayPoint.transform.SetParent(UnityEditor.Selection.activeTransform);
        }
    }
}
