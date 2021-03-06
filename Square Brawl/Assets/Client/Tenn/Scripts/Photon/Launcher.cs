using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
public class Launcher : MonoBehaviourPunCallbacks
{

    public static Launcher instance;
    private List<RoomListItem> _roomItems = new List<RoomListItem>();
    [SerializeField] UnityEngine.UI.InputField nameInput;
    [SerializeField] UnityEngine.UI.InputField roomNameInputField;
    [SerializeField] TextMeshProUGUI errorText;
    [SerializeField] UnityEngine.UI.Text roomNameText;
    [SerializeField] GameObject room;
    [SerializeField] Transform roomListItemPrefab;
    [SerializeField] Transform roomListContent;
    [SerializeField] Transform playerListItemPrefab;
    [SerializeField] Transform playerListContent;
    [SerializeField] GameObject StartGameButton;

    public bool _hasSetName = false;

    public void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        if (!SceneHandler.isBackToCharacterSelection)
        { 
            MenuManager.instance.OpenMenu("loading");
        }
    }
    public override void OnEnable()
    {
        base.OnEnable();
        //connect to server
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
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

        StartCoroutine(SetUpPlayerListItem());
    }

    private IEnumerator SetUpPlayerListItem()
    {
        yield return new WaitUntil(() => room.activeSelf);

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
        var text = message;
        errorText.text = text.ToUpper();
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
        //MenuManager.instance.OpenMenu("loading");
    }


    public override void OnLeftRoom()
    {
        MenuManager.instance.OpenMenu("title");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        //Create room list items:

        //  Check is Delete or Create
        foreach (RoomInfo _room in roomList)
        {
            Debug.Log("room list update " + _room.Name + " " + _room.RemovedFromList);
            //      Delete
            if (_room.RemovedFromList)
            {
                //Debug.Log("delete room list " + (_roomItems.Find(x => x.info.Name == _room.Name).name));
                Destroy(_roomItems.Find(x => x.info.Name == _room.Name)?.gameObject);
            }
            //      Create:
            else if (!_roomItems.Exists(x => x.info.Name == _room.Name))
            {
                RoomListItem _newItem = Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>();
                _newItem.SetUp(_room);
                _roomItems.Add(_newItem);

                //Debug.Log("create room list " + _newItem.name);
            }

        }

    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);

    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        //LocalDataManager.RemovePlayer((int)otherPlayer.CustomProperties[CustomPropertyCode.PLAYERINDEX]);
    }

    public void BeginGame()
    {
        PhotonNetwork.LoadLevel(1);
    }

    public void StartGame()
    {
        bool IsReady = (bool)PhotonNetwork.LocalPlayer.CustomProperties[CustomPropertyCode.READY];
        //PhotonNetwork.LoadLevel(1);
        if (IsReady)
        {
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
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }
    }

    public void OpenRoom()
    {
        PhotonNetwork.CurrentRoom.IsOpen = true;
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
        if (MapSelectManager.fileIndex >= 0 && TillStyleLoader.currentStyleIndex >= 0)
        {
            MenuManager.instance.OpenMenu("weapon");
            SyncMenu("weapon");
        }
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
