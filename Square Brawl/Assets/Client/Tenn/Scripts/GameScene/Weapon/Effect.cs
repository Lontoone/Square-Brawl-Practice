using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Effect : MonoBehaviour,IPoolObject
{
    private ParticleSystem _effect;
    private PhotonView _pv;
    public void OnObjectSpawn()
    {
        _pv.RPC("EnableObj", RpcTarget.All,transform.position,transform.rotation);
        _effect.Play();
        StartCoroutine(DisableObject());
    }

    void Start()
    {
        _pv = GetComponent<PhotonView>();
        _effect = GetComponentInChildren<ParticleSystem>();
        if (_pv.IsMine)
        {
            _pv.RPC("DisableObj", RpcTarget.All);
        }
    }

    IEnumerator DisableObject()
    {
        yield return new WaitForSeconds(2f);
        _pv.RPC("DisableObj", RpcTarget.All);
    }

    [PunRPC]
    public void DisableObj()
    {
        gameObject.SetActive(false);
    }

    [PunRPC]
    public void EnableObj(Vector3 _pos,Quaternion _dir)
    {
        gameObject.SetActive(true);
        transform.position = _pos;
        transform.rotation = _dir;
    }
}
