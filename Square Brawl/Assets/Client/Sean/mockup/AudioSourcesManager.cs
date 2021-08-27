using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourcesManager : MonoBehaviour
{
    [SerializeField] private List<AudioSource> BGM;
    [SerializeField] private List<AudioSource> SFX;

    public static AudioSourcesManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        
    }

    private void Start()
    {
        ChangeBGMVolume();
        BGM[0].Play();
    }

    public void ChangeBGMVolume()
    {
        foreach (AudioSource audio in BGM)
        {
            audio.volume = OptionSetting.MUSICVOLUME;
        }
    }

    public void ChangeSFXVolume()
    {
        foreach (AudioSource audio in SFX)
        {
            audio.volume = OptionSetting.SFXVOLUME;
        }
    }
}
