using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAnimation : MonoBehaviour
{
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(PlayStartAnimation());
    }

    private IEnumerator PlayStartAnimation()
    {
        animator.Play(0);
        yield return new WaitForSeconds(3f);
        CameraShake.instance.SetShakeValue(0.4f, 0.2f, 0.5f);
    }
}
