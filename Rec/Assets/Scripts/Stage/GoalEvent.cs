using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;

public class GoalEvent: IStageEvent
{
    /// <summary>
    /// ��̕��̓G�����i�[���邽�߂̍\����
    /// </summary>
    [System.Serializable]
    public struct EnemyParameter
    {
        public GameObject enemyObj;
        public Vector3 relativeCoordinate;
        public EnemyMove move;
    }

    /// <summary>
    /// ��������G�I�u�W�F�N�g�ɒǉ����ADestroy�����^�C�~���O��ʒm����B
    /// </summary>
    private class DestroyObserver : MonoBehaviour
    {
        public UnityEvent OnDestroyed = new UnityEvent();

        private void OnDestroy()
        {
            OnDestroyed.Invoke();
        }
    }
    
    private EnemyParameter[] enemies;
    private Vector3 position;
    private int numAliveEnemies;
    public float Time { get; set; }

    /// <summary>
    /// ���̃C�x���g�ŏ��������G�����ׂē|���΃X�e�[�W�N���A�ƂȂ�
    /// </summary>
    /// <param name="enemies">�G���</param>
    /// <param name="positions">�G�̍��W���@���W�̂ݓG���ƕʂɎ擾����</param>
    /// <param name="time">��������</param>
    public GoalEvent(IEnumerable<EnemyParameter> enemies, float time, Vector3 position)
    {
        this.Time = time;
        this.enemies = enemies as EnemyParameter[] ?? enemies.ToArray();
        this.position = position;
        numAliveEnemies = 0;
    }

    public void Call()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            var enemy = enemies[i];

            // �G�I�u�W�F�N�g����
            var enemyInstance = GameObject.Instantiate(enemy.enemyObj, enemy.relativeCoordinate + this.position, Quaternion.identity);
            var enemyClass = enemyInstance.GetComponent<LoopEnemy>();
            enemyClass.enemyMove = enemy.move;

            // �I�u�W�F�N�g�̔j������m����R���|�[�l���g��G�I�u�W�F�N�g�ɕt�^
            enemyInstance.AddComponent<DestroyObserver>();
            enemyInstance.GetComponent<DestroyObserver>().OnDestroyed.AddListener(CountEnemyDeath);
        }
        numAliveEnemies = enemies.Length;
    }

    /// <summary>
    /// ���������G���|���ꂽ�Ƃ��ɌĂяo�����C�x���g
    /// </summary>
    private void CountEnemyDeath()
    {
        --numAliveEnemies;
        if (numAliveEnemies == 0)
        {
            StageClear();
        }
    }

    /// <summary>
    /// �X�e�[�W�N���A���ɌĂяo��
    /// </summary>
    private void StageClear()
    {
        Debug.Log("Stage Clear");
        Score.HPRemains = Player.Life;

        // �X�e�[�W�N���AUI�̕\��
        var canvas = GameObject.Find("Canvas").transform;
        canvas.Find("ScoreBackground").gameObject.SetActive(true);
        canvas.Find("StageClearUI").gameObject.SetActive(true);
        // GameObject.Find("ScoreBackground").SetActive(true);
        // GameObject.Find("StageClearUI").SetActive(true);
    }
}
