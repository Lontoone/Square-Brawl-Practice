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

    public void Start()
    {
        startBtn.interactable = false;
    }
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        bool _isAllReady = true;
        if (PhotonNetwork.IsMasterClient)
        {
            //if everyone is ready : open the start button
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                object _data;
                //if (!(bool)PhotonNetwork.PlayerList[i].CustomProperties[CustomPropertyCode.READY])
                if(PhotonNetwork.PlayerList[i].CustomProperties.TryGetValue(CustomPropertyCode.READY, out _data) && !(bool)_data)
                {
                    _isAllReady = false;
                    break;
                }
            }

            startBtn.interactable = _isAllReady;
        }

    }


}
