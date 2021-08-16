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
        //SetColor();
        Destroy(gameObject,2f);
    }

    public void SetColor(Color _color)
    {
        ParticleSystem.MainModule main = _effect.main;
        //Color _color = CustomPropertyCode.COLORS[(int)PhotonNetwork.LocalPlayer.CustomProperties[CustomPropertyCode.TEAM_CODE]];
        main.startColor = _color;
    }

}
