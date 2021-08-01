﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;
using BasicTools.ButtonInspector;
using DG.Tweening;
using TMPro;
namespace Easetype { }

public class SceneHandler : MonoBehaviour//, ISelectHandler, IDeselectHandler
{
    private Gamepad gamepad = Gamepad.current;
    private Keyboard keyboard = Keyboard.current;
    [SerializeField] private GameObject m_Menu;
    [SerializeField] private GameObject m_Option;
    [SerializeField] private GameObject m_OptionTest;
    [SerializeField] private GameObject m_MapEditor;
    [SerializeField] private GameObject m_Control;
    [SerializeField] private GameObject m_OnlineMenu;
    /*
    [SerializeField] private GameObject m_Lobby;
    [SerializeField] private GameObject m_CreateRoom;
    [SerializeField] private GameObject m_RoomList;
    [SerializeField] private GameObject m_Room;
    [SerializeField] private GameObject m_Characterselection;
    [SerializeField] private GameObject m_Gamemode;
    [SerializeField] private GameObject m_MapSelection;
    [SerializeField] private GameObject m_WeaponSelection;
    */
    [SerializeField] private GameObject m_ScoreInfo;
    private Easetype.Current_easetype scene_current_easetype;
    [SerializeField] Easetype.Current_easetype.Easetype easetype;
    [SerializeField] public static float duration = 1f;
    [SerializeField] private float to_x;
    [SerializeField] private float to_y;
    private Vector3 pos;

    [SerializeField] private Color32 m_Green;
    [SerializeField] private Color32 m_Orange;
    [SerializeField] private Color32 m_Red;
    [SerializeField] private Color32 m_Blue;

    public static Color32 green;
    public static Color32 orange;
    public static Color32 red;
    public static Color32 blue;


    public enum axis {x = 0, y = 1, cons = 2}

    private void Awake()
    {
        scene_current_easetype = new Easetype.Current_easetype();
        green = m_Green;
        orange = m_Orange;
        red = m_Red;
        blue = m_Blue;
    }

    void Start()
    {
        m_Option.SetActive(false);
        //m_OptionTest.SetActive(false);
        SetUpMapEditor();
        //StartCoroutine(LoadMapEditor());

        //pos = m_Menu.transform.localPosition;
    }


    public void EnterPage(GameObject gameObject)
    {
        switch (gameObject.name)
        {
            case "option":
                EnterOption();
                break;

            case "map editor":
                EnterMapEditor();
                break;

            case "control":
                EnterController();
                break;

            case "option test":
                StartCoroutine(EnterOptionTest());
                break;

            default:
                Debug.LogWarning(gameObject.name + " :switch page error");
                break;
        }
    }
    public void ExitPage(GameObject gameObject)
    {
        switch (gameObject.name)
        {
            case "option":
                StartCoroutine(ExitOption());
                break;

            case "map editor":
                StartCoroutine(ExitMapEditor());
                break;

            case "control":
                StartCoroutine(ExitController());
                break;

            case "option test":
                StartCoroutine(ExitOptionTest());
                break;

            default:
                Debug.LogWarning(gameObject.name + " :switch page error");
                break;
        }
    }
    private void EnterOption()
    {
        m_Menu.GetComponentInChildren<MenuButtonHandler>().DisableButton();
        m_Option.SetActive(true);
        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(null);
        m_Menu.transform.DOLocalMove(new Vector3(m_Menu.transform.localPosition.x + to_x, m_Menu.transform.localPosition.y + to_y,0),duration).SetEase(scene_current_easetype.GetEasetype(easetype));
        m_Option.transform.DOLocalMove(new Vector3(0, m_Option.transform.localPosition.y + to_y, 0), duration).SetEase(scene_current_easetype.GetEasetype(easetype));
    }

    private IEnumerator ExitOption()
    {
        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(m_Menu.GetComponentInChildren<MenuButtonHandler>().m_FirstSelectedButton);
        m_Menu.transform.DOLocalMove(new Vector3(m_Menu.transform.localPosition.x + -to_x, m_Menu.transform.localPosition.y + -to_y, 0), duration).SetEase(scene_current_easetype.GetEasetype(easetype));
        m_OptionTest.transform.DOLocalMove(new Vector3(Screen.width, m_Option.transform.localPosition.y + -to_y, 0), duration).SetEase(scene_current_easetype.GetEasetype(easetype));
        StartCoroutine(m_Menu.GetComponentInChildren<MenuButtonHandler>().EnableButton(0.5f));
        yield return new WaitForSeconds(duration / 3);
        m_Option.SetActive(false);
    }

    private IEnumerator EnterOptionTest()
    {
        m_Menu.GetComponentInChildren<MenuButtonHandler>().DisableButton();
        m_OptionTest.SetActive(true);
        yield return null;
        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(null);
        m_Menu.transform.DOLocalMove(new Vector3(m_Menu.transform.localPosition.x -1920, m_Menu.transform.localPosition.y + to_y, 0), duration).SetEase(scene_current_easetype.GetEasetype(easetype));
        m_OptionTest.transform.DOLocalMove(new Vector3(0, m_OptionTest.transform.localPosition.y + to_y, 0), duration).SetEase(scene_current_easetype.GetEasetype(easetype));
        StartCoroutine(m_OptionTest.GetComponentInChildren<OptionManager>().EnterAnimation());
    }

