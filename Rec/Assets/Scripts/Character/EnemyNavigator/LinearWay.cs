using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.Enemy.Navigator
{
    // 直線移動
    public class LinearWay: WayPointComponent
    {
        public override WayPoint ToWayPoint(Vector3 startPoint)
        {
            return new Linear(delay, duration, startPoint, EndPoint);
        }

        class Linear : WayPoint
        {
            public Linear(float delay, float duration, Vector3 startPoint, Vector3 endPoint) : 
                base(delay, duration, startPoint, endPoint)
            {

            }

            public override Vector3 Interpolate(float t)
            {
                return (1 - t) * StartPoint + t * EndPoint;
            }
        }
    }
}
