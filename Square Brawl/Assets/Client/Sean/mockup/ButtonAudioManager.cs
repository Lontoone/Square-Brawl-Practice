using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ButtonAudioManager : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, ISelectHandler, ISubmitHandler
{
    [Range (0, 2)]
    public int enterType;
    [Range(0, 2)]
    public int clickType;

    /*private bool pressByMouse = false;

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            pressByMouse = true;
        }
        else
        {
            pressByMouse = false;
        }
        Debug.Log(pressByMouse);
    }*/

    public void OnPointerClick(PointerEventData eventData)
    {
        //AudioSourcesManager.PlaySFX(clickType); //TODO
        Debug.Log("click");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioSourcesManager.PlaySFX(enterType);
    }

    public void OnSelect(BaseEventData eventData)
    {
        AudioSourcesManager.PlaySFX(enterType);
        Debug.Log("selected");
    }

    public void OnSubmit(BaseEventData eventData)
    {
        AudioSourcesManager.PlaySFX(clickType);
        Debug.Log("submit");
    }
}
