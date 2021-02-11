using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyMarkerText : MonoBehaviour
{
    [SerializeField] private GameObject text;
    private Text markerText;
    private string markerTextString;
    private Vector2 offset = new Vector2(0f, 15f);
    private void Start() {
        text = transform.Find("Text").gameObject;
        text.transform.SetParent(GameObject.Find("TextMask").transform);
        markerText = text.GetComponent<Text>();
    }

    private void Update() {
        text.transform.position = transform.position - new Vector3(offset.x, offset.y, 0);
    }

    private void OnDestroy() {
        Destroy(text);
    }

    public void MarkHight(float hight){
        if(hight >= 0){
            markerTextString = "+" + hight.ToString("f0");
            markerText.color = Color.green;
        }else{
            hight *= -1;
            markerTextString = "-" + hight.ToString("f0");
            markerText.color = Color.red;
        }

        markerText.text = markerTextString;
    }
}
