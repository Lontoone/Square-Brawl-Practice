using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioSourcesManager : MonoBehaviour
{
    [SerializeField]
    public static List<AudioSource> BGM= new List<AudioSource>();
    public static List<AudioClip> SFX = new List<AudioClip>();

    public static AudioSource AUDIOSOURCE;
    private AudioSource bgm;

    public static bool audioLock = false;

    private void Awake()
    {
        AUDIOSOURCE = GetComponent<AudioSource>();
        SFX.Add(Resources.Load<AudioClip>("AudioSource/NextSound"));
        SFX.Add(Resources.Load<AudioClip>("AudioSource/BackSound"));
        SFX.Add(Resources.Load<AudioClip>("AudioSource/DefaultSound"));
    }

    private void Start()
    {
        SetUIBGM();
    }

    private void OnDestroy()
    {
        Destroy(bgm);
        BGM.Clear();
    }

    private void SetUIBGM()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            bgm = gameObject.AddComponent<AudioSource>();
            BGM.Add(bgm);
            BGM[0].clip = Resources.Load<AudioClip>("AudioSource/BGM/CylinderSixChrisZabriskie");
            BGM[0].volume = OptionSetting.MUSICVOLUME;
            BGM[0].loop = true;
            BGM[0].Play();
        }
    }

    public static void ChangeBGMVolume()
    {
        foreach (AudioSource audio in BGM)
        {
            audio.volume = OptionSetting.MUSICVOLUME;
        }
    }

    public static void PlaySFX(int index)
    {
        AUDIOSOURCE.PlayOneShot(SFX[index], OptionSetting.SFXVOLUME);
    }
}
