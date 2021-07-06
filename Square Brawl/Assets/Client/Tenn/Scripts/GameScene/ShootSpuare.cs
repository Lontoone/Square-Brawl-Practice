using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ShootSpuare : MonoBehaviour
{
    public Transform target;

    private PhotonView _pv;

    private LagCompensation _lag;

    private void Start()
    {
        _pv = GetComponent<PhotonView>();
        _lag = GetComponent<LagCompensation>();
        if (_pv.IsMine)
        {
            target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        }
    }
    void Update()
    {
        if (!_pv.IsMine)
        {
            return;
        }
        transform.position = target.position;
    }
}
