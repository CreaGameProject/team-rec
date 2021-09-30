using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Core.Stage
{
    /// <summary>
    /// StageDataに格納され、StageRunnerで実行する
    /// </summary>
    public interface IStageEvent
    {
        /// <summary>
        /// イベントの呼び出し
        /// </summary>
        void Call();
    
        /// <summary>
        /// 時間を返す
        /// </summary>
        float Time
        {
            get;
            set;
        }
    }

    /// <summary>
    /// ステージに関する情報を格納する。StageRunnerにインスタンスを渡すことで実行できる。
    /// </summary>
    public class StageData
    {
    
        /// <summary>
        /// BGMのパスを返す
        /// </summary>
        public string MusicPath
        {
            get;
            private set;
        }

        /// <summary>
        /// プレイヤーの移動経路の制御点を返す
        /// </summary>
        public IEnumerable<Vector3> ControlPoints
        {
            get; 
            private set;
        }

        /// <summary>
        /// 敵出現などのイベントを返す
        /// </summary>
        public IEnumerable<IStageEvent> Events
        {
            get;
            private set;
        }

        public StageData(IEnumerable<IStageEvent> events, IEnumerable<Vector3> controlPoints)//, string musicPath = "")
        {
            Events = events;
            ControlPoints = controlPoints;
        }

    }
}