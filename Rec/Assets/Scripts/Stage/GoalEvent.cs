using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.UI;
using UnityEngine;

class GoalEvent: IStageEvent
{
    private SummonEnemyEvent[] enemyEvents;
    private float timeLimit;
    private bool isTimeUpClear;

    /// <summary>
    /// このイベントで召喚される敵をすべて倒せばステージクリアとなる
    /// </summary>
    /// <param name="enemyEvents">エネミー召喚イベントのコレクション</param>
    /// <param name="time">発動時刻</param>
    /// <param name="timeLimit">タイムリミット</param>
    /// <param name="isTimeUpClear">タイムリミットを迎えた際にクリアとなるか</param>
    public GoalEvent(IEnumerable<SummonEnemyEvent> enemyEvents, float time, float timeLimit = 0, bool isTimeUpClear = false)
    {
        this.Time = time;
        this.enemyEvents = enemyEvents.ToArray();
        this.timeLimit = timeLimit;
        this.isTimeUpClear = isTimeUpClear;
    }

    public void Call()
    {
        foreach (var enemyEvent in enemyEvents)
        {
            enemyEvent.Call();
        }
        
    }

    private class BossesObserver: MonoBehaviour
    {
        IEnumerator
    }

    public float Time { get; set; }

    private IEnumerator ObserveEnemies(GameObject observer)
    {

    }
}