using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Core.Stage
{
    /// <summary>
    /// ステージデータを読み込み、敵の出現などを制御する。
    /// </summary>
    public class StageRunner : SingletonMonoBehaviour<StageRunner>
    {
        /// <summary>
        /// ステージの経過時間を返す
        /// </summary>
        public float time { get; private set; }

        public StageData StageData { get; private set; }
        private List<IStageEvent> stageEvents;

        /// <summary>
        /// ステージが実行中であるか
        /// </summary>
        public bool IsRunning
        {
            get;
            private set;
        }

        /// <summary>
        /// 実行するステージデータをセットする
        /// ただしこの時点で実行はしない
        /// </summary>
        /// <param name="stageData">セットするステージ</param>
        public void SetStageData(StageData stageData)
        {
            StageData = stageData;
            StartCoroutine("Timer");
        }

        /// <summary>
        /// ステージを実行する
        /// </summary>
        /// <param name="callback">ステージ終了時に実行する関数　引数はステージを最後まで実行できたか</param>
        public void StartStage(Action<bool> callback = null)
        {
        }


        /// <summary>
        /// ステージの実行を停止する。
        /// </summary>
        public void StopStage()
        {
        
        
        }


        protected override void Awake()
        {
            base.Awake();
        }

        private void Start() {
            //StartCoroutine("Timer");
        }

    
        private void Update() {
       
        }
    
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerator Timer()
        {
            stageEvents = StageData.Events.ToList();



            while (true)
            {
                time += Time.deltaTime;
                //_stageDataはTimeでソートされていることを前提としている。
                foreach (var x in stageEvents.TakeWhile(x => x.Time < time))
                {
                    x.Call();
                }
                stageEvents = stageEvents.SkipWhile(x => x.Time < time).ToList();

                yield return null;
            }
        }
    }
}