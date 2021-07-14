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
        [SerializeField] public int m_DefaultIndex;
        [SerializeField] public Easetype.Current_easetype.Easetype m_easetype;
        [SerializeField] public float m_duration;
    }

    public ListSelection listSelection;

    private Sequence m_SettingSequence;
    private Easetype.Current_easetype m_current_easetype;
    private int m_CurrentIndex = 0;

    [SerializeField] private Slider m_slider;

    public void Start()
    {

        m_current_easetype = new Easetype.Current_easetype();
        switch (m_SettingType)
        {
            case SettingType.ListSelection:
                SetUpListSelection(listSelection);
                break;

            case SettingType.SliderSelection:
                listSelection.m_LeftButton.SetActive(false);
                listSelection.m_RightButton.SetActive(false);
                listSelection.m_SelectionList[listSelection.m_DefaultIndex].enabled = false;
                break;

            default:
                break;
        }
        //Debug.Log(m_SettingOptionList[0].text + "\t" + m_SettingOptionList[0] + "\t" + m_SettingOptionList[0].rectTransform.sizeDelta);
    }
    public void ChangeCurrentSettingOption()
    {
        Debug.Log("ChangeCurrentSettingOption");
        
    }

    public void SetUpListSelection(ListSelection ListSelection)
    {
        m_CurrentIndex = ListSelection.m_DefaultIndex;
        for (int i = 0; i < ListSelection.m_SelectionList.Length; i++)
        {
            if (i < ListSelection.m_DefaultIndex)
            {
                ListSelection.m_SelectionList[i].transform.localPosition = new Vector3(-ListSelection.m_SelectionList[i].rectTransform.sizeDelta.x, 0, 0);
            }
            else if (i == ListSelection.m_DefaultIndex)
            {
            }
            else if (i > ListSelection.m_DefaultIndex)
            {
                ListSelection.m_SelectionList[i].transform.localPosition = new Vector3(ListSelection.m_SelectionList[i].rectTransform.sizeDelta.x, 0, 0);
            }
        }
    }
    public void LeftOnClick()
    {
        DecreaseIndex(listSelection);
    }

    public void RightOnClick()
    {
        IncreaseIndex(listSelection);
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
        ChangeCurrentSettingOption();

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
        ChangeCurrentSettingOption();

    }
    public void ColorIn(GameObject gameObject)
    {
        gameObject.GetComponent<Image>().DOColor(Scenemanager.green, 0.3f).SetEase(Ease.OutCirc);
    }

    public void ColorOut(GameObject gameObject)
    {
        gameObject.GetComponent<Image>().DOColor(new Color32(230, 230, 230, 0), 0.3f).SetEase(Ease.OutCirc);
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

