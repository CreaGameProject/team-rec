using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        //SystemSoundManager testSE = GetComponent<SystemSoundManager>();
        //testSE.PlaySystemSound("ashita");

        MusicManager testBGM = GetComponent<MusicManager>();
        testBGM.PlayMusic("zunou");
    }
    // Update is called once per frame
    void Update()
    {

    }
}
