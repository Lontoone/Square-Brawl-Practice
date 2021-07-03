using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class Button_2_test : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    [TextArea(3,5)]
    [SerializeField] private string text;
    [SerializeField] private Transform button;
    private TMP_Text button_text;
    private string debugtext;


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
        //button_text.ForceMeshUpdate(true);
        button = GetComponent<TMP_Text>().transform;
        button_text = GetComponent<TMP_Text>();
        debugtext = GetComponent<TMP_Text>().text;
        //button_text.text = text;
    }

    public void highlighted()
    {
        button_text.text = text;
        button_text.transform.DOLocalMoveX(100,0.1f);
        //Debug.Log("pointer enter");
        //yield return null;
    }

    public void idle()
    {
        button_text.text = debugtext;
        button_text.transform.DOLocalMoveX(-100, 0.1f);
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
        //button_text.transform.DoMove(new Vector3(10,0,0),0.5f);
        //OnMouseEnter();
    }
}
