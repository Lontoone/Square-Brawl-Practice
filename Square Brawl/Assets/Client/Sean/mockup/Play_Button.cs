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

public class Play_Button : MonoBehaviour//, ISelectHandler, IDeselectHandler
{
    public int m_ButtonIndex;
    private Gamepad gamepad = Gamepad.current;
    private Keyboard keyboard = Keyboard.current;
    [SerializeField] private GameObject play_button;
    private Button button_button;
    private RectTransform button_rect;
    private Easetype.Current_easetype current_easetype;
    [SerializeField] Easetype.Current_easetype.Easetype easetype;
    [SerializeField] float duration =1f;
    void Start()
    {
        //button_rect = play_button.transform;
        button_button = play_button.GetComponent<Button>();
        current_easetype =new Easetype.Current_easetype();
    }

    // Update is called once per frame
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
        //Debug.Log("easetype:" + easetype);
        //play_button.transform.DOScale(new Vector3((Mathf.Pow(GetComponent<Renderer>().bounds.size.x, 2))/Screen.width, (Mathf.Pow(GetComponent<Renderer>().bounds.size.y, 2)) / Screen.height, 0), duration).SetEase(current_easetype.GetEasetype(easetype));
        play_button.transform.DOScale(new Vector3(10,10,0), duration).SetEase(current_easetype.GetEasetype(easetype));
    }

    private void ExitLobby() 
    {
        play_button.transform.DOScale(new Vector3(1, 1, 0), duration).SetEase(current_easetype.GetEasetype(easetype));
    }
    /*
    private void ChangeButtonColor()
    {
        ColorBlock cb = m_button.colors;
        cb.normalColor = m_button.colors.pressedColor;
        m_button.colors = cb;

    }
    public virtual void OnSelect(BaseEventData eventData)
    {
        
    }

    public virtual void OnDeselect(BaseEventData eventData)
    {
        
    }*/
}
