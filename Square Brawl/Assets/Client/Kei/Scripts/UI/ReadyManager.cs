using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadyManager : MonoBehaviourPunCallbacks
{
    public Button startBtn;

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        bool _isAllReady = true;
        if (PhotonNetwork.IsMasterClient)
        {
            //if everyone is ready : open the start button
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                if (!(bool)PhotonNetwork.PlayerList[i].CustomProperties[CustomPropertyCode.READY])
                {
                    _isAllReady = false;
                    break;
                }
            }

            startBtn.interactable = _isAllReady;
        }

    }


}
