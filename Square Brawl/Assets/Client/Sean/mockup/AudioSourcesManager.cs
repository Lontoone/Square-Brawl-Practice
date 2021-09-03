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
                BGM.Add(bgm);
                BGM[i].volume = OptionSetting.MUSICVOLUME;
                BGM[i].loop = true;
                BGM[i].playOnAwake = false;
            }
            BGM[0].clip = Resources.Load<AudioClip>("AudioSource/BGM/CylinderSixChrisZabriskie");
            BGM[0].Play();
            currentIndex = 0;
        }
    }

    public static void SetUpBGM()
    {
        var bgm = Resources.LoadAll<AudioClip>("BGM/");

        for (int i = 1; i <= bgm.Length; i++)
        {
            BGM[i].clip = bgm[i - 1];
        }
    }

    public static IEnumerator ChangeBGM(string styleName)
    {
        var nextIndex = 0;

        sequence.Kill();
        sequence = DOTween.Sequence();

        for (int i = 0; i < BGM.Count; i++)
        {
            if (BGM[i].clip.name == styleName)
            {
                nextIndex = i;
            }
        }
        sequence.Append(BGM[currentIndex].DOFade(0, 0.3f));
        yield return new WaitForSeconds(0.5f);

        BGM[currentIndex].Stop();
        BGM[nextIndex].Play();

        sequence.Append(BGM[nextIndex].DOFade(OptionSetting.MUSICVOLUME, 0.5f));
        currentIndex = nextIndex;
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
