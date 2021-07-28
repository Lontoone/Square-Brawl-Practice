using System.Collections;
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

    [HideInInspector] public bool m_MouseSelectedState = false;
    [HideInInspector] public bool m_KeySelectedState = false;
    TextMeshProUGUI[] textArray;

    private bool Selectedtrigger;

    void Start()
    {
        m_CurrentEasetype = new Easetype.Current_easetype();
        screen = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)) * 2;
    }

    private void Update()
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

        m_IconMove.Kill();
        m_IconMove = DOTween.Sequence();
        m_IconMove.Append(m_Aim.m_AimObject.transform.DOLocalMove(new Vector3(m_Aim.m_Area.x * scale.x, m_Aim.m_Area.y * scale.y), m_Aim.m_Duration).SetEase(m_CurrentEasetype.GetEasetype(m_Aim.m_Easetype)));
        Debug.Log(new Vector3(m_Aim.m_Area.x * scale.x, m_Aim.m_Area.y * scale.y));

    }

    private void Settrigger()
    {
        Selectedtrigger = (OptionManager.m_PressIndex != m_ButtonIndex);
    }
    public void HighlightedBackground() //todo
    {
        if (m_Background != null)
        {
            m_Background.DOColor(SceneHandler.green, 0.5f);
        }
    }

    public void IdleBackground() //todo
    {
        if (m_Background != null)
        {
            m_Background.DOColor(new Color32(230, 230, 230, 255), 0.5f);
        }
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
