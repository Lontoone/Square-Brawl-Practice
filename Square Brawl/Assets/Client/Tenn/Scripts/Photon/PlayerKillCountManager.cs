using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKillCountManager : MonoBehaviourPunCallbacks
{
    public Transform playerItemParent;
    public PlayerKillCountItem playerItemPrefab;

    private void Start()
    {
        CreatePlayerItem();
    }

    private void CreatePlayerItem()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            {
                //foreach player data:
                PlayerKillCountItem _item = Instantiate(playerItemPrefab, parent: playerItemParent);
                Player _player = PhotonNetwork.PlayerList[i];
                _item.SetPlayer(_player);
            }
        }
    }
}
