using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LoadMapUIControl : MonoBehaviour
{
    public Button fileBtnPrefab;
    public GameObject btnContainer;

    private TileMapEditorControl editorControl;
    private SaveTile saveTile;
    private static string[] filePaths;
    public void Start()
    {
        editorControl = FindObjectOfType<TileMapEditorControl>();
        saveTile = FindObjectOfType<SaveTile>();
    }

    public void LoadMapList()
    {
        filePaths = Directory.GetFiles(SaveTile.SAVE_FOLDER.CombinePersistentPath());
        for (int i = 0; i < filePaths.Length; i++)
        {
            Button _btn = Instantiate(fileBtnPrefab, btnContainer.transform);
            string _path = filePaths[i];
            _btn.onClick.AddListener(delegate
            {
                Load(_path);
            });
            _btn.GetComponentInChildren<Text>().text = Path.GetFileName(_path);
        }
    }

    public void Load(string _path)
    {
        MapData mapData = SaveAndLoad.Load<MapData>(_path);
        saveTile.saveName.text = Path.GetFileName(_path);
        //TODO: Load to map
        editorControl.SetUpMapData(mapData);
    }
}
