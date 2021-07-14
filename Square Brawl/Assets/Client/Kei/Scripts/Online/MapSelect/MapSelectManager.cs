﻿using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Linq;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapSelectManager : MonoBehaviourPunCallbacks
{
    public static MapSelectManager instance;
    public void Awake()
    {
        instance = this;
    }
    public static MapData currentSelectedData;
    List<MapData> mapDatas = new List<MapData>();
    public Button fileBtnPrefab;
    public GameObject btnContainer;
    public Text selectText;
    public LoadSceneAsyncUI loadSceneUI;
    public TileMapSetUpManager setupManager;

    private const string BUILTIN_MAPS_FOLDER = "Maps/";
    private static string[] filePaths;
    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.NetworkingClient.EventReceived += OnMapDataChanged;
        SceneManager.sceneLoaded += OnCellGridSceneLoaded;

        if (PhotonNetwork.IsMasterClient)
        {
            LoadMapList();
        }
        else
        {
            //TODO... not master
        }
        loadSceneUI.LoadScene();
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.NetworkingClient.EventReceived -= OnMapDataChanged;
        SceneManager.sceneLoaded -= OnCellGridSceneLoaded;
    }

    public void LoadMapList()
    {
        //ClearContainer();
        if (!Directory.Exists(SaveTile.SAVE_FOLDER.CombinePersistentPath())) { return; }

        filePaths = Directory.GetFiles(SaveTile.SAVE_FOLDER.CombinePersistentPath());
        Debug.Log(" [Load Map] " + filePaths.Length);
        for (int i = 0; i < filePaths.Length; i++)
        {
            string _path = filePaths[i];
            MapSelectButton _btn = Instantiate(fileBtnPrefab, btnContainer.transform).GetComponent<MapSelectButton>();
            MapData _data = SaveAndLoad.Load<MapData>(_path);
            mapDatas.Add(_data);
            _btn.SetButton(_data);
        }

        //For resources Build in:
        List<TextAsset> builtinMapsAssets = Resources.LoadAll<TextAsset>(BUILTIN_MAPS_FOLDER).ToList();
        for (int i = 0; i < builtinMapsAssets.Count; i++)
        {
            MapSelectButton _btn = Instantiate(fileBtnPrefab, btnContainer.transform).GetComponent<MapSelectButton>();
            MapData _data = JsonUtility.FromJson<MapData>(builtinMapsAssets[i].text);
            mapDatas.Add(_data);
            _btn.SetButton(_data);

        }

        //btnContainer.SetActive(true);
    }

    //Called by PhotonEvent: When
    private void OnMapDataChanged(EventData obj)
    {
        byte eventCode = obj.Code;
        if (eventCode == CustomPropertyCode.UPDATE_MAP_EVENTCODE)
        {
            Debug.Log("[OnMapDataChanged] ");
            MapData _data = (MapData)MyPhotonExtension.ByteArrayToObject((byte[])obj.CustomData);
            selectText.text = _data.fileName;
            setupManager.SetUpLevel(_data);
        }
    }

    private void PreviewMap()
    {
        //TODO:....
    }


    private void ClearContainer()
    {
        Debug.Log("Clear btns!!");
        foreach (Transform child in btnContainer.transform)
        {
            Destroy(child.gameObject);
        }
        mapDatas.Clear();
    }

    private void OnCellGridSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //test
        if (scene.name == "GridSample")
        {
            if (mapDatas.Count > 0)
            {
                setupManager.SetUpLevel(mapDatas[0]);
            }
        }
    }

}