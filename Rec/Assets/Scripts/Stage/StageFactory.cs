using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Stage;
using UnityEngine;

public class StageFactory : SingletonMonoBehaviour<StageFactory>
{
    private StageData _stageData;
    private List<IStageEvent> _stageEvents = new List<IStageEvent>();
    private List<Vector3> _controlPoints = new List<Vector3>();



    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        GetControlPoints();//StageNavigator

        GetEnemyData();

        StageDataCreator();
    }

    /// <summary>
    /// プレイヤーが移動するのに使用するControlPointsのリストを作成
    /// </summary>
    private void GetControlPoints()
    {
        Transform controlPointsParent = GameObject.Find("ControlPoints").transform;
        foreach (Transform child in controlPointsParent)
        {
            _controlPoints.Add(child.position);
        }
    }

    /// <summary>
    /// Scene上に置いたEnemyDataからIStageEventのリストを作成
    /// </summary>
    private void GetEnemyData()
    {
        var markers = FindObjectsOfType<StageEventMarker>();
        _stageEvents = markers.Select(x => x.ToStageEvent()).ToList();

        foreach (var marker in markers)
        {
            marker.gameObject.SetActive(false);
        }
        
        //List<EnemyData> enemyDatas = new List<EnemyData>();
        // foreach(GameObject item in GameObject.FindGameObjectsWithTag("EnemyData"))
        // {
        //     if (item.GetComponent<EnemyData>() != null)
        //     {
        //         EnemyData enemyData = item.GetComponent<EnemyData>();
        //         SummonLoopEnemyEvent summonEnemyEvent = new SummonLoopEnemyEvent(enemyData.enemyObj, enemyData.enemyType, enemyData.enemyMove, enemyData.position, enemyData.summonTime);
        //         _stageEvents.Add(summonEnemyEvent);
        //         //Debug.Log(enemyData.position.ToString() + "に" + enemyData.enemyObj.ToString() + "を生成します");
        //     }
        //     else
        //     {
        //         Debug.LogError("EnemyDataが指定されていません。");
        //     }
        // }
    }


    /// <summary>
    /// StageRunnerに値を代入
    /// </summary>
    private void StageDataCreator()
    {
        _stageEvents.Sort((a, b) => (int)(a.Time - b.Time));//時間でソート

        _stageData = new StageData(_stageEvents, _controlPoints);
        StageRunner.Instance.SetStageData(_stageData);
    }
}
