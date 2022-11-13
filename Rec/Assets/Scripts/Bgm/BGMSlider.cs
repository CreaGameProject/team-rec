using UnityEngine;
using UnityEngine.UI;

public class BGMSlider : MonoBehaviour
{
    [SerializeField] private Slider slider;
    
    private void Start()
    {
        slider.onValueChanged
            .AddListener(MusicManager.Instance.ChangeBGMVolume);
    }
}