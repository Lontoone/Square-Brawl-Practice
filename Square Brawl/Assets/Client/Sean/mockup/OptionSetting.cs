using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionSetting
{
    public static int FULLSCREEN = 1;
    public static int RESOLUTION = 5;
    public static float MUSICVOLUME = 0.4f;
    public static float SFXVOLUME = 0.4f;
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

public class SaveAndLoadSetting
{
    public int FULLSCREEN;
    public int RESOLUTION;
    public int MUSICVOLUME;
    public int SFXVOLUME;
    public bool CONTROLLER_RUMBLE;
    public bool TRANSITIONANIMATION;

    public static SaveAndLoadSetting SetData()
    {
        SaveAndLoadSetting saveAndLoadSetting = new SaveAndLoadSetting();

        saveAndLoadSetting.FULLSCREEN = OptionSetting.FULLSCREEN;
        saveAndLoadSetting.RESOLUTION = OptionSetting.RESOLUTION;
        saveAndLoadSetting.MUSICVOLUME = (int)(OptionSetting.MUSICVOLUME * 10);
        saveAndLoadSetting.SFXVOLUME = (int)(OptionSetting.SFXVOLUME * 10);
        saveAndLoadSetting.CONTROLLER_RUMBLE = OptionSetting.CONTROLLER_RUMBLE;
        saveAndLoadSetting.TRANSITIONANIMATION = OptionSetting.TRANSITIONANIMATION;

        return saveAndLoadSetting;
    }

    public static void Save()
    {
        var SettingSavePath = Application.persistentDataPath + "/Setting.txt";
        var data = SetData();

        SaveAndLoad.Save<SaveAndLoadSetting>(data, SettingSavePath);
    }

    public static void Load()
    {
        var SettingSavePath = Application.persistentDataPath + "/Setting.txt";
        SaveAndLoadSetting saveAndLoadSetting = SaveAndLoad.Load<SaveAndLoadSetting>(SettingSavePath);

        OptionSetting.FULLSCREEN = saveAndLoadSetting.FULLSCREEN;
        OptionSetting.RESOLUTION = saveAndLoadSetting.RESOLUTION;
        OptionSetting.MUSICVOLUME = (float)saveAndLoadSetting.MUSICVOLUME / 10;
        OptionSetting.SFXVOLUME = (float)saveAndLoadSetting.SFXVOLUME / 10;
        OptionSetting.CONTROLLER_RUMBLE = saveAndLoadSetting.CONTROLLER_RUMBLE;
        OptionSetting.TRANSITIONANIMATION = saveAndLoadSetting.TRANSITIONANIMATION;
    }
}
