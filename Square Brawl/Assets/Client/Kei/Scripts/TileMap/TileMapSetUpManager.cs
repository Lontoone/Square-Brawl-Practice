using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMapSetUpManager : MonoBehaviour
{
    public SelectRangeData detecateRange;

    public string levelFileName; //temp

    private List<TileCell> activeTileCells = new List<TileCell>();

    private void Start()
    {
        TileMapManager.instance.GenerateGrid();

        //temp
        MapData _mapData = SaveAndLoad.Load<MapData>(levelFileName.CombinePersistentPath());
        SetUpLevelTiles(_mapData);
        SetUpCellOrientation();
    }

    private void SetUpLevelTiles(MapData _mapData)
    {
        for (int i = 0; i < TileMapManager.instance.cellCount; i++)
        {
            //CellData _cell =_mapData.cellDatas.
        }
    }
    private void SetUpCellOrientation() { }

}
