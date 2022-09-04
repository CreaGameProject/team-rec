using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Enemy.TaskBased
{
    public class MoveHermiteByCP: MoveByControlPoints
    {
        protected override Func<IEnumerable<Vector3>, float, Vector3> GenerateTrajectory()
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<Vector3> ControlPoints { get; }
        
        public override Transform TargetPoint { get; }
    }
}

