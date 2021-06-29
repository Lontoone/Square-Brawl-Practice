using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//攻擊範圍資料
[System.Serializable]
public class SelectRangeData
{
    public TextAsset data;
    [HideInInspector]
    //public Vector2Int[] range;//TODO:用edior顯示

    public List<Vector2Int> range = new List<Vector2Int>();//TODO:用edior顯示

    public void ReadData()
    {
        //TODO:....讀取data
        //SaveAndLoad.Load<mVec2Int>();
        if (data == null)
        {
            return;
        }
        mVec2Int v2Data = JsonUtility.FromJson<mVec2Int>(data.text);
        range = v2Data.range;

    }


    /*
    //從from套用該range資料
    public Vector2Int[] ApplyAttackRange(Vector2Int from)
    {
        Vector2Int[] _result = new Vector2Int[range.Length];
        for (int i = 0; i < range.Length; i++)
        {
            _result[i] = range[i] + from;
        }

        return _result;
    }
    */

}
public class mVec2Int
{
    public mVec2Int() { }
    public mVec2Int(Vector2Int v)
    {
        range.Add(v);
    }
    public List<Vector2Int> range = new List<Vector2Int>();
}

