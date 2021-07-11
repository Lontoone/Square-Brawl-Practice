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
namespace Easetype {}

public class Play_Button : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    public int m_ButtonIndex;
    private Gamepad gamepad = Gamepad.current;
    private Keyboard keyboard = Keyboard.current;
    [Header ("Play Button")]
    [SerializeField] private GameObject play_button;
    private Button button_button;
    private RectTransform button_rect;
    private Easetype.Current_easetype m_PlayButtonCurrentEasetype;
    [SerializeField] Easetype.Current_easetype.Easetype easetype;
    [SerializeField] float m_duration =1f;
    
    [Space(10)]
    [Header("Aim Object")]
    private AimAction m_aimAction;
    [SerializeField] private GameObject m_AimObject;
    [SerializeField] Easetype.Current_easetype.Easetype m_AimEasetype;
    private Easetype.Current_easetype m_AimCurrentEasetype;
    [SerializeField] float m_aimduration = 0.3f;
    

    void Start()
    {
        //button_rect = play_button.transform;
        button_button = play_button.GetComponent<Button>();
        m_PlayButtonCurrentEasetype = new Easetype.Current_easetype();
        m_AimCurrentEasetype = new Easetype.Current_easetype();
    }

    /*void Update()
    {
        if (keyboard != null)
        {
            if (keyboard.enterKey.wasPressedThisFrame)
            {
                Debug.Log("keyboard enter");
                EnterLobby();
                //ChangeButtonColor();

            }
            else if (keyboard.escapeKey.wasPressedThisFrame)
            {
                ExitLobby();
            }
        }
        if (gamepad != null)
        {
            if (gamepad.buttonSouth.wasPressedThisFrame)
            {
                Debug.Log("gamepad enter");
                EnterLobby();
                //ChangeButtonColor();

            }
            else if (gamepad.buttonEast.wasPressedThisFrame) 
            {
                ExitLobby();
            }
        }

    }*/

    private void EnterLobby() 
    {
        play_button.transform.DOScale(new Vector3(10,10,0), m_duration).SetEase(m_PlayButtonCurrentEasetype.GetEasetype(easetype));
    }

    private void ExitLobby() 
    {
        play_button.transform.DOScale(new Vector3(1, 1, 0), m_duration).SetEase(m_PlayButtonCurrentEasetype.GetEasetype(easetype));
    }
    /*
    private void ChangeButtonColor()
    {
        ColorBlock cb = m_button.colors;
        cb.normalColor = m_button.colors.pressedColor;
        m_button.colors = cb;

    }
    */
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        //m_aimAction.AimFade(m_AimObject, m_aimduration, m_AimCurrentEasetype, m_AimEasetype);
        AimAction.AimFade(m_AimObject, m_aimduration, m_AimCurrentEasetype, m_AimEasetype);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        //m_aimAction.AimUnFade(m_AimObject, m_aimduration, m_AimCurrentEasetype, m_AimEasetype);
        AimAction.AimUnFade(m_AimObject, m_aimduration, m_AimCurrentEasetype, m_AimEasetype);
        
    }

    public virtual void OnSelect(BaseEventData eventData)
    {
        
    }

    public virtual void OnDeselect(BaseEventData eventData)
    {
        
    }
}
