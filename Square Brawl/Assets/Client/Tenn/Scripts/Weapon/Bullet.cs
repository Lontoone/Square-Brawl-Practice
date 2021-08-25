using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviour, IPoolObject,IPunObservable
{
    [HeaderAttribute("Bullet Setting")]
    private float BulletSpeed;//Bullet Speed
    [SerializeField] private float BulletDamage;//Bullet Damage
    private float BulletBeElasticity;//Bullet Be Elasticity
    private float BulletScaleValue;//Bullet Scale Value

    private Vector3 _cameraShakeValue;

    private bool _isDontShootStraight;//Is Dont Shoot Straighr line?
    private bool _isSniper;
    private bool _isMaster;
    private bool _isBounceBullet;

    private string ExploseEffectName;

    protected Rigidbody2D _rb;

    private Color _effectChangeColor;

    [HeaderAttribute("Sync Setting")]
    protected PhotonView _pv;

    protected Vector2 _newPos;
    protected Vector2 _beginPos;
    protected Quaternion _newDir;
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

    private void SetColor()//設定Bullet顏色，並同步到客戶端
    {
        Color _color = transform.GetComponent<SpriteRenderer>().color =
            CustomPropertyCode.COLORS[(int)PhotonNetwork.LocalPlayer.CustomProperties[CustomPropertyCode.TEAM_CODE]];

        _pv.RPC("ChangeColor", RpcTarget.Others, new Vector3(_color.r, _color.g, _color.b));
    }

    public void OnObjectSpawn()
    {
        if (_pv.IsMine)
        {
            _isMaster = true;
            _pv.RPC("Rpc_EnableObj", RpcTarget.All);
        }
    }

    /// <summary>
    /// Shoot Event
    /// </summary>
    public void ShootEvent(string _name,float _speed,float _damage,float _scaleValue,float _elasticity,bool _isStraight,bool _isSniper, Vector3 _cameraShakeValue)
    {
        ExploseEffectName = _name;
        BulletSpeed = _speed;
        BulletDamage = _damage;
        BulletScaleValue = _scaleValue;
        transform.localScale = new Vector3(_scaleValue, _scaleValue, _scaleValue);
        BulletBeElasticity = _elasticity;
        _isDontShootStraight = _isStraight;
        this._isSniper = _isSniper;
        this._cameraShakeValue = _cameraShakeValue;
        if (_isBounceBullet)//是否是反彈後的子彈(Shield)
        {
            SetColor();
            _isBounceBullet = false;
        }

        _pv.RPC("Rpc_ResetPos", RpcTarget.Others, transform.position, transform.rotation);
        _pv.RPC("Rpc_SetValue", RpcTarget.All, BulletSpeed, BulletDamage, BulletScaleValue, BulletBeElasticity, _isDontShootStraight, this._cameraShakeValue);

        if (_isDontShootStraight)
        {
            transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z + UnityEngine.Random.Range(-15, 16));
        }
    }

    protected virtual void Update()
    {
        BulletCollider();//Bullet Collider Raycast

        if (_isSniper)//如果是Sniper，傷害隨時間增加
        {
            BulletDamage += 200 * Time.deltaTime;
        }
    }

    protected virtual void FixedUpdate()
    {
        if (_pv.IsMine)
        {
            _rb.velocity = transform.right * BulletSpeed;
        }
        else if (!_pv.IsMine)
        {
            _rb.position = Vector2.Lerp(_rb.position, _newPos, 5 * Time.fixedDeltaTime);
        }
    }

    void BulletCollider()//Bullet的Ray Collider
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position - new Vector3(transform.localScale.x / 2, 0, 0), transform.right, transform.localScale.x*2);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.gameObject.CompareTag("Player"))//射到玩家
            {
                PlayerController _playerController = hits[i].collider.gameObject.GetComponent<PlayerController>();
                if (!_playerController.IsShield) //對方沒有發動Shield
                {
                    if(_isMaster != _playerController.Pv.IsMine && !_playerController.Pv.IsMine)//如果是Master
                    {
                        float DirX = Mathf.Cos(transform.eulerAngles.z * Mathf.PI / 180);
                        float DirY = Mathf.Sin(transform.eulerAngles.z * Mathf.PI / 180);
                        _playerController.DamageEvent(BulletDamage, BulletBeElasticity, DirX, DirY, _cameraShakeValue);
                        var IsKill = _playerController.IsKillAnyone();
                        if (IsKill)
                        {
                            PlayerKillCountManager.instance.SetKillCount();
                            _playerController.GenerateDieEffect();
                        }

                        if (_pv.IsMine)
                        {
                            ObjectsPool.Instance.SpawnFromPool(ExploseEffectName, transform.position, transform.rotation, null);
                        }
                        _pv.RPC("Rpc_DisableObj", RpcTarget.All);
                    }
                    else if (_isMaster != _playerController.Pv.IsMine && !_isMaster)//如果不是Master
                    {
                        if (_pv.IsMine&&_isBounceBullet)
                        {
                            GameObject obj = ObjectsPool.Instance.SpawnFromPool(ExploseEffectName, transform.position, transform.rotation, null);
                            Effect effect = obj.GetComponent<Effect>();
                            effect.ChangeColor(_effectChangeColor);
                        }
                        gameObject.SetActive(false);
                    }
                }
                else //對方有發動Shield
                {
                    if (_isMaster != _playerController.Pv.IsMine && _playerController.Pv.IsMine&& !_isBounceBullet)
                    {
                        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + 180);
                        _effectChangeColor = CustomPropertyCode.COLORS[(int)PhotonNetwork.LocalPlayer.CustomProperties[CustomPropertyCode.TEAM_CODE]];
                        _pv.RPC("Rpc_ChangeAngles", RpcTarget.Others, transform.eulerAngles, new Vector3(_effectChangeColor.r, _effectChangeColor.g, _effectChangeColor.b));
                        SetColor();
                        _isBounceBullet = _isMaster = true;
                    }
                }
            }
            else if (hits[i].collider.gameObject.CompareTag("Ground")|| hits[i].collider.gameObject.CompareTag("Saw"))//碰到地面或Saw
            {
                if (_pv.IsMine)
                {
                    if ( _isBounceBullet)//如果是被反彈的子彈(Shield)
                    {
                        GameObject obj = ObjectsPool.Instance.SpawnFromPool(ExploseEffectName, transform.position, transform.rotation, null);
                        Effect effect = obj.GetComponent<Effect>();
                        effect.ChangeColor(_effectChangeColor);
                    }
                    else //如果不是被反彈的子彈(Shield)
                    {
                        ObjectsPool.Instance.SpawnFromPool(ExploseEffectName, transform.position, transform.rotation, null);
                    }
                    _pv.RPC("Rpc_DisableObj", RpcTarget.All);
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }


    /// <summary>
    /// RPC
    /// </summary>
    #region -- Rpc Event --

    //同步Bullet發射後的數值
    [PunRPC]
    public void Rpc_SetValue(float _speed, float _damage, float _scaleValue , float _elasticity, bool IsDontShoot,Vector3 _beShotShake)
    {
        BulletSpeed = _speed;
        BulletDamage = _damage;
        BulletScaleValue = _scaleValue;
        transform.localScale = new Vector3(BulletScaleValue, BulletScaleValue, BulletScaleValue);
        BulletBeElasticity = _elasticity;
        _isDontShootStraight = IsDontShoot;
        _cameraShakeValue = _beShotShake;
    }

    [PunRPC]
    void ChangeColor(Vector3 color)//同步Bullet顏色
    {
        Color _color = new Color(color.x, color.y, color.z);
        transform.GetComponent<SpriteRenderer>().color = _color;
    }

    [PunRPC]
    void Rpc_DisableObj()//同步關掉Bullet
    {
        gameObject.SetActive(false);
    }

    [PunRPC]
    void Rpc_EnableObj()//同步開啟Bullet
    {
        gameObject.SetActive(true);
    }

    [PunRPC]
    void Rpc_ResetPos(Vector3 pos, Quaternion dir)//同步Pos與Dir
    {
        transform.position = pos;
        transform.rotation = dir;
        _newPos = pos;
        _isMaster = _isBounceBullet = false;
    }

    [PunRPC]
    void Rpc_ChangeAngles(Vector3 _dir,Vector3 _color)//同步被反彈後的數值(Shield)
    {
        _effectChangeColor = new Color(_color.x, _color.y, _color.z);
        transform.eulerAngles = _dir;
        _isBounceBullet = true;
        _isMaster = false;
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
            if (_isSniper)
            {
                stream.SendNext(BulletDamage);
            }
        }
        else
        {
            _newPos = (Vector2)stream.ReceiveNext();
            _rb.velocity = (Vector2)stream.ReceiveNext();
            if (_isSniper)
            {
                BulletDamage = (float)stream.ReceiveNext();
            }
            float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime)) + (float)(PhotonNetwork.GetPing() * 0.0001f);
            _newPos += (_rb.velocity * lag);
        }
    }
}
