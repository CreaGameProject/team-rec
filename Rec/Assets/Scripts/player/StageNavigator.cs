using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

public class StageNavigator : MonoBehaviour
{
    private Vector3 navigatorPosition;
    public bool taskIsRunning;

    [Header("経由ポイント")]

    /// <summary>
    /// それぞれの経由ポイントの位置座標
    /// </summary>
    [SerializeField] private List<Vector3> wayList;

    /// <summary>
    /// 次の経由ポイントに行くまでにかかる時間
    /// </summary>
    [SerializeField] private float timeCount = 5f;


    private float time;

    /// <summary>
    /// 現在経由したポイントの数
    /// </summary>
    private int state;

    /// <summary>
    /// 存在する経由ポイントの数
    /// </summary>
    private int maxState;

    /// <summary>
    /// これまで見ていた方向を示すもの(Y座標)
    /// </summary>
    private float beforeLookAt;


    // Start is called before the first frame update
    void Start()
    {
        wayList = new List<Vector3>();
        state = 0;
        time = 0f;

        // 位置を取得
        foreach (Vector3 pos in StageRunner.Instance.StageData.ControlPoints)
        {
            wayList.Add(pos);
        }

        this.transform.position = wayList[0]; // 初期位置
        maxState = wayList.Count - 1;

        taskIsRunning = true;
        Task t = testTask();

        StartCoroutine(OnCameraWork());
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = navigatorPosition;
        time += Time.deltaTime;
    }

    public async Task testTask()
    {
        await Task.Run(() =>
        {
            while (taskIsRunning)
            {
                try
                {
                    // チェックポイントの座標とそこに到達するまでの時間が分かればそのポイントまでの移動ができる。
                    // Task内ではtransform.position等へのアクセスは出来ない。
                                        
                    Vector3 direction = wayList[state + 1] - wayList[state];
                    float progress = 0f; // 100% => 1.00f

                    for (time = 0f; time < timeCount;)
                    {
                        progress = time / timeCount;
                        navigatorPosition = wayList[state] + (direction * progress);
                    }

                    // チェックポイント到達時の処理   
                    if (state != maxState)
                    {
                        state++;
                    }

                    //navigatorPosition = new Vector3(53.8f, 5, -8);
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }

            }


        }).ConfigureAwait(true);

    }

    /// <summary>
    /// ゆっくりカメラを動かす
    /// </summary>
    /// <returns></returns>
    private IEnumerator OnCameraWork()
    {
        while (true)
        {
            // 次のウェイポイントを向かせる
            if (wayList[state + 1] != null)
            {
                beforeLookAt = this.transform.rotation.eulerAngles.y; // 元々見ていた方向
                float aim = Quaternion.LookRotation(wayList[state + 1] - this.transform.position).eulerAngles.y; // 次に見る方向

                for (float t = 0f; t < 0.05f; t += Time.deltaTime) // 要修正(第二引数を小さくしすぎるとバグる)
                {
                    this.transform.rotation = Quaternion.Euler(0, beforeLookAt * (1-t) + aim * t, 0);
                    yield return null;
                }
            }
            yield return null;
        }        
    }

    void OnApplicationQuit()
    {
        taskIsRunning = false;

    }
}
