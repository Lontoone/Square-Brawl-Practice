using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorSelectPlayerItemControl : MonoBehaviour
{
    public Player player;
    public Image colorImage;
    public Text playerText;
    public int colorCode=0;
    public void SetPlayer(Player _p) {
        player = _p;
        playerText.text = _p.NickName;
    }

    public void SetColor(Color _color) {
        colorImage.color = _color;
    }
}
