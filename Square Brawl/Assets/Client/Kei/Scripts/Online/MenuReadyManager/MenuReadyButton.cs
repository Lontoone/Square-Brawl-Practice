using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuReadyButton : MonoBehaviourPunCallbacks
{
    public bool isReady = false;
    public bool setUnReadyOnEnable = true;
    public UnityEvent OnReady;
    public UnityEvent OnCancelReady;
    public Player player;    

    public override void OnEnable()
    {
        base.OnEnable();

        //SetMyButtonActive(false);

    }
    private void SetMyButtonActive(bool _hide)
    {

        foreach (Transform _child in transform)
        {
            _child.GetComponent<UnityEngine.UI.Button>()?.gameObject.SetActive(_hide);
        }
    }

    public void Init(Player _player)
    {
        Debug.Log("Photon btn " + (_player == PhotonNetwork.LocalPlayer));
        player = _player;
        if (setUnReadyOnEnable)
        {
            SetReady(false);
            SetReadyLocal(false);
        }
        else {
            SetReady(true);
            SetReadyLocal(true);
        }

        if (_player == PhotonNetwork.LocalPlayer)
        {
            SetMyButtonActive(true);
        }
        else
        {
            SetMyButtonActive(false);
        }
        //SetReadyLocal(false);
    }

    public void SetReadyInverse()
    {
        SetReady(!isReady);
    }

    public void SetReady(bool _isReady)
    {
        isReady = _isReady;
        if (isReady)
        {
            OnReady?.Invoke();
        }
        else
        {
            OnCancelReady?.Invoke();
        }
        player.SetCustomProperties(MyPhotonExtension.WrapToHash(new object[] { CustomPropertyCode.READY, isReady }));
    }

    public void SetReadyLocal(bool _isReady)
    {
        isReady = _isReady;
        //TODO:Change UI
        if (isReady)
        {
            OnReady?.Invoke();
        }
        else
        {
            OnCancelReady?.Invoke();
        }
    }
}
