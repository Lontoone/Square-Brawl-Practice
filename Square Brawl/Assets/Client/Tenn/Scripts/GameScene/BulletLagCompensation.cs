using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BulletLagCompensation : MonoBehaviour, IPunObservable
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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_rb.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(_rb.velocity);
        }
        else
        {
            _networkPosition = (Vector2)stream.ReceiveNext();
            _networkRotation = (Quaternion)stream.ReceiveNext();
            _rb.velocity = (Vector2)stream.ReceiveNext();

            float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.timestamp));
            _networkPosition += (_rb.velocity * lag);

            /*if (!_pv.IsMine &&!isReset)
            {
                _beginPos = bullet._pos;
                Debug.Log(_beginPos);
            }*/
        }
    }

    public void FixedUpdate()
    {
        if (!_pv.IsMine)
        {
            /*if (!isReset)
            {
                Debug.Log(_beginPos);
                _rb.position = _beginPos;
                isReset = true;
            }
            else*/
            

            _rb.position = Vector2.Lerp(_rb.position, _networkPosition, Time.fixedDeltaTime);
            transform.rotation = _networkRotation;


        }
    }
}
