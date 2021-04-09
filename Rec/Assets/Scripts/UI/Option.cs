using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Option : MonoBehaviour
{

    // クリックできるオブジェクトそれぞれ
    [SerializeField] private GameObject HitMove1;
    [SerializeField] private GameObject HitMove2;
    [SerializeField] private GameObject HitShot1;
    [SerializeField] private GameObject HitShot2;
    [SerializeField] private GameObject HitLaser1;
    [SerializeField] private GameObject HitLaser2;

    [Space(20)]
    // 実際に移動するオブジェクト
    [SerializeField] private GameObject MoveType;
    [SerializeField] private GameObject Shot;
    [SerializeField] private GameObject Laser;

    // 移動にかかるフレーム数
    [SerializeField] private int waitFrame = 30;

    /// <summary>
    /// クリックしたときの処理
    /// </summary>
    public void OnClick()
    {
        string name = this.gameObject.name;
        if (name == "HitMove1")
        {
            StartCoroutine(MoveEasing(MoveType, MoveType.transform.position, HitMove1.transform.position, waitFrame));
        }
        else if (name == "HitMove2")
        {
            StartCoroutine(MoveEasing(MoveType, MoveType.transform.position, HitMove2.transform.position, waitFrame));
        }
        else if (name == "HitShot1")
        {
            StartCoroutine(MoveEasing(Shot, Shot.transform.position, HitShot1.transform.position, waitFrame));
            StartCoroutine(MoveEasing(Laser, Laser.transform.position, HitLaser2.transform.position, waitFrame));
        }
        else if (name == "HitShot2")
        {
            StartCoroutine(MoveEasing(Shot, Shot.transform.position, HitShot2.transform.position, waitFrame));
            StartCoroutine(MoveEasing(Laser, Laser.transform.position, HitLaser1.transform.position, waitFrame));
        }
        else if (name == "HitLaser1")
        {
            StartCoroutine(MoveEasing(Shot, Shot.transform.position, HitShot2.transform.position, waitFrame));
            StartCoroutine(MoveEasing(Laser, Laser.transform.position, HitLaser1.transform.position, waitFrame));
        }
        else if (name == "HitLaser2")
        {
            StartCoroutine(MoveEasing(Shot, Shot.transform.position, HitShot1.transform.position, waitFrame));
            StartCoroutine(MoveEasing(Laser, Laser.transform.position, HitLaser2.transform.position, waitFrame));
        }
        else
        {
            Debug.LogError("存在しない文字列が指定されています。");
        }
    }


    /// <summary>
    /// 移動を目的地まで滑らかに移動させるメソッド
    /// </summary>
    /// <param name="obj">対象のオブジェクト</param>
    /// <param name="from">開始地点</param>
    /// <param name="to">目的地</param>
    /// <param name="frame">掛けるフレーム数</param>
    /// <returns></returns>
    private IEnumerator MoveEasing(GameObject obj, Vector3 from, Vector3 to, int frame)
    {
        Vector3 dir = to - from;
        int nowframe = 0;

        while (true)
        {
            obj.transform.position = from + dir * ((float)nowframe / frame);

            if(nowframe >= frame)
            {
                print("break");
                yield break;
            }
            else
            {
                nowframe++;
            }
            yield return null;
        }
    }
}
