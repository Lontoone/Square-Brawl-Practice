using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
//using AutoMapper;  //Require download from Nuget


public static class MyExtension
{
    public static GameObject GetRayHit2D(Vector2 mouseInput, LayerMask layerMask = default)
    {

        Ray ray = Camera.main.ScreenPointToRay(mouseInput);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 10, layerMask, -1);

        if (hit.collider != null)
        {
            //hit.transform.GetComponent<IClickableGameObject>()?.OnClick();
            return hit.collider.gameObject;
        }
        else
        {
            return null;
        }
    }

    /*
    //Require Nuget AutoMapper
    public static T CloneData<T>(T origin_data)
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<T, T>();
        });
        var mapper = config.CreateMapper();
        return mapper.Map<T>(origin_data);
    }
    public static KeyValuePair<string, CardData> CloneDataWithID(string card_id) 
    {
        CardData data_clone = new CardData();
        //提取副本
        data_clone = MyExtension.CloneData<CardData>(MyExtension.LoadCardData(card_id));

        //給temp id
        string random_id = System.Guid.NewGuid().ToString("N");
        data_clone.id = random_id;

        return new KeyValuePair<string, CardData>(random_id, data_clone);
    }*/


    public static Color RandomColor()
    {
        Color res = new Color();
        res.r = Random.Range(0f, 1f);
        res.g = Random.Range(0f, 1f);
        res.b = Random.Range(0f, 1f);
        return res;
    }

    public static Vector2Int Abs(this Vector2Int v2)
    {
        return new Vector2Int(Mathf.Abs(v2.x), Mathf.Abs(v2.y));
    }

    //檢查該v2是否在grid範圍內
    public static bool IsInsideGridRange(this Vector2Int v2, Vector2Int _mapSize)
    {
        int grid_width = _mapSize.x;
        int grid_height = _mapSize.y;
        if (v2.x >= grid_width || v2.x <= -1 ||
            v2.y >= grid_height || v2.y <= -1
            )
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public static string CombinePersistentPath(this string _path)
    {
        return Path.Combine(Application.persistentDataPath, _path);
    }

    /*
    public static bool IsSameSide(this Player.PlayerSide side, Player.PlayerSide _test_side)
    {
        int res = (int)side * (int)_test_side;
        return !(res == -1 || res == 0);//互為敵對相乘=-1  法術卡不算同一邊 //TODO 在文件資料紀錄卡片是否可共用(none)
    }
    */

    /*
   //取得該卡片的所在cell
   public static Transform GetCellTransform(this TileCell c)
   {
       if (c != null)
           return c.transform.parent;
       else
           return null;
   } */
    /*
    //取得該卡片的所在cell
    public static CellControl GetCell(this Card c)
    {
        return c.transform.parent.GetComponent<CellControl>();
    }*/

}
