using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TillStyleLoader : MonoBehaviour
{
    public GameObject container;
    public Button buttonPrefab;
    private const string styleDataPaht = "TileData/";
    public void Start()
    {
        LoadStyleData();
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
    }

}
