using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private GameObject scoreboard;

    [Header("UI系")]
    [SerializeField] private Text stageTxt;

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
                break;

            case 2:
                background.sprite = Resources.Load<Sprite>("StageBackgrounds/");
                moveUp.sprite = Resources.Load<Sprite>("StageBackgrounds/");
                moveDown.sprite = Resources.Load<Sprite>("StageBackgrounds/");
                shadow.sprite = Resources.Load<Sprite>("StageBackgrounds/");
                frontImage.sprite = Resources.Load<Sprite>("StageBackgrounds/");
                break;

            case 3:
                background.sprite = Resources.Load<Sprite>("StageBackgrounds/");
                moveUp.sprite = Resources.Load<Sprite>("StageBackgrounds/");
                moveDown.sprite = Resources.Load<Sprite>("StageBackgrounds/");
                shadow.sprite = Resources.Load<Sprite>("StageBackgrounds/");
                frontImage.sprite = Resources.Load<Sprite>("StageBackgrounds/");
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
            scoreboard.SetActive(true);
        }
    }


    private void Update()
    {
        if (canInput)
            KeyInput();
    }
}
