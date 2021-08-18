using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WhenBackToMenu : MonoBehaviour
{
    public UnityEvent BackWithConnectedEvent;
    public UnityEvent BackWithDisconnectedEvent;
    void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            BackWithConnectedEvent?.Invoke();
        }
        else
        {
            BackWithDisconnectedEvent?.Invoke();
        }
    }

}
