using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Grenade : MonoBehaviour, IPoolObject, IPunObservable
{
    [HeaderAttribute("Bullet Setting")]
    protected float GrenadeSpeed;//Grenade Speed
    protected float GrenadeDamage;//Grenade Damage
    protected float GrenadeBeElasticity;//Grenade Be Elasticity
    protected float GrenadeScaleValue;//Grenade Scale Value
    [SerializeField]
    protected float FieldExplose;//Explose Field

    protected bool isMaster;

    protected Vector3 _beShotShakeValue;

    protected string ExploseEffectName;

    public LayerMask LayerToExplose;

    protected Rigidbody2D _rb;
    protected GameObject _childObj;

    [HeaderAttribute("Sync Setting")]
    protected PhotonView _pv;

    protected Vector2 _networkPosition;
    protected Vector2 _beginPos;

    protected Vector3 _childObjnetworkPosition;
    protected Vector3 _childObjnetworkScale;
    protected Quaternion _networkDir;

    protected virtual void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _pv = GetComponent<PhotonView>();
        if (_pv.IsMine)
        {
            SetColor();
            _pv.RPC("Rpc_DisableObj", RpcTarget.All);
        }
    }

    private void SetColor()
    {
        Color _color = transform.GetComponent<SpriteRenderer>().color =
            CustomPropertyCode.COLORS[(int)PhotonNetwork.LocalPlayer.CustomProperties[CustomPropertyCode.TEAM_CODE]];

        _pv.RPC("ChangeColor", RpcTarget.Others, new Vector3(_color.r, _color.g, _color.b));
    }

    public void OnObjectSpawn()
    {
        if (_pv.IsMine)
        {
            _pv.RPC("Rpc_EnableObj", RpcTarget.All);
            _pv.RPC("Rpc_ResetPos", RpcTarget.Others, transform.position, transform.rotation);
            StartCoroutine(Boom());
        }
    }

    public void GrenadeEvent(string _effectName,float _speed,float _damage,float _beElasticity,Vector3 _beShotShake)
    {
        ExploseEffectName = _effectName;
        GrenadeSpeed = _speed;
        GrenadeDamage = _damage;
        GrenadeBeElasticity = _beElasticity;
        _beShotShakeValue = _beShotShake;
        _pv.RPC("Rpc_SetValue", RpcTarget.All, GrenadeSpeed, GrenadeDamage, GrenadeScaleValue, GrenadeBeElasticity, _beShotShakeValue);
        _rb.AddForce(GrenadeSpeed * transform.right);
        isMaster = true;
    }

    protected virtual void FixedUpdate()
    {
        if (!_pv.IsMine)
        {
            _rb.position = Vector2.Lerp(_rb.position, _networkPosition, 5 * Time.fixedDeltaTime);
            transform.rotation = _networkDir;
        }
    }

    IEnumerator Boom()
    {
        yield return new WaitForSeconds(1f);
        _pv.RPC("Rpc_Explose", RpcTarget.All);
        if (_pv.IsMine)
        {
            ObjectsPool.Instance.SpawnFromPool(ExploseEffectName, transform.position, transform.rotation, null);
        }
        yield return new WaitForSeconds(0.1f);
        _pv.RPC("Rpc_DisableObj", RpcTarget.All);
    }

    private void Explose()
    {
        Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, FieldExplose, LayerToExplose);
        foreach (Collider2D obj in objects)
        {
            PlayerController _playerController = obj.GetComponent<PlayerController>();
            if (isMaster == _playerController.Pv.IsMine&& isMaster)
            {
                _playerController.BeExplode(GrenadeBeElasticity, transform.position, FieldExplose);
            }

            if (isMaster != _playerController.Pv.IsMine && !_playerController.Pv.IsMine)
            {
                _playerController.TakeDamage(GrenadeDamage, _beShotShakeValue.x, _beShotShakeValue.y, _beShotShakeValue.z);
                _playerController.BeExplode(GrenadeBeElasticity, transform.position, FieldExplose);
                var IsKill = _playerController.IsKillAnyone();
                if (IsKill)
                {
                    PlayerKillCountManager.instance.SetKillCount();
                }
            }
        }
    }

    protected virtual void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController _playerController = other.gameObject.GetComponent<PlayerController>();
            if (_playerController.IsShield && !isMaster)
            {
                Vector2 dir = _playerController.transform.position - gameObject.transform.position;
                _pv.RPC("Rpc_ChangeAngles", RpcTarget.Others, dir);
                isMaster = true;
            }
        }
    }

    [PunRPC]
    void ChangeColor(Vector3 color)
    {
        Color _color = new Color(color.x, color.y, color.z);
        transform.GetComponent<SpriteRenderer>().color = _color;
    }

    [PunRPC]
    public void Rpc_SetValue(float _speed, float _damage, float _scaleValue, float _elasticity,Vector3 _beShotShake)
    {
        GrenadeSpeed = _speed;
        GrenadeDamage = _damage;
        GrenadeScaleValue = _scaleValue;
        GrenadeBeElasticity = _elasticity;
        _beShotShakeValue = _beShotShake;
    }

    [PunRPC]
    public void Rpc_Explose()
    {
        Explose();
    }

    [PunRPC]
    public void Rpc_DisableObj()
    {
        gameObject.SetActive(false);
    }

    [PunRPC]
    public void Rpc_EnableObj()
    {
        gameObject.SetActive(true);
    }

    [PunRPC]
    public void Rpc_ResetPos(Vector3 pos, Quaternion dir)
    {
        transform.position = pos;
        transform.rotation = dir;
        _networkPosition = pos;
        isMaster = false;
    }

    [PunRPC]
    public void Rpc_ChangeAngles(Vector2 _dir)
    {
        _rb.AddForce(-1 * GrenadeSpeed * _dir);
        isMaster = false;
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_rb.position);
            stream.SendNext(_rb.velocity);
            stream.SendNext(transform.rotation);
            if (_childObj != null)
            {
                stream.SendNext(_childObj.transform.localPosition);
                stream.SendNext(_childObj.transform.localScale);
            }
        }
        else
        {
            _networkPosition = (Vector2)stream.ReceiveNext();
            _rb.velocity = (Vector2)stream.ReceiveNext();
            _networkDir = (Quaternion)stream.ReceiveNext();
            if (_childObj != null)
            {
                _childObjnetworkPosition = (Vector3)stream.ReceiveNext();
                _childObjnetworkScale = (Vector3)stream.ReceiveNext();
            }
            float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));// + (float)(PhotonNetwork.GetPing() * 0.001f);
            _networkPosition += (_rb.velocity * lag);
        }
    }
}
