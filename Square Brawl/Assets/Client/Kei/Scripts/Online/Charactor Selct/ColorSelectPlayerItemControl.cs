using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorSelectPlayerItemControl : MonoBehaviour
{
    public Player player;
    public Image colorImage;
    public Image iconImage;
    public Image readyImage;
    public Text playerText;
    public int colorCode = 0;
    public MenuReadyButton readyButton;

    public void SetPlayer(Player _p)
    {
        player = _p;
        playerText.text = _p.NickName;
        readyButton?.Init(_p);
    }

    public void SetColor(Color _color)
    {
        if (colorImage != null)
        {
            colorImage.color = new Color(_color.r, _color.g, _color.b, 0.5f);
        }
        if (playerText != null)
        {
            playerText.color = _color;
        }
        if (iconImage != null)
        { 
            iconImage.color = _color;
        }
        if (readyImage != null)
        { 
            readyImage.color = _color;
        }
    }
}
