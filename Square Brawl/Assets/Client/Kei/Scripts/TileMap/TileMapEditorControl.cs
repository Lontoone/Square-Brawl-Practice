using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class TileMapEditorControl : MonoBehaviour
{
    public SelectRangeData rangeData;
    public bool isBuild = true;
    private bool inMouseHolding = false;

    private static int _currentMouseHoverCellId = 0;
    private static int _previousMouseHoverCellId = 0;

    [SerializeField]
    List<TileCell> previewTileCells = new List<TileCell>();
    public Dictionary<int, CellState> cellStateMap = new Dictionary<int, CellState>();

    private Dictionary<CellState, Color> cellStateColor = new Dictionary<CellState, Color>() {
        { CellState.NONE, Color.white },
        { CellState.CELL, Color.green },
        { CellState.SAW, Color.green },
        { CellState.EMPTY, Color.green },
    };

    public void Start()
    {
        InitDict();
        TileCell.OnCellMouseEnter += SetPreviewRange;
        TileCell.OnCellMouseExit += SetPreviousId;
        //TileCell.OnCellMouseDown += Select;
        rangeData.ReadData();

        TileMapManager.instance.GenerateGrid();
    }
    public void OnDestroy()
    {
        TileCell.OnCellMouseEnter -= SetPreviewRange;
        TileCell.OnCellMouseExit -= SetPreviousId;
        //TileCell.OnCellMouseDown -= Select;
    }
    public void Update()
    {
#if ENABLE_INPUT_SYSTEM
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            inMouseHolding = true;
        }
        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            inMouseHolding = false;
        }
        if (inMouseHolding)
        {
            Select(_currentMouseHoverCellId);
        }
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
        if (Input.GetMouseButtonDown(0))
        {
            inMouseHolding = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            inMouseHolding = false;
        }
        if (Input.GetMouseButton(0))
        {
            Select(_currentMouseHoverCellId);
        }
#endif
    }
    public void SetPreviewRange(int _centerGridId)
    {
        _currentMouseHoverCellId = _centerGridId;

        //temp: Clear preview color
        for (int i = 0; i < TileMapManager.instance.gridCells.Count; i++)
        {
            if (cellStateMap[TileMapManager.instance.gridCells[i].grid_index] == CellState.NONE)
            {
                TileMapManager.instance.gridCells[i].SetWhiteColor();
            }
            previewTileCells.Clear();
        }

        Vector2Int _centerCell = TileMapManager.instance.CellToVector2(TileMapManager.instance.gridCells[_centerGridId].transform);

        for (int i = 0; i < rangeData.range.Count; i++)
        {
            Transform _cellTrans = TileMapManager.instance.Vector2ToCell(_centerCell + rangeData.range[i]);
            if (_cellTrans != null)
            {
                TileCell _cell = _cellTrans.GetComponent<TileCell>();
                previewTileCells.Add(_cell);

                if (cellStateMap[_cell.grid_index] == CellState.NONE)
                {
                    _cell.SetHoverColor();
                }
            }
        }
    }
    public bool MatchState(int _index, params CellState[] _states)
    {
        return _states.ToList().Contains(cellStateMap[_index]);
    }

    public bool CompareState(int _index, int _index2)
    {
        return cellStateMap[_index] == cellStateMap[_index2];
    }
    private void SetPreviousId(int _prev)
    {
        _previousMouseHoverCellId = _prev;
    }
    private void InitDict()
    {
        for (int i = 0; i < TileMapManager.instance.mapSize.x * TileMapManager.instance.mapSize.y; i++)
        {
            cellStateMap.Add(i, CellState.NONE);
        }
    }

    public void SetSelectRange(TextAsset _selectRangeData)
    {
        rangeData.data = _selectRangeData;
        rangeData.ReadData();
    }

    public void Select(int _center)
    {
        if (_currentMouseHoverCellId == _previousMouseHoverCellId)
        {
            return;
        }

        if (isBuild)
        {
            DoSelect();
        }
        else
        {
            DeSelect();
        }
    }
    private void DoSelect()
    {
        for (int i = 0; i < previewTileCells.Count; i++)
        {
            cellStateMap[previewTileCells[i].grid_index] = CellState.CELL;
            previewTileCells[i].SetusedColor();
        }
    }

    private void DeSelect()
    {
        for (int i = 0; i < previewTileCells.Count; i++)
        {
            cellStateMap[previewTileCells[i].grid_index] = CellState.NONE;
            previewTileCells[i].SetWhiteColor();
        }
    }

    public void SetIsBuilding(bool _isBuild)
    {
        isBuild = _isBuild;
    }

    public void SetUpMapData(MapData _data)
    {
        ReSetMap();
        for (int i = 0; i < _data.cellDatas.Count; i++)
        {
            cellStateMap[_data.cellDatas[i].index] = _data.cellDatas[i].state;
            SetCellStateColor(_data.cellDatas[i].index, _data.cellDatas[i].state);
        }
    }
    private void ReSetMap()
    {
        for (int i = 0; i < TileMapManager.instance.cellCount; i++)
        {
            cellStateMap[i] = CellState.NONE;
            SetCellStateColor(i, CellState.NONE);
        }
    }

    private void SetCellStateColor(int _index, CellState _state)
    {
        TileCell _cell = TileMapManager.instance.gridCells[_index];
        _cell.SetColor(cellStateColor[_state]);
    }
}


