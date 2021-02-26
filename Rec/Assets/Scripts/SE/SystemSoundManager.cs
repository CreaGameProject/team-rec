using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemSoundManager : SingletonMonoBehaviour<SystemSoundManager>
{
    [SerializeField, Range(0, 1), Tooltip("SEの音量")]
    float seVolume = 1;

    AudioClip[] se;

    Dictionary<string, int> seIndex = new Dictionary<string, int>();

    AudioSource seAudioSource;

    public void SetVolume(float value)
    {
        seVolume = Mathf.Clamp01(value);
        seAudioSource.volume = seVolume * value;
    }

    public float GetVolume()
    {
        return seVolume;
    }

    public void Awake()
    {
        //別のSystemSoundManagerがあれば削除
        if (this != Instance)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        //AudioSourceをアタッチ
        seAudioSource = gameObject.AddComponent<AudioSource>();

        //音調調節
        seAudioSource.volume = seVolume;

        //AudioClipの読み込み
        se = Resources.LoadAll<AudioClip>("Audio/SE");

        //Dictionaryに値を追加
        for (int i = 0; i < se.Length; i++)
        {
            seIndex.Add(se[i].name, i);
        }
    }

    //ファイル名を受け取ってキーを渡す
    public int PlaySystemSoundIndex(string name)
    {
        if (seIndex.ContainsKey(name))
        {
            return seIndex[name];
        }
        else
        {
            Debug.LogError("指定された名前のSEファイルが存在しません。");
            return 0;
        }
    }

    //受け取ったキーに対応する音声ファイルを再生する
    public void PlaySystemSound(int index)
    {
        index = Mathf.Clamp(index, 0, se.Length);

        seAudioSource.clip = se[index];
        seAudioSource.Play();
    }

    //カプセル化してみました
    public void PlaySystemSound(string name)
    {
        PlaySystemSound(PlaySystemSoundIndex(name));
    }

    public void StopSe()
    {
        seAudioSource.Stop();
        seAudioSource.clip = null;
    }
}
