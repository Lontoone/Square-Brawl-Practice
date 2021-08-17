using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class DieEffect : MonoBehaviour
{
    public ParticleSystem _effect;
    public Color color;
    void Start()
    {
        Destroy(gameObject,2f);
    }

    public void SetColor(Color _color)
    {
        ParticleSystem.MainModule main = _effect.main;
        main.startColor = _color;
    }

}
