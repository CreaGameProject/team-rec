using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using NUnit.Framework.Constraints;
using UnityEngine;

namespace Core.Enemy.TaskBased
{
    /// <summary>
    /// キャラクターを直進させるタスク
    /// </summary>
    [AddComponentMenu("EnemyTask/MoveStraight")]
    public class MoveStraightTask : EnemyTaskComponent
    {
        enum Curve
        {
            Linear, InCubic, OutCubic, InOutCubic
        }
        
        [SerializeField, Label("移動時間")] private float durationTime;
        [SerializeField, Label("移動先座標")] private Vector3 targetPosition;
        [SerializeField, Label("移動先座標が相対指定")] private bool isTargetRelative = true;
        [SerializeField, Label("速度曲線の形状")] private Curve curve;
        [SerializeField, Label("速度曲線の係数(未実装)")] private float curveCoef;

        public override IEnemyTask ToEnemyTask()
        {
            Func<float, float> fn;
            switch (curve)
            {
                case Curve.Linear:
                    fn = (float x) => x;
                    break;
                case Curve.InCubic:
                    fn = (float x) => Mathf.Pow(x, 3);
                    break;
                case Curve.OutCubic:
                    fn = (float x) => 1 - Mathf.Pow(1 - x, 3);
                    break;
                case Curve.InOutCubic:
                    fn = (float x) => x < 0.5 ? 4 * Mathf.Pow(x, 3) : 1 - Mathf.Pow(-2 * x + 2, 3) / 2;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return new MoveStraight(durationTime, targetPosition, isTargetRelative, fn);
        }

        private class MoveStraight : IEnemyTask
        {
            private float durationTime;
            private Vector3 targetPosition;
            private bool isTargetRelative;
            private Func<float, float> fn;

            public MoveStraight(float durationTime, Vector3 targetPosition, bool isTargetRelative, Func<float, float> fn)
            {
                this.durationTime = durationTime;
                this.targetPosition = targetPosition;
                this.isTargetRelative = isTargetRelative;
                this.fn = fn;
            }

            public IEnumerator Call(TaskBasedEnemy enemy)
            {
                var transform = enemy.transform;
                float time = 0;
                Vector3 startPosition = transform.position;
                Vector3 diff = isTargetRelative ? targetPosition : targetPosition - startPosition;
                while (transform != null && time < durationTime)
                {
                    transform.position = fn(time / durationTime) * diff + startPosition;
                    time += UnityEngine.Time.deltaTime;
                    yield return null;
                }
            }

            public IEnemyTask Copy()
            {
                return new MoveStraight(durationTime, targetPosition, isTargetRelative, fn);
            }
        }
    }
}
