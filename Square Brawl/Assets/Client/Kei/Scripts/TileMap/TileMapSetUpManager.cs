using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMapSetUpManager : MonoBehaviour
{
    public SelectRangeData detecateRange;

    public static string levelFileName; //temp

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
        int _dataCount = 0;
        for (int i = 0; i < TileMapManager.instance.cellCount; i++)
        {
            //CellData _cellData = _mapData.cellDatas[_dataCount];
            TileCell _cell = TileMapManager.instance.gridCells[i];
            if (i == _mapData.cellDatas[_dataCount].index)
            {
                activeTileCells.Add(_cell);
                _dataCount = Mathf.Clamp(_dataCount + 1, 0, _mapData.cellDatas.Count - 1); ;
            }
            else
            {
                //empty cell
                _cell.gameObject.SetActive(false);
            }
        }

        //Set up image
        for (int i=0; i <activeTileCells.Count;i++) {
            TileStyleManager.instance.SetCell(activeTileCells[i].grid_index);
            TileStyleManager.instance.SetNearbyCell(activeTileCells[i].grid_index);
        }

    }
    private void SetUpCellOrientation() { }

}
