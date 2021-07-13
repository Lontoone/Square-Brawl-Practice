using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Katada : MonoBehaviour,IPoolObject,IPunObservable
{
    public float KatadaSpeed;//Katada Speed
    public float KatadaDamage;//Katada Damage
    public float KatadaBeElasticity;//Katada BeElasticity
    public float BeElasticityDir;//Be Elasticity Direction
    private float _originSpeed;//Origin Speed
    private float _dir;//Katada Mid Direction

    public bool IsKatadaReverse;//Is Katada Reverse?

    private Quaternion _newSyncDir;

    public PhotonView _pv;

    public void OnObjectSpawn()
    {
        if (_pv.IsMine)
        {
            _pv.RPC("Rpc_EnableObj", RpcTarget.All,transform.eulerAngles.z);

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

            _pv.RPC("Rpc_ResetPos", RpcTarget.Others, transform.position, transform.rotation);

            StartCoroutine(DestroyObj());
        }
    }
    void Start()
    {
        _pv = GetComponent<PhotonView>();
        if (_pv.IsMine)
        {
            _pv.RPC("Rpc_DisableObj", RpcTarget.All);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_pv.IsMine)
        {
            if (_originSpeed != KatadaSpeed && _pv.IsMine)
            {
                _pv.RPC("Rpc_SetValue", RpcTarget.Others, KatadaDamage, KatadaBeElasticity);
                _originSpeed = KatadaSpeed;
            }

            if (IsKatadaReverse)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, _dir + 90), Time.deltaTime * KatadaSpeed);
            }
            else
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, _dir - 90), Time.deltaTime * KatadaSpeed);
            }
        }
        else if (!_pv.IsMine)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, _newSyncDir, 15 * Time.deltaTime);
        }

    }

    IEnumerator DestroyObj()
    {
        yield return new WaitForSeconds(0.4f);
        _pv.RPC("Rpc_DisableObj", RpcTarget.All);
    }

    [PunRPC]
    public void EnableObj(float _dirZ)
    {
        gameObject.SetActive(true);
        BeElasticityDir = _dirZ;
    }

    [PunRPC]
    public void ResetPos(Vector3 _pos, Quaternion _dir)
    {
        transform.position = _pos;
        transform.rotation = _dir;
        _newSyncDir = _dir;
    }

    [PunRPC]
    public void Rpc_DisableObj()
    {
        gameObject.SetActive(false);
    }

    [PunRPC]
    public void Rpc_SetValue(float _damage, float _elasticity)
    {
        KatadaDamage = _damage;
        KatadaBeElasticity = _elasticity;
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
