using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageFactory : SingletonMonoBehaviour<StageFactory>
{
    private StageData _stageData;




    protected override void Awake()
    {
        base.Awake();
    }



    private void StageDataCreator()
    {
        // _stageData = new StageData();
        StageRunner.Instance.SetStageData(_stageData);
    }
}
