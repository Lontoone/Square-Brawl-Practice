using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour
{
    [SerializeField] private GameObject m_OptionGroup;
    [HideInInspector]
    public static int m_CurrentIndex = 99;
    [HideInInspector]
    public static bool m_ResetTrigger = false;
    [SerializeField] private float spacing;
    public static float m_Spacing;

    [Space(15)]
    public GameObject[] m_SettingGroup;
    private Vector3[] m_SettingGroupPos;


    private void Start()
    {
        m_Spacing = spacing;
        m_SettingGroupPos = new Vector3[m_SettingGroup.Length];
        for (int i = 0; i < m_SettingGroup.Length; i++)
        {
            m_SettingGroupPos[i] = m_SettingGroup[i].transform.position;
        }
    }
    private void Update()
    {
        for (int i = 0; i < m_SettingGroup.Length; i++)
        {
            if (m_SettingGroup[i].GetComponent<SettingGroupPrefabManager>().onSelect == true)
            {
                Debug.Log("in");
                m_SettingGroup[i].GetComponent<SettingGroupPrefabManager>().Selected();
            }
            else
            {
                m_SettingGroup[i].GetComponent<SettingGroupPrefabManager>().DeSelected();
            }
        }

    }

    public void SetSelectedIndex(int Index)
    {
        m_CurrentIndex = Index;
        ResetDeselected();
        Debug.Log(m_CurrentIndex);
    }

    public void Test()
    {
        
    }

    private void OnDisable()
    {
        m_CurrentIndex = 99;
    }

    public void ResetDeselected()//todo
    {
        for (int i = 0; i < m_SettingGroup.Length; i++)
        {
            if (i == m_CurrentIndex)
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
