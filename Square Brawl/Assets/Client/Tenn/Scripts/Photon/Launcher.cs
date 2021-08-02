using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
public class Launcher : MonoBehaviourPunCallbacks
{

    public static Launcher instance;
    [SerializeField] TMP_InputField nameInput;
    [SerializeField] UnityEngine.UI.InputField roomNameInputField;
    [SerializeField] UnityEngine.UI.Text errorText;
    [SerializeField] UnityEngine.UI.Text roomNameText;
    [SerializeField] Transform roomListItemPrefab;
    [SerializeField] Transform roomListContent;
    [SerializeField] Transform playerListItemPrefab;
    [SerializeField] Transform playerListContent;
    [SerializeField] GameObject StartGameButton;

    private bool _hasSetName = false;

    public void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        MenuManager.instance.OpenMenu("loading");
    }
    public override void OnEnable()
    {
        base.OnEnable();
        //connect to server
        PhotonNetwork.ConnectUsingSettings();
        if (MenuManager.instance != null)
        {
            MenuManager.instance.OpenMenu("loading");
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("connect to lobby");
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Join lobby " + _hasSetName);
        if (_hasSetName)
        {
            MenuManager.instance.OpenMenu("title");
        }
        else
        {
            MenuManager.instance.OpenMenu("name");
        }

        //?temp
        //PhotonNetwork.NickName = "Player" + Random.Range(0, 1000).ToString("0000");
    }


    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomNameInputField.text)) { return; }
        PhotonNetwork.CreateRoom(roomNameInputField.text);

        //避免玩家在等待server時亂點其他按鈕s
        MenuManager.instance.OpenMenu("loading");
        Debug.Log("Createed room");
    }


    public override void OnJoinedRoom() //called when create or join a room
    {
        Debug.Log("Joinned room");
        MenuManager.instance.OpenMenu("room");
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;

        foreach (Transform child in playerListContent)
        {
            Destroy(child.gameObject);
        }

        Player[] players = PhotonNetwork.PlayerList;
        for (int i = 0; i < players.Length; i++)
        {
            Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
        }

        //只有host可以開始遊戲按鈕
        StartGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }


    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        //switch host時讓下個host可以開始遊戲
        StartGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("failed room");
        errorText.text = "Room create Failed " + message;
        MenuManager.instance.OpenMenu("error");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.instance.OpenMenu("loading");
    }

    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        MenuManager.instance.OpenMenu("loading");
    }


    public override void OnLeftRoom()
    {
        MenuManager.instance.OpenMenu("title");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform child in roomListContent)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList)
            {
                continue;
            }
            Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);

    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        LocalDataManager.RemovePlayer((int)otherPlayer.CustomProperties[CustomPropertyCode.PLAYERINDEX]);
    }

    public void BeginGame()
    {
        PhotonNetwork.LoadLevel(1);
    }

    public void StartGame()
    {
        //PhotonNetwork.LoadLevel(1);
        ConfirmPlayers();
        MenuManager.instance.OpenMenu("characterselection");
        //Sync Room
        SyncMenu("characterselection");
        /*
        PhotonNetwork.CurrentRoom.SetCustomProperties(MyPhotonExtension.WrapToHash(
                                                                        new object[] {
                                                                            CustomPropertyCode.ROOM_MENU,
                                                                            "characterselection"
                                                                        }));*/
    }
    public void ConfirmPlayerName()
    {
        if (nameInput.text != "")
        {
            PhotonNetwork.LocalPlayer.NickName = nameInput.text;
            MenuManager.instance.OpenMenu("title");
            _hasSetName = true;
        }
    }
    public void StartLevel()
    {
        PhotonNetwork.LoadLevel("CustomeLevel");
    }
    //Not really using
    private void ConfirmPlayers()
    {
        //Add Player to local list:
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            //int _playerIndex = PhotonNetwork.PlayerList[i].CustomProperties[];
            LocalDataManager.AddPlayer(PhotonNetwork.PlayerList[i]);
            Debug.Log("Add Player " + PhotonNetwork.PlayerList[i].NickName);
        }
    }

    public void Quit()
    {
        _hasSetName = false;
        PhotonNetwork.LeaveLobby();
        PhotonNetwork.Disconnect();

        Debug.Log("has set name " + _hasSetName);

    }


    public void ModeSelect()
    {
        MenuManager.instance.OpenMenu("mode");
        SyncMenu("mode");
    }
    public void MapSelect()
    {
        MenuManager.instance.OpenMenu("map");
        SyncMenu("map");
    }
    public void WeaponSelect()
    {
        MenuManager.instance.OpenMenu("weapon");
        SyncMenu("weapon");
    }


    private void SyncMenu(string _menuName)
    {
        PhotonNetwork.CurrentRoom.SetCustomProperties(MyPhotonExtension.WrapToHash(
                                                                        new object[] {
                                                                            CustomPropertyCode.ROOM_MENU,
                                                                            _menuName
                                                                        }));
    }
}
