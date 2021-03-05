using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

public class StageNavigator : MonoBehaviour
{
    private Vector3 navigatorPosition;
    public bool taskIsRunning;

   

    // Start is called before the first frame update
    void Start()
    {
        taskIsRunning = true;
        Task t = testTask();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = navigatorPosition;
    }

    public async Task testTask()
    {
        await Task.Run(() =>
        {
            while (taskIsRunning)
            {
                try
                {
                    navigatorPosition = new Vector3(0,0,0);
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }



            }


        }).ConfigureAwait(true);

    }

    void OnApplicationQuit()
    {
        taskIsRunning = false;

    }
}
