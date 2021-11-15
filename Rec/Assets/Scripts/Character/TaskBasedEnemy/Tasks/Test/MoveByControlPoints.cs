using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

/*
namespace Core.Enemy.TaskBased
{
    [AddComponentMenu("EnemyTask/MoveByControlPoints")]
    public class MoveByControlPoints : EnemyTaskComponent
    {
        [SerializeField] private float durationTime;
        [SerializeField] private List<Transform> controlPoints;
        [SerializeField] private Camera camera;

        public override IEnemyTask ToEnemyTask()
        {

            return new Task();
        }

        // /// <summary>
        // /// 制御点をシリアライズするためのクラス
        // /// </summary>
        // [System.Serializable] private class ControlPoint
        // {
        //     public Transform position;
        //     public float coef;
        // }

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
                var camera = GameObject.FindGameObjectWithTag("MainCamera");
                enemy.transform.SetParent(camera.transform);
                controlPoints.Insert(0, enemy.transform.localPosition);
                var sqrDistances = controlPoints.Skip(1).Zip(controlPoints.Take(controlPoints.Count() - 1), (x, y) => (x - y).sqrMagnitude);
                var scale = durationTime / sqrDistances.Sum();
                
                sectionTimes = sqrDistances.Select(x => x * scale).ToList();



                var pos = this.controlPoints[0];
                enemy.transform.localPosition = pos;

                yield break;
            }

            private Vector3 MoveScheduler(float t)
            {
                var sectionIndex = sectionTimes.Aggregate((n, elem) => )
            }

            public IEnemyTask Copy()
            {
                return new Task();
            }
        }
    }
}
*/