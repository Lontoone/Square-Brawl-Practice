using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileStyleManager : MonoBehaviour
{
    public static TileStyleManager instance;
    public static TileImageCollection selectedCollection;
    public TileImageCollection imageCollection;
    public SelectRangeData checkRange;

    private SpriteRenderer foreground { get { return TileMapManager.instance.foreground; } }
    private SpriteRenderer midground { get { return TileMapManager.instance.midground; } }
    private SpriteRenderer background { get { return TileMapManager.instance.background; } }
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
    public void OnEnable()
    {
        checkRange.ReadData();

        if (selectedCollection == null)
        {
            selectedCollection = imageCollection;
        }

        TileMapEditorControl.OnCellChanged += SetCell;
        TileMapEditorControl.OnCellChanged += SetNearbyCell;
    }


    private void OnDisable()
    {
        TileMapEditorControl.OnCellChanged -= SetCell;
        TileMapEditorControl.OnCellChanged -= SetNearbyCell;
        //Destroy(instance);
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
            TileCell _cell = TileMapManager.instance.gridCells[_index];
            SetTillImage(_cell, 255, 0, 0, 0);
            _cell.SetEmptyColor();

            //clear saw
            Destroy(_cell.GetComponent<Saw>());
        }
        else if (_state == CellState.SAW)
        {
            SetTillImage(TileMapManager.instance.gridCells[_index], 300);
            TileCell _cell = TileMapManager.instance.gridCells[_index];
            /*
            if (_cell.GetComponent<Saw>() != null)
            {
                _cell.gameObject.AddComponent<Saw>();
            }*/
        }
        else
        {
            int _tileCondition = CheckCellOrientation(_index);

            TileCell _cell = TileMapManager.instance.gridCells[_index];
            SetTillImage(_cell, _tileCondition);

            //clear saw
            Destroy(_cell.GetComponent<Saw>());
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
        if (_target == null || _center == null || TileMapManager.instance.cellStateMap.Count == 0)
        {
            return false;
        }
        return TileMapManager.instance.cellStateMap[_center.grid_index] == TileMapManager.instance.cellStateMap[_target.grid_index];
    }

    private void SetTillImage(TileCell _cell, int _conditionCode, int _frontOrder = 4, int _midOrder = 3, int _hindOrder = 2)
    {
        //Debug.Log(_cell.grid_index + " " + imageCollection.name);
        if (_cell != null)
        {
            _cell.spriteRenderer.sprite = imageCollection.GetSprite(_conditionCode); //TODO Bugs occur irregularly
            _cell.hindSpriteRenderer.sprite = imageCollection.GetHindSprite(_conditionCode);
            _cell.frontSpriteRenderer.sprite = imageCollection.GetFrontSprite(_conditionCode);
            _cell.OpenCellSprite();

            _cell.spriteRenderer.sortingOrder = _midOrder;
            _cell.hindSpriteRenderer.sortingOrder = _hindOrder;
            _cell.frontSpriteRenderer.sortingOrder = _frontOrder;
        }
    }

    public void ApplyNewStyle(TileImageCollection _data)
    {
        imageCollection = _data;
        selectedCollection = _data;

        if (TileMapManager.instance == null) { return; }

        for (int i = 0; i < TileMapManager.instance.gridCells.Count; i++)
        {
            SetCell(i);
        }
        //Background
        SetupBackground();

        //Materail
        Sprite _mask = _data.GetSprite(900);
        if (_mask != null)
        {
            TileMapManager.instance.gridCells[0].spriteRenderer.sharedMaterial.SetTexture("_NoiseTex", _mask.texture);
        }

        Sprite _below = _data.GetSprite(901);
        if (_below != null)
        {
            TileMapManager.instance.gridCells[0].spriteRenderer.sharedMaterial.SetTexture("_BelowTex", _below.texture);
        }

    }

    private void SetupBackground()
    {
        if (foreground != null)
            foreground.sprite = TileStyleManager.selectedCollection.GetSprite(301);

        if (midground != null)
            midground.sprite = TileStyleManager.selectedCollection.GetSprite(302);

        if (background != null)
            background.sprite = TileStyleManager.selectedCollection.GetSprite(303);
    }

    /*private void SetPlayer()
    {
        if (playerBody != null)
            playerBody.sprite = TileStyleManager.selectedCollection.GetSprite(401);

        if (playerUI != null)
            playerUI.sprite = TileStyleManager.selectedCollection.GetSprite(402);
    }*/

}
