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
    public GameObject[] waypoints;

    /// <summary>
    /// それぞれの経由ポイントの位置座標
    /// </summary>
    [SerializeField] private List<Vector3> wayList;

    // Start is called before the first frame update
    void Start()
    {
        // 位置を取得
        foreach (GameObject obj in waypoints)
        {

        }


        taskIsRunning = true;
        Task t = testTask();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = navigatorPosition;
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
                    navigatorPosition = new Vector3(53.8f, 5, -8);
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }



            }


        }).ConfigureAwait(true);

    }

    void OnApplicationQuit()
    {
        taskIsRunning = false;

    }
}
