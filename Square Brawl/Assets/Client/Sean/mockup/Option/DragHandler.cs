using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector]
    public int m_Indexx;
    public GameObject m_SelectedDot;
    public float m_Min;
    public float m_Max;
    public Color32 m_Color;
    public Color32 m_DefaultColor;
    public float m_Duration = 0.3f;
    public int m_Series;
    public int m_Index;

    private Sequence m_Dotsequence;
    private bool m_OnDrag = false;

    public void SetUp(GameObject selectedDot, Color32 color, Color32 defaultColor, int series, int defaultSelected)
    {
        m_SelectedDot = selectedDot;
        m_Color = color;
        m_DefaultColor = defaultColor;
        m_Series = series;
        m_Index = defaultSelected;
        m_Dotsequence = DOTween.Sequence();
    }

    public void UpdateSetUp(GameObject min, GameObject max)
    {
        m_Min = min.transform.position.x;
        m_Max = max.transform.position.x;
    }
    private void SetSelected()
    {
        m_Index = (int)((m_SelectedDot.transform.position.x - m_Min) / ((m_Max - m_Min) / (m_Series - 1)));
        //Debug.Log((m_SelectedDot.transform.position.x - m_Min) % ((m_Max - m_Min) / (m_Series - 1)) + "\t" + ((m_Max - m_Min) / ((m_Series - 1) * 2)));
        if ((m_SelectedDot.transform.position.x - m_Min) % ((m_Max - m_Min) / (m_Series - 1)) > ((m_Max - m_Min) / ((m_Series - 1) * 2)))
        {
            m_Index++;
        }
        if (m_Index == m_Series)
        {
            m_Index--;
        }
            GetComponentInParent<SettingPrefabManager>().m_SliderSetting.m_SelectedIndex = m_Index;
    }

        public void OnPointerEnter(PointerEventData eventData)
    {
        m_Dotsequence.Kill();
        m_Dotsequence.Append(m_SelectedDot.transform
                        .DOScale(new Vector3(1.3f, 1.3f), m_Duration)
                        .SetEase(Ease.OutBounce))
                     .Join(m_SelectedDot.GetComponent<Image>()
                        .DOColor(m_Color, m_Duration)
                        .SetEase(Ease.InOutCirc));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!m_OnDrag)
        {
            m_Dotsequence.Kill();
            m_Dotsequence.Append(m_SelectedDot.transform
                            .DOScale(new Vector3(1f, 1f), m_Duration)
                            .SetEase(Ease.OutBounce))
                         .Join(m_SelectedDot.GetComponent<Image>()
                            .DOColor(m_DefaultColor, m_Duration)
                            .SetEase(Ease.InOutCirc));
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        m_SelectedDot.transform.position = new Vector3((m_Max - m_Min) / 2, m_SelectedDot.transform.position.y);
        m_OnDrag = true;
        GetComponentInParent<SettingPrefabManager>().m_SliderSetting.onSelect = true;
        GetComponentInParent<SettingPrefabManager>().m_SliderSetting.m_IsChangebyDrag = true;
        m_SelectedDot.transform.localScale = new Vector3(1.3f, 1.3f);
        m_SelectedDot.GetComponent<CanvasGroup>().blocksRaycasts = false;
        m_Dotsequence.Kill();
        m_Dotsequence.Append(m_SelectedDot.GetComponent<Image>().transform
                        .DOScale(new Vector3(1.5f, 1.5f),0))
                     .Join(m_SelectedDot.GetComponent<Image>()
                        .DOColor(m_Color, m_Duration)
                        .SetEase(Ease.InOutCirc));
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Set Max and Min 
        m_Min = m_SelectedDot.GetComponentInParent<SettingPrefabManager>().m_SliderSetting.m_DotSlider[0].transform.position.x;
        m_Max = m_SelectedDot.GetComponentInParent<SettingPrefabManager>().m_SliderSetting.m_DotSlider[m_SelectedDot.GetComponentInParent<SettingPrefabManager>().m_SliderSetting.m_DotSlider.Length - 1].transform.position.x;


        m_Dotsequence.Append(m_SelectedDot.GetComponent<Image>().transform
                        .DOScale(new Vector3(1.3f, 1.3f), 0));
        if (m_Min < eventData.position.x && eventData.position.x < m_Max)
        {
            m_SelectedDot.transform.position = new Vector3(eventData.position.x, m_SelectedDot.transform.position.y);
        }
        else if (m_Min > eventData.position.x)
        {
            m_SelectedDot.transform.position = new Vector3(m_Min, m_SelectedDot.transform.position.y);
        }
        else if (eventData.position.x > m_Max)
        {
            m_SelectedDot.transform.position = new Vector3(m_Max, m_SelectedDot.transform.position.y);
        }
        SetSelected();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        m_SelectedDot.transform.DOMoveX(((m_Max - m_Min) / (m_Series - 1)) * m_Index + m_Min,m_Duration).SetEase(Ease.OutBounce);
        m_SelectedDot.GetComponent<CanvasGroup>().blocksRaycasts = true;
        m_Dotsequence.Kill();
        m_Dotsequence.Append(m_SelectedDot.GetComponent<Image>().transform
                        .DOScale(new Vector3(1f, 1f), 0));
        GetComponentInParent<SettingPrefabManager>().m_SliderSetting.m_IsChangebyDrag = false;
        GetComponentInParent<SettingPrefabManager>().m_SliderSetting.onSelect = false;
        SetSelected();
        StartCoroutine(OnDragTriggerFalse());
        
    }

    private IEnumerator OnDragTriggerFalse()
    {
        yield return null;
        m_OnDrag = false;
    }
}
