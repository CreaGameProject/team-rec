using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.Enemy.Navigator
{
    public class EnemyNavigator : MonoBehaviour
    {
        private List<WayPoint> wayPoints;

        public void SetPath(IEnumerable<WayPoint> wayPoints)
        {
            this.wayPoints = wayPoints.ToList();
        }

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(Navigate());
        }

        private IEnumerator Navigate()
        {
            yield return new WaitWhile(() => wayPoints == null);

            foreach (var wp in wayPoints)
            {
                yield return new WaitForSeconds(wp.Delay);

                for (float t = 0; t < wp.Duration; t += Time.deltaTime)
                {
                    transform.position = wp.Interpolate(t / wp.Duration);
                    yield return null;
                }

                transform.position = wp.Interpolate(1.0f);
            }
        }
    }
}

