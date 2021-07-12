using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BasicTools.ButtonInspector;

public class LineTest : MonoBehaviour
{
    [SerializeField] private GameObject m_Line;
    private Vector3[] m_vertex;
    private Vector3 m_localposition;
    private Vector3 m_size;

    [Space(10)]
    [Button("SetVertex","SetVertex")]
    [SerializeField] private bool m_SetVertex;

    private void Start()
    {
        m_localposition = m_Line.transform.localPosition;
        m_size = m_Line.transform.GetComponent<RectTransform>().sizeDelta;
        m_Line.GetComponentInChildren<LineRenderer>().positionCount = 4;

        m_vertex = new Vector3[4] 
        {
            new Vector3 (m_localposition.x - m_size.x / 2, m_localposition.y - m_size.y / 2),
            new Vector3 (m_localposition.x - m_size.x / 2, m_localposition.y + m_size.y / 2),
            new Vector3 (m_localposition.x + m_size.x / 2, m_localposition.y + m_size.y / 2),
            new Vector3 (m_localposition.x + m_size.x / 2, m_localposition.y - m_size.y / 2)
        };

        for (int i = 0; i < 4; i++)
        {
            Debug.Log(m_vertex[i]);
        }
        //Debug.Log(m_Line.transform.GetComponent<RectTransform>().sizeDelta.x + "\t" + m_Line.transform.GetComponent<RectTransform>().sizeDelta.y);
        
    }

    public void SetVertex()
    {
        Debug.Log("SetVertex");
        for (int i = 0; i < 4; i++ )
        {
            m_Line.GetComponentInChildren<LineRenderer>().SetPosition(i, m_vertex[i]);
        }
    }
}
