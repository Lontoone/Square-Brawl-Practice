﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class LoadTileHelper
{
    public static MapData[] LoadTileMaps()
    {
        List<MapData> res = new List<MapData>();
        string[] filePaths;
        if (!Directory.Exists(SaveTile.SAVE_FOLDER.CombinePersistentPath())) { return null; }

        filePaths = Directory.GetFiles(SaveTile.SAVE_FOLDER.CombinePersistentPath());

        for (int i = 0; i < filePaths.Length; i++)
        {
            string _path = filePaths[i];
            MapData _data = SaveAndLoad.Load<MapData>(_path);
            res.Add(_data);
        }

        //For resources Build in:
        List<TextAsset> builtinMapsAssets = Resources.LoadAll<TextAsset>(MapSelectManager.BUILTIN_MAPS_FOLDER).ToList();
        for (int i = 0; i < builtinMapsAssets.Count; i++)
        {
            MapData _data = JsonUtility.FromJson<MapData>(builtinMapsAssets[i].text);
            res.Add(_data);

        }
        return res.ToArray();
    }
}
