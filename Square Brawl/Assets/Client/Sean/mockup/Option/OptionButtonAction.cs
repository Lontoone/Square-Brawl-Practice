﻿using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;
using BasicTools.ButtonInspector;
using DG.Tweening;
using TMPro;
namespace Easetype { }
namespace ToSplitChar { }

public class OptionButtonAction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    [SerializeField] public int m_ButtonIndex;
    [SerializeField] private GameObject m_button;
    [SerializeField] private Image m_Background;
    [SerializeField] private Image m_Icon;
    [SerializeField] private TextMeshProUGUI m_button_text;
    [SerializeField] private Color32 m_DefaultColor;
    
    [System.Serializable]
    public struct AimAction
    {
        [SerializeField] public GameObject m_AimPos;
        [SerializeField] public GameObject m_AimObject;
        [SerializeField] public Vector2 m_Area;
        [Space(10)]
        [SerializeField] public Easetype.Current_easetype.Easetype m_Easetype;
        [SerializeField] public float m_Duration;
        [HideInInspector]
        public Vector2 m_ObjectPos;
        [HideInInspector]
        public  Vector2 m_MouseWorldPos;
    }
    [Space(10)]
    [SerializeField] private AimAction m_Aim;
    private Vector2 screen;
    private Vector3 pos;

    private Easetype.Current_easetype m_CurrentEasetype;
    
    

    private Sequence m_IconMove;
    private Sequence m_Press;
    public bool onPress = false;

    [HideInInspector] public bool m_MouseSelectedState = false;
    [HideInInspector] public bool m_KeySelectedState = false;
    TextMeshProUGUI[] textArray;

    private bool Selectedtrigger;

    void Start()
    {
        m_Icon.color = SceneHandler.green;
        m_CurrentEasetype = new Easetype.Current_easetype();
        screen = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)) * 2;
    }

    private void FixedUpdate()
    {
        //m_Aim.m_ObjectPos = m_Aim.m_AimPos.transform.position;
        AimToMouse();
        Settrigger();
    }

    private void AimToMouse()
    {
        m_Aim.m_MouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        //Debug.Log(m_Aim.m_MouseWorldPos +"\t"+ screen);

        Vector2 scale = new Vector2(m_Aim.m_MouseWorldPos.x / screen.x, m_Aim.m_MouseWorldPos.y / screen.y);
        var move = new Vector3(m_Aim.m_Area.x * scale.x, m_Aim.m_Area.y * scale.y);

        if (scale.x > 1)
        {
            move.x = m_Aim.m_Area.x;
        } 
        else if (scale.x < -1)
        {
            move.x = -m_Aim.m_Area.x;
        }

        if (scale.y > 1)
        {
            move.y = m_Aim.m_Area.y;
        }
        else if (scale.y < -1)
        {
            move.y = -m_Aim.m_Area.y;
        }

        m_IconMove.Kill();
        m_IconMove = DOTween.Sequence();
        m_IconMove.Append(m_Aim.m_AimObject.transform.DOLocalMove(move, m_Aim.m_Duration).SetEase(m_CurrentEasetype.GetEasetype(m_Aim.m_Easetype)));
        //Debug.Log(new Vector3(m_Aim.m_Area.x * scale.x, m_Aim.m_Area.y * scale.y));

    }

    private void Settrigger()
    {
        Selectedtrigger = (OptionManager.m_PressIndex != m_ButtonIndex);
    }
    public void HighlightedBackground() //todo
    {
        if (m_Background != null && onPress ==false)
        {
            m_Background.DOColor(new Color32(SceneHandler.green.r, SceneHandler.green.g, SceneHandler.green.b,50), 0.2f);
        }
    }

    public void IdleBackground() //todo
    {
        if (m_Background != null)
        {
            m_Background.DOColor(m_DefaultColor, 0.5f);
        }
    }

    public void OnPress()
    {
        onPress = true;
        m_Press.Kill();
        m_Press = DOTween.Sequence();
        m_Press.Append(m_Aim.m_AimPos.transform
                    .DOLocalMove(new Vector3(0, 450), 0.5f)
                    .SetEase(m_CurrentEasetype.GetEasetype(m_Aim.m_Easetype)))
               .Join(m_Background.rectTransform
                    .DOSizeDelta(new Vector2(700, 1000), 0.5f)
                    .SetEase(m_CurrentEasetype.GetEasetype(m_Aim.m_Easetype)))
               .Join(m_Background.DOColor(m_DefaultColor, 0.5f));
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        m_MouseSelectedState = true;
        if (m_KeySelectedState != true && m_MouseSelectedState == true)
        {

        }
        HighlightedBackground();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (Selectedtrigger)
        {

            IdleBackground();
            m_MouseSelectedState = false;
            m_KeySelectedState = false;
        }
    }

    public virtual void OnSelect(BaseEventData eventData)
    {
        m_KeySelectedState = true;
        if (m_MouseSelectedState != true && m_KeySelectedState == true)
        {

        }
        HighlightedBackground();
    }

    public virtual void OnDeselect(BaseEventData eventData)
    {
        if (Selectedtrigger)
        {

            IdleBackground();
            m_MouseSelectedState = false;
            m_KeySelectedState = false;
        }
    }
}
