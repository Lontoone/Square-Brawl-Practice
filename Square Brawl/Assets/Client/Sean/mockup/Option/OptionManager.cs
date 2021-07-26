using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Easetype { }

public class OptionManager : MonoBehaviour
{
    [SerializeField] private GameObject m_OptionGroup;
    [HideInInspector]
    public static int m_PressIndex = 99;
    [HideInInspector]
    public static bool m_ResetTrigger = false;
    [SerializeField] private float spacing;
    public static float m_Spacing;
    [HideInInspector]
    public Easetype.Current_easetype m_CurrentEasetype;
    [SerializeField] private Easetype.Current_easetype.Easetype m_Easetype;
    [SerializeField] private float m_Duration;

    [Space(15)]
    public GameObject[] m_SettingGroup;
    private Vector3[] m_SettingGroupPos;
    private int onSelectIndex = 99;


    private void Start()
    {
        m_CurrentEasetype = new Easetype.Current_easetype();

        m_Spacing = spacing;
        m_SettingGroupPos = new Vector3[m_SettingGroup.Length];
        for (int i = 0; i < m_SettingGroup.Length; i++)
        {
            m_SettingGroupPos[i] = m_SettingGroup[i].transform.localPosition;
            Debug.Log(i + "\t" + m_SettingGroupPos[i]);
        }
    }
    private void Update()
    {
        for (int i = 0; i < m_SettingGroup.Length; i++)
        {
            if (m_SettingGroup[i].GetComponent<SettingGroupPrefabManager>().onSelect == true)
            {
                onSelectIndex = i;
            }
        }

        for (int i = 0; i < m_SettingGroup.Length; i++)
        {
            if (onSelectIndex != 99)
            {
                if (i <= onSelectIndex)
                {
                    m_SettingGroup[i].transform.DOLocalMove(m_SettingGroupPos[i], m_Duration).SetEase(m_CurrentEasetype.GetEasetype(m_Easetype));
                }
                else if (i > onSelectIndex)
                {
                    m_SettingGroup[i].transform.DOLocalMove(new Vector3(m_SettingGroupPos[i].x + m_Spacing,m_SettingGroupPos[i].y), m_Duration).SetEase(m_CurrentEasetype.GetEasetype(m_Easetype));
                }
            }
            

            /*if (m_SettingGroup[i].GetComponent<SettingGroupPrefabManager>().onSelect == true)
            {
                Debug.Log("in");
                m_SettingGroup[i].GetComponent<SettingGroupPrefabManager>().Selected();
            }
            else
            {
                m_SettingGroup[i].GetComponent<SettingGroupPrefabManager>().DeSelected();
            }*/
        }

    }

    public void SetSelectedIndex(int Index)
    {
        m_PressIndex = Index;
        ResetDeselected();
        Debug.Log(m_PressIndex);
    }

    public void Test()
    {
        
    }

    private void OnDisable()
    {
        m_PressIndex = 99;
        onSelectIndex = 99;
    }

    public void ResetDeselected()//todo
    {
        for (int i = 0; i < m_SettingGroup.Length; i++)
        {
            if (i == m_PressIndex)
            {
                m_SettingGroup[i].GetComponentInChildren<SettingGroupPrefabManager>().SettingIn();
            }
            else
            {
                m_SettingGroup[i].GetComponentInChildren<SettingGroupPrefabManager>().SettingOut();
                m_SettingGroup[i].GetComponentInChildren<ButtonAction>().SplitCharAction.IdleChar(m_SettingGroup[i].GetComponentInChildren<ButtonAction>().m_Char);
                m_SettingGroup[i].GetComponentInChildren<ButtonAction>().IdleIcon();
                m_SettingGroup[i].GetComponentInChildren<ButtonAction>().m_MouseSelectedState = false;
                m_SettingGroup[i].GetComponentInChildren<ButtonAction>().m_KeySelectedState = false;
            }
        }
    }
}
