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
        DontDestroyOnLoad(this.gameObject);
        GenerateBGMList();

        BGM[0].clip = Resources.Load<AudioClip>("AudioSource/BGM/CylinderSixChrisZabriskie");
        AUDIOSOURCE = GetComponent<AudioSource>();
        SFX.Add(Resources.Load<AudioClip>("AudioSource/NextSound"));
        SFX.Add(Resources.Load<AudioClip>("AudioSource/BackSound"));
        SFX.Add(Resources.Load<AudioClip>("AudioSource/DefaultSound"));
    }

    /*private void OnDestroy()
    {
        Destroy(bgm);
        BGM.Clear();
    }*/

    private void GenerateBGMList()
    {
        var length = 12;//style count +1

        for (int i = 0; i < length; i++)
        {
            bgm = gameObject.AddComponent<AudioSource>();
            BGM.Add(bgm);
            BGM[i].volume = OptionSetting.MUSICVOLUME;
            BGM[i].loop = true;
            BGM[i].playOnAwake = false;
        }
    }

    public static void PlayBGM()
    {
        var volumeScale = 1f;
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            BGM[0].volume = OptionSetting.MUSICVOLUME;
            BGM[0].Play();
            currentIndex = 0;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            for (int i = 0; i < BGM.Count; i++)
            {
                if (BGM[i].clip != null)
                {
                    if (BGM[i].clip.name == TillStyleLoader.s_StyleName)
                    {
                        if (TillStyleLoader.s_StyleName != "Thorns" || TillStyleLoader.s_StyleName != "Leisurely")
                        {
                            volumeScale = 0.2f;
                        }
                        BGM[i].volume = OptionSetting.MUSICVOLUME * volumeScale;
                        BGM[i].Play();
                        currentIndex = i;
                    }
                }
            }
        } 
    }

    public static void SetUpBGM()
    {
        if (!audioLock)
        {
            var bgm = Resources.LoadAll<AudioClip>("BGM/");

            for (int i = 1; i <= bgm.Length; i++)
            {
                BGM[i].clip = bgm[i - 1];
            }
            audioLock = true;
        }
    }

    public static IEnumerator ChangeBGM(string styleName)
    {
        var nextIndex = 0;
        var volumeScale = 1f;

        if (styleName != "Thorns" || styleName != "Leisurely")
        {
            volumeScale = 0.2f;
        }

        for (int i = 0; i < BGM.Count; i++)
        {
            if (BGM[i].clip != null)
            { 
                if (BGM[i].clip.name == styleName)
                {
                    nextIndex = i;
                }
            }
        }

        //sequence
        sequence.Kill();
        sequence = DOTween.Sequence();
        sequence.Append(BGM[currentIndex].DOFade(0, 0.3f));
        yield return new WaitForSeconds(0.5f);
        BGM[currentIndex].Stop();
        BGM[nextIndex].Play();
        sequence.Append(BGM[nextIndex].DOFade(OptionSetting.MUSICVOLUME * volumeScale, 0.5f));

        currentIndex = nextIndex;
    }

    public static void ChangeBGMVolume()
    {
        foreach (AudioSource audio in BGM)
        {
            audio.volume = OptionSetting.MUSICVOLUME;
        }
    }

    public static void StopBGM()
    {
        BGM[currentIndex].Stop();
    }
    public static void BGMFadeOut()
    {
        sequence.Kill();
        sequence = DOTween.Sequence();
        sequence.Append(BGM[currentIndex].DOFade(0, 0.3f));
        BGM[currentIndex].Stop();
    }


    public static void PlaySFX(int index)
    {
        AUDIOSOURCE.PlayOneShot(SFX[index], OptionSetting.SFXVOLUME);
    }
}
