using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBackground : MonoBehaviour
{
    [Header("動かすUI")]
    [SerializeField] private Image moveUp1;
    [SerializeField] private Image moveDown1;
    [SerializeField] private Image moveUp2;
    [SerializeField] private Image moveDown2;

    [Space(20)]
    /// <summary>
    /// 移動速度
    /// </summary>
    [SerializeField] private float speed;

    private const int height = 1080;


    /// <summary>
    /// 有効になったときに実行されるメソッド
    /// </summary>
    private void OnEnable()
    {
        // 背景素材を読み込む
        int stage = Score.Stage;
        switch (stage)
        {
            case 1:
                moveUp1.sprite = Resources.Load<Sprite>("StageBackgrounds/maru_03-1");
                moveDown1.sprite = Resources.Load<Sprite>("StageBackgrounds/maru_03-2");
                moveUp2.sprite = Resources.Load<Sprite>("StageBackgrounds/maru_03-1");
                moveDown2.sprite = Resources.Load<Sprite>("StageBackgrounds/maru_03-2");
                break;

            case 2:
                moveUp1.sprite = Resources.Load<Sprite>("StageBackgrounds/3kaku_2-1");
                moveDown1.sprite = Resources.Load<Sprite>("StageBackgrounds/3kaku_2-2");
                moveUp2.sprite = Resources.Load<Sprite>("StageBackgrounds/3kaku_2-1");
                moveDown2.sprite = Resources.Load<Sprite>("StageBackgrounds/3kaku_2-2");
                break;

            case 3:
                moveUp1.sprite = Resources.Load<Sprite>("StageBackgrounds/4kaku_2-1");
                moveDown1.sprite = Resources.Load<Sprite>("StageBackgrounds/4kaku_2-2");
                moveUp2.sprite = Resources.Load<Sprite>("StageBackgrounds/4kaku_2-1");
                moveDown2.sprite = Resources.Load<Sprite>("StageBackgrounds/4kaku_2-2");
                break;

            default:
                Debug.LogError("存在しないステージ番号が指定されています -> " + stage);
                break;
        }
    }


    // Update is called once per frame
    void Update()
    {
        Move();
    }


    /// <summary>
    /// 背景図形の移動アニメーションを行う
    /// </summary>
    private void Move()
    {
        float time = Time.deltaTime;
        moveUp1.transform.Translate(new Vector3(0, speed * time, 0));
        moveDown1.transform.Translate(new Vector3(0, -speed * time, 0));
        moveUp2.transform.Translate(new Vector3(0, speed * time, 0));
        moveDown2.transform.Translate(new Vector3(0, -speed * time, 0));

        if (moveUp1.rectTransform.anchoredPosition.y >= height)
        {
            moveUp1.rectTransform.anchoredPosition += new Vector2(0, -2 * height);
        }
        else if (moveDown1.rectTransform.anchoredPosition.y <= -height)
        {
            moveDown1.rectTransform.anchoredPosition += new Vector2(0, 2 * height);
        }
        else if (moveUp2.rectTransform.anchoredPosition.y >= height)
        {
            moveUp2.rectTransform.anchoredPosition += new Vector2(0, -2 * height);
        }
        else if (moveDown2.rectTransform.anchoredPosition.y <= -height)
        {
            moveDown2.rectTransform.anchoredPosition += new Vector2(0, 2 * height);
        }
    }
}
