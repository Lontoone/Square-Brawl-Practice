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
    private Vector3 _beShotShakeValue;

    public Action<float,float,float,Vector3> ShieldFunc;
    public Action<PlayerController> ColliderFunc;

    private PhotonView _pv;

    void Awake()
    {
        _pv = GetComponent<PhotonView>();
        ShieldFunc = ShieldEvent;
        ColliderFunc = ShieldCollider;
        if (_pv.IsMine)
        {
            SetColor();
            //PlayerController.instance.OnChangeColor += SetColor;
            _pv.RPC("Rpc_DisableObj", RpcTarget.All);
        }
    }

    private void SetColor()
    {
        Color _color = transform.GetComponent<SpriteRenderer>().color =
            CustomPropertyCode.COLORS[(int)PhotonNetwork.LocalPlayer.CustomProperties[CustomPropertyCode.TEAM_CODE]];

        _pv.RPC("ChangeColor", RpcTarget.Others, new Vector3(_color.r, _color.g, _color.b));
    }

    [PunRPC]
    void ChangeColor(Vector3 color)
    {
        Color _color = new Color(color.x, color.y, color.z);
        transform.GetComponent<SpriteRenderer>().color = _color;
    }

    /*public void OnObjectSpawn()
    {
        _pv.RPC("Rpc_EnableObj", RpcTarget.Others, ShieldSpeed, ShieldDamage, ShieldBeElasticity);
    }*/

    private void ShieldEvent(float _speed,float _damage,float _beElasticity,Vector3 _beShotShake)
    {
        ShieldSpeed = _speed;
        ShieldDamage = _damage;
        ShieldBeElasticity = _beElasticity;
        _beShotShakeValue = _beShotShake;
        StartCoroutine(DestroyObj());
        _pv.RPC("Rpc_EnableObj", RpcTarget.Others, ShieldSpeed, ShieldDamage, ShieldBeElasticity, _beShotShakeValue);
    }

    private void ShieldCollider(PlayerController _playerController)
    {
        if (!_pv.IsMine)
        {
            Vector2 dir = _playerController.transform.position - gameObject.transform.position;
            _playerController.DamageFunc(ShieldDamage, ShieldBeElasticity, dir.x, dir.y, _beShotShakeValue);
        }
    }

    void Update()
    {
        transform.eulerAngles = new Vector3(0, 0, 0);
    }

    IEnumerator DestroyObj()
    {
        yield return new WaitForSeconds(0.5f);
        _pv.RPC("Rpc_DisableObj", RpcTarget.All);
    }
    [PunRPC]
    public void Rpc_DisableObj()
    {
        gameObject.SetActive(false);
        GetComponentInParent<PlayerController>().IsShield = false;
    }

     [PunRPC]
     public void Rpc_EnableObj(float _speed,float _damage,float _elasticity,Vector3 _beShotShake)
     {
         gameObject.SetActive(true);
        ShieldSpeed = _speed;
        ShieldDamage = _damage;
        ShieldBeElasticity = _elasticity;
        _beShotShakeValue = _beShotShake;
     }

}
