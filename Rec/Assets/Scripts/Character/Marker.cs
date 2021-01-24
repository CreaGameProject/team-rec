using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Marker : MonoBehaviour
{
    private Image marker;
    [SerializeField] private Image markerImage;
    private GameObject compass;
    private GameObject target;
    private float markerStrength = 10.0f;

    private void Start() {
        compass = GameObject.Find("CompassMask");
        target = GameObject.Find("Main Camera");
        marker = Instantiate(markerImage, compass.transform.transform.position, Quaternion.identity) as Image;
        marker.transform.SetParent(compass.transform, false);
    }

    private void Update() {
        Vector3 position = transform.position - target.transform.position;
        position *= markerStrength;
        marker.transform.localPosition = new Vector3(position.x, position.z, 0);
    }
}
