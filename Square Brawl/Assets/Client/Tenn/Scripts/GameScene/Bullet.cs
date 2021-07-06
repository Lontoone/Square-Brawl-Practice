using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviour, IPoolObject
{
    public float ShootSpeed;
    public float ShootDamage;
    public float BeShootElasticity;
    public float BulletScaleValue;
    public float _OriginSpeed;
    public Vector3 Pos;
    public bool IsDontShootStraight;

    public NetWorkPlayer netWork;

    public GameObject ExploseEffectObj;

    private Rigidbody2D _rb;

    public PhotonView _pv;
    bool isTest;
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _pv = GetComponent<PhotonView>();
        if (_pv.IsMine)
        {
            _pv.RPC("DisableObj", RpcTarget.All);
        }
    }

    public void OnObjectSpawn()
    {
        if (_pv.IsMine)
        {
            _pv.RPC("EnableObj", RpcTarget.All);
        }
    }

    void Update()
    {
        if(_OriginSpeed != ShootSpeed&& _pv.IsMine)
        {
            _pv.RPC("SetStatus", RpcTarget.All, ShootSpeed, ShootDamage, BulletScaleValue, BeShootElasticity, IsDontShootStraight, transform.position);
            if (IsDontShootStraight)
            {
                transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z + Random.Range(-10, 11));
            }
            _OriginSpeed = ShootSpeed;
        }

        //transform.Translate(Vector2.right * ShootSpeed * Time.deltaTime);
    }

    void FixedUpdate()
    {
        _rb.velocity = transform.right * ShootSpeed;
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
    public void SetStatus(float _speed, float _damage, float _scaleValue , float _elasticity, bool IsDontShoot,Vector3 _Pos)
    {
        Pos = _Pos;
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
}
