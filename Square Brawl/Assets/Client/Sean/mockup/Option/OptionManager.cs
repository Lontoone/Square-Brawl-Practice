using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionManager : MonoBehaviour
{
    [HideInInspector]
    public static int m_CurrentIndex = 99;
    [HideInInspector]
    public static bool m_ResetTrigger = false;

    public GameObject[] m_OptionGroup;
    public void SetSelectedIndex(int Index)
    {
        m_CurrentIndex = Index;
        ResetDeselected();
        Debug.Log(m_CurrentIndex);
    }

    private void OnDisable()
    {
        m_CurrentIndex = 99;
    }

    public void ResetDeselected()//todo
    {
        for (int i = 0; i < m_OptionGroup.Length; i++)
        {
            if (i == m_CurrentIndex)
            {
                m_OptionGroup[i].GetComponentInChildren<SettingGroupPrefabManager>().SettingIn();
            }
            else
            {
                m_OptionGroup[i].GetComponentInChildren<SettingGroupPrefabManager>().SettingOut();
                m_OptionGroup[i].GetComponentInChildren<ButtonAction>().SplitCharAction.IdleChar(m_OptionGroup[i].GetComponentInChildren<ButtonAction>().m_Char);
                m_OptionGroup[i].GetComponentInChildren<ButtonAction>().IdleIcon();
                m_OptionGroup[i].GetComponentInChildren<ButtonAction>().m_MouseSelectedState = false;
                m_OptionGroup[i].GetComponentInChildren<ButtonAction>().m_KeySelectedState = false;
            }
        }
    }
}
