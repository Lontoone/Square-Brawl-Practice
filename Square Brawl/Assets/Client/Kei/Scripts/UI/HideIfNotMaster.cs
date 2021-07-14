using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideIfNotMaster : MonoBehaviourPunCallbacks
{    
    void Start()
    {
        if (!PhotonNetwork.IsMasterClient) {
            gameObject.SetActive(false);
        }    
    }
    /*
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        //base.OnMasterClientSwitched(newMasterClient);
        if (newMasterClient == PhotonNetwork.LocalPlayer) {

        }
    }*/
}
