using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingAnimationAction : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponentInParent<Animator>().Play("Loading");
    }

    private void OnDisable()
    {
        GetComponentInParent<Animator>().Play("ExitLoading");
    }
}
