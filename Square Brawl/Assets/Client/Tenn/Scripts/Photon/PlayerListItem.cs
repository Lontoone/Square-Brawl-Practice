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
    [SerializeField] Button giveMasterBtn;

    public MenuReadyButton readyButton;

    bool isReady = false;
    Player player;    
    public void SetUp(Player _player)
    {
        player = _player;
        text.text = _player.NickName;

        SetMasterImage(PhotonNetwork.MasterClient);
        //SetReady(isReady);
        readyButton.Init(_player);

        giveMasterBtn.onClick.AddListener(delegate { TransferMaster(); });

    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        base.OnMasterClientSwitched(newMasterClient);
        SetMasterImage(newMasterClient);       
    }

    public void SetMasterImage(Player _newMaster)
    {
        if (_newMaster.IsLocal) //check master's
        {
            if (player.IsMasterClient)
            {
                //masterImage.enabled = true;
                masterImage.gameObject.SetActive(true);
                giveMasterBtn.gameObject.SetActive(false);
            }
            else
            {
                //masterImage.enabled = false;
                masterImage.gameObject.SetActive(false);
                giveMasterBtn.gameObject.SetActive(true);
            }
        }
        else //check not-master's player's view
        {
            if (_newMaster == player)
            {
                //masterImage.enabled = true;
                masterImage.gameObject.SetActive(true);
            }
            else {
                //masterImage.enabled = false;
                masterImage.gameObject.SetActive(false);
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
