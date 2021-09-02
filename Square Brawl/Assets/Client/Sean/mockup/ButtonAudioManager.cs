using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ButtonAudioManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, ISelectHandler, ISubmitHandler
{
    [Range (0, 2)]
    public int enterType;
    [Range(0, 2)]
    public int clickType;

    private bool isMouseEnter = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        AudioSourcesManager.PlaySFX(clickType);
        Debug.Log("click");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseEnter = true;
        AudioSourcesManager.PlaySFX(enterType);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseEnter = false;
    }

    public void OnSelect(BaseEventData eventData)
    {
        //Don't play sound when first click, fix OnSelect conflict 
        if (!isMouseEnter)
        { 
            AudioSourcesManager.PlaySFX(enterType);
        }
        Debug.Log("selected");
    }

    public void OnSubmit(BaseEventData eventData)
    {
        AudioSourcesManager.PlaySFX(clickType);
        Debug.Log("submit");
    }
}
