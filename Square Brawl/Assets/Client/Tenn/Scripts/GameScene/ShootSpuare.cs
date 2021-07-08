using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ShootSpuare : MonoBehaviour
{
    public PlayerController _playerController;

    void Update()
    {
        if (_playerController._pv.IsMine)
        {
            transform.eulerAngles = new Vector3(0, 0, _playerController.ShootSpinAngle);
        }
    }
}
