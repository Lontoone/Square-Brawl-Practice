using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SaveTile : MonoBehaviour
{
    public const string SAVE_FOLDER = "maps/";
    public InputField saveName;
    public SelectRangeData checkRange;

    private TileMapEditorControl tilemapEditor;
    public void Start()
    {
        tilemapEditor = FindObjectOfType<TileMapEditorControl>();
        checkRange.ReadData();
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
                CellData _cell = new CellData(i, tilemapEditor.cellStateMap[i], CheckCellOrientation(i));
                _mapData.cellDatas.Add(_cell);
            }
        }
        Debug.Log("Save!");
        SaveAndLoad.Save<MapData>(_mapData, (SAVE_FOLDER + _mapData.fileName).CombinePersistentPath());

    }

    private CellOrientation CheckCellOrientation(int _cellIndex)
    {
        TileCell[] _cellsToCheck = TileMapManager.instance.GetSelectRangeCells(checkRange, _cellIndex);

        TileCell _center = TileMapManager.instance.gridCells[_cellIndex];
        TileCell _topCell = CheckCell(_cellIndex, _cellsToCheck[0]) ? _cellsToCheck[0] : null;
        TileCell _bottomCell = CheckCell(_cellIndex, _cellsToCheck[1]) ? _cellsToCheck[1] : null;
        TileCell _leftCell = CheckCell(_cellIndex, _cellsToCheck[2]) ? _cellsToCheck[2] : null;
        TileCell _rightCell = CheckCell(_cellIndex, _cellsToCheck[3]) ? _cellsToCheck[3] : null;

        //Top 
        if (_topCell == null && _leftCell == null && (_rightCell != null || _bottomCell != null))
        {
            return CellOrientation.TOP_LEFT;
        }
        else if (_topCell == null && _leftCell != null && _rightCell != null)
        {
            return CellOrientation.TOP_MIDDLE;
        }
        else if (_topCell == null && _rightCell != null && (_leftCell != null || _bottomCell != null))
        {
            return CellOrientation.TOP_RIGHT;
        }

        //Middle
        else if (_topCell != null && _leftCell == null && (_rightCell != null || _bottomCell != null))
        {
            return CellOrientation.MIDDLE_LEFT;
        }
        else if (_topCell != null && _leftCell != null && _rightCell != null && _bottomCell != null)
        {
            return CellOrientation.MIDDLE_FILL;
        }
        else if (_topCell != null && _rightCell != null && (_leftCell != null || _bottomCell != null))
        {
            return CellOrientation.MIDDLE_RIGHT;
        }

        //Bottom
        else if (_topCell != null && _leftCell == null && _bottomCell == null)
        {
            return CellOrientation.BOTTOM_LEFT;
        }
        else if (_topCell != null && _leftCell != null && _rightCell != null && _bottomCell == null)
        {
            return CellOrientation.BOTTOM_MIDDLE;
        }
        else if (_topCell != null && _rightCell != null && (_leftCell != null || _bottomCell == null))
        {
            return CellOrientation.BOTTOM_RIGHT;
        }

        return CellOrientation.TOP_MIDDLE;
    }
    private bool CheckCell(int _cellIndex, TileCell _target)
    {
        if (_target == null)
        {
            return false;
        }
        return tilemapEditor.CompareState(_cellIndex, _target.grid_index);
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
    public CellOrientation orientation;
    public CellData(int _index, CellState _state, CellOrientation _orientation)
    {
        index = _index;
        state = _state;
        orientation = _orientation;
    }
}
