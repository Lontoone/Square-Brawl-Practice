using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerLagCompensation : MonoBehaviour,IPunObservable
{
    // Start is called before the first frame update
    private PhotonView _pv;
    private Vector2 _newPos;
    private Quaternion _newDir;
    public Quaternion _newShootPointDir;
    public Rigidbody2D _rb;
    public Transform ShootPointSpinPos;

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
            stream.SendNext(ShootPointSpinPos.transform.rotation);
            stream.SendNext(_rb.velocity);
        }
        else
        {
            _newPos = (Vector2)stream.ReceiveNext();
            _newDir = (Quaternion)stream.ReceiveNext();
            _newShootPointDir= (Quaternion)stream.ReceiveNext();
            _rb.velocity = (Vector2)stream.ReceiveNext();

            float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.timestamp));
            _newPos += (_rb.velocity * lag);
        }
    }

    public void Update()
    {
        if (!_pv.IsMine)
        {
            _rb.position = Vector2.Lerp(_rb.position, _newPos, 15*Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, _newDir, 15 * Time.deltaTime);
            ShootPointSpinPos.transform.rotation = Quaternion.Lerp(ShootPointSpinPos.transform.rotation, _newShootPointDir, 15 * Time.deltaTime);
        }
    }
}
