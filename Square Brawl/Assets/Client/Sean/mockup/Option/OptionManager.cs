﻿using System.Collections;
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
    [SerializeField] private GameObject m_FirstPos;
    [SerializeField] private GameObject m_LastPos;
    [SerializeField] private float barLength;
    public enum LayoutType {OnSelect, OnPress, Off}
    [SerializeField] private LayoutType m_LayoutType;
    public static float m_BarLength;
    [SerializeField] private float m_MoveSpacing;
    private float m_Spacing;
    [HideInInspector]
    public Easetype.Current_easetype m_CurrentEasetype;

    [Space(15)]
    [SerializeField] private Easetype.Current_easetype.Easetype m_Easetype;
    [SerializeField] private float m_Duration;

    [Space(15)]
    public GameObject[] m_SettingGroup;
    private Vector3[] m_SettingGroupPos;
    private int onSelectIndex = 99;


    private void Start()
    {
        m_CurrentEasetype = new Easetype.Current_easetype();


        ///Set Layout
        m_BarLength = barLength;
        m_Spacing = ((m_LastPos.transform.localPosition.x - m_FirstPos.transform.localPosition.x) - m_BarLength) / (m_SettingGroup.Length - 1);

        m_SettingGroupPos = new Vector3[m_SettingGroup.Length];
        var spacing = (m_LastPos.transform.localPosition.x - m_FirstPos.transform.localPosition.x) / (m_SettingGroup.Length - 1);
        for (int i = 0; i < m_SettingGroup.Length; i++)
        {
            m_SettingGroupPos[i] = new Vector3(m_FirstPos.transform.localPosition.x + spacing * i, m_SettingGroup[i].transform.localPosition.y);
            m_SettingGroup[i].transform.localPosition = m_SettingGroupPos[i];
            Debug.Log(m_SettingGroupPos[i]);
        }
        ///
    }
    private void Update()
    {
        switch(m_LayoutType)
        { 
            case LayoutType.OnSelect:
                var trigger = 0;
                for (int i = 0; i < m_SettingGroup.Length; i++)
                {
                    if (m_SettingGroup[i].GetComponent<SettingGroupPrefabManager>().onSelect == true)
                    {
                        onSelectIndex = i;
                        trigger++;
                    }
                }
                if (trigger == 0 && m_PressIndex == 99)
                {
                    onSelectIndex = 99;
                    for (int i = 0; i < m_SettingGroup.Length; i++)
                    {
                        m_SettingGroup[i].transform.localPosition = m_SettingGroupPos[i];
                    }
                }

                if (onSelectIndex != 99)
                {
                    for (int i = 1; i < m_SettingGroup.Length; i++)
                    {
                        if (i - 1 == onSelectIndex)
                        {
                            m_SettingGroup[i].transform.localPosition = m_SettingGroup[i - 1].transform.localPosition + new Vector3(m_BarLength, 0);
                        }
                        else
                        {
                            m_SettingGroup[i].transform.localPosition = m_SettingGroup[i - 1].transform.localPosition + new Vector3(m_Spacing, 0);
                        }
                    }
                }
                break;

            case LayoutType.OnPress:
                if (m_PressIndex != 99)
                {
                    for (int i = 0; i < m_SettingGroup.Length; i++)
                    {
                        if (i + 1 == m_PressIndex)
                        {
                            m_SettingGroup[i].transform.DOLocalMoveX(m_SettingGroupPos[i].x - m_MoveSpacing, 0.5f);
                            if ((i - 1) >= 0)
                            {
                                m_SettingGroup[i - 1].transform.DOLocalMoveX(m_SettingGroupPos[i - 1].x - (m_MoveSpacing / 2), 0.5f);
                            }
                        }
                        else if (i == m_PressIndex)
                        {
                            m_SettingGroup[i].transform.DOLocalMoveX(m_SettingGroupPos[i].x, 0.5f);
                        }
                        else if (i - 1 == m_PressIndex)
                        {
                            m_SettingGroup[i].transform.DOLocalMoveX(m_SettingGroupPos[i].x + m_MoveSpacing, 0.5f);
                            if ((i + 1) < m_SettingGroup.Length)
                            {
                                m_SettingGroup[i + 1].transform.DOLocalMoveX(m_SettingGroupPos[i + 1].x + (m_MoveSpacing / 2), 0.5f);
                            }
                        }
                        else if (i > m_PressIndex + 2 || i < m_PressIndex - 2)
                        {
                            m_SettingGroup[i].transform.DOLocalMoveX(m_SettingGroupPos[i].x, 0.5f);
                        }
                    }
                }
                break;

            case LayoutType.Off:
                break;
        }
    }


    private void Deselect()
    {
        for (int i = 0; i < m_SettingGroup.Length; i++)
        {
            m_SettingGroup[i].transform.localPosition = m_SettingGroupPos[i];
        }
    }
    public void SetPressedIndex(int Index)
    {
        m_PressIndex = Index;
        ResetDeselected();
        Debug.Log(m_PressIndex);
    }

    private void OnEnable()
    {
        ///Set Layout
        m_BarLength = barLength;
        m_Spacing = ((m_LastPos.transform.localPosition.x - m_FirstPos.transform.localPosition.x) - m_BarLength) / (m_SettingGroup.Length - 1);

        m_SettingGroupPos = new Vector3[m_SettingGroup.Length];
        var spacing = (m_LastPos.transform.localPosition.x - m_FirstPos.transform.localPosition.x) / (m_SettingGroup.Length - 1);
        for (int i = 0; i < m_SettingGroup.Length; i++)
        {
            m_SettingGroupPos[i] = new Vector3(m_FirstPos.transform.localPosition.x + spacing * i, m_SettingGroup[i].transform.localPosition.y);
            m_SettingGroup[i].transform.localPosition = m_SettingGroupPos[i];
            Debug.Log(m_SettingGroupPos[i]);
        }
        ///
    }

    private void OnDisable()
    {
        Debug.Log("OnDisable");
        m_PressIndex = 99;
        onSelectIndex = 99;
        ResetDeselected();
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
                if (m_SettingGroup[i].GetComponentInChildren<SettingGroupPrefabManager>() != null)
                {
                    m_SettingGroup[i].GetComponentInChildren<SettingGroupPrefabManager>().SettingOut();
                }

                if (m_SettingGroup[i].GetComponentInChildren<ButtonAction>() != null)
                {
                    m_SettingGroup[i].GetComponentInChildren<ButtonAction>().SplitCharAction.IdleChar(m_SettingGroup[i].GetComponentInChildren<ButtonAction>().m_Char);
                    m_SettingGroup[i].GetComponentInChildren<ButtonAction>().IdleIcon();
                    m_SettingGroup[i].GetComponentInChildren<ButtonAction>().m_MouseSelectedState = false;
                    m_SettingGroup[i].GetComponentInChildren<ButtonAction>().m_KeySelectedState = false;
                }
                else if (m_SettingGroup[i].GetComponentInChildren<OptionButtonAction>() != null)
                {
                    m_SettingGroup[i].GetComponentInChildren<OptionButtonAction>();
                    m_SettingGroup[i].GetComponentInChildren<OptionButtonAction>().UnPress();
                    m_SettingGroup[i].GetComponentInChildren<OptionButtonAction>().m_MouseSelectedState = false;
                    m_SettingGroup[i].GetComponentInChildren<OptionButtonAction>().m_KeySelectedState = false;
                }
            }
        }
    }
}
