using UnityEngine;

namespace Core.Stage
{
    public static class BossEnemyCounter
    {
        private static int _enemyRemains;

        public static void AddEnemyCount(int count)
        {
            // 同時に最後の敵は生成されている前提
            _enemyRemains += count;
            Debug.Log($"Current Boss Enemy is : {_enemyRemains}");
        }

        public static int GetEnemyCount() => _enemyRemains;

        public static void DecreaseEnemy()
        {
            _enemyRemains--;
            if (_enemyRemains <= 0)
            {
                GoalEvent.Instance.StageClear();
            }
        }
    }
}