using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateOnlineProperty : MonoBehaviourPunCallbacks
{
    private void Awake()
    {
        LocalPlayerProperty.OnDataUpdate += UpdateOnlineData;
        DontDestroyOnLoad(gameObject);
    }
    private void OnDestroy()
    {
        LocalPlayerProperty.OnDataUpdate -= UpdateOnlineData;
    }
    private void UpdateOnlineData(string _key, object _data, Player _player)
    {
        if (_player != null)
        {
            Debug.Log("Photon " + _player.NickName + " update " + _key);
            _player.SetCustomProperties(MyPhotonExtension.WrapToHash(new object[]
                                                               { _key, _data }));
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
        //copy data to local
        //int _playerIndx = (int)targetPlayer.CustomProperties[CustomPropertyCode.PLAYER_INDEX];

        object _data;
        /*
        if (changedProps.TryGetValue(GameResultManager.KILL, out _data))
        {
            LocalRoomManager.instance.players[_playerIndx].SetProperty(GameResultManager.KILL, _data, false);
        }*/
    }
}
