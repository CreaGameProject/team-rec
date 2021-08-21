using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverSystem : MonoBehaviour
{
    [SerializeField] private GameObject GameOverObj;
    public FrontGround frontGround;

    [Space(20)]
    [SerializeField] private float waitTime = 1.0f;
    [SerializeField] private int fadeTime;


    /// <summary>
    /// オブジェクトが有効化されたときに実行する
    /// </summary>
    private void OnEnable()
    {
        StartCoroutine(GameOver(60));
    }

    /// <summary>
    /// ゲームオーバー処理、フェードなど
    /// </summary>
    /// <param name="split">待機時間の分割数</param>
    /// <returns></returns>
    private IEnumerator GameOver(int split)
    {
        if (split <= 0)
        {
            Debug.LogError("1より小さい数は受け付けません。");
            yield break;
        }

        int count = 0;
        GameOverObj.SetActive(true);

        while (true)
        {
            count += 1;
            GameOverObj.GetComponent<CanvasGroup>().alpha = (float)count / split;

            if (count >= split)
            {
                yield break;
            }

            yield return null;
        }
    }


    /// <summary>
    /// 画面をフェードアウトしつつロードを行う
    /// </summary>
    /// <param name="split">待機時間の分割数</param>
    /// <returns></returns>
    public IEnumerator LoadScene(int split, string name)
    {
        if (split <= 0)
        {
            Debug.LogError("1より小さい数は受け付けません。");
            yield break;
        }

        int count = split;
        GameOverObj.SetActive(true);

        while (true)
        {
            count -= 1;
            GameOverObj.GetComponent<CanvasGroup>().alpha = (float)count / split;

            if (count <= 0)
            {
                if (name == "Retry")
                {
                    // ステージをリトライする
                    switch (Score.Stage)
                    {
                        case 1:
                            // テストシーン　後で変更する
                            Time.timeScale = 1.0f;
                            StartCoroutine(frontGround.FadeTransition(fadeTime, true, "TestStage YotoOda2"));
                            break;

                        case 2:
                            Time.timeScale = 1.0f;
                            // SceneManager.LoadScene("Stage2");
                            break;

                        case 3:
                            Time.timeScale = 1.0f;
                            // SceneManager.LoadScene("Stage3");
                            break;

                        default:
                            Debug.LogError("存在しないステージ番号を指定しています -> " + Score.Stage);
                            break;
                    }
                }
                else if (name == "Title")
                {
                    // タイトルをロードする
                    Time.timeScale = 1.0f;
                    WindowTransitionData.Transition = WindowTransition.Title;
                    StartCoroutine(frontGround.FadeTransition(fadeTime, true, "Title"));
                }
                else
                {
                    Debug.LogError("存在しないステージ名を指定しています");
                }
                yield break;
            }

            yield return null;
        }
    }
}
