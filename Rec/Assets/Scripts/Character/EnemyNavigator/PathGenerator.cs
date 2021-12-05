using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.Enemy.Navigator
{
    /// <summary>
    /// 制御点マーカオブジェクトの親にアタッチする。
    /// このコンポーネントを通じて制御点リスト(パス)を取得する。
    /// </summary>
    public class PathGenerator : MonoBehaviour
    {
        public List<WayPoint> CollectWayPoints(Vector3 startPoint)
        {
            var wpList = new List<WayPoint>();
            foreach (var wpc in GetComponentsInChildren<WayPointComponent>())
            {
                wpList.Add(wpc.ToWayPoint(startPoint));
                startPoint = wpc.EndPoint;
            }

            return wpList;
        }
    }
}
