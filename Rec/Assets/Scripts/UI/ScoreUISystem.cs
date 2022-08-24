using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ステージクリア時に表示される背景イメージの管理を行う
/// </summary>
public class ScoreUISystem : MonoBehaviour
{
    /// <summary>
    /// キー・マウス入力が可能かどうか
    /// </summary>
    private bool canInput = false;

    [Header("ゲームオブジェクト")] 
    [SerializeField] private CanvasGroup stageClearCg;
    [SerializeField] private GameObject scoreboard;
    [SerializeField] private GameObject splash_obj1;
    [SerializeField] private GameObject splash_obj2;
    [SerializeField] private GameObject splash_obj3;

    [Header("UI系")]
    [SerializeField] private Text stageTxt;
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("背景素材")]
    [SerializeField] private Image background;
    [SerializeField] private Image moveUp;
    [SerializeField] private Image moveDown;
    [SerializeField] private Image shadow;
    [SerializeField] private Image frontImage;

    [Header("Animation素材")]
    [SerializeField] private Animation splash_anim;
    [SerializeField] private Animation window_anim;

    /// <summary>
    /// このオブジェクトが有効になったときに呼び出す
    /// </summary>
    private void OnEnable()
    {
        canvasGroup.alpha = 1;
        Cursor.visible = true;
        // 背景素材を読み込む
        int stage = Score.Stage;
        switch (stage)
        {
            case 1:
                background.sprite = Resources.Load<Sprite>("StageBackgrounds/maru_04");
                moveUp.sprite = Resources.Load<Sprite>("StageBackgrounds/maru_03-1");
                moveDown.sprite = Resources.Load<Sprite>("StageBackgrounds/maru_03-2");
                shadow.sprite = Resources.Load<Sprite>("StageBackgrounds/maru_02");
                frontImage.sprite = Resources.Load<Sprite>("StageBackgrounds/maru_01");
                splash_obj1.SetActive(true);
                splash_obj1.GetComponent<Animation>().Play();
                break;

            case 2:
                background.sprite = Resources.Load<Sprite>("StageBackgrounds/3kaku_4");
                moveUp.sprite = Resources.Load<Sprite>("StageBackgrounds/3kaku_2-1");
                moveDown.sprite = Resources.Load<Sprite>("StageBackgrounds/3kaku_2-2");
                shadow.sprite = Resources.Load<Sprite>("StageBackgrounds/3kaku_3");
                frontImage.sprite = Resources.Load<Sprite>("StageBackgrounds/3kaku_1");
                splash_obj2.SetActive(true);
                splash_obj2.GetComponent<Animation>().Play();
                break;

            case 3:
                background.sprite = Resources.Load<Sprite>("StageBackgrounds/4kaku_4");
                moveUp.sprite = Resources.Load<Sprite>("StageBackgrounds/4kaku_2-1");
                moveDown.sprite = Resources.Load<Sprite>("StageBackgrounds/4kaku_2-2");
                shadow.sprite = Resources.Load<Sprite>("StageBackgrounds/4kaku_3");
                frontImage.sprite = Resources.Load<Sprite>("StageBackgrounds/4kaku_1");
                splash_obj3.SetActive(true);
                splash_obj3.GetComponent<Animation>().Play();
                break;

            default:
                Debug.LogError("存在しないステージ番号が指定されています -> " + stage);
                break;
        }

        stageTxt.text = "STAGE " + stage.ToString();

        // アニメーション実行
        splash_anim.Play();
        window_anim.Play();
        canInput = true;
    }


    /// <summary>
    /// キー入力を管理するメソッド
    /// </summary>
    private void KeyInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            canInput = false;
            Destroy();
        }
    }


    private void Destroy()
    {
        stageClearCg.DOFade(0f, 1f);
        scoreboard.SetActive(true);
        canvasGroup.alpha = 0;
        canvasGroup.DOFade(1f, 1f);
    }


    private void Update()
    {
        if (canInput)
            KeyInput();
    }
}
