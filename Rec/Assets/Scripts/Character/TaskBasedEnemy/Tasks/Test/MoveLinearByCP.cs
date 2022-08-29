using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.Enemy.TaskBased
{
    // AddComponentからキャラクターにタスクをアタッチするためのコード   
    [AddComponentMenu("EnemyTask/MoveLinearByCP")]
    // AIデザインのためにコンポーネントとしてアタッチされるクラス
    public class MoveLinearByCP: MoveByControlPoints
    {
        protected override Func<IEnumerable<Vector3>, float, Vector3> GenerateTrajectory()
        {
            Vector3 Fn(IEnumerable<Vector3> cps, float t)
            {
                var cpsArray = cps.ToArray();
                var dists = new float[cpsArray.Length];
                
                // 各制御点の累積距離を測る。 dists[n] -> 0番目からn番目までの制御点の経路長。
                dists[0] = 0.0f;
                for (int i = 1; i < dists.Length; i++)
                {
                    dists[i] = Vector3.Magnitude(cpsArray[i] - cpsArray[i-1]) + dists[i-1];
                }

                // [0,1]にスケーリングし、tがどの制御点間にあるか求める。idx=n -> n-1番目とn番目の制御点の間
                var sumDists = dists[dists.Length - 1];
                int idx = dists.Length - 1;
                for (int i = dists.Length - 1; i > 0; i--)
                {
                    dists[i] /= sumDists;
                    if (t < dists[i])
                        idx = i;
                }

                // tが属する区間(idx)の端点となる制御点をtで内分した点をもとめて返す
                var m = t - dists[idx-1];
                var n = dists[idx] - t;

                // Debug.Log(idx);
                // Debug.Log("t " + t);
                // Debug.Log("cps[0]" + cpsArray[0]);
                // Debug.Log("cps[1]" + cpsArray[1]);
                // Debug.Log("0 " + dists[0]);
                // Debug.Log("1 " + dists[1]);
                // Debug.Log("2 " + dists[2]);
                // Debug.Log("3 " + dists[3]);
                // Debug.Log("m" + m);
                // Debug.Log("n" + n);
                return (cpsArray[idx] * m + cpsArray[idx-1] * n) / (m + n);
            }

            return Fn;
        }
    }
}