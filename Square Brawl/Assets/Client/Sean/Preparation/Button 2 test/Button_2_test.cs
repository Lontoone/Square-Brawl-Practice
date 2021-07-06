﻿using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using BasicTools.ButtonInspector;
using DG.Tweening;
using TMPro;

public class Button_2_test : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{

    [SerializeField] private GameObject m_button_text;
    [Space(10)]
    [Button("Set original position", "setf_pos")]
    [SerializeField] private bool set_pos;
    [Space(10)]
    [SerializeField] private float to_x;
    [SerializeField] private float to_y;
    private Vector3 pos;

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
    [Space(10)]
    [SerializeField] private Easetype easetype;
    [SerializeField] private float duration = 1f;

    private Coroutine _c_idle;

    void Awake()
    {

    }

    void Start()
    {
        pos = m_button_text.transform.localPosition;
        Debug.Log("pos" + pos);
    }

    void Update()
    {
        /*
        Debug.Log(m_button_text.name+": \n\tX: "+pos.x +", Y: "+pos.y);
        Debug.Log(easetype.ToString() + (int)easetype);
        */
    }

    public void setf_pos()
    {
        pos = m_button_text.transform.localPosition;
    }
    public void highlighted()
    {
        Debug.Log("m_button_text.transform.position" + m_button_text.transform.position);
        DOTween.To(() => m_button_text.transform.localPosition, x => m_button_text.transform.localPosition = x, new Vector3(pos.x + to_x, pos.y + to_y), duration); //lambda
    }

    public IEnumerator idle()
    {
        //DOTween.To(() => m_button_text.transform.localPosition, x => m_button_text.transform.localPosition = x, new Vector3(pos.x, pos.y), duration);
        DOTween.To(() => m_button_text.transform.localPosition, x => m_button_text.transform.localPosition = x, new Vector3(pos.x + to_x * 5, pos.y + to_y * 5), duration);
        yield return new WaitForSeconds(duration * 2);
        m_button_text.transform.localPosition = pos;

        _c_idle = null;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        highlighted();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (_c_idle == null)
        {
            _c_idle = StartCoroutine(idle());
        }
    }

    public virtual void OnSelect(BaseEventData eventData)
    {
        highlighted();
    }

    public virtual void OnDeselect(BaseEventData eventData)
    {
        idle();
    }

    private Ease current_easetype(Easetype easetype)
    {
        switch (easetype)
        {
            case Easetype.Unset:
                return Ease.Unset;

            case Easetype.Linear:
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