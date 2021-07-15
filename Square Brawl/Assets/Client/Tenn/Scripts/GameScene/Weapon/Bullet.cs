using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviour, IPoolObject,IPunObservable
{
    [HeaderAttribute("Bullet Setting")]
    public float BulletSpeed;//Bullet Speed
    public float BulletDamage;//Bullet Damage
    public float BulletBeElasticity;//Bullet Be Elasticity
    public float BulletScaleValue;//Bullet Scale Value
    private float _originSpeed;//Bullet Origin Speed

    private Vector3 _originPos;//Bullet Origin Position

    public bool IsDontShootStraight;//Is Dont Shoot Straighr line?
    protected bool _isReset;

    public string ExploseEffectName;

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
            _pv.RPC("Rpc_DisableObj", RpcTarget.All);
            /*_pv.RPC("SetStatus", RpcTarget.All, ShootSpeed, ShootDamage, BulletScaleValue, BeShootElasticity, IsDontShootStraight, transform.position);
            if (IsDontShootStraight)
            {
                transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z + Random.Range(-10, 11));
            }*/
        }
    }

    public void OnObjectSpawn()
    {
        if (_pv.IsMine)
        {
            _pv.RPC("Rpc_EnableObj", RpcTarget.All);
            _pv.RPC("Rpc_ResetPos", RpcTarget.Others, transform.position, transform.rotation);
        }
    }

    protected virtual void Update()
    {
        ResetValue();//Reset Bullet Value

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

    public void BulletCollider()
    {
        _originPos = transform.position + new Vector3(transform.localScale.x / 2, 0, 0);

        RaycastHit2D[] hits = Physics2D.RaycastAll(_originPos, (transform.position - _originPos).normalized, (transform.position - _originPos).magnitude);

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.gameObject.CompareTag("Player"))
            {
                PlayerController _playerController = hits[i].collider.gameObject.GetComponent<PlayerController>();
                if (_pv.IsMine != _playerController.Pv.IsMine && _playerController.Pv.IsMine)
                {
                    float DirX = Mathf.Cos(gameObject.transform.eulerAngles.z * Mathf.PI / 180);
                    float DirY = Mathf.Sin(gameObject.transform.eulerAngles.z * Mathf.PI / 180);
                    _playerController.TakeDamage(BulletDamage, BulletBeElasticity, DirX, DirY);
                    DisableObj();
                }
                if (_pv.IsMine)
                {
                    ObjectsPool.Instance.SpawnFromPool(ExploseEffectName, transform.position, transform.rotation, null);
                }
            }
            else if (hits[i].collider.gameObject.CompareTag("Ground"))
            {
                if (_pv.IsMine)
                {
                    ObjectsPool.Instance.SpawnFromPool(ExploseEffectName, transform.position, transform.rotation, null);
                    DisableObj();
                }
            }
        }
    }

    protected virtual void ResetValue()
    {
        if (!_isReset&& _pv.IsMine)
        {
            _pv.RPC("Rpc_SetValue", RpcTarget.All, BulletSpeed, BulletDamage, BulletScaleValue, BulletBeElasticity, IsDontShootStraight);
            if (IsDontShootStraight)
            {
                transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z + Random.Range(-15, 16));
            }
            _isReset = true;
        }
    }

    [PunRPC]
    public void Rpc_SetValue(float _speed, float _damage, float _scaleValue , float _elasticity, bool IsDontShoot)
    {
        BulletSpeed = _speed;
        BulletDamage = _damage;
        BulletScaleValue = _scaleValue;
        BulletBeElasticity = _elasticity;
        IsDontShootStraight = IsDontShoot;
    }

    void DisableObj()
    {
        _pv.RPC("Rpc_DisableObj", RpcTarget.All);
        _isReset = false;
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
