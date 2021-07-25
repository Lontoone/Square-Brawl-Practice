using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DotEvent : MonoBehaviour, IPointerClickHandler
{
    public GameObject m_Dot;
    public int m_Index;
    public Color32 m_Color;
    public Color32 m_DefaultColor;


    public void SetUp(GameObject dot, int index, Color32 color, Color32 defaultColor)
    {
        m_Dot = dot;
        m_Index = index;
        m_Color = color;
        m_DefaultColor = defaultColor;
    }

    public void CurtentSelected()
    {
        ToDotSlider.DotSliderAction.m_IsChangebyClick = true;
        ToDotSlider.DotSliderAction.m_SelectedIndex = m_Index;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        CurtentSelected();
    }
}
