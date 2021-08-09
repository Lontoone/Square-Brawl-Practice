using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TileMapManager : MonoBehaviour
{
    public static TileMapManager instance;
    public GameObject cellPrefab;

    public SpriteRenderer firstCell;

    public Vector2Int mapSize = new Vector2Int(10, 5);
    public int cellCount { get { return mapSize.x * mapSize.y; } }

    public List<TileCell> gridCells = new List<TileCell>();
    public Dictionary<int, CellState> cellStateMap = new Dictionary<int, CellState>();

    //Vector2 gridSize = new Vector2(1.05f, 1.05f);

    public SpriteRenderer foreground, midground, background;


    public TileCell center_cell { get { return (gridCells[(gridCells.Count - 1) / 2]); } }

    [SerializeField]
    private bool _useGizemos = true;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }
    }
    public void Start()
    {
        //GenerateGrid(); 
    }

    [ContextMenu("GenerateGrid")]
    public void GenerateGrid()
    {
        StartCoroutine(GenerateGridCoro());
        /*
        Debug.Log("Generate Grid");
        float _sizePandingFactor = 1f;
        float halfWidth = firstCell.bounds.size.x / 2 * _sizePandingFactor;
        float halfHeight = firstCell.bounds.size.y / 2 * _sizePandingFactor;
        //float halfWidth = gridSize.x / 2 * _sizePandingFactor;
        //float halfHeight = gridSize.y / 2 * _sizePandingFactor;

        int _counter = 0;
        for (int i = 0; i < mapSize.x; i++)
        {
            for (int j = 0; j < mapSize.y; j++)
            {
                Vector2 offset = new Vector2(i * 2 * halfWidth, j * 2 * halfHeight) + (Vector2)firstCell.transform.position;
                GameObject cell = Instantiate(cellPrefab, offset, Quaternion.identity, transform);
                cell.name = "( " + i + " , " + j + " ) ";

                TileCell _cell = cell.GetComponent<TileCell>();
                _cell.grid_index = _counter;

                gridCells.Add(_cell);
                _counter++;
            }
        }*/
    }

    private IEnumerator GenerateGridCoro()
    {
        WaitForEndOfFrame _wait = new WaitForEndOfFrame();

        float _sizePandingFactor = 1f;
        float halfWidth = firstCell.bounds.size.x / 2 * _sizePandingFactor;
        float halfHeight = firstCell.bounds.size.y / 2 * _sizePandingFactor;
        //float halfWidth = gridSize.x / 2 * _sizePandingFactor;
        //float halfHeight = gridSize.y / 2 * _sizePandingFactor;

        int _counter = 0;
        for (int i = 0; i < mapSize.x; i++)
        {
            for (int j = 0; j < mapSize.y; j++)
            {
                Vector2 offset = new Vector2(i * 2 * halfWidth, j * 2 * halfHeight) + (Vector2)firstCell.transform.position;
                GameObject cell = Instantiate(cellPrefab, offset, Quaternion.identity, transform);
                cell.name = "( " + i + " , " + j + " ) ";

                TileCell _cell = cell.GetComponent<TileCell>();
                _cell.SetEmptyColor();
                _cell.grid_index = _counter;

                gridCells.Add(_cell);
                _counter++;
            }
            yield return _wait;
        }
    }

    public Vector2Int CellToVector2(Transform cell)
    {
        //int index = cell.GetSiblingIndex();
        if (cell.GetComponent<TileCell>() == null)
        {
            return new Vector2Int(99, 99);
        }
        int index = cell.GetComponent<TileCell>().grid_index;
        int x = index / (mapSize.y);
        int y = index % (mapSize.y);
        return new Vector2Int(x, y);

    }
    public Transform Vector2ToCell(Vector2Int vec)
    {
        //int index = gridWidth * gridCount * vec.y + vec.x;
        int index = mapSize.y * vec.x + vec.y;

        if (vec.IsInsideGridRange(mapSize) &&
            (index < gridCells.Count && index >= 0))
        {
            return gridCells[index].transform;
        }
        else
        {
            return null;
        }
    }

    public TileCell[] GetSelectRangeCells(SelectRangeData _range, int _cellIndex)
    {
        if (_range.range.Count == 0)
        {
            _range.ReadData();
        }
        List<TileCell> res = new List<TileCell>();
        Vector2Int _centerCell = CellToVector2(gridCells[_cellIndex].transform);

        for (int i = 0; i < _range.range.Count; i++)
        {
            Transform _cellTrans = Vector2ToCell(_centerCell + _range.range[i]);
            if (_cellTrans != null)
            {
                TileCell _cell = _cellTrans.GetComponent<TileCell>();
                res.Add(_cell);
            }
            else
            {
                res.Add(null);
            }
        }
        return res.ToArray();
    }

    public TileCell[] GetRowCell(int x_from, int x_to, int y)
    {
        List<TileCell> res = new List<TileCell>();

        bool virse = false;

        if (x_from > x_to)
        {
            //交換頭尾
            int temp = x_from;
            x_from = Mathf.Clamp(x_to, 0, 10);
            x_to = Mathf.Clamp(temp, 0, 10);

            virse = true;
        }

        for (int i = x_from; i <= x_to; i++)
        {
            Vector2Int v2 = new Vector2Int(i, y);
            Transform cell = Vector2ToCell(v2);

            if (i >= 10 || i < 0 || cell == null)
                continue;
            res.Add(cell.GetComponent<TileCell>());

        }

        if (virse)
        {
            res.Reverse();
        }

        return res.ToArray();
    }
    public TileCell[] GetColumnCell(int y_from, int y_to, int x)
    {
        List<TileCell> res = new List<TileCell>();

        bool virse = false;

        if (y_from > y_to)
        {
            //交換頭尾
            int temp = y_from;
            y_from = Mathf.Clamp(y_to, 0, 5);
            y_to = Mathf.Clamp(temp, 0, 5);

            virse = true;
        }

        for (int i = y_from; i <= y_to; i++)
        {
            Vector2Int v2 = new Vector2Int(x, i);
            Transform cell = Vector2ToCell(v2);

            if (i >= 5 || i < 0 || cell == null)
                continue;
            res.Add(cell.GetComponent<TileCell>());


        }

        if (virse)
        {
            res.Reverse();
        }

        return res.ToArray();
    }

    public void OnDrawGizmos()
    {
        if (!_useGizemos)
            return;
        //temp 
        Gizmos.color = Color.yellow;
        float _sizePandingFactor = 0.95f;
        float _width = firstCell.bounds.size.x * _sizePandingFactor;
        float _height = firstCell.bounds.size.y * _sizePandingFactor;

        Vector3 _coner1 = firstCell.transform.position + new Vector3(0, _height * mapSize.y);
        Vector3 _coner2 = firstCell.transform.position + new Vector3(_width * mapSize.x, _height * mapSize.y);
        Vector3 _coner3 = firstCell.transform.position + new Vector3(_width * mapSize.x, 0);

        Gizmos.DrawLine(firstCell.transform.position, _coner1);
        Gizmos.DrawLine(_coner1, _coner2);
        Gizmos.DrawLine(_coner2, _coner3);
        Gizmos.DrawLine(_coner3, firstCell.transform.position);
    }
}

