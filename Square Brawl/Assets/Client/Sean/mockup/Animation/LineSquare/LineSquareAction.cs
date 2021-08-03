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
    public enum MoveType {Total, Single}
    public MoveType m_MoveType;

    [Space(20)]
    [Header("Total")]
    [SerializeField] private GameObject m_LineSquare;
    [SerializeField] private float m_Angle;
    [SerializeField] private float m_Size;
    [SerializeField] private float m_Thickness;
    [Range(0, 1)]
    [SerializeField] private float m_TotalFillAmount = 1;
    [Range(0, 1)]
    public int FillOrigin;

    Easetype.Current_easetype  m_CurrentEasytype;
    [Space(15)]
    [SerializeField] private Easetype.Current_easetype.Easetype m_Easetpe;
    [SerializeField] private float m_Duration;
    private Sequence lineAnimation;
    
    [System.Serializable]
    struct LineSquare 
    {
        public GameObject Mask;
        public GameObject Line;

        public float x;
        public float y;
        [Range(0, 1)]
        public float FillAmount;
        [Range(0, 1)]
        public int FillOrigin;
    }
    [Space(20)]
    [Header("Single")]
    [SerializeField] private LineSquare m_Up;
    [SerializeField] private LineSquare m_Down;
    [SerializeField] private LineSquare m_Left;
    [SerializeField] private LineSquare m_Right;

    private LineSquare[] m_Line;

    private void Start()
    {
        m_CurrentEasytype = new Easetype.Current_easetype();

        m_Line = new LineSquare[4];
        
    }


    private void Update()
    {
        m_Line[0] = m_Up;
        m_Line[1] = m_Down;
        m_Line[2] = m_Left;
        m_Line[3] = m_Right;

        if (m_Thickness < 0)
        {
            m_Thickness = -m_Thickness;
        }

        if (m_Size < 0)
        {
            m_Size = -m_Size;
        }

        MoveAction(m_MoveType);
    }
    public void MoveAction(MoveType m_MoveType)
    {
        if (m_MoveType == MoveType.Total)
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
                m_Line[i].Line.GetComponent<Image>().fillOrigin = FillOrigin;
                /*
                lineAnimation.Join(m_Line[i].Line.GetComponent<Image>()
                                .DOFillAmount(m_TotalFillAmount, m_Duration)
                                .SetEase(m_CurrentEasytype.GetEasetype(m_Easetpe)));*/
            }
        }
        else if(m_MoveType == MoveType.Single)
        {
            m_LineSquare.GetComponent<RectTransform>().sizeDelta = new Vector2(m_Size, m_Size);
            m_LineSquare.transform.rotation = Quaternion.Euler(new Vector3(0, 0, m_Angle));

            lineAnimation.Kill();
            lineAnimation = DOTween.Sequence();
            for (int i = 0; i < 4; i++)
            {
                m_Line[i].Line.GetComponent<RectTransform>().anchoredPosition = new Vector2(m_Line[i].x, m_Line[i].y);
                m_Line[i].Mask.GetComponent<RectTransform>().sizeDelta = new Vector2(m_Size, m_Thickness);
                m_Line[i].Line.GetComponent<RectTransform>().sizeDelta = new Vector2(m_Size, m_Thickness);
                m_Line[i].Line.GetComponent<Image>().fillOrigin = m_Line[i].FillOrigin;
                m_Line[i].Line.GetComponent<Image>().fillAmount = m_Line[i].FillAmount;
                /*
                lineAnimation.Join(m_Line[i].Line.GetComponent<Image>()
                                .DOFillAmount(m_TotalFillAmount, m_Duration)
                                .SetEase(m_CurrentEasytype.GetEasetype(m_Easetpe)));*/
            }
        }
    }
}
