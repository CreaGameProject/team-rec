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
        [SerializeField, Label("ダメージ量")] int damage = 45;
        // ロックオン～発射までのディレイ
        [SerializeField, Label("発射ディレイ")] float delay = 0.4f;

        [SerializeField, Label("持続時間")] float duration = 1.5f;

        [SerializeField, Label("発射位置オフセット")] private Vector3 offset;

        [SerializeField] private int animationIndex = 0;

        // Taskクラスを生成して返す
        public override IEnemyTask ToEnemyTask()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            // LineDrawerおよびLineRendererコンポーネントはPlayerではなく、蝶に付けるべき？
            return new Task(damage, delay, duration, player, animationIndex, offset);
        }

        // キャラクターに渡され実行されるタスクのクラス
        private class Task : IEnemyTask
        {
            private int damage;
            private float delay;
            private float duration;
            private GameObject target;
            private Vector3 startPos;
            private Vector3 targetPos;
            private Vector3 offset;
            private int animationIndex;

            // コンストラクタ 引数は必要に応じて追加してください
            public Task(int damage, float delay, float duration, GameObject target, int animationIndex, Vector3 offset)
            {
                this.damage = damage;
                this.delay = delay;
                this.duration = duration;
                this.target = target;
                this.animationIndex = animationIndex;
                this.offset = offset;
            }

            // ゲーム中に呼び出されるタスク実行のメソッド
            // 引数enemyは行動主体
            public IEnumerator Call(TaskBasedEnemy enemy)
            {
                // アニメーション処理
                IAnimatable animatable = enemy.transform.GetChild(0).GetComponent<IAnimatable>();
                animatable.OnAttack(animationIndex);
                
                // タスク処理
                startPos = enemy.transform.GetChild(0).position + offset;
                targetPos = target.transform.position;

                yield return new WaitForSeconds(delay);

                LaserPool.Instance.ShotLaser(startPos, targetPos, duration, damage);
                yield return new WaitForSeconds(duration);
                yield break;
            }

            // タスクの複製を行うメソッド
            // 意図したものを除いて 複製元と複製先が同じ参照を持たないように注意してください
            public IEnemyTask Copy()
            {
                return new Task(damage, delay, duration, target, animationIndex, offset);
            }
        }
    }
}
