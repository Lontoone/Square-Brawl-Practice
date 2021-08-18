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
    public static readonly Color[] COLORS = new Color[] { new Color(0.827451f, 0.3372549f, 0.3294118f), 
        new Color(0.3529412f, 0.6627451f, 0.9647059f), 
        new Color(0.9803922f, 0.6666667f, 0.3019608f),
        new Color(0.3411765f, 0.8313726f, 0.4666667f) };

    /* WEAPON */
    public const string WEAPON1CODE = "WEAPON1";
    public const string WEAPON2CODE = "WEAPON2";

    public const string LIFT_COUNT = "LIFT_COUNT";

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
    Revolver,
    BigBoom,
    Sniper,
    CubeShoot,
    ShotGun,
    Grenade,
    Pillar,
    Katana,
    Charge,
    Shield,
    Freeze,
    Bounce
}