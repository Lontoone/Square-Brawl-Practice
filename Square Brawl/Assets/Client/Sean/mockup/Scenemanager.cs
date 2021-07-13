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

public class Scenemanager : MonoBehaviour//, ISelectHandler, IDeselectHandler
{
    private Gamepad gamepad = Gamepad.current;
    private Keyboard keyboard = Keyboard.current;
    [SerializeField] private GameObject m_Menu;
    [SerializeField] private GameObject m_Option;
    [SerializeField] private GameObject m_Mapeditor;
    [SerializeField] private GameObject m_Control;
    [SerializeField] private GameObject m_Lobby;
    [SerializeField] private GameObject m_EnterRoomName;
    [SerializeField] private GameObject m_RoomName;
    [SerializeField] private GameObject m_Characterselection;
    [SerializeField] private GameObject m_Gamemode;
    [SerializeField] private GameObject m_MapSelection;
    [SerializeField] private GameObject m_WeaponSelection;
    [SerializeField] private GameObject m_ScoreInfo;
    private Easetype.Current_easetype scene_current_easetype;
    [SerializeField] Easetype.Current_easetype.Easetype easetype;
    [SerializeField] float duration = 1f;
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


    private void Awake()
    {
        m_Option.SetActive(false);
        scene_current_easetype = new Easetype.Current_easetype();
        green = m_Green;
        orange = m_Orange;
        red = m_Red;
        blue = m_Blue;
    }
    void Start()
    {
        
        //pos = m_Menu.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (keyboard != null)
        {
            if (keyboard.enterKey.wasPressedThisFrame)
            {
                Debug.Log("keyboard enter");
            }
            /*else if (keyboard.escapeKey.wasPressedThisFrame)
            {
                ExitLobby();
            }*/
        }
        if (gamepad != null)
        {
            if (gamepad.buttonSouth.wasPressedThisFrame)
            {
                Debug.Log("gamepad enter");
            }
            /*
            else if (gamepad.buttonEast.wasPressedThisFrame)
            {
                ExitLobby();
            }
            */
        }
    }
    public void EnterOption()
    {
        m_Option.SetActive(true);
        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(null);
        m_Menu.transform.DOLocalMove(new Vector3(m_Menu.transform.localPosition.x + to_x, m_Menu.transform.localPosition.y + to_y,0),duration).SetEase(scene_current_easetype.GetEasetype(easetype));
        m_Option.transform.DOLocalMove(new Vector3(0, m_Option.transform.localPosition.y + to_y, 0), duration).SetEase(scene_current_easetype.GetEasetype(easetype));
    }

    public void ExitOption()
    {
        m_Option.SetActive(false);
        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(null);
        m_Menu.transform.DOLocalMove(new Vector3(m_Menu.transform.localPosition.x + -to_x, m_Menu.transform.localPosition.y + -to_y, 0), duration).SetEase(scene_current_easetype.GetEasetype(easetype));
        m_Option.transform.DOLocalMove(new Vector3(Screen.width, m_Option.transform.localPosition.y + -to_y, 0), duration).SetEase(scene_current_easetype.GetEasetype(easetype));
    }

    public void EnterLobby()
    {
        m_Menu.transform.DOScale(new Vector3(10, 10, 0), duration).SetEase(scene_current_easetype.GetEasetype(easetype));
    }

    public void ExitLobby()
    {
        m_Menu.transform.DOScale(new Vector3(1, 1, 0), duration).SetEase(scene_current_easetype.GetEasetype(easetype));
    }

    public void DiesableOnClickEffect()
    {
        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(null);
    }
}
