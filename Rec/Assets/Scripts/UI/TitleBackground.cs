using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleBackground : MonoBehaviour
{
    /// <summary>
    /// 画面縦のサイズ
    /// </summary>
    private const int height = 1080; 

    [Header("動かすUI")]
    [SerializeField] private RectTransform MoveUp1;
    [SerializeField] private RectTransform MoveDown1;
    [SerializeField] private RectTransform MoveUp2;
    [SerializeField] private RectTransform MoveDown2;

    [Space(20)]
    [SerializeField] private float speed;


    // Start is called before the first frame update
    void Start()
    {
        MoveUp1.position = Vector3.zero;
        MoveDown1.position = Vector3.zero;
        MoveUp1.position = new Vector3(0, -height, 0);
        MoveDown1.position = new Vector3(0, height, 0);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }


    /// <summary>
    /// 動かす
    /// </summary>
    private void Move()
    {
        float time = Time.deltaTime;
        MoveUp1.Translate(0, time * speed, 0);
        MoveDown1.Translate(0, -time * speed, 0);
        MoveUp2.Translate(0, time * speed, 0);
        MoveDown2.Translate(0, -time * speed, 0);

        if (MoveUp1.anchoredPosition.y >= height)
        {
            MoveUp1.anchoredPosition += new Vector2(0, -2 * height);
        }
        else if (MoveDown1.anchoredPosition.y <= -height)
        {
            MoveDown1.anchoredPosition += new Vector2(0, 2 * height);
        }
        else if (MoveUp2.anchoredPosition.y >= height)
        {
            MoveUp2.anchoredPosition += new Vector2(0, -2 * height);
        }
        else if (MoveDown2.anchoredPosition.y <= -height)
        {
            MoveDown2.anchoredPosition += new Vector2(0, 2 * height);
        }
    }
}
