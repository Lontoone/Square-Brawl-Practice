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

public class SettingGroupPrefabManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    [HideInInspector]
    public int SelectedIndex;
    [SerializeField] private GameObject m_Setting;
    [SerializeField] private GameObject[] m_SettingTextList;
    private Easetype.Current_easetype m_CurrentEasetype;
    [Space (15)]
    [SerializeField] private Easetype.Current_easetype.Easetype m_Easetype;
    [SerializeField] private float ToX;
    [SerializeField] private float m_duration;
    private Sequence m_FadeSequence;
    private Vector3[] pos;

    void Start()
    {
        m_CurrentEasetype = new Easetype.Current_easetype();
        pos = new Vector3[m_SettingTextList.Length];
        for (int i = 0; i < m_SettingTextList.Length; i++)
        {
            pos[i] = new Vector3(-m_SettingTextList[i].GetComponent<RectTransform>().sizeDelta.x, m_SettingTextList[i].transform.localPosition.y);
            m_SettingTextList[i].transform.localPosition = pos[i];
        }
    }

    public void SettingFadeIn()
    {
        m_FadeSequence.Kill();
        m_FadeSequence = DOTween.Sequence();
        
        for (int i = 0; i < m_SettingTextList.Length; i++)
        {
            m_FadeSequence.Append(m_SettingTextList[i].transform
                            .DOLocalMoveX(pos[i].x + ToX, m_duration)
                            .SetEase(m_CurrentEasetype.GetEasetype(m_Easetype)));
        }
    }

    public void SettingFadeOut()
    {
        m_FadeSequence.Kill();
        m_FadeSequence = DOTween.Sequence();

        for (int i = 0; i < m_SettingTextList.Length; i++)
        {
            m_FadeSequence.Append(m_SettingTextList[i].transform
                            .DOLocalMoveX(pos[i].x + ToX * 2, m_duration)
                            .SetEase(m_CurrentEasetype.GetEasetype(m_Easetype)));
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

    }
    public void OnPointerExit(PointerEventData eventData)
    {

    }

    public virtual void OnSelect(BaseEventData eventData)
    {

        
    }

    public virtual void OnDeselect(BaseEventData eventData)
    {

    }
}

