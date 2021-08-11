using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class RoomLayout : MonoBehaviour
{
    [SerializeField] private Transform m_Transform;
    [SerializeField] private Vector2 m_Spacing;
    private int m_Index;

    private void Update()
    {
        m_Index = m_Transform.childCount;

        if (m_Index <= 4)
        {
            if (m_Index >= 1)
            {
                m_Transform.GetChild(0).localPosition = new Vector3(-m_Spacing.x / 2, m_Spacing.y / 2);
            }
            if (m_Index >= 2)
            {
                m_Transform.GetChild(1).localPosition = new Vector3(m_Spacing.x / 2, m_Spacing.y / 2);
            }
            if (m_Index >= 3)
            {
                m_Transform.GetChild(2).localPosition = new Vector3(-m_Spacing.x / 2, -m_Spacing.y / 2);
            }
            if (m_Index == 4)
            {
                m_Transform.GetChild(3).localPosition = new Vector3(m_Spacing.x / 2, -m_Spacing.y / 2);
            }
        }
        else
        {
            Debug.LogError("Player Room List Out of Range");
        }
    }
}
