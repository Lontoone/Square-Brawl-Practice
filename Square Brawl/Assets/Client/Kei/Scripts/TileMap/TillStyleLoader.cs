using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;

public class TillStyleLoader : MonoBehaviour
{
    public GameObject container;
    public Button buttonPrefab;
    private const string styleDataPaht = "TileData/";

    public static string s_StyleName;
    public void Start()
    {
        LoadStyleData();
    }

    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnMapStyleChanged;
    }
    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnMapStyleChanged;
    }

    public void LoadStyleData()
    {
        TileImageCollection[] datas = Resources.LoadAll<TileImageCollection>(styleDataPaht);
        for (int i = 0; i < datas.Length; i++)
        {
            Button _btn = Instantiate(buttonPrefab, container.transform);
            _btn.GetComponentInChildren<Text>().text = datas[i].name;
            TileImageCollection _data = datas[i];
            _btn.onClick.AddListener(delegate
            {
                //TODO:
                ChangeStyle(_data);
            });
        }
    }

    private void ChangeStyle(TileImageCollection _data)
    {
        TileStyleManager.instance.ApplyNewStyle(_data);

        //send Data
        var _byteData = MyPhotonExtension.ObjectToByteArray(_data.name);
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(CustomPropertyCode.UPDATE_STYLE_EVENTCODE, _byteData, raiseEventOptions, SendOptions.SendReliable);

    }


    private void OnMapStyleChanged(EventData obj)
    {
        byte eventCode = obj.Code;
        if (eventCode == CustomPropertyCode.UPDATE_STYLE_EVENTCODE)
        {
            string styleName= (string)MyPhotonExtension.ByteArrayToObject((byte[])obj.CustomData);
            s_StyleName = styleName;
            TileImageCollection tileImageCollection = Resources.Load<TileImageCollection>(styleDataPaht+styleName);
            TileStyleManager.instance.ApplyNewStyle(tileImageCollection);
        }
    }

}
