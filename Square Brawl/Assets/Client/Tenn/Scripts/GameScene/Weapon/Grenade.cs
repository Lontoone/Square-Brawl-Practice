using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Grenade : MonoBehaviour, IPoolObject, IPunObservable
{
    [HeaderAttribute("Bullet Setting")]
    public float BulletSpeed;//Bullet Speed
    public float BulletDamage;//Bullet Damage
    public float BulletBeElasticity;//Bullet Be Elasticity
    public float BulletScaleValue;//Bullet Scale Value
    public float FieldExplose;//Explose Field

    protected bool isShoot;
    protected bool _isReset;

    public string ExploseEffectName;

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
            _pv.RPC("Rpc_DisableObj", RpcTarget.All);
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

        ExploseEvent();//Explose Event
    }

    protected virtual void FixedUpdate()
    {
        if (!_pv.IsMine)
        {
            _rb.position = Vector2.Lerp(_rb.position, _networkPosition, 5 * Time.fixedDeltaTime);
        }
    }

    void ExploseEvent()
    {
        if (!isShoot)
        {
            StartCoroutine(Boom());
            isShoot = true;
        }
    }

    IEnumerator Boom()
    {
        yield return new WaitForSeconds(1f);
        Explose();
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
            if (_pv.IsMine == _playerController.Pv.IsMine&&_pv.IsMine)
            {
                _playerController.BeExplode(BulletBeElasticity, transform.position, FieldExplose);
            }

            if (_pv.IsMine != _playerController.Pv.IsMine && _playerController.Pv.IsMine)
            {
                _playerController.TakeDamage(BulletDamage, 0, 0, 0);
                _playerController.BeExplode(BulletBeElasticity, transform.position, FieldExplose);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, FieldExplose);
    }

    protected void ResetValue()
    {
        if (!_isReset && _pv.IsMine)
        {
            _pv.RPC("Rpc_SetValue", RpcTarget.All, BulletSpeed, BulletDamage, BulletScaleValue, BulletBeElasticity);
            _rb.AddForce(BulletSpeed * transform.right);
            transform.eulerAngles = Vector3.zero;
            _isReset = true;
        }
    }
    [PunRPC]
    public void Rpc_SetValue(float _speed, float _damage, float _scaleValue, float _elasticity)
    {
        BulletSpeed = _speed;
        BulletDamage = _damage;
        BulletScaleValue = _scaleValue;
        BulletBeElasticity = _elasticity;
    }

    [PunRPC]
    public void Rpc_DisableObj()
    {
        gameObject.SetActive(false);
        _isReset = isShoot = false;
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
            _childObjnetworkPosition = (Vector3)stream.ReceiveNext();
            _childObjnetworkScale = (Vector3)stream.ReceiveNext();
            //float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
            //_networkPosition += (_rb.velocity * lag);
        }
    }
}
