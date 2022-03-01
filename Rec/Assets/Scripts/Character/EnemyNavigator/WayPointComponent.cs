using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Enemy.Navigator
{
    // 各制御点の基底クラス
    public abstract class WayPointComponent : MonoBehaviour
    {
        [SerializeField] protected float delay;
        [SerializeField] protected float duration;

        public virtual Vector3 EndPoint => transform.position;
        
        public abstract WayPoint ToWayPoint(Vector3 startPoint);
    }

    public abstract class WayPoint
    {
        protected WayPoint(float delay, float duration, Vector3 startPoint, Vector3 endPoint)
        {
            Delay = delay;
            Duration = duration;
            EndPoint = endPoint;
            StartPoint = startPoint;
        }

        public float Duration
        {
            get;
            protected set;
        }
        public float Delay
        {
            get;
            protected set;
        }

        public Vector3 StartPoint
        {
            get;
            protected set;
        }

        public Vector3 EndPoint
        {
            get;
            protected set;
        }

        public abstract Vector3 Interpolate(float t);
    }
}
