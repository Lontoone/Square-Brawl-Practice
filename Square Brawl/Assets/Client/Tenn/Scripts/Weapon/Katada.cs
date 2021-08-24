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

    private Vector3 _cameraShakeValue;

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

    private void SetColor()//設定顏色
    {
        TrailRenderer _trail = transform.GetChild(0).GetComponent<TrailRenderer>();
        Color _color = transform.GetChild(0).GetComponent<SpriteRenderer>().color =
            _trail.startColor = _trail.endColor = 
            CustomPropertyCode.COLORS[(int)PhotonNetwork.LocalPlayer.CustomProperties[CustomPropertyCode.TEAM_CODE]];

        _pv.RPC("ChangeColor", RpcTarget.Others, new Vector3(_color.r, _color.g, _color.b));
    }

    //發動事件
    public void KatadaEvent(float _speed,float _damage,float _blasticity,bool _isReverse,Vector3 _beShootShake)
    {
        KatadaSpeed = _speed;
        KatadaDamage = _damage;
        KatadaBeElasticity = _blasticity;
        _isKatadaReverse= _isReverse;
        _cameraShakeValue = _beShootShake;
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

        _pv.RPC("Rpc_SetValue", RpcTarget.Others, KatadaDamage, KatadaBeElasticity, _cameraShakeValue);

        StartCoroutine(DestroyObj());
    }

    //Katana Collider
    public void KatanaCollider(PlayerController _playerController)
    {
        if (_pv.IsMine)
        {
            _pv.RPC("Rpc_DisableObj", RpcTarget.All);
            float x = Mathf.Cos(BeElasticityDir * Mathf.PI / 180);
            float y = Mathf.Sin(BeElasticityDir * Mathf.PI / 180);
            _playerController.DamageEvent(KatadaDamage, KatadaBeElasticity, x, y, _cameraShakeValue);
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

    //0.4秒後關閉
    IEnumerator DestroyObj()
    {
        yield return new WaitForSeconds(0.4f);
        gameObject.SetActive(false);
        _pv.RPC("Rpc_DisableObj", RpcTarget.All);
    }

    /// <summary>
    /// RPC
    /// </summary>
    #region -- RPC Event --
    [PunRPC]
    void ChangeColor(Vector3 color)//同步顏色
    {
        Color _color = new Color(color.x, color.y, color.z);
        TrailRenderer _trail = transform.GetChild(0).GetComponent<TrailRenderer>();
        transform.GetChild(0).GetComponent<SpriteRenderer>().color =
            _trail.startColor = _trail.endColor = _color;
    }

    [PunRPC]
    public void Rpc_DisableObj()//同步關閉
    {
        gameObject.SetActive(false);
    }

    [PunRPC]
    public void Rpc_EnableObj(float _dirZ)//同步開啟與對手被反彈的方向數值
    {
        gameObject.SetActive(true);
        BeElasticityDir = _dirZ;
    }

    [PunRPC]
    public void Rpc_ResetPos(Vector3 _pos, Quaternion _dir)//同步Pos Dir
    {
        transform.position = _pos;
        transform.rotation = _dir;
    }

    [PunRPC]
    public void Rpc_SetValue(float _damage, float _elasticity, Vector3 _beShootShake)//同步發動後的數值
    {
        KatadaDamage = _damage;
        KatadaBeElasticity = _elasticity;
        _cameraShakeValue = _beShootShake;
    }
    #endregion

    /// <summary>
    /// 讓用戶自定義的component被Photon View監聽
    /// </summary>
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
