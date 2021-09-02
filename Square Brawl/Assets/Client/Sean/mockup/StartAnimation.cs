using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAnimation : MonoBehaviour
{
    private Animator animator;
    private AudioSource audioSource;

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(PlayStartAnimation());
    }

    private IEnumerator PlayStartAnimation()
    {
        animator.Play(0);
        yield return new WaitForSeconds(4f);
        audioSource.volume = OptionSetting.SFXVOLUME;
        audioSource.Play();
        yield return new WaitForSeconds(0.2f);
        CameraShake.instance.SetShakeValue(0.4f, 0.2f, 0.8f);
    }
}
