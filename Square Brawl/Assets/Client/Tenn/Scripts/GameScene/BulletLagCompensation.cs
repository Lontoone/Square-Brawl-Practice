using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BulletLagCompensation : MonoBehaviour, IPunObservable,IPoolObject
{
    public PhotonView _pv;
    public Vector2 _networkPosition;
    public Vector2 _beginPos;
    public Quaternion _networkRotation;
    public Rigidbody2D _rb;
    public Bullet bullet;
    public bool isReset;
    public bool isReset2;
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _pv = GetComponent<PhotonView>();
    }

    public void OnObjectSpawn()
    {
        isReset = false;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_rb.position);
           // stream.SendNext(transform.rotation);
            stream.SendNext(_rb.velocity);
        }
        else
        {
            _networkPosition = (Vector2)stream.ReceiveNext();
            //_networkRotation = (Quaternion)stream.ReceiveNext();
            _rb.velocity = (Vector2)stream.ReceiveNext();

            float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.timestamp));
            _networkPosition += (_rb.velocity * lag);
            
        }
    }

    public void Update()
    {
        if (!_pv.IsMine)
        {
            _rb.position = Vector2.Lerp(_rb.position, _networkPosition, Time.deltaTime);  
        }
    }
}
