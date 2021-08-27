using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;


public class PlayerManager : MonoBehaviour
{
    private PhotonView _pv;

    private GameObject _obj;

    private bool isTest;

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
            /*Debug.Log(tileMap.activeTileCells.Count);
            for (int i = 0; i < tileMap.activeTileCells.Count; i++)
            {
                Debug.Log(tileMap.activeTileCells[i].gameObject.transform.position);
            }*/
            for (int i = 0; i < TileMapSetUpManager.instance.activeTileCells.Count; i++)
            {
                TileCell _cell = TileMapSetUpManager.instance.activeTileCells[i];

               // Debug.Log(_cell.transform.position);
            }
        }
    }


    void CreatController()
    {
        //Vector3 Pos = new Vector3(Random.Range(-4, 11), Random.Range(1, 4), 0);
        Vector3 Pos = new Vector3(Random.Range(-4, 11), 1, 0);

        for (int i = 0; i < TileMapSetUpManager.instance.activeTileCells.Count; i++)
        {
            TileCell _cell = TileMapSetUpManager.instance.activeTileCells[i];
            if (Mathf.Abs(_cell.transform.position.x-Pos.x) <= 0.4f && Mathf.Abs(_cell.transform.position.y - Pos.y) <= 0.4f)
            {
                Pos = new Vector3(Random.Range(-4, 11), Random.Range(1, 4), 0);
                i = 0;
            }
            /*if (Mathf.Floor(_cell.transform.position.x) == Pos.x && Mathf.Floor(_cell.transform.position.y) == Pos.y)
            {
                Pos = new Vector3(Random.Range(-4, 11), Random.Range(1, 4), 0);
                i = 0;
            }*/
        }
        
        /*for (int i = 0; i < TileMapSetUpManager.instance.activeTileCells.Count; i++)
        {
            TileCell _cell = TileMapSetUpManager.instance.activeTileCells[i];
            if (Mathf.Floor(_cell.transform.position.x) == Pos.x)
            {
                Pos = new Vector3(Random.Range(-4, 11), 1, 0);
            }
            //Debug.Log(_cell.transform.position);
        }*/
        _obj = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"), Pos, Quaternion.identity, 0, new object[] { _pv.ViewID });
        
    }

    public void Die()
    {
        PhotonNetwork.Destroy(_obj);
    }
}
