using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompassRader : MonoBehaviour
{
    [SerializeField] private Image compassImage;
    private void Update() {
        compassImage.transform.rotation = Quaternion.Euler(compassImage.transform.rotation.x, compassImage.transform.rotation.y, transform.eulerAngles.y);
    }
}
