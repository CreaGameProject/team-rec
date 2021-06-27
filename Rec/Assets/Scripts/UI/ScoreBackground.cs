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
    
    // Start is called before the first frame update
    void Start()
    {
        
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
