using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ColorSetButton : MonoBehaviour
{
    public Button button;
    public Image colorImage;
    public int colorIndex = 0;
    public void Start()
    {
        button.onClick.AddListener(delegate { SetColor(); });
    }
    public void SetColor()
    {
        Debug.Log("Set Color " + colorIndex);
        colorImage.color = CustomPropertyCode.COLORS[colorIndex];
        PhotonNetwork.LocalPlayer.SetCustomProperties(
                                MyPhotonExtension.WrapToHash(
                                    new object[] { CustomPropertyCode.TEAM_CODE, colorIndex }
                                ));
    }

}
