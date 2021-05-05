using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scoreboard : MonoBehaviour
{
    /// <summary>
    /// ゲージを表示するオブジェクトのリスト(上から順)
    /// </summary>
    [SerializeField] private GameObject[] gaugeObjList;

    /// <summary>
    /// 現在のゲージ表示オブジェクトのリスト番号
    /// </summary>
    private int gaugeObjListCount;

    /// <summary>
    /// 各スコアのゲージ割合を格納したリスト
    /// </summary>
    [SerializeField] private float[] gaugeFillAmount;

    // Start is called before the first frame update
    void Start()
    {
        gaugeObjListCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(IncreaseGauge(gaugeObjList[gaugeObjListCount], gaugeFillAmount[gaugeObjListCount]));
        }
    }


    private IEnumerator IncreaseGauge(GameObject obj, float amount)
    {
        float nowAmount = 0.0f;
        float increaseRate = 0.005f;

        GameObject child = obj.transform.GetChild(1).gameObject;
        Image img = child.GetComponent<Image>();

        while (true)
        {
            nowAmount += increaseRate;

            img.fillAmount = nowAmount;

            if (nowAmount >= amount)
            {
                gaugeObjListCount++;
                yield return new WaitForSeconds(0.8f);

                if (gaugeObjListCount < gaugeObjList.Length)
                    StartCoroutine(IncreaseGauge(gaugeObjList[gaugeObjListCount], gaugeFillAmount[gaugeObjListCount]));
                yield break;
            }
            else
            {
                yield return null;
            }
        }
    }
}
