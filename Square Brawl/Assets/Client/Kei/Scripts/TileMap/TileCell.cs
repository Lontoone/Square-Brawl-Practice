using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileCell : MonoBehaviour//, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public int grid_index;
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer hindSpriteRenderer;
    public SpriteRenderer frontSpriteRenderer;
    //[HideInInspector]
    public TileCell conboundCenter;
    public SelectRangeData conboundRange;
    //public Color onHoverColor, onSelectColor, rangeHintColor;

    //public static event Action<int> OnCellHover;
    public static event Action<int> OnCellMouseEnter;
    public static event Action<int> OnCellMouseExit;
    public static event Action<int> OnCellMouseDown;

    private static readonly Color _s_transprentColor = new Color(1, 1, 1, 0);
    private static readonly Color _s_opaqueColor = new Color(1, 1, 1, 1);

    public void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        TileMapManager.instance.CellsPos.Add(transform.position);
    }
    public void PointerEnter()
    {
        OnCellMouseEnter?.Invoke(grid_index);
    }
    public void PointerExit()
    {
        OnCellMouseExit?.Invoke(grid_index);
    }
    public void PointerDown()
    {
        OnCellMouseDown?.Invoke(grid_index);
    }

    /*
#if ENABLE_INPUT_SYSTEM
    public void OnPointerEnter(PointerEventData eventData)
    {
        OnCellMouseEnter?.Invoke(grid_index);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        OnCellMouseExit?.Invoke(grid_index);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        OnCellMouseDown?.Invoke(grid_index);
    }
#endif
#if ENABLE_LEGACY_INPUT_MANAGER
    public void OnMouseEnter()
    {
        OnCellMouseEnter?.Invoke(grid_index);
    }
    public void OnMouseExit()
    {
        OnCellMouseExit?.Invoke(grid_index);
    }
    public void OnMouseDown()
    {
        OnCellMouseDown?.Invoke(grid_index);
    }
#endif */

    public void SetHoverColor()
    {
        spriteRenderer.color = new Color(1, 0, 0, 1);
        //spriteRenderer.color = Color.red;
    }
    public void SetEmptyColor()
    {
        //spriteRenderer.color = Color.white;
        spriteRenderer.sprite = null;
        spriteRenderer.color = _s_transprentColor;
        hindSpriteRenderer.color = _s_transprentColor;
        frontSpriteRenderer.color = _s_transprentColor;
    }

    public void SetusedColor()
    {
        spriteRenderer.color = Color.blue;
    }

    public void OpenCellSprite()
    {
        spriteRenderer.color = _s_opaqueColor;
        hindSpriteRenderer.color = _s_opaqueColor;
        frontSpriteRenderer.color = _s_opaqueColor;
    }
    public void SetColor(Color _color)
    {
        //spriteRenderer.color = _color;
        spriteRenderer.color = _s_opaqueColor;
        hindSpriteRenderer.color = _s_opaqueColor;
        frontSpriteRenderer.color = _s_opaqueColor;
    }

}
[System.Serializable]
public enum CellState
{
    NONE = 0,
    CELL = 1,
    SAW = 2,
    EMPTY = 3
}


