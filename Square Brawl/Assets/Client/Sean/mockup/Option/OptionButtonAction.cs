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

public class OptionButtonAction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler ,IPointerClickHandler
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
        [SerializeField] public float m_MoveDistance;
        [SerializeField] public float m_DetectDistance;
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
    private Vector3 trans;

    private Easetype.Current_easetype m_CurrentEasetype;

    private Sequence m_IconMove;
    private Sequence m_Press;
    private Sequence m_BackgroundColor;
    public bool onPress = false;

    [HideInInspector] public bool m_MouseSelectedState = false;
    [HideInInspector] public bool m_KeySelectedState = false;
    TextMeshProUGUI[] textArray;

    private bool Selectedtrigger;
    private bool gamepadUpdate = false;
    private string mode;//0 = keyboard & gamepad, 1 = mouse

    private void Awake()
    {
        if (m_Background != null)
        {
            trans = new Vector3(m_Background.GetComponent<RectTransform>().sizeDelta.x, m_Background.GetComponent<RectTransform>().sizeDelta.y);
        }
    }

    void Start()
    {
        if (m_Background != null)
        {
            m_Background.color = m_DefaultColor;
        }
        if (m_Background != null)
        {
            m_Icon.color = SceneHandler.green;
        }
        m_CurrentEasetype = new Easetype.Current_easetype();
        screen = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)) * 2;
    }

    private void FixedUpdate()
    {
        AimToMouse();
        Settrigger();
    }

    private void AimToMouse()
    {
        if (Gamepad.current != null)
        {
            if (Gamepad.current.wasUpdatedThisFrame)
            {
                gamepadUpdate = true;
            }
        }

        if (Mouse.current.wasUpdatedThisFrame)
        {
            mode = "Mouse";
            gamepadUpdate = false;
        }
        if (Keyboard.current.wasUpdatedThisFrame || gamepadUpdate)
        {
            mode = "Keyboard";
        }

        if (mode == "Keyboard")
        {
            m_Aim.m_MouseWorldPos = new Vector2(0, 0);
        }
        else
        {
            //Type 1
            m_Aim.m_MouseWorldPos = Mouse.current.position.ReadValue();

            //Type 2
            //m_Aim.m_MouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        }

        //Move Icon
        //Type 1
        var localVec = m_Aim.m_MouseWorldPos - new Vector2(m_Aim.m_AimPos.transform.position.x, m_Aim.m_AimPos.transform.position.y);
        var length = Vector3.Magnitude(localVec);
        Vector3 move;
        if (length <= m_Aim.m_DetectDistance)
        {
            move = (m_Aim.m_MoveDistance / m_Aim.m_DetectDistance) * localVec;
        }
        else
        {
            move = Vector3.zero;
        }

        //Type 2
        /* 
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
        */
        m_IconMove.Kill();
        m_IconMove = DOTween.Sequence();
        m_IconMove.Append(m_Aim.m_AimObject.transform.DOLocalMove(move, m_Aim.m_Duration).SetEase(m_CurrentEasetype.GetEasetype(m_Aim.m_Easetype)));
        //Debug.Log(new Vector3(m_Aim.m_Area.x * scale.x, m_Aim.m_Area.y * scale.y));
        

    }

    private void Settrigger()
    {
        Selectedtrigger = (OptionManager.onPressIndex != m_ButtonIndex);
    }
    public void HighlightedBackground()
    {
        if (m_Background != null && onPress ==false)
        {
            m_BackgroundColor.Kill();
            m_BackgroundColor = DOTween.Sequence();
            m_BackgroundColor.Append(m_Background.DOColor(new Color32(SceneHandler.green.r, SceneHandler.green.g, SceneHandler.green.b,50), 0.2f));
        }
        if (m_button_text != null && onPress == false)
        {
            m_button_text.DOColor(new Color32(120, 120, 120, 250), 0.2f);
        }
    }

    public void IdleBackground() //todo
    {
        if (m_Background != null)
        {
            m_BackgroundColor.Kill();
            m_BackgroundColor = DOTween.Sequence();
            m_BackgroundColor.Append(m_Background.DOColor(m_DefaultColor, 0.5f));
        }
        if (m_button_text != null)
        {
            m_button_text.DOColor(new Color32(237, 237, 237, 250), 0.2f);
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
                    .DOSizeDelta(new Vector2(680, 1000), 0.5f)
                    .SetEase(m_CurrentEasetype.GetEasetype(m_Aim.m_Easetype)))
               .Join(m_Background.DOColor(m_DefaultColor, 0.5f));
        if (m_button_text != null)
        {
            m_button_text.DOColor(new Color32(237, 237, 237, 0), 0.2f);
        }
    }

    public void UnPress()
    {
        onPress = false;
        m_Press.Kill();
        m_Press = DOTween.Sequence();
        m_Press.Append(m_Aim.m_AimPos.transform
                    .DOLocalMove(new Vector3(0, 0, 0), 0.5f))
               .Join(m_Background.rectTransform
                    .DOSizeDelta(trans, 0.3f))
               .Join(m_Background.DOColor(m_DefaultColor, 0));
        {
            m_button_text.DOColor(new Color32(237, 237, 237, 250), 0.2f);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        m_MouseSelectedState = true;
        if (m_KeySelectedState != true && m_MouseSelectedState == true)
        {
            //m_button_text.gameObject.SetActive(true);
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
            //m_button_text.gameObject.SetActive(false);
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

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("button click");
    }
}
