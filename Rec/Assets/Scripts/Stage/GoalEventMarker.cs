﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Stage
{
    /// <summary>
    /// ゴールイベントのマーカー
    /// </summary>
    class GoalEventMarker: StageEventMarker
    {
        [SerializeField] private List<GoalEvent.EnemyParameter> enemies;
        [SerializeField] private float time;
        
        public override IStageEvent ToStageEvent()
        {
            return new GoalEvent(enemies, time);
        }
    }
}
