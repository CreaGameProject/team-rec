using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.Enemy.Navigator
{
    public class PathGenerator : MonoBehaviour
    {
        public List<WayPoint> CollectWayPoints(Vector3 startPoint)
        {
            var wpList = new List<WayPoint>();
            foreach (var wpc in GetComponentsInChildren<WayPointComponent>())
            {
                wpList.Add(wpc.ToWayPoint(startPoint));
                startPoint = wpc.EndPoint;
            }

            return wpList;
        }
    }
}
