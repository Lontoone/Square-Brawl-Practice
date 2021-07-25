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
    [SerializeField] private GameObject m_SettingPrefab;
    [SerializeField] private GameObject[] m_SettingList;
    private Easetype.Current_easetype m_CurrentEasetype;

    [HideInInspector]
    public enum AnimationType {Flash, Fade}
    [Space(15)]
    [SerializeField] private AnimationType type;
    [SerializeField] private Easetype.Current_easetype.Easetype m_Easetype;
    [SerializeField] private float ToX;
    [SerializeField] private float m_duration;
    private Sequence m_Sequence;
    private Vector3[] pos;
    private bool onSelect;

    void Start()
    {
        m_CurrentEasetype = new Easetype.Current_easetype();
        m_Sequence = DOTween.Sequence();

        switch (type)
        {
            case AnimationType.Flash:
                pos = new Vector3[m_SettingList.Length];
                for (int i = 0; i < m_SettingList.Length; i++)
                {
                    pos[i] = new Vector3(-m_SettingList[i].GetComponent<RectTransform>().sizeDelta.x, m_SettingList[i].transform.localPosition.y);
                    m_SettingList[i].transform.localPosition = pos[i];
                }
                break;

            case AnimationType.Fade:

                break;
        }
    }

    public void SettingIn()
    {
        switch (type)
        {
            case AnimationType.Flash:

                onSelect = true;
                m_Sequence.Kill();

                for (int i = 0; i < m_SettingList.Length; i++)
                {
                    pos[i] = new Vector3(-m_SettingList[i].GetComponent<RectTransform>().sizeDelta.x, m_SettingList[i].transform.localPosition.y);
                    m_SettingList[i].transform.localPosition = pos[i];
                }

                for (int i = 0; i < m_SettingList.Length; i++)
                {
                    m_Sequence.Append(m_SettingList[i].transform
                                    .DOLocalMoveX(pos[i].x + ToX, m_duration)
                                    .SetEase(m_CurrentEasetype.GetEasetype(m_Easetype)));
                }
                break;

            case AnimationType.Fade:

                /*onSelect = true;
                m_Sequence.Kill();

                for (int i = 0; i < m_SettingList.Length; i++)
                {
                    m_Sequence.Append(m_SettingList[i]
                                    .DOFade()
                                    .SetEase(m_CurrentEasetype.GetEasetype(m_Easetype)));
                }*/

                break;
        }
    }

    public void SettingOut()
    {
        switch (type)
        {
            case AnimationType.Flash:
                if (onSelect == true)
                {
                    m_Sequence.Kill();

                    for (int i = 0; i < m_SettingList.Length; i++)
                    {
                        m_Sequence.Append(m_SettingList[i].transform
                                        .DOLocalMoveX(pos[i].x + ToX * 2, m_duration)
                                        .SetEase(m_CurrentEasetype.GetEasetype(m_Easetype)));

                    }
                    onSelect = false;
                }
                break;

            case AnimationType.Fade:

                break;
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

