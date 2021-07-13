using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionManager : MonoBehaviour
{
    [HideInInspector]
    public static int m_CurrentIndex = 99;
    [HideInInspector]
    public static bool m_ResetTrigger = false;

    public GameObject[] m_Option;
    public void SetSelectedIndex(int Index)
    {
        m_CurrentIndex = Index;
        ResetDeselected();
        Debug.Log(m_CurrentIndex);
    }

    public void ResetDeselected()//todo
    {
        for (int i = 0; i < m_Option.Length; i++)
        {
            if (i == m_CurrentIndex)
            {

            }
            else
            {
                m_Option[i].GetComponent<ButtonAction>().SplitCharAction.IdleChar(m_Option[i].GetComponent<ButtonAction>().m_Char);
                m_Option[i].GetComponent<ButtonAction>().IdleIcon();
                m_Option[i].GetComponent<ButtonAction>().m_MouseSelectedState = false;
                m_Option[i].GetComponent<ButtonAction>().m_KeySelectedState = false;
            }
        }
    }
}
