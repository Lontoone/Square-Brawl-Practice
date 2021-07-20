using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "TillMap/ImageCollection")]
[System.Serializable]
public class TileImageCollection : ScriptableObject
{
    //public string setName;
    private Dictionary<int, Sprite> dataMap = new Dictionary<int, Sprite>();
    private Dictionary<int, Sprite> dataMapHind = new Dictionary<int, Sprite>();
    public void LoadData()
    {
        //back ground sprite
        for (int i = 0; i < tileImages.Count; i++)
        {
            Sprite _sp = Resources.Load<Sprite>("Tile/" + name + "/" + tileImages[i].name);
            tileImages[i].sprite = _sp;

            //log to dictionary
            for (int j = 0; j < tileImages[i].indexs.Count; j++)
            {            
                dataMap.Add(tileImages[i].indexs[j], _sp);
            }
        }

        //load front
        for (int i = 0; i < tileImagesHind.Count; i++)
        {
            Sprite _sp = Resources.Load<Sprite>("Tile/" + name + "/" + tileImagesHind[i].name);
            tileImagesHind[i].sprite = _sp;

            //log to dictionary
            for (int j = 0; j < tileImagesHind[i].indexs.Count; j++)
            {
                dataMapHind.Add(tileImagesHind[i].indexs[j], _sp);
            }
        }
    }
    private static List<TileImage> tileImages = new List<TileImage>()
    {
        { new TileImage("Fill",0,5,160,32,4,1,36,129,161,37,128,33,164,132,133,165) },

        { new TileImage("Three Sides -0",64,224,192,68,96,100,229,65,69,169,97,196,193,225,228,197,101) },
        { new TileImage("Three Sides -90",8,41,12,44,137,9,168,136,172,13,40,141,45,140,173) },
        { new TileImage("Three Sides -180",2,7,34,3,38,6,131,134,35,39,130,135,163,162,166 , 167) },
        { new TileImage("Three Sides -270",16,148,48,17,145,176,144,52,20,177,49,149,53,21,181,180) },

        { new TileImage("Two Sides and One Corner -0",72,73,201,200,77,205,204,76) },
        { new TileImage("Two Sides and One Corner -90",10,42,14,142,46,138,170,174) },
        { new TileImage("Two Sides and One Corner -180",18,146,147,19,51,50,179,178) },
        { new TileImage("Two Sides and One Corner -270",80,84,112,117,113,85,116,81) },

        { new TileImage("Two Adjacent Sides -0",104,232,233,105,236,109,108,237) },
        { new TileImage("Two Adjacent Sides -90",11,43,47,15,143,175,171,139) },
        { new TileImage("Two Adjacent Sides -180",22,150,151,23,182,55,54,183) },
        { new TileImage("Two Adjacent Sides -270",208,212,244,240,241,213,245,209) },

        { new TileImage("Two Opposite Sides -0",66,67,198,98,194,195,70,99,103,231,230,102,199,71,226,227) },
        { new TileImage("Two Opposite Sides -90",24,25,56,28,156,60,152,188,57,184,29,189,185,153,61,157) },

        { new TileImage("One Side and Two Corners -0",74,78,206,202) },
        { new TileImage("One Side and Two Corners -90",26,186,58,154) },
        { new TileImage("One Side and Two Corners -180",82,83,114,115) },
        { new TileImage("One Side and Two Corners -270",88,89,93) },

        { new TileImage("One Side and One Lower Corner -0",106,110,238,234) },
        { new TileImage("One Side and One Lower Corner -90",27,155,59,187) },
        { new TileImage("One Side and One Lower Corner -180",86,119,118,87) },
        { new TileImage("One Side and One Lower Corner -270",216,217,221,220) },

        { new TileImage("One Side and One Upper Corner -0",75,79,203,207) },
        { new TileImage("One Side and One Upper Corner -90",30,158,62,190) },
        { new TileImage("One Side and One Upper Corner -180",210,242,211,243) },
        { new TileImage("One Side and One Upper Corner -270",120,121,125,124,92) },

        { new TileImage("One Side -0",107,235,111,239) },
        { new TileImage("One Side -90",31,63,191,159) },
        { new TileImage("One Side -180",214,247,246,215) },
        { new TileImage("One Side -270",248,249,253,252) },

        { new TileImage("Four Corners -0",90) },

        { new TileImage("Three Corners -0",122) },
        { new TileImage("Three Corners -90",91) },
        { new TileImage("Three Corners -180",94) },
        { new TileImage("Three Corners -270",218) },

        { new TileImage("Two Adjacent Corners -0",123) },
        { new TileImage("Two Adjacent Corners -90",95) },
        { new TileImage("Two Adjacent Corners -180",222) },
        { new TileImage("Two Adjacent Corners -270",250) },

        { new TileImage("Two Opposite Corners -0",126) },
        { new TileImage("Two Opposite Corners -90",219) },

        { new TileImage("One Corner -0",127) },
        { new TileImage("One Corner -90",223) },
        { new TileImage("One Corner -180",254) },
        { new TileImage("One Corner -270",251) },

        { new TileImage("Empty",255) },

        { new TileImage("Saw",300) }

    };
    private static List<TileImage> tileImagesHind = new List<TileImage>()
    {
        { new TileImage("Hind Fill",0,5,160,32,4,1,36,129,161,37,128,33,164,132,133,165) },

        { new TileImage("Hind Three Sides -0",64,224,192,68,96,100,229,65,69,169,97,196,193,225,228,197,101) },
        { new TileImage("Hind Three Sides -90",8,41,12,44,137,9,168,136,172,13,40,141,45,140,173) },
        { new TileImage("Hind Three Sides -180",2,7,34,3,38,6,131,134,35,39,130,135,163,162,166 , 167) },
        { new TileImage("Hind Three Sides -270",16,148,48,17,145,176,144,52,20,177,49,149,53,21,181,180) },

        { new TileImage("Hind Two Sides and One Corner -0",72,73,201,200,77,205,204,76) },
        { new TileImage("Hind Two Sides and One Corner -90",10,42,14,142,46,138,170,174) },
        { new TileImage("Hind Two Sides and One Corner -180",18,146,147,19,51,50,179,178) },
        { new TileImage("Hind Two Sides and One Corner -270",80,84,112,117,113,85,116,81) },

        { new TileImage("Hind Two Adjacent Sides -0",104,232,233,105,236,109,108,237) },
        { new TileImage("Hind Two Adjacent Sides -90",11,43,47,15,143,175,171,139) },
        { new TileImage("Hind Two Adjacent Sides -180",22,150,151,23,182,55,54,183) },
        { new TileImage("Hind Two Adjacent Sides -270",208,212,244,240,241,213,245,209) },

        { new TileImage("Hind Two Opposite Sides -0",66,67,198,98,194,195,70,99,103,231,230,102,199,71,226,227) },
        { new TileImage("Hind Two Opposite Sides -90",24,25,56,28,156,60,152,188,57,184,29,189,185,153,61,157) },

        { new TileImage("Hind One Side and Two Corners -0",74,78,206,202) },
        { new TileImage("Hind One Side and Two Corners -90",26,186,58,154) },
        { new TileImage("Hind One Side and Two Corners -180",82,83,114,115) },
        { new TileImage("Hind One Side and Two Corners -270",88,89,93) },

        { new TileImage("Hind One Side and One Lower Corner -0",106,110,238,234) },
        { new TileImage("Hind One Side and One Lower Corner -90",27,155,59,187) },
        { new TileImage("Hind One Side and One Lower Corner -180",86,119,118,87) },
        { new TileImage("Hind One Side and One Lower Corner -270",216,217,221,220) },

        { new TileImage("Hind One Side and One Upper Corner -0",75,79,203,207) },
        { new TileImage("Hind One Side and One Upper Corner -90",30,158,62,190) },
        { new TileImage("Hind One Side and One Upper Corner -180",210,242,211,243) },
        { new TileImage("Hind One Side and One Upper Corner -270",120,121,125,124,92) },

        { new TileImage("Hind One Side -0",107,235,111,239) },
        { new TileImage("Hind One Side -90",31,63,191,159) },
        { new TileImage("Hind One Side -180",214,247,246,215) },
        { new TileImage("Hind One Side -270",248,249,253,252) },

        { new TileImage("Hind Four Corners -0",90) },

        { new TileImage("Hind Three Corners -0",122) },
        { new TileImage("Hind Three Corners -90",91) },
        { new TileImage("Hind Three Corners -180",94) },
        { new TileImage("Hind Three Corners -270",218) },

        { new TileImage("Hind Two Adjacent Corners -0",123) },
        { new TileImage("Hind Two Adjacent Corners -90",95) },
        { new TileImage("Hind Two Adjacent Corners -180",222) },
        { new TileImage("Hind Two Adjacent Corners -270",250) },

        { new TileImage("Hind Two Opposite Corners -0",126) },
        { new TileImage("Hind Two Opposite Corners -90",219) },

        { new TileImage("Hind One Corner -0",127) },
        { new TileImage("Hind One Corner -90",223) },
        { new TileImage("Hind One Corner -180",254) },
        { new TileImage("Hind One Corner -270",251) },

        { new TileImage("Hind Empty",255) },

        { new TileImage("Hind Saw",300) },


         { new TileImage("Foreground",301) },
         { new TileImage("Midground",302) },
         { new TileImage("Background",303) }
    };


    public Sprite GetSprite(int _index)
    {
        if (dataMap.Count < 1)
        {
            LoadData();
        }

        Sprite _res;
        if (dataMap.TryGetValue(_index, out _res))
        {
            return (_res);
        }
        else
        {
            //Debug.Log("<color=red>tile image not found " + _index + "</color>");
            return (dataMap[255]);
        }
    }

    public Sprite GetHindSprite(int _index)
    {
        if (dataMapHind.Count < 1)
        {
            LoadData();
        }

        Sprite _res;
        if (dataMapHind.TryGetValue(_index, out _res))
        {
            return (_res);
        }
        else
        {
            //Debug.Log("<color=red>tile image not found " + _index + "</color>");
            return (dataMapHind[255]);
        }
    }

}

[System.Serializable]
public class TileImage
{
    public string name;
    public List<int> indexs;
    public Sprite sprite;

    public TileImage(string _name, params int[] _indexs)
    {
        name = _name;
        indexs = _indexs.ToList();
    }
}


