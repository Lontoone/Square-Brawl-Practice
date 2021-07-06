﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetWorkPlayer : MonoBehaviour, IPunObservable
{
    private PhotonView _pv;

    Vector3 latestPos;
    Quaternion latestRot;
    //Lag compensation
    float currentTime = 0;
    double currentPacketTime = 0;
    double lastPacketTime = 0;
    public Vector3 positionAtLastPacket = Vector3.zero;
    public Quaternion rotationAtLastPacket = Quaternion.identity;
    public Bullet bullet;
    public bool isTest;
    void Start()
    {
        _pv = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_pv.IsMine)
        {
            if(isTest)
            {
                //Lag compensation
                double timeToReachGoal = currentPacketTime - lastPacketTime;
                currentTime += Time.deltaTime;

                //Update remote player
                transform.position = Vector3.Lerp(positionAtLastPacket, latestPos, (float)(currentTime / timeToReachGoal));
                transform.rotation = Quaternion.Lerp(rotationAtLastPacket, latestRot, (float)(currentTime / timeToReachGoal));
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //We own this player: send the others our data
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            //Network player, receive data
            latestPos = (Vector3)stream.ReceiveNext();
            latestRot = (Quaternion)stream.ReceiveNext();
            //Lag compensation
            currentTime = 0.0f;
            lastPacketTime = currentPacketTime;
            currentPacketTime = info.SentServerTime;
            if (!_pv.IsMine && !isTest)
            {
                positionAtLastPacket = bullet.Pos;
                isTest = true;
            }
            else
            {
                positionAtLastPacket = transform.position;
            }
            rotationAtLastPacket = transform.rotation;
        }
    }
}
