using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverSystem : MonoBehaviour
{
    [SerializeField] private GameObject GameOverObj;

    [Space(20)]
    [SerializeField] private float waitTime = 1.0f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(GameOver(60));
        }
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

            yield return new WaitForSeconds(waitTime / split);
        }
    }
}
