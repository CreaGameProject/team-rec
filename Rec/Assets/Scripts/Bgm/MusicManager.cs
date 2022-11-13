using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : SingletonMonoBehaviour<MusicManager>
{
    private const string BGM_VOLUME_KEY = "SE_VOLUME_KEY";
    private const float BGM_VOLUME_DEFULT = 0.5f;

    public const float BGM_FADE_SPEED_RATE_HIGH = 0.9f;
    public const float BGM_FADE_SPEED_RATE_LOW = 0.3f;
    private float bgmFadeSpeedRate = BGM_FADE_SPEED_RATE_HIGH;

    private string nextBGMName;

    private bool isFadeOut = false;

    private Dictionary<string, AudioClip> bgmDictionary;

    private AudioSource bgmSource;


    public void Awake()
    {
        //別のMusicManagerがあれば削除
        if (this != Instance)
        {
            Destroy(this);
            return;
        }

        DontDestroyOnLoad(gameObject);

        //オーディオソースを1作成
        gameObject.AddComponent<AudioSource>();

        //作成したオーディオソースを取得して各変数に設定、ボリュームも設定
        AudioSource bgmAudioSource = GetComponent<AudioSource>();

        bgmAudioSource.playOnAwake = false;
        bgmAudioSource.loop = true;
        bgmSource = bgmAudioSource;
        bgmSource.volume = PlayerPrefs.GetFloat(BGM_VOLUME_KEY, BGM_VOLUME_DEFULT);

        bgmDictionary = new Dictionary<string, AudioClip>();
        object[] bgmList = Resources.LoadAll("Audio/BGM");

        foreach (AudioClip bgm in bgmList)
        {
            bgmDictionary[bgm.name] = bgm;
        }

    }

    public void PlayBGM(string bgmName, float fadeSpeedRate = BGM_FADE_SPEED_RATE_HIGH)
    {
        if (!bgmDictionary.ContainsKey(bgmName))
        {
            Debug.Log(bgmName + "という名前のBGMがありません");
            return;
        }

        if (!bgmSource.isPlaying)
        {
            nextBGMName = "";
            bgmSource.clip = bgmDictionary[bgmName] as AudioClip;
            bgmSource.Play();
        }
        else if (bgmSource.clip.name != bgmName)
        {
            nextBGMName = bgmName;
            FadeOutBGM(fadeSpeedRate);
        }
    }
    
    public void StopBGM ()
    {
        bgmSource.Stop ();
    }

    public void FadeOutBGM(float fadeSpeedRate = BGM_FADE_SPEED_RATE_LOW)
    {
        bgmFadeSpeedRate = fadeSpeedRate;
        isFadeOut = true;
    }

    private void Update()
    {
        if (!isFadeOut)
        {
            return;
        }

        bgmSource.volume -= Time.deltaTime * bgmFadeSpeedRate;
        if (bgmSource.volume <= 0)
        {
            bgmSource.Stop();
            bgmSource.volume = PlayerPrefs.GetFloat(BGM_VOLUME_KEY, BGM_VOLUME_DEFULT);
            isFadeOut = false;

            if (!string.IsNullOrEmpty(nextBGMName))
            {
                PlayBGM(nextBGMName);
            }
        }
    }

    public void ChangeBGMVolume(float BGMVolume)
    {
        bgmSource.volume = BGMVolume;
        PlayerPrefs.SetFloat(BGM_VOLUME_KEY, BGMVolume);
    }
}