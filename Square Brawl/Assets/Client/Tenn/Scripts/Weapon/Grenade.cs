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
    protected bool _isBounceBullet;

    protected Vector3 _cameraShakeValue;

    protected string ExploseEffectName;

    public LayerMask LayerToExplose;

    protected Rigidbody2D _rb;
    protected GameObject _childObj;

    protected Color _effectChangeColor;

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

    private void SetColor()//設定Grenade顏色
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
            StartCoroutine(Boom());
        }
    }

    // 發射Grenade
    public void GrenadeEvent(string _effectName,float _speed,float _damage,float _scaleValue,float _beElasticity,Vector3 _beShootShake)
    {
        ExploseEffectName = _effectName;
        GrenadeSpeed = _speed;
        GrenadeDamage = _damage;
        GrenadeScaleValue = _scaleValue;
        transform.localScale = new Vector3(_scaleValue, _scaleValue, _scaleValue);
        GrenadeBeElasticity = _beElasticity;
        _cameraShakeValue = _beShootShake;
        if (_isBounceBullet)
        {
            SetColor();
            _isBounceBullet = false;
        }
        _pv.RPC("Rpc_ResetPos", RpcTarget.Others, transform.position, transform.rotation);
        _pv.RPC("Rpc_SetValue", RpcTarget.All, GrenadeSpeed, GrenadeDamage, GrenadeScaleValue, GrenadeBeElasticity, _cameraShakeValue);
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

    //一秒後爆炸
    IEnumerator Boom()
    {
        yield return new WaitForSeconds(1f);
        _pv.RPC("Rpc_Explose", RpcTarget.All);
        if (_pv.IsMine)
        {
            if (_isBounceBullet)//如果是反彈後(Shield)
            {
                GameObject obj = ObjectsPool.Instance.SpawnFromPool(ExploseEffectName, transform.position, transform.rotation, null);
                Effect effect = obj.GetComponent<Effect>();
                effect.ChangeColor(_effectChangeColor);
            }
            else //如果不是反彈後(Shield)
            {
                ObjectsPool.Instance.SpawnFromPool(ExploseEffectName, transform.position, transform.rotation, null);
            }
        }
        yield return new WaitForSeconds(0.1f);
        _pv.RPC("Rpc_DisableObj", RpcTarget.All);
    }

    private void Explose()//爆炸事件
    {
        Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, FieldExplose, LayerToExplose);
        foreach (Collider2D obj in objects)
        {
            PlayerController _playerController = obj.GetComponent<PlayerController>();
            if (isMaster == _playerController.Pv.IsMine&& isMaster)//如果是Master
            {
                _playerController.BeExplode(GrenadeBeElasticity, transform.position, FieldExplose);
            }

            if (isMaster != _playerController.Pv.IsMine && !_playerController.Pv.IsMine)//如果不是Master
            {
                _playerController.TakeDamage(GrenadeDamage, _cameraShakeValue.x, _cameraShakeValue.y, _cameraShakeValue.z);
                _playerController.BeExplode(GrenadeBeElasticity, transform.position, FieldExplose);
                var IsKill = _playerController.IsKillAnyone();
                if (IsKill)
                {
                    PlayerKillCountManager.instance.SetKillCount();
                    _playerController.GenerateDieEffect();
                }
            }
        }
    }

    protected virtual void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))//碰到玩家
        {
            PlayerController _playerController = other.gameObject.GetComponent<PlayerController>();
            if (_playerController.IsShield && !isMaster)//如果玩家有發動Shield
            {
                Vector2 dir = _playerController.transform.position - gameObject.transform.position;
                _effectChangeColor = CustomPropertyCode.COLORS[(int)PhotonNetwork.LocalPlayer.CustomProperties[CustomPropertyCode.TEAM_CODE]];
                _pv.RPC("Rpc_ChangeAngles", RpcTarget.Others, dir, new Vector3(_effectChangeColor.r, _effectChangeColor.g, _effectChangeColor.b));
                SetColor();
                isMaster = _isBounceBullet = true;
            }
        }
    }


    /// <summary>
    /// RPC
    /// </summary>
    #region -- RPC Event --
    [PunRPC]
    void ChangeColor(Vector3 color)//同步顏色
    {
        Color _color = new Color(color.x, color.y, color.z);
        transform.GetComponent<SpriteRenderer>().color = _color;
    }

    //同步發射後的數值
    [PunRPC]
    protected void Rpc_SetValue(float _speed, float _damage, float _scaleValue, float _elasticity,Vector3 _beShotShake)
    {
        GrenadeSpeed = _speed;
        GrenadeDamage = _damage;
        GrenadeScaleValue = _scaleValue;
        transform.localScale = new Vector3(GrenadeScaleValue, GrenadeScaleValue, GrenadeScaleValue);
        GrenadeBeElasticity = _elasticity;
        _cameraShakeValue = _beShotShake;
    }

    [PunRPC]
    public void Rpc_Explose()//同步爆炸
    {
        Explose();
    }

    [PunRPC]
    protected void Rpc_DisableObj()//同步關閉
    {
        gameObject.SetActive(false);
    }

    [PunRPC]
    protected void Rpc_EnableObj()//同步開啟
    {
        gameObject.SetActive(true);
    }

    [PunRPC]
    protected void Rpc_ResetPos(Vector3 pos, Quaternion dir)//同步Pos Dir
    {
        transform.position = pos;
        transform.rotation = dir;
        _networkPosition = pos;
        isMaster = _isBounceBullet = false;
    }

    [PunRPC]
    void Rpc_ChangeAngles(Vector2 _dir,Vector3 _color)//同步改變角度
    {
        _effectChangeColor = new Color(_color.x, _color.y, _color.z);
        _rb.AddForce(-1 * GrenadeSpeed * _dir);
        isMaster = false;
        _isBounceBullet = true;
    }
    #endregion

    /// <summary>
    /// 讓用戶自定義的component被Photon View監聽
    /// </summary>
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
            float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime)) + (float)(PhotonNetwork.GetPing() * 0.0001f);
            _networkPosition += (_rb.velocity * lag);
        }
    }
}
