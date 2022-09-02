using System;
using UnityEngine;

namespace Core.Enemy.TaskBased
{
    public class ControlPointForMoveTasks: MonoBehaviour
    {
        public void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(transform.position, 0.3f);
        }
    }
}