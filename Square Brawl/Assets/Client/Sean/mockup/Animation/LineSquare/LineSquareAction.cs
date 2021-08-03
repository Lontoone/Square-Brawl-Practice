using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.EventSystems;
using DG.Tweening;
using BasicTools.ButtonInspector;
namespace Easetype { }

[ExecuteInEditMode]
public class LineSquareAction : MonoBehaviour
{
    [SerializeField] private GameObject m_LineSquare;
    [SerializeField] private float m_Angle;
    [SerializeField] private float m_Size;
    [SerializeField] private float m_Thickness;
    [Range(0, 1)]
    [SerializeField] private float m_TotalFillAmount = 1;

    Easetype.Current_easetype  m_CurrentEasytype;
    [Space(15)]
    [Button("TotalMove", "TotalMoveTrigger")]
    [SerializeField] private bool m_TotalMoveTrigger;
    [SerializeField] private Easetype.Current_easetype.Easetype m_Easetpe;
    [SerializeField] private float m_Duration;
    private Sequence lineAnimation;
    
    [System.Serializable]
    struct LineSquare 
    {
        public GameObject Mask;
        public GameObject Line;
        [HideInInspector]
        public float FillAmount;
    }
    [Space(15)]
    [SerializeField] private LineSquare m_Up;
    [SerializeField] private LineSquare m_Down;
    [SerializeField] private LineSquare m_Left;
    [SerializeField] private LineSquare m_Right;

    private LineSquare[] m_Line;




    private void Start()
    {
        m_CurrentEasytype = new Easetype.Current_easetype();

        m_Line = new LineSquare[4];
        m_Line[0] = m_Up;
        m_Line[1] = m_Down;
        m_Line[2] = m_Left;
        m_Line[3] = m_Right;
    }


    private void Update()
    {
        if (m_Thickness < 0)
        {
            m_Thickness = -m_Thickness;
        }

        TotalMove(m_TotalMoveTrigger);
        
    }

    public void TotalMoveTrigger()
    {
        m_TotalMoveTrigger = !m_TotalMoveTrigger;
        Debug.Log("Total Move : " + m_TotalMoveTrigger);
    }

    public void TotalMove(bool trigger)
    {
        if (trigger)
        {
            m_LineSquare.GetComponent<RectTransform>().sizeDelta = new Vector2(m_Size, m_Size);
            m_LineSquare.transform.rotation = Quaternion.Euler(new Vector3(0, 0, m_Angle));

            lineAnimation.Kill();
            lineAnimation = DOTween.Sequence();
            for (int i = 0; i < 4; i++)
            {
                m_Line[i].Mask.GetComponent<RectTransform>().sizeDelta = new Vector2(m_Size, m_Thickness);
                m_Line[i].Line.GetComponent<RectTransform>().sizeDelta = new Vector2(m_Size, m_Thickness);
                m_Line[i].Line.GetComponent<Image>().fillAmount = m_TotalFillAmount;
                /*
                lineAnimation.Join(m_Line[i].Line.GetComponent<Image>()
                                .DOFillAmount(m_TotalFillAmount, m_Duration)
                                .SetEase(m_CurrentEasytype.GetEasetype(m_Easetpe)));*/
            }
        }
    }
}
