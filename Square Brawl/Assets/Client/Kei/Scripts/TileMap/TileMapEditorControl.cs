using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class TileMapEditorControl : MonoBehaviour
{
    public SelectRangeData rangeData;
    public CellState buildType = CellState.CELL;
    public static event Action<int> OnCellChanged;
    public bool isBuild = true;
    private bool inMouseHolding = false;

    private static int _currentMouseHoverCellId = 0;
    private static int _previousMouseHoverCellId = 0;

    [SerializeField]
    List<TileCell> previewTileCells = new List<TileCell>();
    private Dictionary<int, CellState> cellStateMap
    {
        get { return TileMapManager.instance.cellStateMap; }
        set { TileMapManager.instance.cellStateMap = value; }
    }

    private Dictionary<CellState, Color> cellStateColor = new Dictionary<CellState, Color>() {
        { CellState.NONE, Color.white },
        { CellState.CELL, Color.blue },
        { CellState.SAW, Color.cyan },
        { CellState.EMPTY, Color.cyan },
    };

    public void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoad;

        TileCell.OnCellMouseEnter += SetPreviewRange;
        TileCell.OnCellMouseExit += ClearPreviewCell;
        //TileCell.OnCellMouseExit += SetPreviousId;

        LoadMapUIControl.OnLevelFileLoaded += Load;

        rangeData.ReadData();

        //Wait for tile map to gernerate:
        /*
        WaitForEndOfFrame _wait = new WaitForEndOfFrame();
        Debug.Log(TileMapManager.instance == null);
        while (TileMapManager.instance == null)
        {
            yield return _wait;
        }
        InitDict();
        TileMapManager.instance.GenerateGrid();
        //TileStyleManager.instance.ApplyNewStyle();*/
    }

    private void OnDisable()
    {

        previewTileCells.Clear();
    }

    public void OnDestroy()
    {
        TileCell.OnCellMouseEnter -= SetPreviewRange;
        TileCell.OnCellMouseExit -= ClearPreviewCell;
        //TileCell.OnCellMouseExit -= SetPreviousId;

        LoadMapUIControl.OnLevelFileLoaded -= Load;
        SceneManager.sceneLoaded -= OnSceneLoad;


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
        if (inMouseHolding && IsMousePosInsideMapBounds())
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

    public void ClearPreviewCell(int _centerGridid)
    {
        TileCell[] leavedCell = TileMapManager.instance.GetSelectRangeCells(rangeData, _centerGridid);

        foreach (TileCell _cell in leavedCell)
        {
            //if (cellStateMap[_cell.grid_index] == CellState.NONE)
            if (previewTileCells.Remove(_cell) && cellStateMap[_cell.grid_index] == CellState.NONE)
            {
                _cell.SetEmptyColor();
            }
        }
        _currentMouseHoverCellId = 0;
    }
    public void SetPreviewRange(int _centerGridId)
    {
        _currentMouseHoverCellId = _centerGridId;

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
            if (cellStateMap.ContainsKey(i))
            {
                //cellStateMap[i] = CellState.NONE;
            }
            else
            {
                cellStateMap.Add(i, CellState.NONE);
            }
        }
    }

    public void Select(int _center)
    {
        if (_currentMouseHoverCellId == _previousMouseHoverCellId)
        {
            return;
        }

        if (isBuild)
        {
            DoSelect(TileMapManager.instance.gridCells[_center]);
            //DoSelect();
        }
        else
        {
            DeSelect();
        }
        _previousMouseHoverCellId = _currentMouseHoverCellId;
    }
    private void DoSelect(TileCell _center)
    {
        //Test if is interset another cell?
        if (buildType != CellState.CELL && IsInterset(_center, rangeData))
        {
            return;
        }

        for (int i = 0; i < previewTileCells.Count; i++)
        {
            if (cellStateMap[previewTileCells[i].grid_index] == CellState.NONE)
            {
                if (buildType == CellState.CELL)
                {
                    BuildSingleCell(previewTileCells[i]);
                }
                else if (buildType == CellState.SAW)
                {
                    TileCell cell = BuildSingleCell(previewTileCells[i]);
                    if (cell != _center)
                    {
                        cellStateMap[cell.grid_index] = CellState.EMPTY;
                    }
                    else
                    {
                        cell.conboundRange.data = rangeData.data;
                    }
                    cell.conboundCenter = _center;
                }
                OnCellChanged?.Invoke(previewTileCells[i].grid_index);
            }
        }
    }

    //check is the current mouse position is inside the map bounds:
    private bool IsMousePosInsideMapBounds() {
        Vector2 _mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        return TileMapManager.instance.mapBounds.Contains(_mousePos);
    }

    private TileCell BuildSingleCell(TileCell _cell)
    {
        cellStateMap[_cell.grid_index] = buildType;
        _cell.SetColor(cellStateColor[buildType]);
        _cell.conboundCenter = _cell;
        return _cell;
    }
    //private void BuildSaw(TileCell _target , TileCell _center) {}

    private void DeSelect()
    {
        for (int i = 0; i < previewTileCells.Count; i++)
        {
            //Check if is single cell or counbound cell?  (single cell's conboundCenter = self)
            if (previewTileCells[i].conboundCenter == previewTileCells[i])
            {
                cellStateMap[previewTileCells[i].grid_index] = CellState.NONE;
                previewTileCells[i].SetEmptyColor();

                OnCellChanged?.Invoke(previewTileCells[i].grid_index);
            }
            else if (previewTileCells[i].conboundCenter != null)
            {
                //Find the center, and clear cells around it.
                TileCell _conboundCenter = previewTileCells[i].conboundCenter;
                TileCell[] _conboundCells = TileMapManager.instance.GetSelectRangeCells(_conboundCenter.conboundRange, _conboundCenter.grid_index);

                //remove center cell's saw (TODO:做成通用類型)
                DeleteSpecialCell(_conboundCenter);

                foreach (TileCell _cell in _conboundCells)
                {
                    if (_cell == null) { continue; }

                    cellStateMap[_cell.grid_index] = CellState.NONE;
                    _cell.SetColor(cellStateColor[CellState.NONE]);
                    previewTileCells.Remove(_cell);

                    OnCellChanged?.Invoke(_cell.grid_index);
                }
            }

        }
    }
    private void DeleteSpecialCell(TileCell _cell)
    {
        //TODO:做成泛型?
        Saw saw = _cell.GetComponent<Saw>();
        if (saw != null)
        {
            Destroy(saw);
        }
    }

    private bool IsInterset(TileCell _center, SelectRangeData _range)
    {
        foreach (TileCell _cell in TileMapManager.instance.GetSelectRangeCells(_range, _center.grid_index))
        {
            if (_cell != null && cellStateMap[_cell.grid_index] != CellState.NONE)
            {
                return true;
            }
        }
        return false;
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
        
        TileStyleManager.instance.SetCell(_index);
        TileStyleManager.instance.SetNearbyCell(_index);
        _cell.SetColor(cellStateColor[_state]);
    }

    private void Load(string _path)
    {
        Debug.Log("load Map "+ _path);
        //reset picture
        MapData mapData = SaveAndLoad.Load<MapData>(_path);

        //Load to map
        SetUpMapData(mapData);
    }

    /*BUTTON FUNCTION*/
    public void SetSelectRange(TextAsset _selectRangeData)
    {
        rangeData.data = _selectRangeData;
        rangeData.ReadData();
    }

    public void SetIsBuilding(bool _isBuild)
    {
        isBuild = _isBuild;
    }

    public void SetBuildCell()
    {
        SetBuildType(CellState.CELL);
    }
    public void SetBuildSaw()
    {
        SetBuildType(CellState.SAW);
    }
    public void SetBuildEmpty()
    {
        SetBuildType(CellState.EMPTY);
    }
    public void SetBuildType(CellState _state)
    {
        buildType = _state;
    }

    
    private void OnSceneLoad(Scene arg0, LoadSceneMode arg1)
    {

        if (arg0.name == "GridSample")
        {
            FindObjectOfType<TileMapManager>().GenerateGrid();
            InitDict();
        }
    }
}


