using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerKillCountItem : MonoBehaviour
{
    public Player player;

    void Start()
    {
        SetColor(CustomPropertyCode.COLORS[(int)player.CustomProperties[CustomPropertyCode.TEAM_CODE]]);
    }

    public void SetPlayer(Player _p)
    {
        player = _p;
    }

    public void SetColor(Color _color)
    {
        GetComponent<Image>().color = _color;
    }
}
