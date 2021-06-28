using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;


public class PlayerManager : MonoBehaviour
{
    private PhotonView _photonView;
    // Start is called before the first frame update
    void Awake()
    {
        _photonView = GetComponent<PhotonView>();
    }

    void Start()
    {
        if (_photonView.IsMine)
        {
            CreatController();
        }
    }

    // Update is called once per frame
    void CreatController()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"), Vector3.zero, Quaternion.identity);
    }
}
