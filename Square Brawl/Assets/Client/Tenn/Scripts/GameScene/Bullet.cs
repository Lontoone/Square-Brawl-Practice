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
    public bool IsDontShootStraight;

    public GameObject ExploseEffectObj;

    public PhotonView _pv;
    void Start()
    {
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
        if (!_pv.IsMine)
        {
            return;
        }

        if(_OriginSpeed != ShootSpeed)
        {
            _pv.RPC("SetStatus", RpcTarget.All, ShootSpeed, ShootDamage, BulletScaleValue , transform.position , transform.rotation , IsDontShootStraight, BeShootElasticity);
            if (IsDontShootStraight)
            {
                transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z + Random.Range(-10, 11));
            }
            _OriginSpeed = ShootSpeed;
        }

        transform.Translate(Vector2.right * ShootSpeed * Time.deltaTime);
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
    public void SetStatus(float _speed, float _damage, float _scaleValue , Vector3 _position , Quaternion _rotation ,bool IsDontShoot,float _elasticity)
    {
        transform.position = _position;
        transform.rotation = _rotation;
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
