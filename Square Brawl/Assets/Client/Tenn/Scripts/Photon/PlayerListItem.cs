using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerListItem : MonoBehaviourPunCallbacks
{
    [SerializeField] Text text;
    [SerializeField] Image readyImage;
    [SerializeField] Image masterImage;
    [SerializeField] Button readyBtn;
    [SerializeField] Button unreadyBtn;
    [SerializeField] Button giveMasterBtn;

    bool isReady = false;
    Player player;
    public void SetUp(Player _player)
    {
        player = _player;
        text.text = _player.NickName;

        SetMasterImage(PhotonNetwork.MasterClient);
        SetReady(isReady);

        readyBtn.onClick.AddListener(delegate { SetReadyBtn(true); });
        unreadyBtn.onClick.AddListener(delegate { SetReadyBtn(false); });
        giveMasterBtn.onClick.AddListener(delegate { TransferMaster(); });

        if (!player.IsLocal)
        {
            readyBtn.gameObject.SetActive(false);
            unreadyBtn.gameObject.SetActive(false);
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);

        Debug.Log("ready btn update " + targetPlayer.NickName + " " + player.NickName);
        if (targetPlayer == player)
        {
            object _data;
            if (changedProps.TryGetValue(CustomPropertyCode.READY, out _data))
            {
                SetReady((bool)_data);
            }
        }
    }
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        base.OnMasterClientSwitched(newMasterClient);
        SetMasterImage(newMasterClient);       
    }

    public void SetReadyBtn(bool _isReady)
    {
        SetReady(_isReady);
        //int _playerIndex=
        //LocalDataManager.localPlayers[];

        player.SetCustomProperties(MyPhotonExtension.WrapToHash(new object[] { CustomPropertyCode.READY, isReady }));

    }
    private void SetReady(bool _isReady)
    {
        isReady = _isReady;
        if (isReady)
        {
            readyImage.color = Color.green;
        }
        else
        {
            readyImage.color = Color.white;
        }

        if (player.IsLocal)
        {
            readyBtn.gameObject.SetActive(!_isReady);
            unreadyBtn.gameObject.SetActive(_isReady);
        }
    }
    public void SetMasterImage(Player _newMaster)
    {
        if (_newMaster.IsLocal) //check master's
        {
            if (player.IsMasterClient)
            {
                masterImage.enabled = true;
                giveMasterBtn.gameObject.SetActive(false);
            }
            else
            {
                masterImage.enabled = false;
                giveMasterBtn.gameObject.SetActive(true);
            }
        }
        else //check not-master's player's view
        {
            if (_newMaster == player)
            {
                masterImage.enabled = true;
            }
            else {
                masterImage.enabled = false;
            }
            giveMasterBtn.gameObject.SetActive(false);
        }
    }
    public void TransferMaster()
    {
        PhotonNetwork.SetMasterClient(player);
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (player == otherPlayer)
        {
            Destroy(gameObject);
        }
    }

    public override void OnLeftRoom()
    {
        Destroy(gameObject);
    }
}
