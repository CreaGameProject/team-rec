using UnityEngine;
using UnityEngine.UI;

public class SESlider : MonoBehaviour
{
    [SerializeField] private Slider slider;
    
    private void Start()
    {
        slider.onValueChanged
            .AddListener(SystemSoundManager.Instance.ChangeVolume);
    }
}