    public IEnumerator ExitOptionTest()
    {
        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(m_Menu.GetComponentInChildren<MenuButtonHandler>().m_FirstSelectedButton);
        m_Menu.transform.DOLocalMove(new Vector3(m_Menu.transform.localPosition.x + -to_x, m_Menu.transform.localPosition.y + -to_y, 0), duration).SetEase(scene_current_easetype.GetEasetype(easetype));
        m_OptionTest.transform.DOLocalMove(new Vector3(1920, m_OptionTest.transform.localPosition.y + -to_y, 0), duration).SetEase(scene_current_easetype.GetEasetype(easetype));
        StartCoroutine(m_Menu.GetComponentInChildren<MenuButtonHandler>().EnableButton(0.5f));
        m_OptionTest.GetComponentInChildren<OptionManager>().ExitAnimation();
        yield return new WaitForSeconds(duration/3);
        m_OptionTest.SetActive(false);
    }

    public void EnterLobby()
    {
        m_OnlineMenu.SetActive(true);
        m_Menu.SetActive(false);
        m_Menu.transform.DOScale(new Vector3(10, 10, 0), duration).SetEase(scene_current_easetype.GetEasetype(easetype));
    }

    public void ExitLobby()
    {
        m_OnlineMenu.SetActive(false);
        m_Menu.SetActive(true);
        m_Menu.transform.DOScale(new Vector3(1, 1, 0), duration).SetEase(scene_current_easetype.GetEasetype(easetype));
    }

    private void SetUpMapEditor()
    {
        m_MapEditor.transform.localPosition = new Vector3(0, WorldToCamera(1080,1)*2);
    }

    private void EnterMapEditor()
    {
        Debug.Log(Screen.width +"\t"+Screen.height);
        m_MapEditor.SetActive(true);
        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(null);
        m_Menu.transform.DOLocalMove(new Vector3(m_Menu.transform.localPosition.x, -1080, 0), duration).SetEase(scene_current_easetype.GetEasetype(easetype));
        m_MapEditor.transform.DOLocalMove(new Vector3(m_Menu.transform.localPosition.x, 0, 0), duration).SetEase(scene_current_easetype.GetEasetype(easetype));
    }

    private IEnumerator ExitMapEditor()
    {   
        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(m_Menu.GetComponentInChildren<MenuButtonHandler>().m_FirstSelectedButton);
        m_Menu.transform.DOLocalMove(new Vector3(m_Menu.transform.localPosition.x, 0, 0), duration).SetEase(scene_current_easetype.GetEasetype(easetype));
        m_MapEditor.transform.DOLocalMove(new Vector3(m_Menu.transform.localPosition.x, 1080, 0), duration).SetEase(scene_current_easetype.GetEasetype(easetype));
        yield return new WaitForSeconds(duration / 3);
        m_MapEditor.SetActive(false);
    }

    private IEnumerator LoadMapEditor()
    {
        m_MapEditor.SetActive(true);
        m_MapEditor.GetComponentInChildren<LoadSceneAsyncUI>().LoadScene();
        yield return null;
        m_MapEditor.SetActive(false);
    }

    private void EnterController()
    {
        m_Menu.GetComponentInChildren<MenuButtonHandler>().DisableButton();
        m_Control.SetActive(true);
        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(null);
        m_Menu.transform.DOLocalMove(new Vector3(m_Menu.transform.localPosition.x + -to_x, m_Menu.transform.localPosition.y + -to_y, 0), duration).SetEase(scene_current_easetype.GetEasetype(easetype));
        m_Control.transform.DOLocalMove(new Vector3(0, m_Control.transform.localPosition.y + to_y, 0), duration).SetEase(scene_current_easetype.GetEasetype(easetype));
    }

    private IEnumerator ExitController()
    {
        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(m_Menu.GetComponentInChildren<MenuButtonHandler>().m_FirstSelectedButton);
        m_Menu.transform.DOLocalMove(new Vector3(0, m_Menu.transform.localPosition.y + to_y, 0), duration).SetEase(scene_current_easetype.GetEasetype(easetype));
        m_Control.transform.DOLocalMove(new Vector3(-Screen.width, m_Control.transform.localPosition.y + -to_y, 0), duration).SetEase(scene_current_easetype.GetEasetype(easetype));
        StartCoroutine(m_Menu.GetComponentInChildren<MenuButtonHandler>().EnableButton(0.5f));
        yield return new WaitForSeconds(duration / 3);
        m_Control.SetActive(false);
    }

    public static float WorldToCamera(float num, int axis)
    {
        switch (axis)
        {
            case 0:
                num *= ((float)Screen.width / 1920);
                //Debug.Log(num);
                break;

            case 1:
                num *= ((float)Screen.height / 1080);
                //Debug.Log(num);
                break;

            case 2:
                num *= (((float)Screen.width / 1920) + ((float)Screen.height / 1080)) / 2;
                //Debug.Log(num);
                break;
        }
        return num;
    }

    public void Quit()
    {
        Debug.Log("Quitting");

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif

        Application.Quit();
    }

    public void DiesableOnClickEffect()
    {
        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(null);
    }
}
