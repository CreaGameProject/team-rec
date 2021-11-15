using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FrontGround : MonoBehaviour
{

    [SerializeField] private int fadeTime;

    /// <summary>
    /// シーン遷移時のフェード処理
    /// </summary>
    /// <param name="frame">フェードにかけるフレーム数</param>
    /// <returns></returns>
    public IEnumerator FadeTransition(int frame, bool isfadeIn, string sceneName = null)
    {
        if (frame <= 0)
        {
            Debug.Log("1以上の値を指定してください");
            yield break;
        }

        if (GetComponent<SpriteRenderer>())
        {
            SpriteRenderer sp = this.GetComponent<SpriteRenderer>();

            if (isfadeIn)
            {
                // フェードイン時の処理（＋シーン読み込み）
                int nowframe = 0;
                sp.material.SetColor("_BaseColor", new Color(0, 0, 0, (float)nowframe / frame));

                while (true)
                {
                    nowframe++;
                    sp.material.SetColor("_BaseColor", new Color(0, 0, 0, (float)nowframe / frame));

                    if (nowframe >= frame)
                    {
                        SceneManager.LoadScene(sceneName);
                        yield break;
                    }
                    yield return null;
                }
            }
            else
            {
                // フェードアウト時の処理
                int nowframe = frame;
                sp.material.SetColor("_BaseColor", new Color(0, 0, 0, (float)nowframe / frame));

                while (true)
                {
                    nowframe--;
                    sp.material.SetColor("_BaseColor", new Color(0, 0, 0, (float)nowframe / frame));

                    if (nowframe <= 0)
                    {
                        yield break;
                    }
                    yield return null;
                }
            }
        }

        else if (GetComponent<Image>())
        {
            Image img = GetComponent<Image>();

            if (isfadeIn)
            {
                // フェードイン時の処理（＋シーン読み込み）
                int nowframe = 0;
                img.color = new Color(0, 0, 0, (float)nowframe / frame);

                while (true)
                {
                    nowframe++;
                    img.color = new Color(0, 0, 0, (float)nowframe / frame);

                    if (nowframe >= frame)
                    {
                        SceneManager.LoadScene(sceneName);
                        yield break;
                    }
                    yield return null;
                }
            }
            else
            {
                // フェードアウト時の処理
                int nowframe = frame;
                img.color = new Color(0, 0, 0, (float)nowframe / frame);

                while (true)
                {
                    nowframe--;
                    img.color = new Color(0, 0, 0, (float)nowframe / frame);

                    if (nowframe <= 0)
                    {
                        yield break;
                    }
                    yield return null;
                }
            }
        }
        else
        {
            yield break;
        }
    }


    private void Start()
    {
        // フェードアウト処理を一度だけ行う
        StartCoroutine(FadeTransition(fadeTime, false));
    }
}
