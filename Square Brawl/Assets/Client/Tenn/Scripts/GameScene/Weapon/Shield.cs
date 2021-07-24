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

    public Action<float,float,float> ShieldFunc;
    public Action<PlayerController> ColliderFunc;

    private PhotonView _pv;

    void Awake()
    {
        _pv = GetComponent<PhotonView>();
        ShieldFunc = ShieldEvent;
        ColliderFunc = ShieldCollider;
    }

    /*public void OnObjectSpawn()
    {
        _pv.RPC("Rpc_EnableObj", RpcTarget.Others, ShieldSpeed, ShieldDamage, ShieldBeElasticity);
    }*/

    private void ShieldEvent(float _speed,float _damage,float _beElasticity)
    {
        ShieldSpeed = _speed;
        ShieldDamage = _damage;
        ShieldBeElasticity = _beElasticity;
        StartCoroutine(DestroyObj());
        _pv.RPC("Rpc_EnableObj", RpcTarget.Others, ShieldSpeed, ShieldDamage, ShieldBeElasticity);
    }

    private void ShieldCollider(PlayerController _playerController)
    {
        if (!_pv.IsMine)
        {
            Vector2 dir = _playerController.transform.position - gameObject.transform.position;
            _playerController.DamageFunc(ShieldDamage, ShieldBeElasticity, dir.x, dir.y);
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
     public void Rpc_EnableObj(float _speed,float _damage,float _elasticity)
     {
         gameObject.SetActive(true);
        ShieldSpeed = _speed;
        ShieldDamage = _damage;
        ShieldBeElasticity = _elasticity;
     }

}
