using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Stage
{
    /// <summary>
    /// ゴールイベントのマーカー
    /// </summary>
    class GoalEventMarker: StageEventMarker
    {
        [Header("Enemy obj in \"Asset/Prefabs/Characters/\"")]
        [SerializeField] private List<GoalEvent.EnemyParameter> enemies;
        [SerializeField] private float time;
        
        public override IStageEvent ToStageEvent()
        {
            return new GoalEvent(enemies, time, transform.position);
        }

        class GoalEvent : IStageEvent
        {
            /// <summary>
            /// 一体分の敵情報を格納するための構造体
            /// </summary>
            [System.Serializable]
            public struct EnemyParameter
            {
                public GameObject enemyObj;
                public Vector3 relativeCoordinate;
                public EnemyMove move;
            }

            /// <summary>
            /// 召喚する敵オブジェクトに追加し、Destroyされるタイミングを通知する。
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
            /// このイベントで召喚される敵をすべて倒せばステージクリアとなる
            /// </summary>
            /// <param name="enemies">敵情報</param>
            /// <param name="positions">敵の座標情報　座標のみ敵情報と別に取得する</param>
            /// <param name="time">発動時刻</param>
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

                    // 敵オブジェクト生成
                    var enemyInstance = GameObject.Instantiate(enemy.enemyObj, enemy.relativeCoordinate + this.position, Quaternion.identity);
                    var enemyClass = enemyInstance.GetComponent<LoopEnemy>();
                    enemyClass.enemyMove = enemy.move;

                    // オブジェクトの破壊を検知するコンポーネントを敵オブジェクトに付与
                    enemyInstance.AddComponent<DestroyObserver>();
                    enemyInstance.GetComponent<DestroyObserver>().OnDestroyed.AddListener(CountEnemyDeath);
                }
                numAliveEnemies = enemies.Length;
            }

            /// <summary>
            /// 召喚した敵が倒されたときに呼び出されるイベント
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
            /// ステージクリア時に呼び出す
            /// </summary>
            private void StageClear()
            {
                Debug.Log("Stage Clear");
                Score.HPRemains = Player.Life;

                // ステージクリアUIの表示
                var canvas = GameObject.Find("Canvas").transform;
                canvas.Find("ScoreBackground").gameObject.SetActive(true);
                canvas.Find("StageClearUI").gameObject.SetActive(true);
                // GameObject.Find("ScoreBackground").SetActive(true);
                // GameObject.Find("StageClearUI").SetActive(true);
            }
        }
    }
}
