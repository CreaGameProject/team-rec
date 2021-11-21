﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Enemy.TaskBased
{
    // AddComponentからキャラクターにタスクをアタッチするためのコード   
    [AddComponentMenu("EnemyTask/LaserTask")]
    // AIデザインのためにコンポーネントとしてアタッチされるクラス
    public class LaserTask : EnemyTaskComponent
    {
        // ロックオン～発射までのディレイ
        [SerializeField, Label("チャージ時間")] float chargeTime = 2f;

        [SerializeField, Label("持続時間")] float duration = 1.5f;

        // Taskクラスを生成して返す
        public override IEnemyTask ToEnemyTask()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            // LineDrawerおよびLineRendererコンポーネントはPlayerではなく、蝶に付けるべき？
            return new Task(chargeTime, duration, player);
        }

        // キャラクターに渡され実行されるタスクのクラス
        private class Task : IEnemyTask
        {
            private float chargeTime;
            private float duration;
            private GameObject target;
            private Vector3 startPos;
            private Vector3 targetPos;

            // コンストラクタ 引数は必要に応じて追加してください
            public Task(float chargeTime, float duration, GameObject target)
            {
                this.chargeTime = chargeTime;
                this.duration = duration;
                this.target = target;
            }

            // ゲーム中に呼び出されるタスク実行のメソッド
            // 引数enemyは行動主体
            public IEnumerator Call(TaskBasedEnemy enemy)
            {
                startPos = enemy.transform.GetChild(0).position;
                targetPos = target.transform.position;
                float damage = 10; // 仮の値

                yield return new WaitForSeconds(chargeTime);

                Debug.Log(startPos.ToString() + targetPos.ToString());
                LaserPool.Instance.ShotLaser(startPos, targetPos, duration, damage);
                yield break;
            }

            // タスクの複製を行うメソッド
            // 意図したものを除いて 複製元と複製先が同じ参照を持たないように注意してください
            public IEnemyTask Copy()
            {
                return new Task(chargeTime, duration, target);
            }
        }
    }
}
