using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))  // 左クリック
        {
            StartCoroutine(MoveUp());
        }
        if (Input.GetMouseButtonDown(1))  // 右クリック
        {
            MoveDown();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(UpDown());
        }
    }

    IEnumerator MoveUp()
    {
        for (int i = 0; i < 30; i++)
        {
            transform.Translate(0, 0.1f, 0);
            yield return null;  // ここで次のフレームまで待つ
        }
    }

    void MoveDown()
    {
        for (int i = 0; i < 30; i++)
        {
            transform.Translate(0, -0.1f, 0);
        }
    }
    IEnumerator UpDown()
    {
        yield return MoveUp();  // MoveUp()が終わるまで待つ
        MoveDown();
    }
}
