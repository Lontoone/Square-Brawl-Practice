using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileStyleManager : MonoBehaviour
{
    public static TileStyleManager instance;
    public static TileImageCollection selectedCollection;
    public TileImageCollection imageCollection;
    public SelectRangeData checkRange;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void Start()
    {
        checkRange.ReadData();

        TileMapEditorControl.OnCellChanged += SetCell;
        TileMapEditorControl.OnCellChanged += SetNearbyCell;

    }
    private void OnDestroy()
    {
        TileMapEditorControl.OnCellChanged -= SetCell;
        TileMapEditorControl.OnCellChanged -= SetNearbyCell;
    }

    public void SetCell(int _index)
    {
        CellState _state;
        if (TileMapManager.instance.cellStateMap.TryGetValue(_index, out _state) && _state == CellState.NONE)
        {
            //Set none to white
            SetTillImage(TileMapManager.instance.gridCells[_index], 255);
        }
        else if (_state == CellState.SAW)
        {
            SetTillImage(TileMapManager.instance.gridCells[_index], 300);
            TileCell _cell = TileMapManager.instance.gridCells[_index];

            if (_cell.GetComponent<Saw>() != null)
            {
                _cell.gameObject.AddComponent<Saw>();
            }
        }
        else
        {
            int _tileCondition = CheckCellOrientation(_index);
            SetTillImage(TileMapManager.instance.gridCells[_index], _tileCondition);
        }
    }

    public void SetNearbyCell(int _index)
    {
        TileCell _center = TileMapManager.instance.gridCells[_index];
        TileCell[] _cells = TileMapManager.instance.GetSelectRangeCells(checkRange, _index);
        for (int i = 0; i < _cells.Length; i++)
        {
            //if (_cells[i] != null && CheckCellIsSameState(_center, _cells[i]))
            if (_cells[i] != null)
            {
                SetCell(_cells[i].grid_index);
            }
        }
    }

    private int CheckCellOrientation(int _cellIndex)
    {
        TileCell[] _cellsToCheck = TileMapManager.instance.GetSelectRangeCells(checkRange, _cellIndex);
        return GetConditionIndex(_cellIndex, _cellsToCheck);
    }
    private int GetConditionIndex(int _cellIndex, TileCell[] _jiugongge)
    {
        TileCell _center = TileMapManager.instance.gridCells[_cellIndex];
        float _res = 0;
        for (int i = 0; i < 8; i++)
        {
            if (_jiugongge.Length > 0 && CheckCellIsSameState(_center, _jiugongge[i]))
            {
                int _pow = (7 - i);
                _res += Mathf.Pow(2, _pow);
            }
        }
        //Debug.Log("cell " + _cellIndex + " condition " + _res);
        return (int)_res;
    }
    private bool CheckCellIsSameState(TileCell _center, TileCell _target)
    {
        if (_target == null || _center == null)
        {
            return false;
        }
        return TileMapManager.instance.cellStateMap[_center.grid_index] == TileMapManager.instance.cellStateMap[_target.grid_index];
    }

    private void SetTillImage(TileCell _cell, int _conditionCode)
    {
        //Debug.Log(_cell.grid_index + " " + imageCollection.name);
        _cell.spriteRenderer.sprite = imageCollection.GetSprite(_conditionCode);
        _cell.hindSpriteRenderer.sprite = imageCollection.GetHindSprite(_conditionCode);
        _cell.frontSpriteRenderer.sprite = imageCollection.GetFtontSprite(_conditionCode);
    }

    public void ApplyNewStyle(TileImageCollection _data)
    {
        imageCollection = _data;
        selectedCollection = _data;
        for (int i = 0; i < TileMapManager.instance.gridCells.Count; i++)
        {
            SetCell(i);
        }
    }

}
