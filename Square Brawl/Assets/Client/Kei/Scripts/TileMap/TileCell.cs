using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileCell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,IPointerDownHandler
{
    public int grid_index;
    [HideInInspector]
    public SpriteRenderer spriteRenderer;
    public Color onHoverColor, onSelectColor, rangeHintColor;

    //public static event Action<int> OnCellHover;
    public static event Action<int> OnCellMouseEnter;
    public static event Action<int> OnCellMouseExit;
    public static event Action<int> OnCellMouseDown;


    public void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }
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
#endif    

    public void SetHoverColor()
    {

        spriteRenderer.color = onHoverColor;
    }
    public void SetWhiteColor()
    {
        spriteRenderer.color = Color.white;
    }

    public void SetusedColor()
    {
        spriteRenderer.color = Color.green;
    }

    public void SetColor(Color _color)
    {
        spriteRenderer.color = _color;
    }

}

public enum CellState
{
    NONE = 0,
    CELL = 1,
    SAW = 2,
    EMPTY = 3
}
public enum CellOrientation
{
    TOP_LEFT = 0,
    TOP_MIDDLE = 1,
    TOP_RIGHT = 2,

    MIDDLE_LEFT = 3,
    MIDDLE_FILL = 4,
    MIDDLE_RIGHT = 5,

    BOTTOM_LEFT = 6,
    BOTTOM_MIDDLE = 7,
    BOTTOM_RIGHT = 8,

    SINGLE = 9,

    SINGLE_LEFT = 10,
    SINGLE_MIDDLE = 11,
    SINGLE_RIGHT = 12,
    SINGLE_TOP = 13,
    SINGLE_BOTTOM = 14,
    SINGLE_BRIDGE = 15,
}


