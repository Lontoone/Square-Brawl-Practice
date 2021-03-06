using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;
using BasicTools.ButtonInspector;
using DG.Tweening;
using TMPro;
namespace Easetype {}
namespace ToSplitChar {}

public class ButtonAction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    [SerializeField] public int m_ButtonIndex;
    [SerializeField] private GameObject m_button;
    [SerializeField] private Image m_Icon;
    [SerializeField] private TextMeshProUGUI m_button_text;
    private int m_TextLength;
    [Space(10)]
    [SerializeField] private GameObject m_AimObject;
    [Space(10)]
    [SerializeField] private float to_x;
    [SerializeField] private float to_y;
    [Range(0,2)]
    [SerializeField] private int m_AudioType;
    private Vector3 pos;
    
    private Sequence _moveSequence_stirng;
    private Sequence _moveSequence_char;

    private Easetype.Current_easetype menu_current_easetype;
    
    private enum Dotweentype{ m_string = 0 , m_char = 1 , None = 2}
    [Space(10)]
    [SerializeField] private Dotweentype dotweentype;
    [Space(10)]
    [HeaderAttribute("String")]
    [SerializeField] Easetype.Current_easetype.Easetype string_easetype;
    [SerializeField] float string_duration = 1f;
    [SerializeField] private float stirng_outdistance = 5;

    [HideInInspector]
    public ToSplitChar.SplitCharAction SplitCharAction;
    [Space(15)]
    public ToSplitChar.SplitCharAction.SplitChar m_Char;


    [HideInInspector] public bool m_MouseSelectedState = false;
    [HideInInspector] public bool m_KeySelectedState = false;
    TextMeshProUGUI[] textArray;


    void Start()
    {
        pos = m_button_text.transform.localPosition;
        menu_current_easetype = new Easetype.Current_easetype();
        m_TextLength = m_button_text.text.Length;
        if (m_Icon != null)
        {
            m_Icon.DOColor(new Color32(205, 205, 205, 255), 0.5f);
        }

        SplitCharAction = m_button.AddComponent<ToSplitChar.SplitCharAction>();
        m_Char = SplitCharAction.SetUp(m_Char);

    }

    public void HighlightedIcon() //todo
    {
        if (m_Icon != null)
        {
            m_Icon.DOColor(SceneHandler.green, 0.5f);
        }
    }

    public void IdleIcon() //todo
    {
        if (m_Icon != null) 
        {
            m_Icon.DOColor(new Color32(205, 205, 205, 255), 0.5f);
        }
    }
    public void HighlightedString()
    {
        //Debug.Log("HighlightedString");
        _moveSequence_stirng.Kill();
        _moveSequence_stirng = DOTween.Sequence();
        m_button_text.transform.localPosition = pos;
        _moveSequence_stirng.Append(m_button_text.transform
                              .DOLocalMove(new Vector3(pos.x + to_x, pos.y + to_y), string_duration)
                              .SetEase(menu_current_easetype.GetEasetype(string_easetype)));
    }

    public void IdleString()
    {
        //Debug.Log("IdleString");
        _moveSequence_stirng.Kill();
        _moveSequence_stirng = DOTween.Sequence();
        m_button_text.transform.localPosition = new Vector3(pos.x + to_x, pos.y + to_y, pos.z);

        _moveSequence_stirng.Append(m_button_text.transform
                              .DOLocalMove(new Vector3(pos.x + to_x * stirng_outdistance, pos.y + to_y * stirng_outdistance), string_duration * stirng_outdistance)
                              .SetEase(menu_current_easetype.GetEasetype(string_easetype)));
    }

    public void IdleChar()
    {
        SplitCharAction.IdleChar(m_Char);
    }

    public void IdleController()
    {
        switch (OptionSetting.TRANSITIONANIMATION)
        {
            case false:
                IdleString();
                break;

            case true:
                SplitCharAction.IdleChar(m_Char);
                break;
        }
    }

    public void HighlightedConrtoller()
    {
        switch (OptionSetting.TRANSITIONANIMATION)
        {
            case false:
                HighlightedString();
                break;

            case true:
                SplitCharAction.HighlightedChar(m_Char);
                break;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        m_MouseSelectedState = true;
        HighlightedConrtoller();
        HighlightedIcon();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        IdleController();
        IdleIcon();
        m_MouseSelectedState = false;
        m_KeySelectedState = false;
    }

    public virtual void OnSelect(BaseEventData eventData)
    {
        m_KeySelectedState = true;
        HighlightedConrtoller();
        HighlightedIcon();
    }

    public virtual void OnDeselect(BaseEventData eventData)
    {
        IdleController();
        IdleIcon();
        m_MouseSelectedState = false;
        m_KeySelectedState = false;
    }
}
