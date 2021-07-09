using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TillStyleLoader : MonoBehaviour
{
    public GameObject container;
    public Button buttonPrefab;
    private const string styleDataPaht = "TileData/";

    public void LoadStyleData()
    {
        TileImageCollection[] datas = Resources.LoadAll<TileImageCollection>(styleDataPaht);
        for (int i = 0; i < datas.Length; i++)
        {
            Button _btn = Instantiate(buttonPrefab, container.transform);
            _btn.onClick.AddListener(delegate {
                //TODO:
            });
        }

    }

}
