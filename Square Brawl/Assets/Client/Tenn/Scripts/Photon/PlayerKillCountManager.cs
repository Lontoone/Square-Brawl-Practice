using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerKillCountManager : MonoBehaviourPunCallbacks
{
    public Transform playerItemParent;
    public PlayerKillCountItem playerItemPrefab;

    public static PlayerKillCountManager instance;

    public int KillCount;

    private ExitGames.Client.Photon.Hashtable _myCustom = new ExitGames.Client.Photon.Hashtable();

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
        KillCount++;
        _myCustom["KillCount"] = KillCount;
        PhotonNetwork.SetPlayerCustomProperties(_myCustom);
    }
}
