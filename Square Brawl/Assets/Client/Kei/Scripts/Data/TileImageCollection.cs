using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TillMap/ImageCollection")]
public class TileImageCollection : ScriptableObject
{
    public Sprite topLeft;
    public Sprite topMiddle;
    public Sprite topRight;

    public Sprite middleLeft;
    public Sprite middleCenter;
    public Sprite middleRight;

    public Sprite bottomLeft;
    public Sprite bottomMiddle;
    public Sprite bottomRight;

    public Sprite single;

    public Sprite singleLeft;
    public Sprite singleMiddle;
    public Sprite singleRight;
    public Sprite singleTop;
    public Sprite singleBottom;

    public Sprite sigleBridge;

    public Sprite GetSprite(int _orientation)
    {
        switch (_orientation)
        {
            case 0:
                return topLeft;
            case 1:
                return topMiddle;
            case 2:
                return topRight;

            case 3:
                return middleLeft;
            case 4:
                return middleCenter;
            case 5:
                return middleRight;

            case 6:
                return bottomLeft;
            case 7:
                return bottomMiddle;
            case 8:
                return bottomRight;

            case 9:
                return single;
            case 10:
                return singleLeft;
            case 11:
                return singleMiddle;
            case 12:
                return singleRight;
            case 13:
                return singleTop;
            case 14:
                return singleBottom;
            case 15:
                return sigleBridge;

        }

        return single;
    }

}
