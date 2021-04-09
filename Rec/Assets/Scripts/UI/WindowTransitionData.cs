using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowTransitionData : MonoBehaviour
{
    /// <summary>
    /// UIウィンドウの状態をここに保存する
    /// </summary>
    public static WindowTransition Transition = WindowTransition.None;
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
    Option
}