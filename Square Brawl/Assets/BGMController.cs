using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMController : MonoBehaviour
{
    private AudioSource _bgmAudio;
    void Start()
    {
        /*
        _bgmAudio = GetComponent<AudioSource>();
        _bgmAudio.clip = Resources.Load<AudioClip>("BGM/" + TillStyleLoader.s_StyleName);
        _bgmAudio.volume = OptionSetting.MUSICVOLUME / 4;*/
        Invoke("PlayBgm", 4f);
    }

    void PlayBgm()
    {
        //_bgmAudio.Play();
        AudioSourcesManager.PlayBGM();
    }
}
