using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;

public class TillStyleLoader : MonoBehaviour
{
    public GameObject container;
    public Button buttonPrefab;
    public Text styleNameText;

    public Image currentStyleIcon;
    public Image nextStyleIcon;
    public Image prevStyleIcon;

    public static int currentStyleIndex = 0;
    private TileImageCollection[] styleDatas;

    private const string styleDataPaht = "TileData/";

    public static string s_StyleName;
    public void Start()
    {
        currentStyleIndex = -1;
        LoadStyleData();

        //TODO:改成preview btn
        styleDatas = Resources.LoadAll<TileImageCollection>(styleDataPaht);

        //Switch(0); //error
    }

    public void Switch(int _optration)
    {
        if (MapSelectionTrigger.GridFinish)
        { 
            if (prevStyleIcon == null)
            {
                return;
            }

            currentStyleIndex = Mathf.Clamp(currentStyleIndex + _optration, 0, styleDatas.Length - 1);
            ChangeStyle(styleDatas[currentStyleIndex]);

            styleNameText.text = styleDatas[currentStyleIndex].name;

            currentStyleIcon.sprite = styleDatas[currentStyleIndex].GetIcon();
            if (currentStyleIndex > 0)
            {
                prevStyleIcon.sprite = styleDatas[currentStyleIndex - 1].GetIcon();
                //Debug.Log("prevStyleIcon " + (prevStyleIcon.sprite == null));
            }
            if (currentStyleIndex < styleDatas.Length - 1)
            {
                nextStyleIcon.sprite = styleDatas[currentStyleIndex + 1].GetIcon();
            }

            MapSelectionTrigger.StyleFinish = true;
        }
    }

    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnMapStyleChanged;
    }
    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnMapStyleChanged;
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
        Debug.Log("Send Style ");
        TileStyleManager.instance.ApplyNewStyle(_data);

        //send Data
        var _byteData = MyPhotonExtension.ObjectToByteArray(_data.name);
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(CustomPropertyCode.UPDATE_STYLE_EVENTCODE, _byteData, raiseEventOptions, SendOptions.SendReliable);

    }


    private void OnMapStyleChanged(EventData obj) //TODO  Load Style Animation
    {
        Debug.LogWarning("Change Style");
        StartCoroutine(ChangeStyle(obj));
    }

    private IEnumerator ChangeStyle(EventData obj)
    {
        yield return new WaitUntil(() => MapSelectionTrigger.GridFinish);
        Debug.Log("[OnStyleChanged]");

        byte eventCode = obj.Code;
        if (eventCode == CustomPropertyCode.UPDATE_STYLE_EVENTCODE)
        {
            string styleName = (string)MyPhotonExtension.ByteArrayToObject((byte[])obj.CustomData);
            s_StyleName = styleName;
            //Debug.Log(s_StyleName);
            if (styleNameText != null)
            {
                styleNameText.text = s_StyleName;
            }
            TileImageCollection tileImageCollection = Resources.Load<TileImageCollection>(styleDataPaht + styleName);
            TileStyleManager.instance.ApplyNewStyle(tileImageCollection);
        }

        yield return null;
        MapSelectionTrigger.StyleFinish = true;
    }
}
