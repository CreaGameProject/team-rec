using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Stage
{
    public class SummonLoopEnemyMarker : StageEventMarker
    {
        [SerializeField] private float time;
        [SerializeField] private GameObject enemyObj;
        //[SerializeField] private EnemyType enemyType;
        [SerializeField] private EnemyMove enemyMove;
    
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public override IStageEvent ToStageEvent()
        {
            return new SummonLoopEnemyEvent(enemyObj, /*enemyType,*/ enemyMove, transform.position, time);
        }

        class SummonLoopEnemyEvent : IStageEvent
        {
            private GameObject _enemy;
            private Vector3 _position;
            private float _time;
            //private EnemyType _enemyType;
            private EnemyMove _enemyMove;


            public float Time
            {
                get { return _time; }
                set { _time = value; }
            }

            public SummonLoopEnemyEvent(GameObject enemy, /*EnemyType enemyType,*/ EnemyMove enemyMove, Vector3 position, float time)
            {
                _enemy = enemy;
                //_enemyType = enemyType;
                _enemyMove = enemyMove;
                _position = position;
                Time = time;
            }

            public void Call()
            {
                // Instantiate
                GameObject enemyInstance = GameObject.Instantiate(_enemy, _position, Quaternion.identity);
                // Get component
                LoopEnemy enemyClass = enemyInstance.GetComponent<LoopEnemy>();
                // Set parameters
                //enemyClass.enemyType = _enemyType;
                enemyClass.enemyMove = _enemyMove;

                // Debug.Log("Called");
            }
        }
    }
}