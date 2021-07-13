using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LoadMapUIControl : MonoBehaviour
{
    public static event Action<string> OnLevelFileLoaded;
    public static string currentSelectedFile;
    public Button fileBtnPrefab;
    public GameObject btnContainer;

    private TileMapEditorControl editorControl;
    private SaveTile saveTile;
    private static string[] filePaths;
    public void Start()
    {
        editorControl = FindObjectOfType<TileMapEditorControl>();
        saveTile = FindObjectOfType<SaveTile>();

        btnContainer.SetActive(false);
        LoadMapList();
    }

    public void LoadMapList()
    {
        ClearContainer();
        if (!Directory.Exists(SaveTile.SAVE_FOLDER.CombinePersistentPath())) { return; }

        filePaths = Directory.GetFiles(SaveTile.SAVE_FOLDER.CombinePersistentPath());
        for (int i = 0; i < filePaths.Length; i++)
        {
            Button _btn = Instantiate(fileBtnPrefab, btnContainer.transform);
            string _path = filePaths[i];
            _btn.onClick.AddListener(delegate
            {
                OnLevelFileLoaded?.Invoke(_path);
                currentSelectedFile = _path;
                //Load(_path);
            });
            _btn.GetComponentInChildren<Text>().text = Path.GetFileName(_path);
        }

      
    }

    private void ClearContainer()
    {
        foreach (Transform child in btnContainer.transform)
        {
            Destroy(child.gameObject);
        }
    }
   

}
