using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMapEditorControl : MonoBehaviour
{
    public SelectRangeData rangeData;
    public bool isBuild = true;
    private bool inMouseHolding = false;

    private static int _currentMouseHoverCellId = 0;
    private static int _previousMouseHoverCellId = 0;

    [SerializeField]
    List<TileCell> previewTileCells = new List<TileCell>();
    Dictionary<int, bool> m_cellIsSelectedMap = new Dictionary<int, bool>();


    public void Start()
    {
        InitDict();
        TileCell.OnCellMouseEnter += SetPreviewRange;
        TileCell.OnCellMouseExit += SetPreviousId;
        //TileCell.OnCellMouseDown += Select;
        rangeData.ReadData();
    }
    public void OnDestroy()
    {
        TileCell.OnCellMouseEnter -= SetPreviewRange;
        TileCell.OnCellMouseExit -= SetPreviousId;
        //TileCell.OnCellMouseDown -= Select;
    }
    public void Update()
    {
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
    }
    public void SetPreviewRange(int _centerGridId)
    {
        _currentMouseHoverCellId = _centerGridId;

        //temp: Clear preview color
        for (int i = 0; i < TileMapManager.instance.gridCells.Count; i++)
        {
            if (!m_cellIsSelectedMap[TileMapManager.instance.gridCells[i].grid_index])
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

                if (!m_cellIsSelectedMap[_cell.grid_index])
                {
                    _cell.SetHoverColor();
                }
            }
        }
    }

    private void SetPreviousId(int _prev)
    {
        _previousMouseHoverCellId = _prev;
    }
    private void InitDict()
    {
        for (int i = 0; i < TileMapManager.instance.mapSize.x * TileMapManager.instance.mapSize.y; i++)
        {
            m_cellIsSelectedMap.Add(i, false);
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
            m_cellIsSelectedMap[previewTileCells[i].grid_index] = true;
            previewTileCells[i].SetusedColor();
        }
    }

    private void DeSelect()
    {
        for (int i = 0; i < previewTileCells.Count; i++)
        {
            m_cellIsSelectedMap[previewTileCells[i].grid_index] = false;
            previewTileCells[i].SetWhiteColor();
        }
    }

    public void SetIsBuilding(bool _isBuild)
    {
        isBuild = _isBuild;
    }


}
