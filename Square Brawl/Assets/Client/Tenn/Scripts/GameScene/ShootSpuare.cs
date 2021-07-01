﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ShootSpuare : MonoBehaviour
{
    public Transform target;

    private PhotonView _photonView;

    private void Start()
    {
        _photonView = GetComponent<PhotonView>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }
    void Update()
    {
        /*if (!_photonView.IsMine)
        {
            return;
        }*/
        transform.position = target.position;
    }
}
