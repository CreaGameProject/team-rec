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
    private float markerStrength;
    private EnemyMarkerText enemyMarkerText;

    private void Start() {
        compass = GameObject.Find("CompassMask");
        target = GameObject.Find("Main Camera");
        
        marker = Instantiate(markerImage, compass.transform.transform.position, Quaternion.identity) as Image;
        marker.transform.SetParent(compass.transform, false);
        enemyMarkerText = marker.gameObject.GetComponent<EnemyMarkerText>();
    }

    private void Update() {
        markerStrength = target.GetComponent<CompassRader>().raderStrength;

        Vector3 position = transform.position - target.transform.position;
        position *= markerStrength;
        marker.transform.localPosition = new Vector3(position.x, position.z, 0);

        enemyMarkerText.MarkHight(transform.position.y - target.transform.position.y);
    }

    private void OnDestroy() {
        Destroy(marker.gameObject);    
    }
}
