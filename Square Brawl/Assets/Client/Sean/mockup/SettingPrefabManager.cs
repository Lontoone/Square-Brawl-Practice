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

public class SettingPrefabManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    [SerializeField] private Button m_Header;
    //[SerializeField] private TextMeshProUGUI m_PropertyReference;
    [SerializeField] private GameObject m_Dot;
    [ColorUsage(true)]
    [SerializeField] private Color m_Color;
    [SerializeField] private GameObject m_LeftButton;
    [SerializeField] private GameObject m_RightButton;
    [Space (10)]
    [SerializeField] private int m_DefaultIndex;
    [SerializeField] private TextMeshProUGUI[] m_SettingOptionList;

    private int m_CurrentIndex = 0;

    [Space(10)]
    [HeaderAttribute("Setting Option")]
    [SerializeField] Easetype.Current_easetype.Easetype m_easetype;
    private Easetype.Current_easetype m_current_easetype;
    [SerializeField] float m_duration = 1f;
    [SerializeField] private float m_outdistance = 500;
    [SerializeField] private Sequence m_SettingSequence;


    public void Start()
    {
        
        m_current_easetype = new Easetype.Current_easetype();
        m_CurrentIndex = m_DefaultIndex-1;
        for (int i = 0;i < m_SettingOptionList.Length ;i++ )
        {
            if (i < m_DefaultIndex-1)
            {
                m_SettingOptionList[i].transform.localPosition = new Vector3(-m_outdistance, 0, 0);
            }
            else if (i == m_DefaultIndex-1)
            {
            }
            else if (i > m_DefaultIndex-1)
            {
                m_SettingOptionList[i].transform.localPosition = new Vector3(m_outdistance, 0, 0);
            }
        }
        //Debug.Log(m_SettingOptionList[0].bounds); //?
    }
    public void ChangeCurrentSettingOption()
    {
        Debug.Log("ChangeCurrentSettingOption");

    }

    public void IncreaseIndex()
    {
        if (m_CurrentIndex < m_SettingOptionList.Length-1)
        {
            m_SettingSequence.Kill();
            m_SettingSequence = DOTween.Sequence();
            m_SettingSequence.Append(m_SettingOptionList[m_CurrentIndex].transform
                                .DOLocalMoveX(-m_outdistance,m_duration))
                                .SetEase(m_current_easetype.GetEasetype(m_easetype));
            
            m_CurrentIndex++;
            m_SettingSequence.Append(m_SettingOptionList[m_CurrentIndex].transform
                                .DOLocalMoveX(0, m_duration))
                                .SetEase(m_current_easetype.GetEasetype(m_easetype));
        }
        ChangeCurrentSettingOption();

    }

    public void DecreaseIndex()
    {
        if (m_CurrentIndex > 0)
        {
            m_SettingSequence.Kill();
            m_SettingSequence = DOTween.Sequence();
            m_SettingSequence.Append(m_SettingOptionList[m_CurrentIndex].transform
                                .DOLocalMoveX(m_outdistance, m_duration))
                                .SetEase(m_current_easetype.GetEasetype(m_easetype));
            m_CurrentIndex--;
            m_SettingSequence.Append(m_SettingOptionList[m_CurrentIndex].transform
                                .DOLocalMoveX(0, m_duration))
                                .SetEase(m_current_easetype.GetEasetype(m_easetype));
        }
        ChangeCurrentSettingOption();

    }
    public void ColorIn(GameObject gameObject)
    {
        gameObject.GetComponent<Image>().DOColor(m_Color, m_duration).SetEase(m_current_easetype.GetEasetype(m_easetype));
    }

    public void ColorOut(GameObject gameObject)
    {
        gameObject.GetComponent<Image>().DOColor(new Color(0.9f, 0.9f, 0.9f, 1), m_duration).SetEase(m_current_easetype.GetEasetype(m_easetype));
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        m_Dot.GetComponent<Image>().DOColor(m_Color, m_duration).SetEase(m_current_easetype.GetEasetype(m_easetype));
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        m_Dot.GetComponent<Image>().DOColor(new Color(0.9f, 0.9f, 0.9f, 1), m_duration).SetEase(m_current_easetype.GetEasetype(m_easetype));
    }

    public virtual void OnSelect(BaseEventData eventData)
    {

    }

    public virtual void OnDeselect(BaseEventData eventData)
    {

    }
}
