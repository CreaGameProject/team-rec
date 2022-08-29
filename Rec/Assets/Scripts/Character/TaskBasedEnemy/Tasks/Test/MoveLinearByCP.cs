using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.Enemy.TaskBased
{
    public class MoveLinearByCP: MoveByControlPoints
    {
        protected override Func<IEnumerable<Vector3>, float, Vector3> GenerateTrajectory()
        {
            Vector3 Fn(IEnumerable<Vector3> cps, float t)
            {
                var cpsArray = cps.ToArray();
                var dists = new float[cpsArray.Length - 1];
                var sumDists = 0.0f;
                for (int i = 0; i < dists.Length; i++)
                {
                    // 各区間の距離を測る
                    dists[i] = Vector3.Magnitude(cpsArray[i + 1] - cpsArray[i]);
                    sumDists += dists[i];
                }

                for (int i = 0; i < dists.Length; i++)
                {
                    dists[i] /= sumDists;
                }

                for (int i = 1; i < dists.Length; i++)
                {
                    dists[i] += dists[i - 1];
                }
                
                int idx;
                for (idx = 0; idx < dists.Length; idx++)
                {
                    if (t <= dists[idx])
                        break;
                }

                var m = t - dists[idx];
                var n = dists[idx + 1] - t;
        
                return (cpsArray[idx] * m + cpsArray[idx + 1] * n) / (m + n);
            }

            return Fn;
        }
    }
}