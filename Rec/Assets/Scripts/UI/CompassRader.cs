using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompassRader : MonoBehaviour
{
    [SerializeField] private Image compassImage;
    [Header("コンパスレーダーの範囲の設定項目")]
    [Range(1, 30)]
    public float raderStrength = 15;
    private void Update() {
        compassImage.transform.rotation = Quaternion.Euler(compassImage.transform.rotation.x, compassImage.transform.rotation.y, transform.eulerAngles.y);
    }
}
