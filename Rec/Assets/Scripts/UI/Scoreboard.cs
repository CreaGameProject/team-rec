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
    /// スコアを代入した配列
    /// </summary>
    private int[] scorePoint;

    [Header("UIスコアテキスト")]
    [SerializeField] private Text[] scoreObjList;


    [Header("UIスコアゲージ等")] // B..Back  F..Front

    [SerializeField] private Image bulletGaugeB;
    [SerializeField] private Image bulletGaugeF;
    [SerializeField] private Image laserGaugeB;
    [SerializeField] private Image laserGaugeF;
    [SerializeField] private Image HPGaugeB;
    [SerializeField] private Image HPGaugeF;

    [Space(20)]
    [SerializeField] private CanvasGroup TotalCG;
    [SerializeField] private Text TotalScore;

    [Space(20)]
    [SerializeField] private GameObject NextButton;

    [Header("ゲージカラーデータ")]
    [SerializeField] private Color stage1Color;
    [SerializeField] private Color stage2Color;
    [SerializeField] private Color stage3Color;


    /// <summary>
    /// このオブジェクトが有効になったときに呼び出す
    /// </summary>
    private void OnEnable()
    {
        int stage = Score.Stage;
        gaugeObjListCount = 0;

        // UI素材をロードする
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

        StartCoroutine(PlayAnimation());
    }


    /// <summary>
    /// 演出用のアニメーション処理を行う
    /// </summary>
    /// <returns></returns>
    private IEnumerator PlayAnimation()
    {
        CanvasGroup cg = this.GetComponent<CanvasGroup>();
        cg.alpha = 0;
        float waitTime = 1.0f;

        // 透明度を下げる
        while (true)
        {
            cg.alpha += 0.01f;
            if (cg.alpha >= 1)
            {
                break;
            }

            yield return null;
        }

        // スコアデータを入力する
        yield return new WaitForSeconds(waitTime);
        Score.NormalKills = 4; // ここ４行はテストデータ(後で消す)
        Score.LazerKills = 2;
        Score.HPRemains = 100;
        Score.EnemyCounts = 5;
        SetScoreValue();
        StartCoroutine(IncreaseGauge(gaugeObjList[gaugeObjListCount], gaugeFillAmount[gaugeObjListCount]));
    }


    /// <summary>
    /// ゲージと数字を滑らかに増加させるメソッド
    /// </summary>
    /// <param name="obj">増加させる対象のオブジェクト</param>
    /// <param name="amount">ゲージ量</param>
    /// <returns></returns>
    private IEnumerator IncreaseGauge(GameObject obj, float amount)
    {
        int frameCount = 0;
        int maxFrame = 100;
        int nowScorePoint = 0;
        int maxScore = scorePoint[gaugeObjListCount];
        float nowAmount = 0.0f;

        GameObject child = obj.transform.GetChild(1).gameObject;
        Image img = child.GetComponent<Image>();

        while (true)
        {
            // ゲージ増加
            nowAmount += amount / maxFrame;

            // 数値増加
            nowScorePoint = (int)(maxScore / ((float)maxFrame / frameCount));
            scoreObjList[gaugeObjListCount].text = nowScorePoint.ToString();

            frameCount++;

            img.fillAmount = nowAmount;

            if (nowAmount >= amount)
            {
                scoreObjList[gaugeObjListCount].text = maxScore.ToString();
                gaugeObjListCount++;
                yield return new WaitForSeconds(1f);

                
                if (gaugeObjListCount < gaugeObjList.Length)
                {
                    StartCoroutine(IncreaseGauge(gaugeObjList[gaugeObjListCount], gaugeFillAmount[gaugeObjListCount]));
                }
                else
                {
                    // ３ゲージ表示した後の処理
                    StartCoroutine(ShowTotal());
                }
                yield break;
            }
            else
            {
                yield return null;
            }
        }
    }



    private IEnumerator ShowTotal()
    {
        // フェードイン
        TotalCG.alpha = 0;
        while (true)
        {
            TotalCG.alpha += 0.01f;
            Debug.Log(TotalCG.alpha);

            if (TotalCG.alpha >= 1)
            {
                yield return new WaitForSeconds(1f);
                break;
            }

            yield return null;
        }

        // スコア表示
        int total = 0;
        foreach(int score in scorePoint)
        {
            total += score;
        }

        int nowScore = 0;
        int nowFrame = 0;
        int maxFrame = 100;

        while (true)
        {
            nowFrame++;

            nowScore = (int)(total / ((float)maxFrame / nowFrame));
            TotalScore.text = nowScore.ToString();

            if (nowFrame >= maxFrame)
            {
                TotalScore.text = total.ToString();
                yield return new WaitForSeconds(1f);
                NextButton.SetActive(true);
            }

            yield return null;
        }
    }


    /// <summary>
    /// スコアの値をScore.csから取ってきて計算、代入する。
    /// </summary>
    private void SetScoreValue()
    {
        scorePoint = new int[]{ 
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
