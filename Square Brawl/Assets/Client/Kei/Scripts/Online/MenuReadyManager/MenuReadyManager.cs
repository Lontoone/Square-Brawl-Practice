using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class MenuReadyManager : MonoBehaviourPunCallbacks
{
    public UnityEvent OnAllReady;
    public UnityEvent OnAllReadyMaster;

    public UnityEvent OnReadyCancel;
    public UnityEvent OnReadyCancelMaster;
    private MenuReadyButton[] allReadyButtons { get { return FindObjectsOfType<MenuReadyButton>(); } }
    public void CheckAllReady()
    {
        bool _isAllReady = true;
        foreach (MenuReadyButton _btn in allReadyButtons)
        {
            if (!_btn.isReady)
            {
                _isAllReady = false;
                break;
            }
        }

        if (_isAllReady)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                OnAllReadyMaster?.Invoke();
            }
            OnAllReady?.Invoke();
        }
        else
        {
            if (PhotonNetwork.IsMasterClient)
            {
                OnReadyCancelMaster?.Invoke();
            }
            OnReadyCancel?.Invoke();
        }
    }

    public void UpdateReadyButton(Player _target, bool _isReady)
    {
        foreach (MenuReadyButton _btn in allReadyButtons)
        {
            if (_btn.player == _target)
            {
                _btn.SetReadyLocal(_isReady);
            }
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        //if any player update their ready button:
        object _data;
        if (changedProps.TryGetValue(CustomPropertyCode.READY, out _data))
        {
            bool _isReady = (bool)_data;

            UpdateReadyButton(targetPlayer, _isReady);
            CheckAllReady();
        }
    }

    public void UnReadyAllButtons()
    {
        foreach (MenuReadyButton _btn in allReadyButtons)
        {
            _btn.SetReady(false);
        }
    }
}
