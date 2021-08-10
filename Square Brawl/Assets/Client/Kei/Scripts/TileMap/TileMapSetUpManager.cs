using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMapSetUpManager : MonoBehaviour
{
    public SelectRangeData detecateRange;

    private string levelFileName; //temp

    [SerializeField]
    private List<TileCell> activeTileCells = new List<TileCell>();

    //public SpriteRenderer foreground, midground, background;
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
    public void SetUpLevel(MapData _mapData)
    {
        StartCoroutine(SetUpLevelCoro(_mapData));
    }
    public IEnumerator SetUpLevelCoro(MapData _mapData)
    {
        if (TileMapManager.instance.gridCells.Count < TileMapManager.instance.cellCount)
        {
            Debug.Log("Gernerate Grid");
            TileMapManager.instance.GenerateGrid(false);
            yield return new WaitForSeconds(1.5f);
            Debug.Log("Gernerate Grid - finished");
        }
        SetUpLevelTiles(_mapData);
        SetUpCellOrientation();
    }

    private void SetUpLevelTiles(MapData _mapData)
    {
        activeTileCells.Clear();
        int _dataCount = 0;
        for (int i = 0; i < TileMapManager.instance.cellCount; i++)
        {
            TileCell _cell = TileMapManager.instance.gridCells[i];

            if (i == _mapData.cellDatas[_dataCount].index)
            {
                Debug.Log("i is active " + i);
                TileMapManager.instance.cellStateMap[i] = _mapData.cellDatas[_dataCount].state;
                activeTileCells.Add(_cell);
                //_dataCount++;
                _dataCount = Mathf.Clamp(_dataCount + 1, 0, _mapData.cellDatas.Count - 1);
                _cell.gameObject.SetActive(true);
            }
            else
            {
                TileMapManager.instance.cellStateMap[i] = CellState.NONE;
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
    /*
    private void SetupBackground()
    {
        if (foreground != null)
            foreground.sprite = TileStyleManager.selectedCollection.GetSprite(301);

        if (midground != null)
            midground.sprite = TileStyleManager.selectedCollection.GetSprite(302);

        if (background != null)
            background.sprite = TileStyleManager.selectedCollection.GetSprite(303);
    }*/
}
