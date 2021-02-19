using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private int nowPosition;
    private int cameraAngle;

    [SerializeField] GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        nowPosition = 1;
    }

    // Update is called once per frame
    void Update()
    {
        getInput();
    }

    void getInput()
    {


        if (Input.GetKeyDown(KeyCode.E) && nowPosition <2)
        {
            nowPosition++;
            transform.localPosition = new Vector3(10 * Mathf.Cos(Mathf.PI / 2f * nowPosition), 1,
                                                -10 * Mathf.Sin(Mathf.PI / 2f * nowPosition));

            transform.eulerAngles = new Vector3(0, (nowPosition -1)*90, 0);
            player.transform.eulerAngles = new Vector3(0, (nowPosition - 1) * 90, 0);

        }
        else if (Input.GetKeyDown(KeyCode.Q) && nowPosition >0)
        {
            nowPosition--;
            transform.localPosition = new Vector3(10 * Mathf.Cos(Mathf.PI / 2f * nowPosition), 1,
                                                -10 * Mathf.Sin(Mathf.PI / 2f * nowPosition));

            transform.eulerAngles = new Vector3(0, (nowPosition -1) * 90, 0);
            player.transform.eulerAngles = new Vector3(0, (nowPosition - 1) * 90, 0);
        }
    }
}
