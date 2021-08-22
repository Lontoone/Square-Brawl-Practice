using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionSetting : MonoBehaviour
{
    public static FullScreenMode FULLSCREEN;
    public static Resolution RESOLUTION;
    public static float MUSICVOLUME = 0.5f;
    public static float SFXVOLUME = 0.5f;
    public static bool CONTROLLER_RUMBLE = true;
    public static bool TRANSITIONANIMATION = true;

    public static Vector2[] resolution = new Vector2[] { new Vector2(640, 360),
                                                         new Vector2(854, 480),
                                                         new Vector2(1280, 720),
                                                         new Vector2(1366, 768),
                                                         new Vector2(1600, 900),
                                                         new Vector2(1920, 1080),
                                                         new Vector2(2560, 1440)};
    //using in Setting Prefab
    public enum ChangeType 
    {
        FullScreen,
        Resolution,
        MusicVolume,
        SFXVolume,
        ControllerRumble,
        TransitionAnimation
    }

}
