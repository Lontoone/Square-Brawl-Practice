using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class AudioSourcesManager : MonoBehaviour
{
    [SerializeField]
    public static List<AudioSource> BGM= new List<AudioSource>();
    public static List<AudioClip> SFX = new List<AudioClip>();

    public static AudioSource AUDIOSOURCE;
    private AudioSource bgm;
    public static Sequence sequence;
    public static int currentIndex = 0;

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
            for (int i = 0; i < 10; i++)
            {
                bgm = gameObject.AddComponent<AudioSource>();
            }
            BGM.Add(bgm);
            BGM[0].clip = Resources.Load<AudioClip>("AudioSource/BGM/CylinderSixChrisZabriskie");
            BGM[0].volume = OptionSetting.MUSICVOLUME;
            BGM[0].loop = true;
            BGM[0].Play();
            currentIndex = 0;
        }
    }

    public static void SetUpBGM()
    {
        var bgm = Resources.LoadAll<AudioClip>("BGM/");
        for (int i = 1; i < bgm.Length; i++)
        {
            BGM[i].clip = bgm[i - 1];
        }
    }

    public static void ChangeBGM(string styleName)
    {
        var bgm = Resources.Load<AudioClip>("BGM/" + styleName + "/" + styleName);

        sequence.Kill();
        sequence = DOTween.Sequence();

        for (int i = 0; i < BGM.Count; i++)
        {
            if (BGM[i].clip.name == styleName)
            {
                sequence.Append(BGM[currentIndex].DOFade(0, 1f))
                        .Append(BGM[i].DOFade(OptionSetting.MUSICVOLUME, 1f));
            }
                BGM[i].Play();
                currentIndex = i;
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
