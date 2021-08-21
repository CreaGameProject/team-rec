using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// スコア用変数保存クラス
/// </summary>
public class Score
{
    /// <summary>
    /// 一般的な敵キルの加算。レーザーキルを含む。
    /// </summary>
    public static int NormalKills = 0;

    /// <summary>
    /// レーザーを用いたキルのみを加算。
    /// </summary>
    public static int HomingKills = 0;

    /// <summary>
    /// プレイヤーの体力残量の管理。
    /// </summary>
    public static int HPRemains = 100;

    /// <summary>
    /// ステージを通して発生する敵の個体数
    /// </summary>
    public static int EnemyCounts = 21;

    /// <summary>
    /// ステージ番号
    /// </summary>
    public static int Stage = 1;
}
