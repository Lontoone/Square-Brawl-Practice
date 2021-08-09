using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerKillCountManager : MonoBehaviourPunCallbacks
{
    private int _killCount;

    public Transform playerItemParent;

    public PlayerKillCountItem playerItemPrefab;

    public static PlayerKillCountManager instance;

    private Hashtable _myCustom = new Hashtable();

    private void Start()
    {
        instance = this;
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

    public void SetKillCount()
    {
        _killCount++;
        _myCustom["KillCount"] = _killCount;
        PhotonNetwork.SetPlayerCustomProperties(_myCustom);
    }
}
