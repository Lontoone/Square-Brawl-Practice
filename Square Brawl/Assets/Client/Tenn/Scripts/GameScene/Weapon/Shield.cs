using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Shield : MonoBehaviour,IPoolObject
{
    public float ShieldSpeed;//Shield Speed
    public float ShieldDamage;//Shield Damage
    public float ShieldBeElasticity;//Shield BeElasticity

    private bool _isReverse;

    private Vector3 _newSyncScale;
    private Animator _anim;

    public PhotonView _pv;

    void Awake()
    {
        _pv = GetComponent<PhotonView>();
    }

    public void OnObjectSpawn()
    {
        _pv.RPC("Rpc_EnableObj", RpcTarget.Others, ShieldSpeed, ShieldDamage, ShieldBeElasticity);
        StartCoroutine(DestroyObj());
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
