﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileCell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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


