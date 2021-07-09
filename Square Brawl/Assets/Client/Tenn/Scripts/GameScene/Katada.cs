using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Katada : MonoBehaviour,IPoolObject,IPunObservable
{
    public float Speed;
    private float _originSpeed;
    public float Damage;
    public float _beElasticityDir;
    private float _dir;
    public float BeAttackElasticity;

    public bool IsKatadaReverse;

    private Quaternion _newSyncDir;

    public PhotonView _pv;

    public void OnObjectSpawn()
    {
        if (_pv.IsMine)
        {
            _pv.RPC("EnableObj", RpcTarget.All,transform.eulerAngles.z);
            if (IsKatadaReverse)
            {
                _dir = transform.eulerAngles.z + 45;
                transform.eulerAngles = new Vector3(0, 0, _dir);
            }
            else
            {
                _dir = transform.eulerAngles.z - 45;
                transform.eulerAngles = new Vector3(0, 0, _dir);
            }
            _pv.RPC("ResetPos", RpcTarget.Others, transform.position, transform.rotation);
            StartCoroutine(DestroyObj());
        }
    }
    void Start()
    {
        _pv = GetComponent<PhotonView>();
        if (_pv.IsMine)
        {
            _pv.RPC("DisableObj", RpcTarget.All);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_pv.IsMine)
        {
            if (_originSpeed != Speed && _pv.IsMine)
            {
                _pv.RPC("SetStatus", RpcTarget.Others, Damage, BeAttackElasticity);
                _originSpeed = Speed;
            }

            if (IsKatadaReverse)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, _dir + 90), Time.deltaTime * Speed);
            }
            else
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, _dir - 90), Time.deltaTime * Speed);
            }
        }
        else if (!_pv.IsMine)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, _newSyncDir, 30 * Time.deltaTime);
        }

    }

    IEnumerator DestroyObj()
    {
        yield return new WaitForSeconds(0.4f);
        _pv.RPC("DisableObj", RpcTarget.All);
    }

    [PunRPC]
    public void EnableObj(float _dirZ)
    {
        gameObject.SetActive(true);
        _beElasticityDir = _dirZ;
    }

    [PunRPC]
    public void ResetPos(Vector3 _pos, Quaternion _dir)
    {
        transform.position = _pos;
        transform.rotation = _dir;
        _newSyncDir = _dir;
    }

    [PunRPC]
    public void DisableObj()
    {
        gameObject.SetActive(false);
    }

    [PunRPC]
    public void SetStatus(float _damage, float _elasticity)
    {
        Damage = _damage;
        BeAttackElasticity = _elasticity;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.rotation);
        }
        else
        {
            _newSyncDir = (Quaternion)stream.ReceiveNext();
            
        }
    }
}
