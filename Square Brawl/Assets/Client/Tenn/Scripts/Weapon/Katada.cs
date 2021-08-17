using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Katada : MonoBehaviour,IPoolObject,IPunObservable
{
    private float KatadaSpeed;//Katada Speed
    private float KatadaDamage;//Katada Damage
    private float KatadaBeElasticity;//Katada BeElasticity
    private float BeElasticityDir;//Be Elasticity Direction
    private float _dir;//Katada Mid Direction

    private Vector3 _beShootShakeValue;

    private bool _isKatadaReverse;//Is Katada Reverse?

    private Quaternion _newSyncDir;

    public PhotonView _pv;

    void Start()
    {
        _pv = GetComponent<PhotonView>();

        if (_pv.IsMine)
        {
            SetColor();
            _pv.RPC("Rpc_DisableObj", RpcTarget.All);
        }
    }

    public void OnObjectSpawn()
    {
        if (_pv.IsMine)
        {
            _pv.RPC("Rpc_EnableObj", RpcTarget.All,transform.eulerAngles.z);
            _pv.RPC("Rpc_ResetPos", RpcTarget.Others, transform.position, transform.rotation);
        }
    }

    private void SetColor()
    {
        TrailRenderer _trail = transform.GetChild(0).GetComponent<TrailRenderer>();
        Color _color = transform.GetChild(0).GetComponent<SpriteRenderer>().color =
            _trail.startColor = _trail.endColor = 
            CustomPropertyCode.COLORS[(int)PhotonNetwork.LocalPlayer.CustomProperties[CustomPropertyCode.TEAM_CODE]];

        _pv.RPC("ChangeColor", RpcTarget.Others, new Vector3(_color.r, _color.g, _color.b));
    }

    public void KatadaEvent(float _speed,float _damage,float _blasticity,bool _isReverse,Vector3 _beShootShake)
    {
        KatadaSpeed = _speed;
        KatadaDamage = _damage;
        KatadaBeElasticity = _blasticity;
        _isKatadaReverse= _isReverse;
        _beShootShakeValue = _beShootShake;
        if (_isKatadaReverse)
        {
            _dir = transform.eulerAngles.z + 45;
            transform.eulerAngles = new Vector3(0, 0, _dir);
        }
        else
        {
            _dir = transform.eulerAngles.z - 45;
            transform.eulerAngles = new Vector3(0, 0, _dir);
        }

        _pv.RPC("Rpc_SetValue", RpcTarget.Others, KatadaDamage, KatadaBeElasticity, _beShootShakeValue);

        StartCoroutine(DestroyObj());
    }

    public void KatadaCollider(PlayerController _playerController)
    {
        if (_pv.IsMine)
        {
            _pv.RPC("Rpc_DisableObj", RpcTarget.All);
            float x = Mathf.Cos(BeElasticityDir * Mathf.PI / 180);
            float y = Mathf.Sin(BeElasticityDir * Mathf.PI / 180);
            _playerController.DamageEvent(KatadaDamage, KatadaBeElasticity, x, y, _beShootShakeValue);
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
        if (_pv.IsMine)
        {
            if (!_isKatadaReverse)
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
        gameObject.SetActive(false);
        _pv.RPC("Rpc_DisableObj", RpcTarget.All);
    }

    [PunRPC]
    void ChangeColor(Vector3 color)
    {
        Color _color = new Color(color.x, color.y, color.z);
        TrailRenderer _trail = transform.GetChild(0).GetComponent<TrailRenderer>();
        transform.GetChild(0).GetComponent<SpriteRenderer>().color =
            _trail.startColor = _trail.endColor = _color;
    }

    [PunRPC]
    public void Rpc_EnableObj(float _dirZ)
    {
        gameObject.SetActive(true);
        BeElasticityDir = _dirZ;
    }

    [PunRPC]
    public void Rpc_ResetPos(Vector3 _pos, Quaternion _dir)
    {
        transform.position = _pos;
        transform.rotation = _dir;
    }

    [PunRPC]
    public void Rpc_SetValue(float _damage, float _elasticity, Vector3 _beShootShake)
    {
        KatadaDamage = _damage;
        KatadaBeElasticity = _elasticity;
        _beShootShakeValue = _beShootShake;
    }

    [PunRPC]
    public void Rpc_DisableObj()
    {
        gameObject.SetActive(false);
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
