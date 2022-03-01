using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*
namespace Core.Enemy.TaskBased
{
    [AddComponentMenu("EnemyTask/MoveByControlPoints")]
    public class MoveByControlPoints : EnemyTaskComponent
    {
        [SerializeField] private float durationTime;
        [SerializeField] private List<Transform> controlPoints;
        [SerializeField] private TrajectoryCurve trajectoryCurve;
        [SerializeField] private VelocityCurve velocityCurve;


        public override IEnemyTask ToEnemyTask()
        {

            return new Task();
        }

        private Func<float, Vector3> GenerateTrajectoryCurve(TrajectoryCurve c)
        {
            Func<float, Vector3> fn;

            switch (c)
            {
            }

            return fn;
        }

        private Func<float, float> GenerateVelocityCurve(VelocityCurve c)
        {
            Func<float, float> fn;

            switch (c)
            {
                case VelocityCurve.Linear:
                    fn = (float x) => x;
                    break;
                case VelocityCurve.InCubic:
                    fn = (float x) => Mathf.Pow(x, 3);
                    break;
                case VelocityCurve.OutCubic:
                    fn = (float x) => 1 - Mathf.Pow(1 - x, 3);
                    break;
                case VelocityCurve.InOutCubic:
                    fn = (float x) => x < 0.5 ? 4 * Mathf.Pow(x, 3) : 1 - Mathf.Pow(-2 * x + 2, 3) / 2;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return fn;
        }

        private enum TrajectoryCurve
        {
            // 線形, エルミート, 2次ベジエ, 3次ベジエ, スプライン, Bスプライン
            Linear, Hermite, QuadraticBezier, CubicBezier, Spline, BSpline
        }

        private enum VelocityCurve
        {
            Linear, InCubic, OutCubic, InOutCubic
        }

        private class Task : IEnemyTask
        {
            // コントロールポイントリスト
            private float durationTime;
            private List<Vector3> controlPoints;

            private List<float> sectionTimes;

            public Task(float durationTime, IEnumerable<Vector3> controlPoints)
            {
                this.controlPoints = controlPoints.ToList();
                this.durationTime = durationTime;
            }

            public IEnumerator Call(TaskBasedEnemy enemy)
            {
                // var camera = GameObject.FindGameObjectWithTag("MainCamera");
                // enemy.transform.SetParent(camera.transform);
                // controlPoints.Insert(0, enemy.transform.localPosition);
                // var sqrDistances = controlPoints.Skip(1).Zip(controlPoints.Take(controlPoints.Count() - 1), (x, y) => (x - y).sqrMagnitude);
                // var scale = durationTime / sqrDistances.Sum();
                //
                // sectionTimes = sqrDistances.Select(x => x * scale).ToList();
                //
                //
                //
                // var pos = this.controlPoints[0];
                // enemy.transform.localPosition = pos;
                
                yield break;
            }
            
            public IEnemyTask Copy()
            {
                return new Task();
            }
        }
    }
}
*/