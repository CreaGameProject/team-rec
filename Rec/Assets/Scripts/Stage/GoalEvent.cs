using UnityEngine;

namespace Core.Stage
{
    public class GoalEvent : SingletonMonoBehaviour<GoalEvent>
    {
        [SerializeField] private GameObject scoreBackground;
        [SerializeField] private GameObject stageClearWindow;
        
        public void StageClear()
        {
            Debug.Log("Stage Clear");
            Score.HPRemains = Player.Life;
            
            scoreBackground.SetActive(true);
            stageClearWindow.SetActive(true);
        }
    }
}