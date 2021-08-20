using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ColorSelectManager : MonoBehaviourPunCallbacks
{
    public Transform playerItemContainer;
    public ColorSelectPlayerItemControl playerItemPrefab;

    public Transform buttonItemContainer;
    public ColorSetButton buttonItemPrefab;

    private List<ColorSetButton> colorBtns = new List<ColorSetButton>();

    private void Start()
    {
        CreatePlayerItem();
        CreateButtonItem();
    }

    private void CreateButtonItem()
    {
        foreach (Transform child in buttonItemContainer)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < CustomPropertyCode.COLORS.Length; i++)
        {
            ColorSetButton _button = Instantiate(buttonItemPrefab, parent: buttonItemContainer);
            _button.colorImage.color = CustomPropertyCode.COLORS[i];
            _button.colorIndex = i;

            colorBtns.Add(_button);
        }
        //預設隊伍
        //colorBtns[0].SetColor();
    }

    private void CreatePlayerItem()
    {
        foreach (Transform child in playerItemContainer)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            ColorSelectPlayerItemControl _item = Instantiate(playerItemPrefab, parent: playerItemContainer);
            Player _player = PhotonNetwork.PlayerList[i];
            _item.SetPlayer(_player);
            _item.colorCode = -1;

        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        object _data;
        //if someone updates it's team color code
        //  =>find the matching player item and update
        if (changedProps.TryGetValue(CustomPropertyCode.TEAM_CODE, out _data))
        {
            int _colorCode = (int)_data;
            ColorSelectPlayerItemControl _item = FindObjectsOfType<ColorSelectPlayerItemControl>().ToList().Find(x => x.player == targetPlayer);
            if (_item == null)
            {
                Debug.Log("Cant find the item");
                return;
            }
            _item.SetColor(CustomPropertyCode.COLORS[_colorCode]);
            _item.colorCode = _colorCode;

        }
    }

}
