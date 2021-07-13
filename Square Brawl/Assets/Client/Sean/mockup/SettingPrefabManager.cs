using System.Collections;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;
using BasicTools.ButtonInspector;
using DG.Tweening;
using TMPro;
namespace Easetype { }

public class SettingPrefabManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    [SerializeField] private Button m_Header;
    //[SerializeField] private TextMeshProUGUI m_PropertyReference;
    [SerializeField] private GameObject m_Dot;
    
    [SerializeField] private GameObject m_LeftButton;
    [SerializeField] private GameObject m_RightButton;
    [Space (10)]
    [SerializeField] private int m_DefaultIndex;
    [SerializeField] private TextMeshProUGUI[] m_SettingSelectionList;

    private int m_CurrentIndex = 0;

    [Space(10)]
    [HeaderAttribute("Setting Option")]
    [SerializeField] Easetype.Current_easetype.Easetype m_easetype;
    private Easetype.Current_easetype m_current_easetype;
    [SerializeField] float m_duration = 1f;
    //[SerializeField] private float m_outdistance = 500;
    [SerializeField] private Sequence m_SettingSequence;


    public void Start()
    {
        
        m_current_easetype = new Easetype.Current_easetype();
        m_CurrentIndex = m_DefaultIndex-1;
        for (int i = 0;i < m_SettingSelectionList.Length ;i++ )
        {
            if (i < m_DefaultIndex-1)
            {
                m_SettingSelectionList[i].transform.localPosition = new Vector3(-m_SettingSelectionList[i].rectTransform.sizeDelta.x, 0, 0);
            }
            else if (i == m_DefaultIndex-1)
            {
            }
            else if (i > m_DefaultIndex-1)
            {
                m_SettingSelectionList[i].transform.localPosition = new Vector3(m_SettingSelectionList[i].rectTransform.sizeDelta.x, 0, 0);
            }
        }
        //Debug.Log(m_SettingOptionList[0].text + "\t" + m_SettingOptionList[0] + "\t" + m_SettingOptionList[0].rectTransform.sizeDelta);
    }
    public void ChangeCurrentSettingOption()
    {
        Debug.Log("ChangeCurrentSettingOption");
        
    }

    public void IncreaseIndex()
    {
        OnClickAnimation(m_RightButton);
        if (m_CurrentIndex < m_SettingSelectionList.Length-1)
        {
            m_SettingSequence.Kill();
            m_SettingSequence = DOTween.Sequence();
            m_SettingSequence.Append(m_SettingSelectionList[m_CurrentIndex].transform
                                .DOLocalMoveX(-m_SettingSelectionList[m_CurrentIndex].rectTransform.sizeDelta.x, m_duration))
                                .SetEase(m_current_easetype.GetEasetype(m_easetype));
            
            m_CurrentIndex++;
            m_SettingSequence.Append(m_SettingSelectionList[m_CurrentIndex].transform
                                .DOLocalMoveX(0, m_duration))
                                .SetEase(m_current_easetype.GetEasetype(m_easetype));
        }
        ChangeCurrentSettingOption();

    }

    public void DecreaseIndex()
    {
        OnClickAnimation(m_LeftButton);
        if (m_CurrentIndex > 0)
        {
            m_SettingSequence.Kill();
            m_SettingSequence = DOTween.Sequence();
            m_SettingSequence.Append(m_SettingSelectionList[m_CurrentIndex].transform
                                .DOLocalMoveX(m_SettingSelectionList[m_CurrentIndex].rectTransform.sizeDelta.x, m_duration))
                                .SetEase(m_current_easetype.GetEasetype(m_easetype));
            m_CurrentIndex--;
            m_SettingSequence.Append(m_SettingSelectionList[m_CurrentIndex].transform
                                .DOLocalMoveX(0, m_duration))
                                .SetEase(m_current_easetype.GetEasetype(m_easetype));
        }
        ChangeCurrentSettingOption();

    }
    public void ColorIn(GameObject gameObject)
    {
        gameObject.GetComponent<Image>().DOColor(Scenemanager.green, m_duration).SetEase(m_current_easetype.GetEasetype(m_easetype));
    }

    public void ColorOut(GameObject gameObject)
    {
        gameObject.GetComponent<Image>().DOColor(new Color32(230, 230, 230, 0), m_duration).SetEase(m_current_easetype.GetEasetype(m_easetype));
    }

    public void OnClickAnimation(GameObject gameObject)
    {
        Debug.Log("OnclickAnimation");
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        ColorIn(m_Dot);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        ColorOut(m_Dot);
    }

    public virtual void OnSelect(BaseEventData eventData)
    {
        ColorIn(m_Dot);
    }

    public virtual void OnDeselect(BaseEventData eventData)
    {
        ColorOut(m_Dot);
    }
}

