using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviour, IPoolObject,IPunObservable
{
    [HeaderAttribute("Bullet Setting")]
    private float BulletSpeed;//Bullet Speed
    private float BulletDamage;//Bullet Damage
    private float BulletBeElasticity;//Bullet Be Elasticity
    private float BulletScaleValue;//Bullet Scale Value

    private Vector3 _cameraShakeValue;

    private Vector3 _originPos;//Bullet Origin Position

    private bool _isDontShootStraight;//Is Dont Shoot Straighr line?
    protected bool _isMaster;
    private bool _isBounce;

    private string ExploseEffectName;

    protected Rigidbody2D _rb;

    [HeaderAttribute("Sync Setting")]
    protected PhotonView _pv;

    protected Vector2 _networkPosition;
    protected Vector2 _beginPos;
    protected Quaternion _networkRotation;
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

    [PunRPC]
    void ChangeColor(Vector3 color)
    {
        Color _color = new Color(color.x, color.y, color.z);
        transform.GetComponent<SpriteRenderer>().color = _color;
    }

    public void OnObjectSpawn()
    {
        if (_pv.IsMine)
        {
            _isMaster = true;
            _pv.RPC("Rpc_EnableObj", RpcTarget.All);
            //_pv.RPC("Rpc_ResetPos", RpcTarget.Others, transform.position, transform.rotation);
        }
    }

    public void ShootEvent(string _name,float _speed,float _damage,float _scaleValue,float _elasticity,bool _isStraight, Vector3 _cameraShakeValue)
    {
        ExploseEffectName = _name;
        BulletSpeed = _speed;
        BulletDamage = _damage;
        BulletScaleValue = _scaleValue;
        BulletBeElasticity = _elasticity;
        _isDontShootStraight = _isStraight;
        this._cameraShakeValue = _cameraShakeValue;
        _pv.RPC("Rpc_ResetPos", RpcTarget.Others, transform.position, transform.rotation);
        _pv.RPC("Rpc_SetValue", RpcTarget.All, BulletSpeed, BulletDamage, BulletScaleValue, BulletBeElasticity, _isDontShootStraight, this._cameraShakeValue);

        if (_isDontShootStraight)
        {
            transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z + UnityEngine.Random.Range(-15, 16));
        }
    }

    protected virtual void Update()
    {
        //ResetValue();//Reset Bullet Value

        BulletCollider();//Bullet Collider Raycast
    }

    protected virtual void FixedUpdate()
    {
        if (_pv.IsMine)
        {
            _rb.velocity = transform.right * BulletSpeed;
        }
        else if (!_pv.IsMine)
        {
            _rb.position = Vector2.Lerp(_rb.position, _networkPosition, 5 * Time.fixedDeltaTime);
        }
    }

    void BulletCollider()
    {
        //_originPos = transform.position;// + new Vector3(transform.localScale.x / 2, 0, 0);
                                        //RaycastHit2D[] hits = Physics2D.RaycastAll(_originPos, (transform.position - _originPos).normalized, (transform.position - _originPos).magnitude);
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position - new Vector3(transform.localScale.x / 2, 0, 0), transform.right, transform.localScale.x);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.gameObject.CompareTag("Player"))
            {
                PlayerController _playerController = hits[i].collider.gameObject.GetComponent<PlayerController>();
                if (!_playerController.IsShield) 
                {
                    if (_isMaster != _playerController.Pv.IsMine && _playerController.Pv.IsMine)
                    {
                        /*float DirX = Mathf.Cos(transform.eulerAngles.z * Mathf.PI / 180);
                        float DirY = Mathf.Sin(transform.eulerAngles.z * Mathf.PI / 180);
                        _playerController.DamageEvent(BulletDamage, BulletBeElasticity, DirX, DirY, _cameraShakeValue);
                        if (_pv.IsMine)
                        {
                            ObjectsPool.Instance.SpawnFromPool(ExploseEffectName, transform.position, transform.rotation, null);
                        }
                        gameObject.SetActive(false);*/
                        //_pv.RPC("Rpc_DisableObj", RpcTarget.All);
                    }
                    else if((_pv.IsMine != _playerController.Pv.IsMine && !_playerController.Pv.IsMine))
                    {
                        ObjectsPool.Instance.SpawnFromPool(ExploseEffectName, transform.position, transform.rotation, null);
                        _pv.RPC("Rpc_DisableObj", RpcTarget.All);
                        float DirX = Mathf.Cos(transform.eulerAngles.z * Mathf.PI / 180);
                        float DirY = Mathf.Sin(transform.eulerAngles.z * Mathf.PI / 180);
                        _playerController.DamageEvent(BulletDamage, BulletBeElasticity, DirX, DirY, _cameraShakeValue);
                        if (_pv.IsMine)
                        {
                            ObjectsPool.Instance.SpawnFromPool(ExploseEffectName, transform.position, transform.rotation, null);
                        }
                    } 
                }
                else
                {
                    if (_isMaster != _playerController.Pv.IsMine && _playerController.Pv.IsMine&& !_isBounce)
                    {
                        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + 180);
                        _pv.RPC("Rpc_ChangeAngles", RpcTarget.Others,transform.eulerAngles);
                        _isBounce = _isMaster = true;
                    }
                }
            }
            else if (hits[i].collider.gameObject.CompareTag("Ground"))
            {
                if (_pv.IsMine)
                {
                    ObjectsPool.Instance.SpawnFromPool(ExploseEffectName, transform.position, transform.rotation, null);
                    _pv.RPC("Rpc_DisableObj", RpcTarget.All);
                }
            }
        }

        Debug.DrawLine(transform.position, _originPos, Color.red);
    }

    /*private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController _playerController = other.gameObject.GetComponent<PlayerController>();
            if (!_playerController.IsShield)
            {
                if (_isMaster != _playerController.Pv.IsMine && _playerController.Pv.IsMine)
                {
                    float DirX = Mathf.Cos(transform.eulerAngles.z * Mathf.PI / 180);
                    float DirY = Mathf.Sin(transform.eulerAngles.z * Mathf.PI / 180);
                    _playerController.TakeDamage(BulletDamage, _cameraShakeValue.x, _cameraShakeValue.y, _cameraShakeValue.z);
                    _playerController.BeBounce(BulletBeElasticity, DirX, DirY);
                    if (_pv.IsMine)
                    {
                        ObjectsPool.Instance.SpawnFromPool(ExploseEffectName, transform.position, transform.rotation, null);
                    }
                    DisableObj();
                }
                else if ((_pv.IsMine != _playerController.Pv.IsMine && !_playerController.Pv.IsMine))
                {
                    ObjectsPool.Instance.SpawnFromPool(ExploseEffectName, transform.position, transform.rotation, null);
                    DisableObj();
                }
            }
            else
            {
                if (_isMaster != _playerController.Pv.IsMine && _playerController.Pv.IsMine && !_isBounce)
                {
                    transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + 180);
                    _pv.RPC("Rpc_ChangeAngles", RpcTarget.Others, transform.eulerAngles);
                    _isBounce = _isMaster = true;
                }
            }
        }
        else if (other.gameObject.CompareTag("Ground"))
        {
            if (_pv.IsMine)
            {
                ObjectsPool.Instance.SpawnFromPool(ExploseEffectName, transform.position, transform.rotation, null);
                DisableObj();
            }
        }
    }*/

    [PunRPC]
    public void Rpc_SetValue(float _speed, float _damage, float _scaleValue , float _elasticity, bool IsDontShoot,Vector3 _beShotShake)
    {
        BulletSpeed = _speed;
        BulletDamage = _damage;
        BulletScaleValue = _scaleValue;
        BulletBeElasticity = _elasticity;
        _isDontShootStraight = IsDontShoot;
        _cameraShakeValue = _beShotShake;
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
        _isMaster = _isBounce = false;
    }

    [PunRPC]
    public void Rpc_ChangeAngles(Vector3 _dir)
    {
        transform.eulerAngles = _dir;
        _isMaster = false;
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_rb.position);
            stream.SendNext(_rb.velocity);
        }
        else
        {
            _networkPosition = (Vector2)stream.ReceiveNext();
            _rb.velocity = (Vector2)stream.ReceiveNext();
            float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime)) + (float)(PhotonNetwork.GetPing() * 0.001f);
            _networkPosition += (_rb.velocity * lag);
        }
    }
}
