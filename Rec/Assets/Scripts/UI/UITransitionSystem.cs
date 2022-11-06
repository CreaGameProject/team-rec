using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class UITransitionSystem : MonoBehaviour
{
    /// <summary>
    /// 選択されているステージ番号
    /// </summary>
    public static int Stage = 1;

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

    [Header("画面変化用画像")]
    [SerializeField] private Image prev_backImg;
    [SerializeField] private Image prev_frontImg;
    [SerializeField] private Image backImg;
    [SerializeField] private Image frontImg;

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
        SystemSoundManager.Instance.PlaySE("UI_Select");
        switch (gameObject.name)
        {
            case "Stage1":
                Stage = 1;
                break;
            case "Stage2":
                Stage = 2;
                break;
            case "Stage3":
                Stage = 3;
                break;
        }

        if (WindowTransitionData.Transition == WindowTransition.StageSelect || WindowTransitionData.Transition == WindowTransition.MainMenu)
            SwitchStageObj(Stage);

        if (WindowTransitionData.Transition == WindowTransition.StageSelect || WindowTransitionData.Transition == WindowTransition.GameOver)
        {
            FadeIn(this.gameObject, 0.5f);
        }
        else StartCoroutine(Fade(true));
    }


    /// <summary>
    /// カーソルをボタンの上から外したときの処理
    /// </summary>
    public void CursorOut()
    {
        if (WindowTransitionData.Transition == WindowTransition.StageSelect || WindowTransitionData.Transition == WindowTransition.GameOver)
        {
            FadeOut(this.gameObject, 0.5f);
        }
        else StartCoroutine(Fade(false));
    }


    /// <summary>
    /// ボタンをクリックしたときの処理
    /// </summary>
    public void OnClick()
    {
        // タイトル～
        if (WindowTransitionData.Transition == WindowTransition.Title)
        {
            SystemSoundManager.Instance.PlaySE("UI_Button");
            WindowTransitionData.Transition = WindowTransition.MainMenu;
            StartCoroutine(OpenWindow(MainMenuWindow, waitFrame));
            StartCoroutine(CloseWindow(TitleWindow, waitFrame));
        }

        // メインメニュー～
        else if (WindowTransitionData.Transition == WindowTransition.MainMenu)
        {
            SystemSoundManager.Instance.PlaySE("UI_Button");
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
                SystemSoundManager.Instance.PlaySE("UI_Button");
                // Stage1をロードする
                WindowTransitionData.Transition = WindowTransition.InGame;
                StartCoroutine(frontGround.FadeTransition(fadeTime, true, "TestStage YotoOda2"));
            }
            else if (name == "Stage2")
            {
                SystemSoundManager.Instance.PlaySE("UI_Button");
                // Stage2をロードする
                WindowTransitionData.Transition = WindowTransition.InGame;
            }
            else if (name == "Stage3")
            {
                SystemSoundManager.Instance.PlaySE("UI_Button");
                // Stage3をロードする
                WindowTransitionData.Transition = WindowTransition.InGame;
            }
            else if (name == "Back")
            {
                SystemSoundManager.Instance.PlaySE("UI_Cancel");
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
                SystemSoundManager.Instance.PlaySE("UI_Cancel");
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
                SystemSoundManager.Instance.PlaySE("UI_Button");
                // オプション画面を開く
                WindowTransitionData.Transition = WindowTransition.Option;
                WindowTransitionData._DefaultUIWindow = DefaultUIWindow.Pause;
                StartCoroutine(OpenWindow(OptionWindow, waitFrame));
                StartCoroutine(CloseWindow(PauseWindow, waitFrame));
            }
            else if (name == "Quit")
            {
                SystemSoundManager.Instance.PlaySE("UI_Cancel");
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
                SystemSoundManager.Instance.PlaySE("UI_Button");
                // ステージをリトライする（同じシーンをリロードする）
                StartCoroutine(gameOverSystem.LoadScene(60, "Retry"));
            }
            else if (name == "Quit")
            {
                SystemSoundManager.Instance.PlaySE("UI_Cancel");
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
    /// ステージを表す図形を変化させる
    /// </summary>
    private void SwitchStageObj(int stage)
    {
        if (WindowTransitionData.Transition == WindowTransition.StageSelect)
        {
            switch (stage)
            {
                case 1:
                    UIStageObj1.SetActive(true);
                    UIStageObj2.SetActive(false);
                    UIStageObj3.SetActive(false);
                    break;

                case 2:
                    UIStageObj1.SetActive(false);
                    UIStageObj2.SetActive(true);
                    UIStageObj3.SetActive(false);
                    break;

                case 3:
                    UIStageObj1.SetActive(false);
                    UIStageObj2.SetActive(false);
                    UIStageObj3.SetActive(true);
                    break;

                default:
                    Debug.LogError("範囲外のステージ番号が指定されています -> " + stage);
                    break;
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
    /// フェードイン処理を行う
    /// </summary>
    /// <param name="obj">対象となるゲームオブジェクト</param>
    /// <param name="time">変化にかかる時間</param>
    private void FadeIn(GameObject obj, float time)
    {
        GameObject child = obj.transform.GetChild(0).gameObject;
        Image img = child.GetComponent<Image>();
        img.DOColor(new Color(1, 1, 1, 1), time).SetEase(Ease.OutCirc).SetUpdate(true);
    }


    /// <summary>
    /// フェードアウト処理を行う
    /// </summary>
    /// <param name="obj">対象となるゲームオブジェクト</param>
    /// <param name="time">変化にかかる時間</param>
    private void FadeOut(GameObject obj, float time)
    {
        GameObject child = obj.transform.GetChild(0).gameObject;
        Image img = child.GetComponent<Image>();
        img.DOColor(new Color(1, 1, 1, 0), time).SetUpdate(true);
    }


    /// <summary>
    /// 背景の色が変化するときのフェード処理
    /// </summary>
    /// <param name="stage">ステージ番号</param>
    /// <returns></returns>
    private IEnumerator BackGroundFade(int stage)
    {
        int waitTime = 20;
        int nowTime = 0;

        prev_backImg.color = new Color(1, 1, 1, 1);
        prev_frontImg.color = new Color(1, 1, 1, 1);
        backImg.color = new Color(1, 1, 1, 0);
        frontImg.color = new Color(1, 1, 1, 0);

        while (true)
        {
            nowTime++;
            backImg.color = new Color(1, 1, 1, nowTime / (float)waitTime);
            frontImg.color = new Color(1, 1, 1, nowTime / (float)waitTime);

            if (nowTime >= waitTime)
            {
                switch (stage)
                {
                    case 1:
                        prev_backImg.sprite = Resources.Load<Sprite>("StageBackgrounds/maru_04");
                        prev_frontImg.sprite = Resources.Load<Sprite>("StageBackgrounds/maru_02");
                        break;

                    case 2:
                        prev_backImg.sprite = Resources.Load<Sprite>("StageBackgrounds/3kaku_4");
                        prev_frontImg.sprite = Resources.Load<Sprite>("StageBackgrounds/4kaku_3");
                        break;

                    case 3:
                        prev_backImg.sprite = Resources.Load<Sprite>("StageBackgrounds/4kaku_4");
                        prev_frontImg.sprite = Resources.Load<Sprite>("StageBackgrounds/4kaku_3");
                        break;

                    default:
                        print("eee");
                        break;
                }

                yield break;
            }

            yield return null;
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
            WindowTransitionData.Transition = WindowTransition.Title;
        }
    }


    private void OnEnable()
    {
        if (WindowTransitionData.Transition == WindowTransition.Title)
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            Stage = 1;
            SwitchStageObj(Stage);
        }
        else if (WindowTransitionData.Transition == WindowTransition.MainMenu)
        {
            SwitchStageObj(Stage);
        }
    }
}
