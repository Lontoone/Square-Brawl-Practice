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
    [HideInInspector]
    public bool isLocal = false;

    public override void OnEnable()
    {
        base.OnEnable();

        //SetMyButtonActive(false);

    }
    private void SetMyButtonActive(bool _hide)
    {
        foreach (Transform _child in transform)
        {
            UnityEngine.UI.Button btn = _child.GetComponent<UnityEngine.UI.Button>();
            if (btn != null)
                btn.interactable = _hide;
            Debug.Log("child " + _child.name + " " + btn.interactable);
        }
    }

    public void Init(Player _player)
    {
        Debug.Log("Photon btn " + (_player == PhotonNetwork.LocalPlayer));
        player = _player;


        if (_player == PhotonNetwork.LocalPlayer)
        {
            isLocal = true;

            if (setUnReadyOnEnable)
            {
                SetReadyWithoutAudio(false);
                //SetReadyLocal(false);
            }
            else
            {
                SetReadyWithoutAudio(true);
                //SetReadyLocal(true);
            }
            SetMyButtonActive(true);
        }
        else
        {
            SetMyButtonActive(false);
            //Debug.Log(player.NickName + " ready " + (bool)player.CustomProperties[CustomPropertyCode.READY]);
            SetReadyLocalWithoutAudio((bool)player.CustomProperties[CustomPropertyCode.READY]);
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
            AudioSourcesManager.PlaySFX(0);
            OnReady?.Invoke();
        }
        else
        {
            AudioSourcesManager.PlaySFX(1);
            OnCancelReady?.Invoke();
        }
        player.SetCustomProperties(MyPhotonExtension.WrapToHash(new object[] { CustomPropertyCode.READY, isReady }));
    }

    public void SetReadyWithoutAudio(bool _isReady)
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

    public void ColorSetReady(bool _isReady)
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties[CustomPropertyCode.TEAM_CODE] == null)
        {
            return;
        }
        SetReady(_isReady);
    }

    public void WeaponSetReady(bool _isReady)
    {
        if (((int)PhotonNetwork.LocalPlayer.CustomProperties[CustomPropertyCode.WEAPON1CODE] == 0
            || (int)PhotonNetwork.LocalPlayer.CustomProperties[CustomPropertyCode.WEAPON2CODE] == 0))
        {
            return;
        }
        SetReady(_isReady);
    }

    public void SetReadyLocal(bool _isReady)
    {
        isReady = _isReady;
        //TODO:Change UI
        if (isReady)
        {
            AudioSourcesManager.PlaySFX(0);
            OnReady?.Invoke();
        }
        else
        {
            AudioSourcesManager.PlaySFX(1);
            OnCancelReady?.Invoke();
        }
    }

    public void SetReadyLocalWithoutAudio(bool _isReady)
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
    }
}
