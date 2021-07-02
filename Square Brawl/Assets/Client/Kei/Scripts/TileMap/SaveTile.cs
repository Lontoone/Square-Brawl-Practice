using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
public class SaveTile : MonoBehaviour
{
    public const string SAVE_FOLDER = "maps/";
    public InputField saveName;
    public SelectRangeData checkRange;

    private TileMapEditorControl tilemapEditor;

    public TileImageCollection imageCollection; //TEST

    public void Start()
    {

        LoadMapUIControl.OnLevelFileLoaded += SetFileNameOnLoad;
        tilemapEditor = FindObjectOfType<TileMapEditorControl>();
        checkRange.ReadData();
    }
    public void OnDestroy()
    {
        LoadMapUIControl.OnLevelFileLoaded -= SetFileNameOnLoad;
    }
    public void Save()
    {
        MapData _mapData = new MapData();
        _mapData.fileName = saveName.text;

        for (int i = 0; i < TileMapManager.instance.cellCount; i++)
        {
            //if (tilemapEditor.cellStateMap[i] != CellState.NONE)
            if (!tilemapEditor.MatchState(i, CellState.NONE, CellState.EMPTY))
            {
                CellData _cell = new CellData(i, TileMapManager.instance.cellStateMap[i]);
                _mapData.cellDatas.Add(_cell);

            }
        }
        Debug.Log("Save!");
        SaveAndLoad.Save<MapData>(_mapData, (SAVE_FOLDER + _mapData.fileName).CombinePersistentPath());

    }

    private void SetFileNameOnLoad(string _path)
    {
        saveName.text = Path.GetFileName(_path);
    }

}
[System.Serializable]
public class MapData
{
    public string fileName;
    public List<CellData> cellDatas = new List<CellData>();


}
[System.Serializable]
public struct CellData
{
    public int index;
    public CellState state;
    //public int tileCondition;
    public CellData(int _index, CellState _state)//, int _tileCondition)
    {
        index = _index;
        state = _state;
        //tileCondition = _tileCondition;
    }
}
