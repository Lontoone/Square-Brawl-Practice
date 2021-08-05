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

        if (!IsColorUsed(colorIndex))
        {
            colorImage.color = CustomPropertyCode.COLORS[colorIndex];

            PhotonNetwork.LocalPlayer.SetCustomProperties(
                                    MyPhotonExtension.WrapToHash(
                                        new object[] { CustomPropertyCode.TEAM_CODE, colorIndex }
                                    ));
        }
        else
        {
            //該顏色已被使用
        }
    }

    private bool IsColorUsed(int _colorIndex)
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (PhotonNetwork.PlayerList[i] != PhotonNetwork.LocalPlayer &&
                _colorIndex == (int)PhotonNetwork.PlayerList[i].CustomProperties[CustomPropertyCode.TEAM_CODE])
            {
                return true;
            }
        }
        return false;
    }

}
