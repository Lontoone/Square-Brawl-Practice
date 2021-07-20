using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadinCustomeLevel : MonoBehaviour
{
    public TileMapSetUpManager editorControl;
    public void Start()
    {
        if (MapSelectManager.currentSelectedData != null)
        {
            editorControl.SetUpLevel(MapSelectManager.currentSelectedData);
            TileStyleManager.instance.ApplyNewStyle(TileStyleManager.selectedCollection);
        }
    }
}
