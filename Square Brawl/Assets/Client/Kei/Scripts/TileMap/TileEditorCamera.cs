using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TileEditorCamera : MonoBehaviour
{
    public Camera camera;
    TileCell _currentCell;
    //int _currentCellIndex = 0;
    public void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(camera.ScreenToWorldPoint(Mouse.current.position.ReadValue()), Vector2.one);
        //Vector3 mousePosWorld = camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        if (hit.collider != null)
        {
            TileCell _cell = hit.collider.GetComponent<TileCell>();
            if (_cell != null)
            {
                if (_currentCell == null || _currentCell.grid_index != _cell.grid_index)
                {
                    _currentCell?.PointerExit();
                    _cell.PointerEnter();
                    _currentCell = _cell;
                }
                else
                {
                    //Stay....
                    //if (Mouse.) { }
                }
            }
        }
    }
}
