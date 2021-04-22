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
    /// ���̃C�x���g�ŏ��������G�����ׂē|���΃X�e�[�W�N���A�ƂȂ�
    /// </summary>
    /// <param name="enemyEvents">�G�l�~�[�����C�x���g�̃R���N�V����</param>
    /// <param name="time">��������</param>
    /// <param name="timeLimit">�^�C�����~�b�g</param>
    /// <param name="isTimeUpClear">�^�C�����~�b�g���}�����ۂɃN���A�ƂȂ邩</param>
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