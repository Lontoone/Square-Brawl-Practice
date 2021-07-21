using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultManager : MonoBehaviour
{
    public static event Action<Player> OnShowResult;
    public GameObject resultPanel;
    public GameObject resultListContainer;
    public GameObject endGameMenu;
    public ResultPlayerListItem playerListItem;
    public TileMapSetUpManager setupManager;

    private Dictionary<Player, ResultPlayerListItem> m_playerList = new Dictionary<Player, ResultPlayerListItem>();

    MapData[] maps;

    public void Awake()
    {
        OnShowResult += ShowResult;
    }
    private void Start()
    {
        GeneratePlayerList();
        endGameMenu.SetActive(false);
        resultPanel.SetActive(false);

        //Load map data for random loop:
        if (PhotonNetwork.IsMasterClient)
        {
            maps = LoadTileHelper.LoadTileMaps();
        }

        PhotonNetwork.NetworkingClient.EventReceived += OnMapDataChanged;
    }
    public void OnDestroy()
    {
        OnShowResult -= ShowResult;
        PhotonNetwork.NetworkingClient.EventReceived -= OnMapDataChanged;
    }
    public static void ResultCaller(Player _winner)
    {
        OnShowResult?.Invoke(_winner);
    }

    private void GeneratePlayerList()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            ResultPlayerListItem _item = Instantiate(playerListItem, resultListContainer.transform);
            _item.SetUp(PhotonNetwork.PlayerList[i]);
            m_playerList.Add(PhotonNetwork.PlayerList[i], _item);
        }
    }

    [ContextMenu("ShowResult")]
    private void ShowResult(Player _winner)
    {
        resultPanel.SetActive(true);
        m_playerList[_winner].SetWinColor();

        LoadNewMap();
    }

    public void EndGame()
    {
        endGameMenu.SetActive(true);
    }

    public void OkButton() {        
    }
    [ContextMenu("Random Map")]
    private void LoadNewMap()
    {
        //temp
        maps = LoadTileHelper.LoadTileMaps();
        Debug.Log(maps.Length);
        int _ran = UnityEngine.Random.Range(0, maps.Length-1);
        MapData _data = maps[_ran];
        SendData(_data);
        setupManager.SetUpLevel(_data);
    }

    private void OnMapDataChanged(EventData obj)
    {
        byte eventCode = obj.Code;
        if (eventCode == CustomPropertyCode.UPDATE_MAP_EVENTCODE)
        {
            Debug.Log("[OnMapDataChanged] ");

            MapData _data = (MapData)MyPhotonExtension.ByteArrayToObject((byte[])obj.CustomData);            
            
            setupManager.SetUpLevel(_data);
        }
    }
    private void SendData(MapData _data)
    {
        var _byteData = MyPhotonExtension.ObjectToByteArray(_data);
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; 
        PhotonNetwork.RaiseEvent(CustomPropertyCode.UPDATE_MAP_EVENTCODE, _byteData, raiseEventOptions, SendOptions.SendReliable);
   
    }
}
