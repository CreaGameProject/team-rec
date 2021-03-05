using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemSoundManager : SingletonMonoBehaviour<SystemSoundManager>
{
    private const string SE_VOLUME_KEY = "SE_VOLUME_KEY";
    private const float SE_VOLUME_DEFULT = 1.0f;

    private const string SE_PATH = "Audio/SE";

    private Dictionary<string, AudioClip> seDictionary;

    private List<AudioSource> seSourceList;




    private void Awake()
    {
        //別のSystemSoundManagerがあれば削除
        if (this != Instance)
        {
            Destroy(this);
            return;
        }

        DontDestroyOnLoad(this.gameObject);

        //AudioSourceを作成
        for (int i = 0; i < 10; i++)
        {
            gameObject.AddComponent<AudioSource>();
        }


        //作成したオーディオソースを取得して各変数に設定、ボリュームも設定
        AudioSource[] seAudioSource = GetComponents<AudioSource>();
        seSourceList = new List<AudioSource>();


        for (int i = 0; i < seAudioSource.Length; i++)
        {
            seAudioSource[i].playOnAwake = false;

            seSourceList.Add(seAudioSource[i]);
            seAudioSource[i].volume = PlayerPrefs.GetFloat(SE_VOLUME_KEY, SE_VOLUME_DEFULT);
        }

        //Resorceフォルダから全SEのファイルを読み込みセット
        seDictionary = new Dictionary<string, AudioClip>();

        object[] seList = Resources.LoadAll("Audio/SE");

        foreach(AudioClip se in seList)
        {
            seDictionary[se.name] = se;
        }

    }
    public void PlaySE(string seName)
    {
        if (!seDictionary.ContainsKey(seName))
        {
            Debug.Log(seName + "という名前のSEファイルが存在しません");
            return;
        }
        
        foreach(AudioSource seSource in seSourceList)
        {
            if (!seSource.isPlaying)
            {
                seSource.PlayOneShot(seDictionary[seName] as AudioClip);
                return;
            }
        }
    }

    public void ChangeVolume(float SEVolume)
    {
        foreach(AudioSource seSource in seSourceList)
        {
            seSource.volume = SEVolume;
        }

        PlayerPrefs.SetFloat(SE_VOLUME_KEY, SEVolume);
    }


}
