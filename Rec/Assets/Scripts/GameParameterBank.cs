using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameParameterBank : SingletonMonoBehaviour<GameParameterBank>
{
    [Range(0, 1)]
    private float _bgmVolume;
    public float BGMVolume
    {
        get { return _bgmVolume; }
        set { _bgmVolume = value; }
    }

    [Range(0, 1)]
    private float _seVolume;
    public float SEVolume
    {
        get { return _seVolume;}
        set { _seVolume = value; }
    }

    private string keyBGMVolume = "BGMVolume";
    private string keySEVolume = "SEVolume";

    override protected void Awake()
    {
        // 子クラスでAwakeを使う場合は
        // 必ず親クラスのAwakeをCallして
        // 複数のGameObjectにアタッチされないようにします.
        base.Awake();

        GetData();
    }

    private void Start()
    {
        
    }

    public void SaveData()
    {
        PlayerPrefs.SetFloat(keyBGMVolume, BGMVolume);
        PlayerPrefs.SetFloat(keySEVolume, SEVolume);
        Debug.Log("セーブしました。");
    }

    public void GetData()
    {
        if(!PlayerPrefs.HasKey(keyBGMVolume) ||
            !PlayerPrefs.HasKey(keySEVolume))
        {
            
            BGMVolume = 0.2f;
            SEVolume = 0.2f;
            Debug.Log("セーブデータを新規作成しました。");
        }
        else
        {
            BGMVolume = PlayerPrefs.GetFloat(keyBGMVolume);
            SEVolume = PlayerPrefs.GetFloat(keySEVolume);
            Debug.Log("セーブデータを読み込みました。");
        }
    }

}
