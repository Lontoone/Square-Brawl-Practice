using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class LoadTileHelper
{
    public static int mapCounts
    {
        get
        {
            int fCount = Directory.GetFiles(SaveTile.SAVE_FOLDER.CombinePersistentPath(), "*", SearchOption.TopDirectoryOnly).Length;
            int resCount = Resources.LoadAll(MapSelectManager.BUILTIN_MAPS_FOLDER).Length;
            Resources.UnloadUnusedAssets();
            Debug.Log("map count " + (fCount + resCount));
            return fCount + resCount;
        }
    }
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
        TextAsset[] builtinMapsAssets = Resources.LoadAll<TextAsset>(MapSelectManager.BUILTIN_MAPS_FOLDER);
        Debug.Log("load res maps " + builtinMapsAssets.Length);
        //object[] builtinMapsAssets = Resources.LoadAll("DefaultMaps");
        /*
        TextAsset _t = Resources.Load<TextAsset>("DefaultMaps/Default1");
        Debug.Log("load res maps " + _t);*/

        for (int i = 0; i < builtinMapsAssets.Length; i++)
        {
            MapData _data = JsonUtility.FromJson<MapData>(builtinMapsAssets[i].text);
            res.Add(_data);

        }
        return res.ToArray();
    }
}
