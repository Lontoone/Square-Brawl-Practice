using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;


public class PlayerManager : MonoBehaviour
{
    private PhotonView _pv;

    private GameObject _obj;

    void Awake()
    {
        _pv = GetComponent<PhotonView>();
        if (_pv.IsMine)
        {
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "ObjectPool"), Vector3.zero, Quaternion.identity);
        }
    }

    void Start()
    {
        if (_pv.IsMine)
        {
            CreatController();
        }
    }

    void CreatController()
    {
        _obj = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"), new Vector3(Random.Range(-5,6),0,0), Quaternion.identity, 0, new object[] { _pv.ViewID });
        
    }

    public void Die()
    {
        PhotonNetwork.Destroy(_obj);
    }
}
