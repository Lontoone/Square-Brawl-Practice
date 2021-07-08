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
    private Easetype.Current_easetype current_easetype;
    [SerializeField] Easetype.Current_easetype.Easetype easetype;
    [SerializeField] float duration = 1f;
    [SerializeField] private float to_x;
    [SerializeField] private float to_y;
    private Vector3 pos;
    void Start()
    {
        current_easetype = new Easetype.Current_easetype();
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
        m_Menu.transform.DOLocalMove(new Vector3(m_Menu.transform.localPosition.x + to_x, m_Menu.transform.localPosition.y + to_y,0),duration).SetEase(current_easetype.GetEasetype(easetype));
        m_Option.transform.DOLocalMove(new Vector3(0, m_Option.transform.localPosition.y + to_y, 0), duration).SetEase(current_easetype.GetEasetype(easetype));
    }

    public void ExitOption()
    {
        m_Menu.transform.DOLocalMove(new Vector3(m_Menu.transform.localPosition.x + -to_x, m_Menu.transform.localPosition.y + -to_y, 0), duration).SetEase(current_easetype.GetEasetype(easetype));
        m_Option.transform.DOLocalMove(new Vector3(Screen.width, m_Option.transform.localPosition.y + -to_y, 0), duration).SetEase(current_easetype.GetEasetype(easetype));
    }

    public void EnterLobby()
    {
        m_Menu.transform.DOScale(new Vector3(10, 10, 0), duration).SetEase(current_easetype.GetEasetype(easetype));
    }

    public void ExitLobby()
    {
        m_Menu.transform.DOScale(new Vector3(1, 1, 0), duration).SetEase(current_easetype.GetEasetype(easetype));
    }
}
