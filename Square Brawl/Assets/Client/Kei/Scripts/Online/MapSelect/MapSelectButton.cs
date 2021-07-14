using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapSelectButton : MonoBehaviour
{
    public bool isBuildIn = false;
    public MapData data;
    public Text btnText;
    private Button button;


    public void SetButton(MapData _data)
    {
        data = _data;
        btnText.text = _data.fileName;
        button = GetComponent<Button>();

        button.onClick.AddListener(delegate { SendData(data); });
    }

    private void SendData(MapData _data)
    {
        //object[] _datasToSent = new object[1] { (object)_data };
        var _byteData = MyPhotonExtension.ObjectToByteArray(_data);
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; // You would have to set the Receivers to All in order to receive this event on the local client as well
        PhotonNetwork.RaiseEvent(CustomPropertyCode.UPDATE_MAP_EVENTCODE, _byteData, raiseEventOptions, SendOptions.SendReliable);

        //* RPC OK
        //Debug.Log(" [Set Map] " + _data.fileName + " [data] " + _byteData);
        //MapSelectManager.instance.CallRPC(_byteData);

        /* PROPERTY NOT WORKING
        PhotonNetwork.CurrentRoom.SetCustomProperties(MyPhotonExtension.WrapToHash(new object[] {
            CustomPropertyCode.ROOM_MAP, _byteData
        }));*/
    }
}
