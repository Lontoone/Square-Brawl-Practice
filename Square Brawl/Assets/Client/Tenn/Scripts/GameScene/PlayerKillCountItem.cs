using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerKillCountItem : MonoBehaviourPunCallbacks
{
    public Player player;
    public TextMeshProUGUI KillCountText;
    public Image image;

    void Start()
    {
        KillCountText = transform.GetComponentInChildren<TextMeshProUGUI>();
        SetColor(CustomPropertyCode.COLORS[(int)player.CustomProperties[CustomPropertyCode.TEAM_CODE]]);
    }

    public void SetPlayer(Player _p)
    {
        player = _p;
    }

    public void SetColor(Color _color)
    {
        GetComponent<Image>().color = _color;
        KillCountText.color = _color;
        image.color = new Color(_color.r, _color.g, _color.b, 0.3f);
    }

    public void SetKillCount(Player _p)
    {
        int index = 0;
        player = _p;
        index = (int)_p.CustomProperties["KillCount"];
        KillCountText.text = index.ToString();
        if (index >= 2)
        {
            ResultManager.ResultCaller(_p);
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if(targetPlayer!=null&&targetPlayer == player)
        {
            SetKillCount(targetPlayer);
        }
    }
}
