using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCell : MonoBehaviour
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

    public void SetHoverColor()
    {

        spriteRenderer.color = onHoverColor;
    }
    public void SetWhiteColor()
    {
        spriteRenderer.color = Color.white;
    }

    public void SetusedColor() {
        spriteRenderer.color = Color.green;
    }
}


