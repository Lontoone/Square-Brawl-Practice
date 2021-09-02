using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelectManager : MonoBehaviourPunCallbacks
{
    public Transform playerItemContainer;
    public WeaponSelectPlayerItem playerItemPrefab;

    //public Transform buttonItemContainer;
    //public WeaponSelectBtn buttonItemPrefab;

    private void Start()
    {
        //CreatePlayerItem();
    }

    public override void OnEnable()
    {
        base.OnEnable();
        CreatePlayerItem();
        PhotonNetwork.AddCallbackTarget(this);
    }


    private void CreatePlayerItem()
    {
        foreach (Transform child in playerItemContainer)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            {
                //foreach player data:
                WeaponSelectPlayerItem _item = Instantiate(playerItemPrefab, parent: playerItemContainer);
                Player _player = PhotonNetwork.PlayerList[i];
                Debug.Log(_player == null);
                _item.SetPlayer(_player);
            }
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        object _data;
        //if someone updates it's weapon
        //  =>find the matching player item and update
        if (changedProps.TryGetValue(CustomPropertyCode.WEAPON1CODE, out _data))
        {
            WeaponSelectPlayerItem _item = FindObjectsOfType<WeaponSelectPlayerItem>().ToList().Find(x => x.player == targetPlayer);
            if (_item == null)
            {
                Debug.Log("Cant find the item");
                return;
            }
            _item.SetWeapon1((WeaponType)_data);

        }
        else if (changedProps.TryGetValue(CustomPropertyCode.WEAPON2CODE, out _data))
        {
            WeaponSelectPlayerItem _item = FindObjectsOfType<WeaponSelectPlayerItem>().ToList().Find(x => x.player == targetPlayer);
            if (_item == null)
            {
                Debug.Log("Cant find the item");
                return;
            }
            _item.SetWeapon2((WeaponType)_data);

        }
    }

}
