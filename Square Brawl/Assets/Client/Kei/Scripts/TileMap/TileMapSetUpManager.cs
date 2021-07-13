using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMapSetUpManager : MonoBehaviour
{
    public SelectRangeData detecateRange;

    private string levelFileName; //temp

    private List<TileCell> activeTileCells = new List<TileCell>();

    private void Start()
    {
        /*
        TileMapManager.instance.GenerateGrid();

        //TEMP
        levelFileName = LoadMapUIControl.currentSelectedFile;

        //temp
        MapData _mapData = SaveAndLoad.Load<MapData>(levelFileName.CombinePersistentPath());
        SetUpLevelTiles(_mapData);
        SetUpCellOrientation();*/
    }
    public void SetUpLevel(MapData _mapData) {
        TileMapManager.instance.GenerateGrid();

        //TEMP
        levelFileName = LoadMapUIControl.currentSelectedFile;

        //temp
        //MapData _mapData = SaveAndLoad.Load<MapData>(levelFileName.CombinePersistentPath());
        SetUpLevelTiles(_mapData);
        SetUpCellOrientation();
    }

    private void SetUpLevelTiles(MapData _mapData)
    {
        int _dataCount = 0;
        for (int i = 0; i < TileMapManager.instance.cellCount; i++)
        {
            TileCell _cell = TileMapManager.instance.gridCells[i];

            if (i == _mapData.cellDatas[_dataCount].index)
            {
                TileMapManager.instance.cellStateMap[i] = _mapData.cellDatas[_dataCount].state;
                activeTileCells.Add(_cell);
                //_dataCount++;
                _dataCount = Mathf.Clamp(_dataCount + 1, 0, _mapData.cellDatas.Count - 1);
            }
            else
            {
                TileMapManager.instance.cellStateMap[i] = CellState.NONE ;
                //empty cell
                _cell.gameObject.SetActive(false);
            }
        }

    }
    private void SetUpCellOrientation()
    {
        //Set up image
        for (int i = 0; i < activeTileCells.Count; i++)
        {
            TileStyleManager.instance.SetCell(activeTileCells[i].grid_index);
            TileStyleManager.instance.SetNearbyCell(activeTileCells[i].grid_index);
        }

    }

}
