using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : SingletonMonoBehaviour<MusicManager>
{
    [SerializeField, Range(0, 1), Tooltip("MMの音量")]
    float mmVolume = 1;

    AudioClip[] mm;

    Dictionary<string, int> mmIndex = new Dictionary<string, int>();

    AudioSource mmAudioSource;

    public void SetMMVolume(float value)
    {
        mmVolume = Mathf.Clamp01(value);
        mmAudioSource.volume = mmVolume * value;
    }

    public float GetMMVolume()
    {
        return mmVolume;
    }

    public void Awake()
    {
        //別のMusicManagerがあれば削除
        if (this != Instance)
        {
        Destroy(gameObject);
        return;
        }

        DontDestroyOnLoad(gameObject);

        //AudioSourceをアタッチ
        mmAudioSource = gameObject.AddComponent<AudioSource>();
        
        mmAudioSource.loop = !mmAudioSource.loop;
        //AudioClipの読み込み
        mm = Resources.LoadAll<AudioClip>("Audio/BGM");


        //Dictionaryに値を追加
        for (int i = 0; i < mm.Length; i++)
        {
            mmIndex.Add(mm[i].name, i);
        }
    }
    
    

    //ファイル名を受け取ってキーを渡す
    public int PlayMusicIndex(string name)
    {
        if (mmIndex.ContainsKey(name))
        {
            return mmIndex[name];
        }
        else
        {
            Debug.LogError("指定された名前のMMファイルが存在しません。");
            return 0;
        }
    }

    //受け取ったキーに対応する音声ファイルを再生する
    public void PlayMusic(int index)
    {
        index = Mathf.Clamp(index, 0, mm.Length);

        mmAudioSource.clip = mm[index];
        mmAudioSource.Play();
    }

    //カプセル化してみました
    public void PlayMusic(string name)
    {
        PlayMusic(PlayMusicIndex(name));
    }

    public void StopMm()
    {
        mmAudioSource.Stop();
        mmAudioSource.clip = null;
    }
}
