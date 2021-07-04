using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class Button_2_test : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    /*[TextArea(3,5)]
    [SerializeField] private string text;
    [SerializeField] private TMP_Text button_text;
    private string debugtext;*/
    
    [SerializeField] private GameObject m_button;
    
    
    private enum Easetype
    {
        Unset = 0,
        Linear = 1,
        InSine = 2,
        OutSine = 3,
        InOutSine = 4,
        InQuad = 5,
        OutQuad = 6,
        InOutQuad = 7,
        InCubic = 8,
        OutCubic = 9,
        InOutCubic = 10,
        InQuart = 11,
        OutQuart = 12,
        InOutQuart = 13,
        InQuint = 14,
        OutQuint = 15,
        InOutQuint = 16,
        InExpo = 17,
        OutExpo = 18,
        InOutExpo = 19,
        InCirc = 20,
        OutCirc = 21,
        InOutCirc = 22,
        InElastic = 23,
        OutElastic = 24,
        InOutElastic = 25,
        InBack = 26,
        OutBack = 27,
        InOutBack = 28,
        InBounce = 29,
        OutBounce = 30,
        InOutBounce = 31,
        Flash = 32,
        InFlash = 33,
        OutFlash = 34,
        InOutFlash = 35

    }
    [SerializeField] private Easetype easetype;
    [SerializeField] private float to_x;
    //[FormerlySerializedAs(m_button.postion)]
    [SerializeField] private float to_y;
    [SerializeField] private float duration=1f;
    
   

    /*void Awake()
    {
        GameObject m_button;
        button_text = GetComponent<TMP_Text>();
        //button_text.text = text;
        button_text.enableWordWrapping = true;
        button_text.alignment = TextAlignmentOptions.Center;



        //if (GetComponentInParent(typeof(Canvas)) as Canvas == null)
        //{
        //    GameObject canvas = new GameObject("Canvas", typeof(Canvas));
        //    gameObject.transform.SetParent(canvas.transform);
        //    canvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;

        //    // Set RectTransform Size
        //    gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(500, 300);
        //    m_textMeshPro.fontSize = 48;
        //}


    }*/

    void Start()
    {
        
        /*//button_text.ForceMeshUpdate(true);
        button = GetComponent<TMP_Text>().transform;
        button_text = GetComponent<TMP_Text>();
        debugtext = GetComponent<TMP_Text>().text;
        //button_text.text = text;
       */
    }
    GameObject test;
    public void highlighted()
    {
        //button_text.text = text;
        m_button.transform.DOLocalMoveX(to_x, duration).SetEase(current_easetype(easetype));
        //m_button.transform.DOLocalMoveY(to_y, duration).SetEase(current_easetype(easetype));
        //button_text.transform.DOLocalMove(to_vector, duration).SetEase(current_easetype(easetype));
        //Debug.Log("pointer enter");


    }

    public void idle()
    {
        //button_text.text = debugtext;
        m_button.transform.DOLocalMoveX(0, duration).SetEase(current_easetype(easetype));
        m_button.transform.DOLocalMoveY(to_y, duration).SetEase(current_easetype(easetype));
        //button_text.transform.DOLocalMove(-to_vector, duration).SetEase(current_easetype(easetype));
        //Debug.Log("pointer exit"+debugtext);
        //yield return null;
    }
    // Start is called before the first frame update
    /*IEnumerator Start()
    {
        
    }*/
    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("pointerenter");
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("pointerexit");
    }

    public virtual void OnSelect(BaseEventData eventData)
    {
        //Execute(EventTriggerType.Select, eventData);
    }

    public virtual void OnDeselect(BaseEventData eventData)
    {
        //Execute(EventTriggerType.Deselect, eventData);
    }
    // Update is called once per frame
    void Update()
    {
        //current_easetype = Ease.(easetype.ToString);
        to_y = m_button.transform.localPosition.y;
        Debug.Log(easetype.ToString()+ (int)easetype);
        //button_text.transform.DoMove(new Vector3(10,0,0),0.5f);
        //OnMouseEnter();
    }

    private Ease current_easetype (Easetype easetype)
    {
        switch (easetype)
        {
            case Easetype.Unset:
                return Ease.Unset;

            case Easetype.Linear :
                return Ease.Linear;

            case Easetype.InSine:
                return Ease.InSine;

            case Easetype.OutSine:
                return Ease.OutSine;

            case Easetype.InOutSine:
                return Ease.InOutSine;

            case Easetype.InQuad:
                return Ease.InQuad;

            case Easetype.OutQuad:
                return Ease.OutQuad;

            case Easetype.InOutQuad:
                return Ease.InOutQuad;

            case Easetype.InCubic:
                return Ease.InCubic;

            case Easetype.OutCubic:
                return Ease.OutCubic;

            case Easetype.InOutCubic:
                return Ease.InOutCubic;

            case Easetype.InQuart:
                return Ease.InQuart;

            case Easetype.OutQuart:
                return Ease.OutQuart;

            case Easetype.InOutQuart:
                return Ease.InOutQuart;

            case Easetype.InQuint:
                return Ease.InQuint;

            case Easetype.OutQuint:
                return Ease.OutQuint;

            case Easetype.InOutQuint:
                return Ease.InOutQuint;

            case Easetype.InExpo:
                return Ease.InExpo;

            case Easetype.OutExpo:
                return Ease.OutExpo;

            case Easetype.InOutExpo:
                return Ease.InOutExpo;

            case Easetype.InCirc:
                return Ease.InCirc;

            case Easetype.OutCirc:
                return Ease.OutCirc;

            case Easetype.InOutCirc:
                return Ease.InOutCirc;

            case Easetype.InElastic:
                return Ease.InElastic;

            case Easetype.OutElastic:
                return Ease.OutElastic;

            case Easetype.InOutElastic:
                return Ease.InOutElastic;

            case Easetype.InBack:
                return Ease.InBack;

            case Easetype.OutBack:
                return Ease.OutBack;

            case Easetype.InOutBack:
                return Ease.InOutBack;

            case Easetype.InBounce:
                return Ease.InBounce;

            case Easetype.OutBounce:
                return Ease.OutBounce;

            case Easetype.InOutBounce:
                return Ease.InOutBounce;

            case Easetype.Flash:
                return Ease.Flash;

            case Easetype.InFlash:
                return Ease.InFlash;

            case Easetype.OutFlash:
                return Ease.OutFlash;

            case Easetype.InOutFlash:
                return Ease.InOutFlash;

            default:
                return Ease.Unset;

        }
    }
}
