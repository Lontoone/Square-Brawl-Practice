using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class DieEffect : MonoBehaviour
{
    public ParticleSystem Effect;
    public AudioSource Aduio;
    public Color color;
    void Start()
    {
        Destroy(gameObject,2f);
        Aduio.volume = OptionSetting.SFXVOLUME;
    }

    public void SetColor(Color _color)
    {
        ParticleSystem.MainModule main = Effect.main;
        main.startColor = _color;
    }

}
