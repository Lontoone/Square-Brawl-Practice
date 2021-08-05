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


    }

    public void Init(Player _player)
    {
        player = _player;
        if (setUnReadyOnEnable)
        {
            SetReady(false);
        }
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
