using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomPropertyCode
{
    public const string PLAYER = "PLAYER";
    public const string PLAYERINDEX = "PLAYERINDEX";
    public const string READY = "READY";

    /* TEAM */
    public const string TEAM_CODE = "TEAMCODE";
    public static readonly Color[] COLORS = new Color[] { Color.red, Color.blue, Color.yellow, Color.green };

    /* WEAPON */
    public const string WEAPON1CODE = "WEAPON1";
    public const string WEAPON2CODE = "WEAPON2";

    /* ROOM PROPERTY */
    public const string ROOM_MENU = "ROOMMENU";
    public const byte UPDATE_MAP_EVENTCODE = 1;
    public const byte UPDATE_STYLE_EVENTCODE = 2;
    /* MODE */
    public const string ROOM_MODE_CODE = "MODECODE";
    //public static readonly ModeType MODES = new string[] { };
}

public enum ModeType
{
    FFA = 0, HH = 1
}

public enum WeaponType {
    None,
    Aevolver,
    BigBoom,
    Charge,
    CubeShoot,
    Katada,
    Sniper
}