using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinnerController : MonoBehaviour
{
    public Player player;
    public Image playerColor;
    public Image winnerIcon;
    public Text playerNameText;

    private Color color;

    public void SetWinner(Player _player)
    {
        color = CustomPropertyCode.COLORS[(int)_player.CustomProperties[CustomPropertyCode.TEAM_CODE]];
        player = _player;
        playerColor.color = new Color(color.r, color.g, color.b, 0.7f);
        winnerIcon.color = color;
        playerNameText.color = color;
        playerNameText.text = player.NickName;
    }
}
