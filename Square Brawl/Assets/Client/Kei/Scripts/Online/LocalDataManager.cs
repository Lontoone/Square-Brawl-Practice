using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalDataManager
{
    public static List<LocalPlayerProperty> localPlayers = new List<LocalPlayerProperty>();

    public static void AddPlayer(Player _player)
    {
        LocalPlayerProperty newPlayer = new LocalPlayerProperty();
        newPlayer.SetProperty(CustomPropertyCode.PLAYER, _player);
        newPlayer.SetProperty(CustomPropertyCode.PLAYERINDEX, localPlayers.Count);
        localPlayers.Add(newPlayer);
    }

    public static void RemovePlayer(int _player)
    {
        //int _index=_player.CustomProperties[]
        localPlayers.RemoveAt(_player);
    }
}
