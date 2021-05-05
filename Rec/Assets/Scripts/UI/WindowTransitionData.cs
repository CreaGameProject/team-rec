using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowTransitionData : MonoBehaviour
{
    /// <summary>
    /// UIウィンドウの状態をここに保存する
    /// </summary>
    public static WindowTransition Transition = WindowTransition.None;

    /// <summary>
    /// デフォルトのUIウィンドウがなにかをここに保存する。
    /// </summary>
    public static DefaultUIWindow _DefaultUIWindow = DefaultUIWindow.None;
}


/// <summary>
/// UIウィンドウの管理
/// </summary>
public enum WindowTransition
{
    None,
    Title,
    MainMenu,
    StageSelect,
    Option,
    Pause,
    ScoreBoard,
    StageClear
}


/// <summary>
/// 開いている元のUIウィンドウの管理を行う
/// </summary>
public enum DefaultUIWindow
{
    None,
    Main, // タイトル画面ーステージセレクト
    Pause // ポーズ画面中
}
