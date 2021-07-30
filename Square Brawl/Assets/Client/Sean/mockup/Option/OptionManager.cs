using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using DG.Tweening;

namespace Easetype { }

public class OptionManager : MonoBehaviour
{
    [SerializeField] private GameObject m_OptionGroup;
    [HideInInspector]
    public static int onPressIndex = 99;
    private bool onPress;

    [HideInInspector]
    public static bool m_ResetTrigger = false;
    [SerializeField] private GameObject m_FirstPos;
    [SerializeField] private GameObject m_LastPos;
    [SerializeField] private GameObject m_BackButton;
    private Vector2 m_BackButtonPos;
    
    [Space(15)]
    [SerializeField] private float barLength;    
    public static float m_BarLength;

    public enum LayoutType {OnSelect, OnPress, Off}
    [SerializeField] private LayoutType m_LayoutType;

    [SerializeField] private float m_MoveSpacing;
    private float m_DefaultSpacing;
    private float m_BarSpacing;

    [HideInInspector]
    public Easetype.Current_easetype m_CurrentEasetype;
    [Space(15)]
    [SerializeField] public Easetype.Current_easetype.Easetype m_Easetype;
    [SerializeField] private float m_Duration;
    private Sequence m_OptionAnimation;

    [Space(15)]
    [SerializeField] public GameObject[] m_SettingGroup;
    private Vector3[] m_SettingGroupPos;
    private int onSelectIndex = 99;

    private PlayerInputManager m_Input;
    private Vector2 moveValue;
    private bool firstPress;
    private bool zeroTrigger;
    private bool confrim;
    private bool back;
    private bool start;

    private void Awake()
    {
        m_Input = new PlayerInputManager();
        m_Input.UI.UIMovement.performed += ctx => moveValue = ctx.ReadValue<Vector2>();
        m_Input.UI.UIMovement.canceled += ctx => moveValue = Vector2.zero;
        m_Input.UI.confirmclick.performed += ctx => KeyPressed();
        m_Input.UI.backclick.performed += ctx => KeyUnSelected();
    }

    private void test123()
    {
        Debug.Log("start");
    }

    private void Start()
    {
        m_CurrentEasetype = new Easetype.Current_easetype();
        if (m_BackButton != null)
        {
            m_BackButtonPos = m_BackButton.transform.localPosition;
        }

        ///Set Layout
        m_BarLength = barLength;
        m_BarSpacing = ((m_LastPos.transform.localPosition.x - m_FirstPos.transform.localPosition.x) - m_BarLength) / (m_SettingGroup.Length - 1);

        m_SettingGroupPos = new Vector3[m_SettingGroup.Length];
        m_DefaultSpacing = (m_LastPos.transform.localPosition.x - m_FirstPos.transform.localPosition.x) / (m_SettingGroup.Length - 1);
        for (int i = 0; i < m_SettingGroup.Length; i++)
        {
            m_SettingGroupPos[i] = new Vector3(m_FirstPos.transform.localPosition.x + m_DefaultSpacing * i, m_SettingGroup[i].transform.localPosition.y);
            m_SettingGroup[i].transform.localPosition = m_SettingGroupPos[i];
            //Debug.Log(m_SettingGroupPos[i]);
        }
        ///
    }

