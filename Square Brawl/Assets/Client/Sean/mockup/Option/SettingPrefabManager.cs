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
namespace ToDotSlider { }

public class SettingPrefabManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    [SerializeField] public int m_Index;
    [SerializeField] private Button m_Header;
    [SerializeField] private GameObject m_Dot;
    
    public enum SettingType {ListSelection, SliderSelection}
    [Space (15)]
    [SerializeField] private SettingType m_SettingType;
    [Serializable]
    public struct ListSelection
    {
        [SerializeField] public GameObject m_LeftButton;
        [SerializeField] public GameObject m_RightButton;
        [SerializeField] public TextMeshProUGUI[] m_SelectionList;
        [SerializeField] public int m_DefaultElement;
        [SerializeField] public Easetype.Current_easetype.Easetype m_easetype;
        [SerializeField] public float m_duration;
    }
    [Space(20)]
    [SerializeField] private GameObject m_ListParent;
    public ListSelection m_ListSelection;

    private Sequence m_SettingSequence;
    private Easetype.Current_easetype m_current_easetype;
    private int m_CurrentIndex = 0;

    [Space(15)]
    [SerializeField] private GameObject m_SliderParent;
    public ToDotSlider.DotSliderAction.DotSlider m_SliderSetting;

    public void Start()
    {

        m_current_easetype = new Easetype.Current_easetype();
        switch (m_SettingType)
        {
            case SettingType.ListSelection:
                SetUpListSelection(m_ListSelection);
                break;

            case SettingType.SliderSelection:
                m_ListParent.SetActive(false);
                m_ListSelection.m_LeftButton.SetActive(false);
                m_ListSelection.m_RightButton.SetActive(false);
                m_ListSelection.m_SelectionList[m_ListSelection.m_DefaultElement].enabled = false;


                m_SliderParent.AddComponent<ToDotSlider.DotSliderAction>();
                m_SliderSetting = m_SliderParent.GetComponent<ToDotSlider.DotSliderAction>().SetUp(m_SliderSetting);
                break;

            default:
                break;
        }
    }

    private void Update()
    {
        if (m_SettingType == SettingType.SliderSelection && m_SliderSetting.onSelect == true)
        {
            m_CurrentIndex = m_SliderParent.GetComponent<ToDotSlider.DotSliderAction>().OnLoad(m_SliderSetting);
            ReturnSettingValue();
        }
    }

    public int ReturnSettingValue()
    {
        //Debug.Log(m_CurrentIndex);
        return m_CurrentIndex;
    }

    public void SetUpListSelection(ListSelection ListSelection)
    {
        m_CurrentIndex = ListSelection.m_DefaultElement;
        for (int i = 0; i < ListSelection.m_SelectionList.Length; i++)
        {
            if (i < ListSelection.m_DefaultElement)
            {
                ListSelection.m_SelectionList[i].transform.localPosition = new Vector3(-ListSelection.m_SelectionList[i].rectTransform.sizeDelta.x, 0, 0);
            }
            else if (i == ListSelection.m_DefaultElement)
            {
            }
            else if (i > ListSelection.m_DefaultElement)
            {
                ListSelection.m_SelectionList[i].transform.localPosition = new Vector3(ListSelection.m_SelectionList[i].rectTransform.sizeDelta.x, 0, 0);
            }
        }
    }
    public void LeftOnClick()
    {
        DecreaseIndex(m_ListSelection);
    }

    public void RightOnClick()
    {
        IncreaseIndex(m_ListSelection);
    }

    public void IncreaseIndex(ListSelection ListSelection)
    {
        OnClickAnimation(ListSelection.m_RightButton);
        if (m_CurrentIndex < ListSelection.m_SelectionList.Length-1)
        {
            m_SettingSequence.Kill();
            m_SettingSequence = DOTween.Sequence();
            m_SettingSequence.Append(ListSelection.m_SelectionList[m_CurrentIndex].transform
                                .DOLocalMoveX(-ListSelection.m_SelectionList[m_CurrentIndex].rectTransform.sizeDelta.x, ListSelection.m_duration))
                                .SetEase(m_current_easetype.GetEasetype(ListSelection.m_easetype));
            
            m_CurrentIndex++;
            m_SettingSequence.Append(ListSelection.m_SelectionList[m_CurrentIndex].transform
                                .DOLocalMoveX(0, ListSelection.m_duration))
                                .SetEase(m_current_easetype.GetEasetype(ListSelection.m_easetype));
        }
        ReturnSettingValue();
    }

    public void DecreaseIndex(ListSelection ListSelection)
    {
        OnClickAnimation(ListSelection.m_LeftButton);
        if (m_CurrentIndex > 0)
        {
            m_SettingSequence.Kill();
            m_SettingSequence = DOTween.Sequence();
            m_SettingSequence.Append(ListSelection.m_SelectionList[m_CurrentIndex].transform
                                .DOLocalMoveX(ListSelection.m_SelectionList[m_CurrentIndex].rectTransform.sizeDelta.x, ListSelection.m_duration))
                                .SetEase(m_current_easetype.GetEasetype(ListSelection.m_easetype));
            m_CurrentIndex--;
            m_SettingSequence.Append(ListSelection.m_SelectionList[m_CurrentIndex].transform
                                .DOLocalMoveX(0, ListSelection.m_duration))
                                .SetEase(m_current_easetype.GetEasetype(ListSelection.m_easetype));
        }
        ReturnSettingValue();

    }

    public void ColorIn(GameObject gameObject)
    {
        gameObject.GetComponent<Image>().DOColor(SceneHandler.green, 0.3f).SetEase(Ease.OutCirc);
    }

    public void ColorOut(GameObject gameObject)
    {
        gameObject.GetComponent<Image>().DOColor(new Color32(230, 230, 230, 0), 0.3f).SetEase(Ease.OutCirc);
    }

    public void OnClickAnimation(GameObject gameObject) //todo
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
        m_SliderSetting.onSelect = true;
        ColorIn(m_Dot);
    }

    public virtual void OnDeselect(BaseEventData eventData)
    {
        m_SliderSetting.onSelect = false;
        ColorOut(m_Dot);
    }
}

