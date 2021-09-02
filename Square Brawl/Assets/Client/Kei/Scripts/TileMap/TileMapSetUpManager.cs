using System.Linq;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TileMapSetUpManager : MonoBehaviour
{
    public SelectRangeData detecateRange;

    private string levelFileName; //temp

    [SerializeField]
    public List<TileCell> activeTileCells = new List<TileCell>();

    public static TileMapSetUpManager instance;
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
        instance = this;
    }

    public void SetUpLevel(MapData _mapData)
    {
        StartCoroutine(SetUpLevelCoro(_mapData));
    }
    public IEnumerator SetUpLevelCoro(MapData _mapData)
    {
        var ifNeedGenerated = false;

        if (TileMapManager.instance==null) {
            yield break;
        }
        if (TileMapManager.instance.gridCells.Count < TileMapManager.instance.cellCount)
        {
            Debug.Log("Gernerate Grid");
            if (!TileMapManager.instance.generated)
                TileMapManager.instance.GenerateGrid(false);

            yield return new WaitUntil(() => MapSelectionTrigger.GridFinish);
            Debug.Log("Gernerate Grid - finished");
            SetUpLevelTiles(_mapData);
            SetUpCellOrientation();
        }
        else 
        {
            SetUpLevelTiles(_mapData);
            SetUpCellOrientation();
        }
    }

    private void SetUpLevelTiles(MapData _mapData)
    {
        if (_mapData == null)
        {
            return;
        }
        activeTileCells.Clear();
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
                _cell.gameObject.SetActive(true);
                //Debug.Log(_cell.transform.position);
            }
            else
            {
                TileMapManager.instance.cellStateMap[i] = CellState.NONE;
                //empty cell
                _cell.gameObject.SetActive(false);
            }
        }

        if(SceneManager.GetActiveScene().buildIndex == 1)
        {         
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager"), Vector3.zero, Quaternion.identity);
            /*if (PlayerController.instance != null)
            {
                PlayerController.instance.StopBeFreeze();
            }*/
        }
    }
    private void SetUpCellOrientation()
    {
        //Set up image
        for (int i = 0; i < activeTileCells.Count; i++)
        {
            TileCell _cell = activeTileCells[i];
            TileStyleManager.instance.SetCell(_cell.grid_index);
            TileStyleManager.instance.SetNearbyCell(_cell.grid_index);

            //saw:
            if (TileMapManager.instance.cellStateMap[_cell.grid_index] == CellState.SAW &&
                 TileMapManager.instance.gridCells[_cell.grid_index].gameObject.GetComponent<Saw>() == null)
            {
                Debug.Log("add saw");
                TileMapManager.instance.gridCells[_cell.grid_index].gameObject.AddComponent<Saw>();
                BoxCollider2D boxCollider2D = TileMapManager.instance.gridCells[_cell.grid_index].gameObject.GetComponent<BoxCollider2D>();
                boxCollider2D.size = new Vector2(7.5f, 7.5f);
                boxCollider2D.gameObject.tag = "Saw";
            }
            else
            {
                Destroy(TileMapManager.instance.gridCells[_cell.grid_index].gameObject.GetComponent<Saw>());
            }
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
