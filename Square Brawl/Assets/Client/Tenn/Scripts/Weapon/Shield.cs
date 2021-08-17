using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Shield : MonoBehaviour
{
    private float ShieldSpeed;//Shield Speed
    private float ShieldDamage;//Shield Damage
    private float ShieldBeElasticity;//Shield BeElasticity
    private Vector3 _cameraShakeValue;

    private PhotonView _pv;

    void Awake()
    {
        _pv = GetComponent<PhotonView>();
        if (_pv.IsMine)
        {
            SetColor();
        }
    }

    private void SetColor()
    {
        Color _color = transform.GetComponent<SpriteRenderer>().color =
            CustomPropertyCode.COLORS[(int)PhotonNetwork.LocalPlayer.CustomProperties[CustomPropertyCode.TEAM_CODE]];

        _pv.RPC("ChangeColor", RpcTarget.Others, new Vector3(_color.r, _color.g, _color.b));
        _pv.RPC("Rpc_DisableObj", RpcTarget.All);
    }

    public void ShieldEvent(float _speed,float _damage,float _beElasticity,Vector3 _cameraShakeValue)
    {
        ShieldSpeed = _speed;
        ShieldDamage = _damage;
        ShieldBeElasticity = _beElasticity;
        this._cameraShakeValue = _cameraShakeValue;
        Invoke("DestroyObj",0.8f);
        _pv.RPC("Rpc_EnableObj", RpcTarget.Others, ShieldSpeed, ShieldDamage, ShieldBeElasticity, this._cameraShakeValue);
    }

    public void ShieldCollider(PlayerController _playerController)
    {
        if (_pv.IsMine)
        {
            Vector2 dir = _playerController.transform.position - gameObject.transform.position;
            _playerController.DamageEvent(ShieldDamage, ShieldBeElasticity, dir.x, dir.y, _cameraShakeValue);
            _playerController.IsBeShieldTrue();
            var IsKill = _playerController.IsKillAnyone();
            if (IsKill)
            {
                PlayerKillCountManager.instance.SetKillCount();
                _playerController.GenerateDieEffect();
            }
        }
    }

    void Update()
    {
        transform.eulerAngles = Vector3.zero;
    }

    void DestroyObj()
    {
        _pv.RPC("Rpc_DisableObj", RpcTarget.All);
    }

    [PunRPC]
    void ChangeColor(Vector3 color)
    {
        Color _color = new Color(color.x, color.y, color.z);
        transform.GetComponent<SpriteRenderer>().color = _color;
    }

    [PunRPC]
    public void Rpc_DisableObj()
    {
        gameObject.SetActive(false);
        GetComponentInParent<PlayerController>().IsBeShield = false;
    }

     [PunRPC]
     public void Rpc_EnableObj(float _speed,float _damage,float _elasticity,Vector3 _beShotShake)
     {
         gameObject.SetActive(true);
        ShieldSpeed = _speed;
        ShieldDamage = _damage;
        ShieldBeElasticity = _elasticity;
        _cameraShakeValue = _beShotShake;
     }

}
