using UnityEngine;

public class TitleBGMPlayer : MonoBehaviour
{
    private void Start()
    {
        MusicManager.Instance.PlayBGM("BGM_Title");
    }
}