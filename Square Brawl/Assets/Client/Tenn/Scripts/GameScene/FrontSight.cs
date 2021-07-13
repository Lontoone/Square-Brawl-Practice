using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class FrontSight : MonoBehaviour
{
    private PlayerController _playerController;

    private void Start()
    {
        _playerController=GetComponentInParent<PlayerController>();
    }

    void Update()
    {
        if (_playerController.Pv.IsMine)
        {
            transform.eulerAngles = new Vector3(0, 0, _playerController.FrontSightAngle);
        }
    }
}
