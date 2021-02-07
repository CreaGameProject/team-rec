using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// ステージデータを読み込み、敵の出現などを制御する。
/// </summary>
public class StageRunner : MonoBehaviour
{
    private StageData stageData;
    private Coroutine routine;
    private Action<bool> stageEndCallback;
    
    /// <summary>
    /// ステージの経過時間を返す
    /// </summary>
    public float Time { get; private set; }

    /// <summary>
    /// ステージが実行中であるか
    /// </summary>
    public bool IsRunning
    {
        get;
        private set;
    }

    /// <summary>
    /// 実行するステージデータをセットする
    /// ただしこの時点で実行はしない
    /// </summary>
    /// <param name="stageData">セットするステージ</param>
    public void SetStageData(StageData stageData)
    {
        this.stageData = stageData;
    }

    /// <summary>
    /// ステージを実行する
    /// </summary>
    /// <param name="callback">ステージ終了時に実行する関数　引数はステージを最後まで実行できたか</param>
    public void StartStage(Action<bool> callback = null)
    {
        stageEndCallback = callback;
        routine = StartCoroutine(Run());
        IsRunning = true;
    }

    /// <summary>
    /// ステージの実行を停止する。
    /// </summary>
    public void StopStage(bool clear)
    {
        IsRunning = false;
        StopCoroutine(routine);
        stageEndCallback?.Invoke(clear);
    }

    private IEnumerator Run()
    {
        Time = 0;
        Queue<IStageEvent> events = new Queue<IStageEvent>(stageData.Events);
        while (stageData.Events.Any())
        {
            if (Time > events.Peek().Time)
            {
                events.Dequeue().Call();
            }

            Time += UnityEngine.Time.unscaledDeltaTime;
            yield return null;
        }
        StopStage(true);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
