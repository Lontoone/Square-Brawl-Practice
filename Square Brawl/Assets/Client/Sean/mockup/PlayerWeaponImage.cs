using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWeaponImage : MonoBehaviour
{
    public Image[] IconGroup;
    public static PlayerWeaponImage instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
}
