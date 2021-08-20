using System.Collections;
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

    [SerializeField] private AnimationClip[] m_AnimationClips;

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
        m_MapEditor.SetActive(false);
        m_Control.SetActive(false);
        m_OnlineMenu.SetActive(false);
        m_NameInput.SetActive(false);
        m_Lobby.SetActive(false);
        m_CreateRoom.SetActive(false);
        m_RoomList.SetActive(false);
        m_Room.SetActive(false);
        m_CharacterSelection.SetActive(false);
        m_GameMode.SetActive(false);
        m_MapSelection.SetActive(false);
        m_WeaponSelection.SetActive(false);
        m_ScoreInfo.SetActive(false);
        m_Loading.SetActive(false);
        m_Error.SetActive(false);
        SetUpMapEditor();
    }

    /*private void Update()
    {
        var length = 0f;
        var name = "";
        length = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        name = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        Debug.Log(name +": "+ length);
    }*/

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

    public void BackToCharacterSelection()
    {
        m_OnlineMenu.SetActive(true);
        StartCoroutine(EnterCharacterSelection());
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
        m_Menu.GetComponentInChildren<MenuButtonHandler>().OnExitMenuAction();
        m_Menu.GetComponentInChildren<AimAction>().OnExitMenuAction();
        yield return new WaitForSeconds(m_AnimationClips[1].length);
        m_OnlineMenu.SetActive(true);
        m_Menu.SetActive(false);
    }

    public IEnumerator ExitOnlineMenu()
    {
        var time = 0f;
        yield return null;
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("ExitName"))
        {
            time = m_AnimationClips[3].length;
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("ExitLobby"))
        {
            time = m_AnimationClips[7].length;
        }
        else
        {
            time = 0.5f;
        }

        yield return new WaitForSeconds(time);
        m_NameInput.SetActive(false);
        m_Menu.SetActive(true);
        m_Menu.GetComponentInChildren<Animator>().enabled = true;
        m_Menu.GetComponentInChildren<MenuButtonHandler>().OnEnterMenuAction();
        m_Menu.GetComponentInChildren<AimAction>().m_AimObject.SetActive(true);
        animator.Play("EnterMenu");
        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(m_Menu.GetComponentInChildren<MenuButtonHandler>().m_FirstSelectedButton);
        yield return new WaitForSeconds(m_AnimationClips[0].length);
        m_OnlineMenu.SetActive(false);
        GetComponent<Animator>().enabled = false;
        Launcher.instance.Quit();
    }
    #endregion

    #region -- NameInput --

    private IEnumerator EnterNameInput()
    {
        yield return new WaitForSeconds(m_AnimationClips[5].length);
        m_NameInput.SetActive(true);
        animator.Play("EnterName");
    }

    private IEnumerator ExitNameInput()
    {
        if (m_NameInput.activeSelf)
        {
            animator.Play("ExitName");
            yield return new WaitForSeconds(m_AnimationClips[3].length);
            m_NameInput.SetActive(false);
        }
    }

    #endregion

    #region -- Lobby --

    private IEnumerator EnterLobby()
    {
        Debug.Log("enter lobby");
        var time = 0f;
        if (m_NameInput.activeSelf || m_CreateRoom.activeSelf || m_RoomList.activeSelf || m_Loading.activeSelf)
        {
            if (m_NameInput.activeSelf)
            {
                time = m_AnimationClips[3].length;
            }
            else if (m_CreateRoom.activeSelf)
            {
                time = m_AnimationClips[9].length;
            }
            else if (m_RoomList.activeSelf)
            {
                time = m_AnimationClips[11].length;
            }
            else if (m_Loading.activeSelf)
            {
                time = m_AnimationClips[5].length;
            }
            yield return new WaitForSeconds(time);
            m_Loading.SetActive(false);     //Todo : 關掉Room的時候流程問題
            m_Lobby.SetActive(true);
            animator.Play("EnterLobby");
        }
    }

    private IEnumerator ExitLobby()
    {
        if (m_Lobby.activeSelf)
        {
            animator.Play("ExitLobby");
            yield return new WaitForSeconds(m_AnimationClips[7].length);
            m_Lobby.SetActive(false);
        }
    }

    #endregion

    #region -- CreateRoom --

    private IEnumerator EnterCreateRoom()
    {
        m_CreateRoom.SetActive(true);
        animator.Play("EnterCreateRoom");
        yield return null;
    }

    private IEnumerator ExitCreateRoom()
    {
        if (m_CreateRoom.activeSelf)
        {
            animator.Play("ExitCreateRoom");
            yield return new WaitForSeconds(m_AnimationClips[9].length);
            m_CreateRoom.SetActive(false);
        }
    }

    #endregion

    #region -- RoomList --

    private IEnumerator EnterRoomList()
    {
        if (m_Lobby.activeSelf)
        {
            m_RoomList.SetActive(true);
            animator.Play("EnterRoomList");
            yield return null;
        }
    }

    private IEnumerator ExitRoomList()
    {
        if (m_RoomList.activeSelf)
        {
            animator.Play("ExitRoomList");
            yield return new WaitForSeconds(m_AnimationClips[11].length);
            m_RoomList.SetActive(false);
        }
    }

    #endregion

    #region -- Room --

    private IEnumerator EnterRoom()
    {
        if (m_CreateRoom.activeSelf || m_RoomList.activeSelf || m_Loading.activeSelf ||m_CharacterSelection.activeSelf)
        {
            var time = 0f;
            if (m_CharacterSelection.activeSelf)
            {
                time = m_AnimationClips[15].length;
            }
            else if (m_Loading.activeSelf)
            {
                time = m_AnimationClips[5].length;
            }
            yield return new WaitForSeconds(time);
            animator.Play("EnterRoom");
            m_Room.SetActive(true);
        }
    }

    private IEnumerator ExitRoom()
    {
        //Todo : 關掉Room的時候流程問題
        if (m_Room.activeSelf)
        {
            animator.Play("ExitRoom");
            yield return new WaitForSeconds(m_AnimationClips[13].length);
            m_Room.SetActive(false);
        }
    }

    #endregion

    #region -- CharacterSelection --

    private IEnumerator EnterCharacterSelection()
    {
        var time = 0f;
        if (m_GameMode.activeSelf)
        { 
        
        }

        yield return new WaitForSeconds(time);
        m_CharacterSelection.SetActive(true);
        animator.Play("EnterCharacterSelection");
    }

    private IEnumerator ExitCharacterSelection()
    {
        if (m_CharacterSelection.activeSelf)
        {
            animator.Play("ExitCharacterSelection");
            yield return new WaitForSeconds(m_AnimationClips[15].length);
            m_CharacterSelection.SetActive(false);
        }
    }

    #endregion

    #region -- GameMode --

    private IEnumerator EnterGameMode()
    {
        var time = 0f;
        if (m_WeaponSelection.activeSelf)
        {

        }

        yield return new WaitForSeconds(time);
        animator.Play("EnterGameMode");
        m_GameMode.SetActive(true);
    }

    private IEnumerator ExitGameMode()
    {
        if (m_GameMode.activeSelf)
        {
            animator.Play("ExitGameMode");
            yield return new WaitForSeconds(m_AnimationClips[17].length);  
            m_GameMode.SetActive(false);
        }
    }

    #endregion

    #region -- MapSelection --

    private IEnumerator EnterMapSelection()
    {
        var time = 0f;
        if (m_GameMode.activeSelf)
        {
            time = m_AnimationClips[17].length;
        }
        else if (m_WeaponSelection.activeSelf)
        {
            time = m_AnimationClips[21].length;
        }
        m_MapSelection.SetActive(true);
        yield return new WaitForSeconds(time);
        animator.Play("EnterMapSelection");
    }

    private IEnumerator ExitMapSelection()
    {
        animator.Play("ExitMapSelection");
        yield return new WaitForSeconds(m_AnimationClips[19].length);
        m_MapSelection.SetActive(false);
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
        Debug.Log("enter loading");
        var time = 0f;
        if (m_CreateRoom.activeSelf)
        {
            time = m_AnimationClips[9].length;
        }
        else if (m_RoomList.activeSelf)
        {
            time = m_AnimationClips[11].length;
        }
        else if (m_Room.activeSelf)
        {
            time = m_AnimationClips[13].length;
        }
        yield return new WaitForSeconds(time);
        m_Loading.SetActive(true);
        animator.Play("Loading");
    }

    private IEnumerator ExitLoading()
    {
        //Todo : 關掉Room的時候流程問題
        if (m_Loading.activeSelf)
        {
            animator.Play("ExitLoading");
            Debug.Log("enter exit loading");
            yield return new WaitForSeconds(m_AnimationClips[5].length);
            Debug.Log("enter loading false");
            m_Loading.SetActive(false);
        }
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