    private void Update()
    {
        KeyMove();
        if (onSelectIndex != 99 && Mouse.current.wasUpdatedThisFrame)
        {
            ResetDeselected();
            m_BackButton.GetComponent<ButtonAction>().SplitCharAction.IdleChar(m_BackButton.GetComponent<ButtonAction>().m_Char);
        }

        switch (m_LayoutType)
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
                if (trigger == 0 && onPressIndex == 99)
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
                            m_SettingGroup[i].transform.localPosition = m_SettingGroup[i - 1].transform.localPosition + new Vector3(m_BarSpacing, 0);
                        }
                    }
                }
                break;

            case LayoutType.OnPress:
                if (onPressIndex != 99)
                {
                    if (onPressIndex == m_SettingGroup.Length)
                    {
                        StartCoroutine(ResetPosition());
                        onPress = false;
                    }
                    else
                    {
                        for (int i = 0; i < m_SettingGroup.Length; i++)
                        {
                            if (i + 1 == onPressIndex)
                            {
                                m_SettingGroup[i].transform.DOLocalMoveX(m_SettingGroupPos[i].x - m_MoveSpacing, 0.5f);
                                if ((i - 1) >= 0)
                                {
                                    m_SettingGroup[i - 1].transform.DOLocalMoveX(m_SettingGroupPos[i - 1].x - (m_MoveSpacing / 2), 0.5f);
                                }
                            }
                            else if (i == onPressIndex)
                            {
                                m_SettingGroup[i].transform.DOLocalMoveX(m_SettingGroupPos[i].x, 0.5f);
                            }
                            else if (i - 1 == onPressIndex)
                            {
                                m_SettingGroup[i].transform.DOLocalMoveX(m_SettingGroupPos[i].x + m_MoveSpacing, 0.5f);
                                if ((i + 1) < m_SettingGroup.Length)
                                {
                                    m_SettingGroup[i + 1].transform.DOLocalMoveX(m_SettingGroupPos[i + 1].x + (m_MoveSpacing / 2), 0.5f);
                                }
                            }
                            else if (i > onPressIndex + 2 || i < onPressIndex - 2)
                            {
                                m_SettingGroup[i].transform.DOLocalMoveX(m_SettingGroupPos[i].x, 0.5f);
                            }
                        }
                    }
                }
                break;

            case LayoutType.Off:
                break;
        }
    }

    private void OnEnable()
    {
        m_Input.Enable();
    }

    private void OnDisable()
    {
        m_Input.Disable();
        Debug.Log("OnDisable");
        onPressIndex = 99;
        onSelectIndex = 99;
        ResetDeselected();
    }

    public IEnumerator EnterAnimation()
    {
        m_OptionAnimation.Kill();
        m_OptionAnimation = DOTween.Sequence();

        m_BackButton.transform.localPosition = new Vector3(-500, 0);
        for (int i = 0; i < m_SettingGroup.Length; i++)
        {
            m_SettingGroup[i].transform.localPosition = new Vector3(-500,0);
        }
        yield return null;

        m_OptionAnimation.Append(m_BackButton.transform.DOLocalMove(m_BackButtonPos, m_Duration * 2)
                            .SetEase(m_CurrentEasetype.GetEasetype(m_Easetype)));
        yield return new WaitForSeconds(0.2f);

        for (int i = m_SettingGroup.Length - 1; i >= 0; i--)
        {
            yield return new WaitForSeconds(0.2f);
            m_OptionAnimation.Append(m_SettingGroup[i].transform
                                .DOLocalMoveX(m_SettingGroupPos[i].x, m_Duration)
                                .SetEase(m_CurrentEasetype.GetEasetype(m_Easetype)));
        }
    }

    public void ExitAnimation()
    {
        m_OptionAnimation.Kill();
        m_OptionAnimation = DOTween.Sequence();

        for (int i = m_SettingGroup.Length - 1; i >= 0; i--)
        {
            m_OptionAnimation.Append(m_SettingGroup[i].transform
                                .DOLocalMoveX(2000, m_Duration)
                                .SetEase(m_CurrentEasetype.GetEasetype(m_Easetype)));
        }
    }

    // Used in button on click 
    public void SetPressedIndex(int Index)
    {
        onPressIndex = Index;
        onPress = true;
        ResetDeselected();
        Debug.Log(onPressIndex);
    }

    public IEnumerator ResetPosition()
    {
        yield return null;

        ///Set Layout
        for (int i = 0; i < m_SettingGroup.Length; i++)
        {
            //m_SettingGroupPos[i] = new Vector3(m_FirstPos.transform.localPosition.x + m_DefaultSpacing * i, m_SettingGroup[i].transform.localPosition.y);
            m_SettingGroup[i].transform.DOLocalMove(m_SettingGroupPos[i], 0.5f);
            Debug.Log(m_SettingGroupPos[i]);
        }
        ///
    }


    public void ResetDeselected()
    {
        
        for (int i = 0; i < m_SettingGroup.Length; i++)

        {
            if (i == onPressIndex)
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
    private void Firstpress()
    {
        if (moveValue == new Vector2(0, 0))
        {
            zeroTrigger = true;
            firstPress = false;
        }

        if (moveValue != new Vector2(0, 0) && zeroTrigger)
        {
            zeroTrigger = false;
            firstPress = true;
        }
    }

    private void KeyMove()
    {
        bool onSelectThisFrame =false;
        Firstpress();
        if (firstPress)
        {
            firstPress = false;
            onSelectThisFrame = true;

            //Change onSelectIndex
            if (onPress == false)
            {
                if (onSelectIndex > m_SettingGroup.Length || onSelectIndex < 0)
                {
                    onSelectIndex = 0;
                }
                else if (moveValue.x > 0)
                {
                    onSelectIndex++;
                    if (onSelectIndex > m_SettingGroup.Length)
                    {
                        onSelectIndex = 0;
                    }
                }
                else if (moveValue.x < 0)
                {
                    onSelectIndex--;
                    if (onSelectIndex < 0)
                    {
                        onSelectIndex = m_SettingGroup.Length;
                    }
                }
            }

            //Change onPressIndex
            else
            {
                if (moveValue.x > 0)
                {
                    onPressIndex++;
                    if (onPressIndex > m_SettingGroup.Length)
                    {
                        onPressIndex = 0;
                    }
                }
                else if (moveValue.x < 0)
                {
                    onPressIndex--;
                    if (onPressIndex < 0)
                    {
                        onPressIndex = m_SettingGroup.Length;
                    }
                }
                onSelectIndex = onPressIndex;
            }
            Debug.Log(onSelectIndex +"\t"+ onPressIndex);
        }

        //Move onSelected
        if (onSelectThisFrame == true && onPress != true)
        {
            if (onSelectIndex == m_SettingGroup.Length)
            {
                m_BackButton.GetComponent<ButtonAction>().SplitCharAction.HighlightedChar(m_BackButton.GetComponent<ButtonAction>().m_Char);
            }
            else
            {
                m_BackButton.GetComponent<ButtonAction>().SplitCharAction.IdleChar(m_BackButton.GetComponent<ButtonAction>().m_Char);
            }

            for (int i = 0; i < m_SettingGroup.Length; i++)
            {

                if (i == onSelectIndex)
                {
                    m_SettingGroup[i].GetComponentInChildren<OptionButtonAction>().HighlightedBackground();
                }
                else
                {
                    m_SettingGroup[i].GetComponentInChildren<OptionButtonAction>().IdleBackground();
                }
            }
        }

        //Move onPressed
        else if(onSelectThisFrame == true && onPress == true)
        {
            if (onPressIndex == m_SettingGroup.Length)
            {
                m_BackButton.GetComponent<ButtonAction>().SplitCharAction.HighlightedChar(m_BackButton.GetComponent<ButtonAction>().m_Char);
            }
            else
            {
                m_BackButton.GetComponent<ButtonAction>().SplitCharAction.IdleChar(m_BackButton.GetComponent<ButtonAction>().m_Char);
            }

            for (int i = 0; i < m_SettingGroup.Length; i++)
            {
                if (i == onPressIndex)
                {
                    m_SettingGroup[i].GetComponentInChildren<OptionButtonAction>().OnPress();
                }
                ResetDeselected();
            }
        }
    }

    private void KeyPressed()
    {
        if (onSelectIndex != 99)
        {
            onPressIndex = onSelectIndex;
            if (onPressIndex < m_SettingGroup.Length)
            {
                m_SettingGroup[onPressIndex].GetComponentInChildren<OptionButtonAction>().OnPress();
                ResetDeselected();
                m_BackButton.GetComponent<ButtonAction>().SplitCharAction.IdleChar(m_BackButton.GetComponent<ButtonAction>().m_Char);
            }
            else if (onPressIndex == m_SettingGroup.Length)
            {
                StartCoroutine(GetComponentInParent<SceneHandler>().ExitOptionTest());
                m_BackButton.GetComponent<ButtonAction>().SplitCharAction.HighlightedChar(m_BackButton.GetComponent<ButtonAction>().m_Char);
                ResetDeselected();
            }
            onPress = true;
        }
    }

    private void KeyUnSelected()
    {
        if (onPressIndex == 99 || onPressIndex == m_SettingGroup.Length)
        {
            StartCoroutine(GetComponentInParent<SceneHandler>().ExitOptionTest());
        }
        else
        {
            if (onPressIndex < m_SettingGroup.Length)
            {
                onPressIndex = 99;
                ResetDeselected();
                StartCoroutine(ResetPosition());
                onPress = false;
            }
        }
    }
}
