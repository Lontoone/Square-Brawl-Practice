using ExitGames.Client.Photon;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncMasterMenu : MonoBehaviourPunCallbacks
{
    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        //base.OnRoomPropertiesUpdate(propertiesThatChanged);
        if (PhotonNetwork.IsMasterClient) { return; }

        object _data;
        if (propertiesThatChanged.TryGetValue(CustomPropertyCode.ROOM_MENU, out _data))
        {
            string _menuName = (string)_data;
            MenuManager.instance.OpenMenu(_menuName);
        }
    }
}
