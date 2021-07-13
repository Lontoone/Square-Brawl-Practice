using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SetModeButton : MonoBehaviour
{
    public ModeType modeType;
    private Button button;
    public void Start()
    {
        button = GetComponent<Button>();
        //only master can operate the button
        if (PhotonNetwork.IsMasterClient)
        {
            button.onClick.AddListener(delegate { SelectMode(); });
        }
        else
        {
            //close the button:
            button.interactable = false;
        }
    }

    private void SelectMode()
    {
        //update to room property
        PhotonNetwork.CurrentRoom.SetCustomProperties(MyPhotonExtension.WrapToHash(new object[] { CustomPropertyCode.ROOM_MODE_CODE, modeType }));
    }
}
