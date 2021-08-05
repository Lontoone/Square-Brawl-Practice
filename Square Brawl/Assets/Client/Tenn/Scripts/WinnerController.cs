using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinnerController : MonoBehaviour
{
    public Player player;
    public Image playerColor;
    public Text playerNameText;

    public void SetWinner(Player _player)
    {
        player = _player;
        playerColor.color = CustomPropertyCode.COLORS[(int)_player.CustomProperties[CustomPropertyCode.TEAM_CODE]];
        playerNameText.text = player.NickName;
    }
}
