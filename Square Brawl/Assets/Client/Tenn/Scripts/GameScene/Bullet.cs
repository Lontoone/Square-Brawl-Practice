using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviour, IPoolObject,IPunObservable
{
    public float ShootSpeed;
    public float ShootDamage;
    public float BeShootElasticity;
    public float BulletScaleValue;
    public float _OriginSpeed;

    private Vector3 _mPrevPos;
    public Vector3 _pos;

    public bool IsDontShootStraight;

    public GameObject ExploseEffectObj;

    private Rigidbody2D _rb;

    public PhotonView _pv;

    public Vector2 _networkPosition;
    public Vector2 _beginPos;
    public Quaternion _networkRotation;
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _pv = GetComponent<PhotonView>();
        if (_pv.IsMine)
        {
            _pv.RPC("DisableObj", RpcTarget.All);
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
            _pv.RPC("EnableObj", RpcTarget.All);
            _pv.RPC("ResetPos", RpcTarget.Others, transform.position, transform.rotation);
        }
    }

    void Update()
    {
        if(_OriginSpeed != ShootSpeed&& _pv.IsMine)
        {
            _pv.RPC("SetStatus", RpcTarget.All, ShootSpeed, ShootDamage, BulletScaleValue, BeShootElasticity, IsDontShootStraight);
            if (IsDontShootStraight)
            {
                transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z + Random.Range(-10, 11));
            }
            _OriginSpeed = ShootSpeed;
        }

        _mPrevPos = transform.position;

        RaycastHit2D[] hits = Physics2D.RaycastAll(_mPrevPos, (transform.position - _mPrevPos).normalized, (transform.position - _mPrevPos).magnitude);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.gameObject.CompareTag("Player"))
            {
                PlayerController _playerController = hits[i].collider.gameObject.GetComponent<PlayerController>();
                if (_pv.IsMine != _playerController._pv.IsMine&& _playerController._pv.IsMine)
                {
                    float DirX = Mathf.Cos(gameObject.transform.eulerAngles.z * Mathf.PI / 180);
                    float DirY = Mathf.Sin(gameObject.transform.eulerAngles.z * Mathf.PI / 180);
                    _playerController.TakeDamage(ShootDamage, BeShootElasticity, DirX, DirY);
                    _pv.RPC("DisableObj", RpcTarget.All);
                }
                //gameObject.SetActive(false);
            }
        }
        //transform.Translate(Vector2.right * ShootSpeed * Time.deltaTime);
    }

    void FixedUpdate()
    {
        if (_pv.IsMine)
        {
            _rb.velocity = transform.right * ShootSpeed;
        }
        else if (!_pv.IsMine)
        {
            _rb.position = Vector2.Lerp(_rb.position, _networkPosition, 5 * Time.fixedDeltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            if (_pv.IsMine)
            {
                _pv.RPC("DisableObj", RpcTarget.All);
            }
        }
    }
    [PunRPC]
    public void SetStatus(float _speed, float _damage, float _scaleValue , float _elasticity, bool IsDontShoot)
    {
        ShootSpeed = _speed;
        ShootDamage = _damage;
        BulletScaleValue = _scaleValue;
        BeShootElasticity = _elasticity;
        IsDontShootStraight = IsDontShoot;
    }

    [PunRPC]
    public void DisableObj()
    {
        gameObject.SetActive(false);
    }

    [PunRPC]
    public void EnableObj()
    {
        gameObject.SetActive(true);
    }

    [PunRPC]
    public void ResetPos(Vector3 pos, Quaternion dir)
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

            // float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.timestamp));
            float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime)) + (float)(PhotonNetwork.GetPing() * 0.001f);
            _networkPosition += (_rb.velocity * lag);
        }
    }
}
