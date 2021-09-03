using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using NUnit.Framework.Constraints;
using UnityEngine;

public class TestMoveTask : EnemyTaskComponent
{
    enum Curve
    {
        Linear, InCubic, OutCubic, InOutCubic
    }
    
    [SerializeField] private float startTime;
    [SerializeField] private float durationTime;
    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private bool isTargetRelative = true;
    [SerializeField] private Curve curve;
    [SerializeField] private float curveCoef;

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

        return new TestMove(startTime, durationTime, targetPosition, isTargetRelative, fn);
    }

    public class TestMove : IEnemyTask
    {
        public float Time => startTime;

        private float startTime;
        private float durationTime;
        private Vector3 targetPosition;
        private bool isTargetRelative;
        private Func<float, float> fn;

        public TestMove(float startTime, float durationTime, Vector3 targetPosition, bool isTargetRelative, Func<float, float> fn)
        {
            this.startTime = startTime;
            this.durationTime = durationTime;
            this.targetPosition = targetPosition;
            this.isTargetRelative = isTargetRelative;
            this.fn = fn;
        }

        private IEnumerator Run(Transform transform)
        {
            float time = 0;
            Vector3 startPosition = transform.position;
            Vector3 diff = isTargetRelative ? targetPosition : targetPosition - startPosition;
            while (transform != null && time < durationTime)
            {
                transform.position = fn(time / durationTime) * diff + startPosition;
                time += UnityEngine.Time.deltaTime;
                yield return null;
            }
            Debug.Log("move finished");
        }
        
        public void Call(TaskBasedEnemy enemy)
        {
            Debug.Log("called move");
            enemy.StartCoroutine(Run(enemy.transform));
        }

        public IEnemyTask Copy()
        {
            return new TestMove(startTime, durationTime, targetPosition, isTargetRelative, fn);
        }
    }
}
