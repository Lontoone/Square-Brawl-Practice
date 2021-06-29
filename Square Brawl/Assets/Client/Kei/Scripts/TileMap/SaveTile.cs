using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SaveTile : MonoBehaviour
{
    public const string SAVE_FOLDER = "maps/";
    public InputField saveName;
    private TileMapEditorControl tilemapEditor;
    public void Start()
    {
        tilemapEditor = FindObjectOfType<TileMapEditorControl>();
    }
    public void Save()
    {
        MapData _mapData = new MapData();
        _mapData.fileName = saveName.text;

        for (int i = 0; i < TileMapManager.instance.cellCount; i++)
        {
            CellData _cell = new CellData(i, tilemapEditor.cellIsSelectedMap[i]);
            _mapData.cellDatas.Add(_cell);
        }
        Debug.Log("Save!");
        SaveAndLoad.Save<MapData>(_mapData, (SAVE_FOLDER+_mapData.fileName).CombinePersistentPath());
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
    public TileMapEditorControl.CellState state;
    public CellData(int _index, TileMapEditorControl.CellState _state)
    {
        index = _index;
        state = _state;
    }
}
