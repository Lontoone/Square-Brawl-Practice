using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameAnimationAction : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponentInParent<Animator>().Play("EnterName");
    }

    private void OnDisable()
    {
        GetComponentInParent<Animator>().Play("ExitName");
    }
}
