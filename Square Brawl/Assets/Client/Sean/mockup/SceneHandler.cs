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
    [SerializeField] private GameObject m_MapEditor;
    [SerializeField] private GameObject m_Control;
    [SerializeField] private GameObject m_OnlineMenu;
    [SerializeField] private GameObject m_NameInput;
    [SerializeField] private GameObject m_Lobby;
    [SerializeField] private GameObject m_CreateRoom;
    [SerializeField] private GameObject m_RoomList;
    [SerializeField] private GameObject m_Room;
    [SerializeField] private GameObject m_CharacterSelection;
    [SerializeField] private GameObject m_GameMode;
    [SerializeField] private GameObject m_MapSelection;
    [SerializeField] private GameObject m_WeaponSelection;
    [SerializeField] private GameObject m_ScoreInfo;
    [SerializeField] private GameObject m_Loading;
    [SerializeField] private GameObject m_Error;
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

    public static SceneHandler instance;
    private Animator animator;

    public static Color32 green;
    public static Color32 orange;
    public static Color32 red;
    public static Color32 blue;
    public enum axis {x = 0, y = 1, cons = 2}
    public enum AnimationEnd
    { 
        Menu,
        Option,
        MapEditor,
        Control,
        OnlineMenu,
        NameInput,
        Lobby,
        CreateRoom,
        RoomList,
        Room,
        CharacterSelection,
        GameSelection,
        GameMode,
        MapSelection,
        WeaponSelection,
        Loading,
        Error
    }
    public bool onAnimationEnd;


    private void Awake()
    {
        instance = this;
        animator = GetComponent<Animator>();

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
    }

    private void Update()
    {
        //Debug.Log(animator.GetCurrentAnimatorStateInfo(0).IsName("ExitMenu") +"\t"+ animator.GetCurrentAnimatorClipInfoCount(0));
    }

    public void EnterPage(string gameObject)
    {
        switch (gameObject)
        {
            case "Option":
                StartCoroutine(EnterOption());
                break;

            case "MapEditor":
                EnterMapEditor();
                break;

            case "Control":
                EnterControl();
                break;

            case "OnlineMenu":
                StartCoroutine(EnterOnlineMenu());
                break;

            case "NameInput":
                StartCoroutine(EnterNameInput());
                break;

            case "Lobby":
                StartCoroutine(EnterLobby());
                break;

            case "CreateRoom":
                StartCoroutine(EnterCreateRoom());
                break;

            case "RoomList":
                StartCoroutine(EnterRoomList());
                break;

            case "Room":
                StartCoroutine(EnterRoom());
                break;

            case "CharacterSelection":
                StartCoroutine(EnterCharacterSelection());
                break;

            case "GameMode":
                StartCoroutine(EnterGameMode());
                break;

            case "MapSelection":
                StartCoroutine(EnterMapSelection());
                break;

            case "WeaponSelection":
                StartCoroutine(EnterWeaponSelection());
                break;

            case "Loading":
                StartCoroutine(EnterLoading());
                break;

            default:
                Debug.LogWarning(gameObject + " :switch page error");
                break;
        }
    }
    public void ExitPage(string gameObject)
    {
        switch (gameObject)
        {
            case "Option":
                StartCoroutine(ExitOption());
                break;

            case "MapEditor":
                StartCoroutine(ExitMapEditor());
                break;

            case "Control":
                StartCoroutine(ExitControl());
                break;

            case "OnlineMenu":
                StartCoroutine(ExitOnlineMenu());
                break;

            case "NameInput":
                StartCoroutine(ExitNameInput());
                break;

            case "Lobby":
                StartCoroutine(ExitLobby());
                break;

            case "CreateRoom":
                StartCoroutine(ExitCreateRoom());
                break;

            case "RoomList":
                StartCoroutine(ExitRoomList());
                break;

            case "Room":
                StartCoroutine(ExitRoom());
                break;

            case "CharacterSelection":
                StartCoroutine(ExitCharacterSelection());
                break;

            case "GameMode":
                StartCoroutine(ExitGameMode());
                break;

            case "MapSelection":
                StartCoroutine(ExitMapSelection());
                break;

            case "WeaponSelection":
                StartCoroutine(ExitWeaponSelection());
                break;

            case "Loading":
                StartCoroutine(ExitLoading());
                break;

            default:
                Debug.LogWarning(gameObject + " :switch page error");
                break;
        }
    }

    #region -- Option --

    private IEnumerator EnterOption()
    {
        m_Menu.GetComponentInChildren<MenuButtonHandler>().DisableButton();
        m_Option.SetActive(true);
        yield return null;
        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(null);
        m_Menu.transform.DOLocalMove(new Vector3(m_Menu.transform.localPosition.x - 1920, m_Menu.transform.localPosition.y + to_y, 0), duration).SetEase(scene_current_easetype.GetEasetype(easetype));
        m_Option.transform.DOLocalMove(new Vector3(0, m_Option.transform.localPosition.y + to_y, 0), duration).SetEase(scene_current_easetype.GetEasetype(easetype));
        StartCoroutine(m_Option.GetComponentInChildren<OptionManager>().EnterAnimation());
    }

    public IEnumerator ExitOption()
    {
        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(m_Menu.GetComponentInChildren<MenuButtonHandler>().m_FirstSelectedButton);
        m_Menu.transform.DOLocalMove(new Vector3(0, m_Menu.transform.localPosition.y + -to_y, 0), duration).SetEase(scene_current_easetype.GetEasetype(easetype));
        m_Option.transform.DOLocalMove(new Vector3(1920, m_Option.transform.localPosition.y + -to_y, 0), duration).SetEase(scene_current_easetype.GetEasetype(easetype));
        StartCoroutine(m_Menu.GetComponentInChildren<MenuButtonHandler>().EnableButton(0.8f));
        m_Option.GetComponentInChildren<OptionManager>().ExitAnimation();
        yield return new WaitForSeconds(duration / 3);
        m_Option.SetActive(false);
    }

    #endregion

    #region -- MapEditor --

    private void SetUpMapEditor()
    {
        m_MapEditor.transform.localPosition = new Vector3(0, WorldToCamera(1080, 1) * 2);
    }

    private void EnterMapEditor()
    {
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
    #endregion

    #region -- Control --
    private void EnterControl()
    {
        m_Menu.GetComponentInChildren<MenuButtonHandler>().DisableButton();
        m_Control.SetActive(true);
        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(null);
        m_Menu.transform.DOLocalMove(new Vector3(m_Menu.transform.localPosition.x + -to_x, m_Menu.transform.localPosition.y + -to_y, 0), duration).SetEase(scene_current_easetype.GetEasetype(easetype));
        m_Control.transform.DOLocalMove(new Vector3(0, m_Control.transform.localPosition.y + to_y, 0), duration).SetEase(scene_current_easetype.GetEasetype(easetype));
    }

    private IEnumerator ExitControl()
    {
        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(m_Menu.GetComponentInChildren<MenuButtonHandler>().m_FirstSelectedButton);
        m_Menu.transform.DOLocalMove(new Vector3(0, m_Menu.transform.localPosition.y + to_y, 0), duration).SetEase(scene_current_easetype.GetEasetype(easetype));
        m_Control.transform.DOLocalMove(new Vector3(-Screen.width, m_Control.transform.localPosition.y + -to_y, 0), duration).SetEase(scene_current_easetype.GetEasetype(easetype));
        StartCoroutine(m_Menu.GetComponentInChildren<MenuButtonHandler>().EnableButton(0.5f));
        yield return new WaitForSeconds(duration / 3);
        m_Control.SetActive(false);
    }
    #endregion

    #region -- OnlineMenu --
    public IEnumerator EnterOnlineMenu()
    {
        //m_Menu.GetComponentInChildren<MenuButtonHandler>().DisableButton();
        m_OnlineMenu.SetActive(true);
        m_Menu.GetComponentInChildren<PlayButton>().EnterMenuSetUp();
        yield return null;
        m_Menu.SetActive(false);
    }

    public IEnumerator ExitOnlineMenu()
    {
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
        m_NameInput.SetActive(false);
        m_Menu.SetActive(true);
        animator.Play("EnterMenu");
        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(null);
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
        m_OnlineMenu.SetActive(false);
        GetComponent<Animator>().enabled = false;
        Launcher.instance.Quit();
    }
    #endregion

    #region -- NameInput --

    private IEnumerator EnterNameInput()
    {
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
        m_NameInput.SetActive(true);
        GetComponent<Animator>().Play("EnterName");
        yield return null;
    }

    private IEnumerator ExitNameInput()
    {
        animator.Play("ExitName");
        yield return new WaitUntil(()=> animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
        m_NameInput.SetActive(false);
    }

    #endregion

    #region -- Lobby --

    private IEnumerator EnterLobby()
    {
        m_Lobby.SetActive(true);
        yield return null;
    }

    private IEnumerator ExitLobby()
    {
        m_Lobby.SetActive(false);
        yield return null;
    }

    #endregion

    #region -- CreateRoom --

    private IEnumerator EnterCreateRoom()
    {
        Debug.Log("create room");
        m_CreateRoom.SetActive(true);
        yield return null;
    }

    private IEnumerator ExitCreateRoom()
    {
        yield return null;
        m_CreateRoom.SetActive(false);
    }

    #endregion

    #region -- RoomList --

    private IEnumerator EnterRoomList()
    {
        m_RoomList.SetActive(true);
        yield return null;
    }

    private IEnumerator ExitRoomList()
    {
        m_RoomList.SetActive(false);
        yield return null;
    }

    #endregion

    #region -- Room --

    private IEnumerator EnterRoom()
    {
        m_Room.SetActive(true);
        yield return null;
    }

    private IEnumerator ExitRoom()
    {
        m_Room.SetActive(false);
        yield return null;
    }

    #endregion

    #region -- CharacterSelection --

    private IEnumerator EnterCharacterSelection()
    {
        m_CharacterSelection.SetActive(true);
        yield return null;
    }

    private IEnumerator ExitCharacterSelection()
    {
        m_CharacterSelection.SetActive(false);
        yield return null;
    }

    #endregion

    #region -- GameMode --

    private IEnumerator EnterGameMode()
    {
        m_GameMode.SetActive(true);
        yield return null;
    }

    private IEnumerator ExitGameMode()
    {
        m_GameMode.SetActive(false);
        yield return null;
    }

    #endregion

    #region -- MapSelection --

    private IEnumerator EnterMapSelection()
    {
        m_MapSelection.SetActive(true);
        yield return null;
    }

    private IEnumerator ExitMapSelection()
    {
        m_MapSelection.SetActive(false);
        yield return null;
    }

    #endregion

    #region -- WeaponSelection --

    private IEnumerator EnterWeaponSelection()
    {
        m_WeaponSelection.SetActive(true);
        yield return null;
    }

    private IEnumerator ExitWeaponSelection()
    {
        m_WeaponSelection.SetActive(false);
        yield return null;
    }

    #endregion

    #region -- ScoreInfo --

    private IEnumerator EnterScoreInfo()
    {
        m_ScoreInfo.SetActive(true);
        yield return null;
    }

    private IEnumerator ExitScoreInfo()
    {
        m_ScoreInfo.SetActive(false);
        yield return null;
    }

    #endregion

    #region -- Loading --

    private IEnumerator EnterLoading()
    {
        m_Loading.SetActive(true);
        animator.Play("Loading");
        yield return null;
    }

    private IEnumerator ExitLoading()
    {
        animator.Play("ExitLoading");
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >=1);
        m_Loading.SetActive(false);
    }

    #endregion

    #region -- Error --

    private IEnumerator EnterError()
    {
        m_Error.SetActive(true);
        yield return null;
    }

    private IEnumerator ExitError()
    {
        m_Error.SetActive(false);
        yield return null;
    }

    #endregion

    #region -- Tool --
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

    public bool OnAnimationEnd()
    {
        return onAnimationEnd = true;
    }

    public void Quit()
    {
        Debug.Log("Quitting");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif

        Application.Quit();
    }

    public void DisableOnClickEffect()
    {
        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(null);
    } 
    #endregion
}
