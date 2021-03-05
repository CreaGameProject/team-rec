using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test2 : MonoBehaviour
{
    /// <summary>
    /// SystemSoundManager.Instance.PlaySE("流したいSE");
    /// で効果音を流すことが出来ます。
    /// </summary>
    
    void Start()
    {
        //MusicManager.Instance.PlayMusic("ashita");
    }

    

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        { 
            SystemSoundManager.Instance.PlaySE("ショット");
        }
        if (Input.GetMouseButtonDown(1))
        {
            SystemSoundManager.Instance.PlaySE("ビーム砲1");
        }
    }
}
