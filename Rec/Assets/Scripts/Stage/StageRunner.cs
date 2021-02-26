using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ステージデータを読み込み、敵の出現などを制御する。
/// </summary>
public class StageRunner : MonoBehaviour
{
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

    }

    /// <summary>
    /// ステージを実行する
    /// </summary>
    /// <param name="callback">ステージ終了時に実行する関数　引数はステージを最後まで実行できたか</param>
    public void StartStage(Action<bool> callback = null)
    {

    }

    /// <summary>
    /// ステージの実行を停止する。
    /// </summary>
    public void StopStage()
    {

    }

    private void Start() {
        
    }

    private void Update() {
        
    }
}
