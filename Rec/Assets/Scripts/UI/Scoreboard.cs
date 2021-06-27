using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scoreboard : MonoBehaviour
{
    [Header("オブジェクトデータ")]
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

    /// <summary>
    /// リザルトを表示しているステージ番号
    /// </summary>
    private int stage = 1; // 外からデータを引っ張ってくる


    [Header("UIスコアゲージ")] // B..Back  F..Front

    [SerializeField] private Image bulletGaugeB;
    [SerializeField] private Image bulletGaugeF;
    [SerializeField] private Image laserGaugeB;
    [SerializeField] private Image laserGaugeF;
    [SerializeField] private Image HPGaugeB;
    [SerializeField] private Image HPGaugeF;

    [Header("ゲージカラーデータ")]
    [SerializeField] private Color stage1Color;
    [SerializeField] private Color stage2Color;
    [SerializeField] private Color stage3Color;


    /// <summary>
    /// このオブジェクトが有効になったときに呼び出す
    /// </summary>
    private void OnEnable()
    {
        switch (stage)
        {
            case 1:
                bulletGaugeB.sprite = Resources.Load<Sprite>("Score/maru_1");
                bulletGaugeF.sprite = Resources.Load<Sprite>("Score/maru_1");
                laserGaugeB.sprite = Resources.Load<Sprite>("Score/maru_2");
                laserGaugeF.sprite = Resources.Load<Sprite>("Score/maru_2");
                HPGaugeB.sprite = Resources.Load<Sprite>("Score/maru_3");
                HPGaugeF.sprite = Resources.Load<Sprite>("Score/maru_3");
                bulletGaugeF.color = stage1Color;
                laserGaugeF.color = stage1Color;
                HPGaugeF.color = stage1Color;
                break;

            case 2:
                bulletGaugeB.sprite = Resources.Load<Sprite>("Score/3kaku_1");
                bulletGaugeF.sprite = Resources.Load<Sprite>("Score/3kaku_1");
                laserGaugeB.sprite = Resources.Load<Sprite>("Score/3kaku_2");
                laserGaugeF.sprite = Resources.Load<Sprite>("Score/3kaku_2");
                HPGaugeB.sprite = Resources.Load<Sprite>("Score/3kaku_3");
                HPGaugeF.sprite = Resources.Load<Sprite>("Score/3kaku_3");
                bulletGaugeF.color = stage2Color;
                laserGaugeF.color = stage2Color;
                HPGaugeF.color = stage2Color;
                break;

            case 3:
                bulletGaugeB.sprite = Resources.Load<Sprite>("Score/4kaku_1");
                bulletGaugeF.sprite = Resources.Load<Sprite>("Score/4kaku_1");
                laserGaugeB.sprite = Resources.Load<Sprite>("Score/4kaku_2");
                laserGaugeF.sprite = Resources.Load<Sprite>("Score/4kaku_2");
                HPGaugeB.sprite = Resources.Load<Sprite>("Score/4kaku_3");
                HPGaugeF.sprite = Resources.Load<Sprite>("Score/4kaku_3");
                bulletGaugeF.color = stage3Color;
                laserGaugeF.color = stage3Color;
                HPGaugeF.color = stage3Color;
                break;

            default:
                Debug.LogError("存在しないステージ番号が指定されています -> " + stage);
                break;
        }
    }


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

        // テストプレイ用、後で消す
        if (Input.GetKeyDown(KeyCode.R))
        {
            Score.NormalKills = 4;
            Score.LazerKills = 2;
            Score.HPRemains = 100;
            Score.EnemyCounts = 5;
            SetScoreValue(); // 本来ならスコア表示の瞬間に実行
        }
    }


    /// <summary>
    /// ゲージを滑らかに増加させるメソッド
    /// </summary>
    /// <param name="obj">増加させる対象のオブジェクト</param>
    /// <param name="amount">ゲージ量</param>
    /// <returns></returns>
    private IEnumerator IncreaseGauge(GameObject obj, float amount)
    {
        float nowAmount = 0.0f;
        float increaseRate = 0.01f;

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


    /// <summary>
    /// スコアの値をScore.csから取ってきて計算、代入する。
    /// </summary>
    private void SetScoreValue()
    {
        int[] scorePoint = { 
            Score.NormalKills,
            Score.LazerKills,
            (int)((float)Score.HPRemains / Player.MaxLife * 10000)
        };

        gaugeFillAmount[0] = (float)Score.NormalKills / Score.EnemyCounts;
        gaugeFillAmount[1] = (float)Score.LazerKills / Score.EnemyCounts;
        gaugeFillAmount[2] = (float)Score.HPRemains / Player.MaxLife;

        foreach (float a in gaugeFillAmount)
        {
            Debug.Log(a);
        }
    }
}
