using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UITransitionSystem : MonoBehaviour
{
    [Header("ウィンドウ一覧")]
    [SerializeField] private GameObject TitleWindow;
    [SerializeField] private GameObject MainMenuWindow;
    [SerializeField] private GameObject StageSelectWindow;
    [SerializeField] private GameObject OptionWindow;
    [SerializeField] private GameObject PauseWindow;

    [Header("UI用3Dゲームオブジェクト")]
    [SerializeField] private GameObject UIStageObj1;
    [SerializeField] private GameObject UIStageObj2;
    [SerializeField] private GameObject UIStageObj3;

    [Header("スクリプト類")]
    public GameOverSystem gameOverSystem;
    public FrontGround frontGround;

    [Space(20)]
    /// <summary>
    /// ウィンドウ表示時に待機するフレーム数
    /// </summary>
    [SerializeField] private int waitFrame = 60;

    /// <summary>
    /// シーン遷移時のフェード時間
    /// </summary>
    [SerializeField] private int fadeTime;


    /// <summary>
    /// カーソルをボタンの上に乗せたときの処理
    /// </summary>
    public void CursorOn()
    {
        // ステージ選択の時の特殊な場合
        string name = this.gameObject.name;
        if (name == "Stage1")
        {
            UIStageObj1.SetActive(true);
            UIStageObj2.SetActive(false);
            UIStageObj3.SetActive(false);
        }
        else if (name == "Stage2")
        {
            UIStageObj1.SetActive(false);
            UIStageObj2.SetActive(true);
            UIStageObj3.SetActive(false);
        }
        else if (name == "Stage3")
        {
            UIStageObj1.SetActive(false);
            UIStageObj2.SetActive(false);
            UIStageObj3.SetActive(true);
        }

        StartCoroutine(Fade(true));
    }


    /// <summary>
    /// カーソルをボタンの上から外したときの処理
    /// </summary>
    public void CursorOut()
    {
        StartCoroutine(Fade(false));
    }


    /// <summary>
    /// ボタンをクリックしたときの処理
    /// </summary>
    public void OnClick()
    {
        // タイトル～
        if (WindowTransitionData.Transition == WindowTransition.Title)
        {
            WindowTransitionData.Transition = WindowTransition.MainMenu;
            StartCoroutine(OpenWindow(MainMenuWindow, waitFrame));
            StartCoroutine(CloseWindow(TitleWindow, waitFrame));
        }

        // メインメニュー～
        else if (WindowTransitionData.Transition == WindowTransition.MainMenu)
        {
            string name = this.gameObject.name;
            Debug.Log(name);
            if (name == "NewGame")
            {
                // 新しいゲームを始める
                WindowTransitionData.Transition = WindowTransition.InGame;
                StartCoroutine(frontGround.FadeTransition(fadeTime, true, "TestStage YotoOda2"));
                //SceneManager.LoadScene("TestStage YotoOda2");
            }
            else if (name == "Continue")
            {
                // 前回のセーブデータ(クリアステージの次)から再開する
                //WindowTransitionData.Transition = WindowTransition.InGame;
                //StartCoroutine(frontGround.FadeTransition(60, true, "TestStage YotoOda2"));
            }
            else if (name == "StageSelect")
            {
                // ステージを好きに選ぶ
                WindowTransitionData.Transition = WindowTransition.StageSelect;
                StartCoroutine(OpenWindow(StageSelectWindow, waitFrame));
                StartCoroutine(CloseWindow(MainMenuWindow, waitFrame));
            }
            else if (name == "Option")
            {
                // オプション画面を開ける
                WindowTransitionData.Transition = WindowTransition.Option;
                WindowTransitionData._DefaultUIWindow = DefaultUIWindow.Main;
                StartCoroutine(OpenWindow(OptionWindow, waitFrame));
                StartCoroutine(CloseWindow(MainMenuWindow, waitFrame));
            }
            else
            {
                Debug.LogError("存在しない文字列を指定しています。");
            }
        }

        // ステージセレクト～
        else if (WindowTransitionData.Transition == WindowTransition.StageSelect)
        {
            string name = this.gameObject.name;
            if (name == "Stage1")
            {
                // Stage1をロードする
                WindowTransitionData.Transition = WindowTransition.InGame;
                StartCoroutine(frontGround.FadeTransition(fadeTime, true, "TestStage YotoOda2"));
            }
            else if (name == "Stage2")
            {
                // Stage2をロードする
                WindowTransitionData.Transition = WindowTransition.InGame;
            }
            else if (name == "Stage3")
            {
                // Stage3をロードする
                WindowTransitionData.Transition = WindowTransition.InGame;
            }
            else if (name == "Back")
            {
                // 1つ前の画面に戻る
                WindowTransitionData.Transition = WindowTransition.MainMenu;
                StartCoroutine(OpenWindow(MainMenuWindow, waitFrame));
                StartCoroutine(CloseWindow(StageSelectWindow, waitFrame));
            }
            else
            {
                Debug.LogError("存在しない文字列を指定しています。");
            }
        }

        // オプション～
        else if (WindowTransitionData.Transition == WindowTransition.Option)
        {
            string name = this.gameObject.name;
            if (name == "Back")
            {
                if (WindowTransitionData._DefaultUIWindow == DefaultUIWindow.Main)
                {
                    WindowTransitionData.Transition = WindowTransition.MainMenu;
                    StartCoroutine(OpenWindow(MainMenuWindow, waitFrame));
                }
                else if (WindowTransitionData._DefaultUIWindow == DefaultUIWindow.Pause)
                {
                    WindowTransitionData.Transition = WindowTransition.Pause;
                    StartCoroutine(OpenWindow(PauseWindow, waitFrame));
                }
                else
                {
                    Debug.LogError("デフォルトのUIウィンドウが正しくありません。 : " + WindowTransitionData._DefaultUIWindow );
                }
                StartCoroutine(CloseWindow(OptionWindow, waitFrame));
            }
            else
            {
                Debug.LogError("存在しない文字列を指定しています。");
            }
        }

        // ポーズ～
        else if (WindowTransitionData.Transition == WindowTransition.Pause)
        {
            string name = this.gameObject.name;
            if (name == "Option")
            {
                // オプション画面を開く
                WindowTransitionData.Transition = WindowTransition.Option;
                WindowTransitionData._DefaultUIWindow = DefaultUIWindow.Pause;
                StartCoroutine(OpenWindow(OptionWindow, waitFrame));
                StartCoroutine(CloseWindow(PauseWindow, waitFrame));
            }
            else if (name == "Quit")
            {
                // タイトルをロードする
                Time.timeScale = 1.0f;
                WindowTransitionData.Transition = WindowTransition.Title;
                StartCoroutine(frontGround.FadeTransition(fadeTime, true, "Title"));
            }
            else
            {
                Debug.LogError("存在しない文字列を指定しています。");
            }
        }

        // ゲームオーバー～
        else if (WindowTransitionData.Transition == WindowTransition.GameOver)
        {
            string name = this.gameObject.name;
            Debug.Log(name);
            if (name == "Retry")
            {
                // ステージをリトライする（同じシーンをリロードする）
                StartCoroutine(gameOverSystem.LoadScene(60, "Retry"));
            }
            else if (name == "Quit")
            {
                // タイトルをロードする
                StartCoroutine(gameOverSystem.LoadScene(60, "Title"));
            }
            else
            {
                Debug.LogError("存在しない文字列を指定しています。");
            }
        }
    }


    /// <summary>
    /// フェード処理を行う
    /// </summary>
    /// <param name="isFadeIn">フェードインであるかどうか</param>
    /// <returns></returns>
    private IEnumerator Fade(bool isFadeIn)
    {
        GameObject obj = this.transform.GetChild(1).gameObject;
        Text txt = obj.transform.GetChild(0).GetComponent<Text>();
        Image img;
        int waitFrame = 30;
        int count;
        if (isFadeIn)
            count = 0;
        else
            count = waitFrame;

        if (obj.GetComponent<Image>())
        {
            img = obj.GetComponent<Image>();
        }
        else
        {
            Debug.LogError("Imageがアタッチされていません");
            yield break;
        }

        while (true)
        {
            if (isFadeIn)
            {
                // フェードイン
                img.color = new Color(1, 1, 1, (float)count / waitFrame);
                txt.color = new Color(0, 0, 0, (float)count / waitFrame);
                count++;

                if (count > waitFrame)
                {
                    yield break;
                }
                else
                {
                    yield return null;
                }
            }
            else
            {
                // フェードアウト
                img.color = new Color(1, 1, 1, (float)count / waitFrame);
                txt.color = new Color(0, 0, 0, (float)count / waitFrame);
                count--;

                if (count < 0)
                {
                    yield break;
                }
                else
                {
                    yield return null;
                }
            }
        }
    }

    /// <summary>
    /// ウィンドウを表示する
    /// </summary>
    /// <param name="obj">対象となるウィンドウ</param>
    /// <param name="frame">表示にかかるフレーム数</param>
    /// <returns></returns>
    private IEnumerator OpenWindow(GameObject obj, int frame)
    {
        obj.SetActive(true);
        obj.transform.localScale = Vector3.zero;
        int nowFrame = 0;

        while (true)
        {
            obj.transform.localScale = Vector3.one * ((float)nowFrame / frame);

            if (nowFrame >= frame)
            {
                yield break;
            }
            else
            {
                nowFrame++;
            }
            yield return null;
        }
    }

    /// <summary>
    /// ウィンドウを閉じる
    /// </summary>
    /// <param name="obj">対象となるウィンドウ</param>
    /// <param name="frame">閉じるのにかかるフレーム数</param>
    /// <returns></returns>
    private IEnumerator CloseWindow(GameObject obj, int frame)
    {
        obj.transform.localScale = Vector3.one;
        int nowFrame = 0;

        while (true)
        {
            obj.transform.localScale = Vector3.one * (1 - ((float)nowFrame / frame));

            if (nowFrame >= frame)
            {
                obj.SetActive(false);
                yield break;
            }
            else
            {
                nowFrame++;
            }
            yield return null;
        }
    }


    private void Start()
    {
        if (WindowTransitionData.Transition == WindowTransition.None)
        {
            WindowTransitionData.Transition = WindowTransition.InGame;
        }
    }
}
