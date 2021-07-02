using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ShootSpuare : MonoBehaviour
{
    public Transform target;

    private PhotonView _pv;

    private void Start()
    {
        _pv = GetComponent<PhotonView>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
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
