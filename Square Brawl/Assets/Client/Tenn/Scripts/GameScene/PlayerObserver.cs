using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObserver : MonoBehaviour
{
    public static PlayerObserver instance;
    void Awake()
    {
        instance = this;
    }

    public event Action OnBeAttack;
    public void BeAttack()
    {
        if (OnBeAttack != null)
        {
            BeAttack();
        }
    }
}
