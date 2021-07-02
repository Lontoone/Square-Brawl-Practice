using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;


public class PlayerManager : MonoBehaviour
{
    private PhotonView _pv;

    private GameObject _obj;

    public GameObject _shootObj;
    // Start is called before the first frame update
    void Awake()
    {
        _pv = GetComponent<PhotonView>();
    }

    void Start()
    {
        if (_pv.IsMine)
        {
            CreatController();
        }
    }

    // Update is called once per frame
    void CreatController()
    {
        _obj = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"), Vector3.zero, Quaternion.identity, 0, new object[] { _pv.ViewID });
        _shootObj = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "ShootSpinPointMid"), Vector3.zero, Quaternion.identity, 0, new object[] { _pv.ViewID });
    }

    public void Die()
    {
        PhotonNetwork.Destroy(_shootObj);
        PhotonNetwork.Destroy(_obj);
    }
}
