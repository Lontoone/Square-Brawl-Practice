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
    private int player1 = 0;
    private int player2 = 0;
    private int player3 = 0;
    private int player4 = 0;


    private void Update()
    {
        m_Index = m_Transform.childCount;

        if (m_Index <= 4)
        {
            if (m_Index >= 1 && player1 == 0)
            {
                m_Transform.GetChild(0).localPosition = new Vector3(-m_Spacing.x / 2, m_Spacing.y / 2);
                player1++;
            }
            if (m_Index >= 2 && player2 == 0)
            {
                m_Transform.GetChild(1).localPosition = new Vector3(m_Spacing.x / 2, m_Spacing.y / 2);
                player2++;
            }
            if (m_Index >= 3 && player3 == 0)
            {
                m_Transform.GetChild(2).localPosition = new Vector3(-m_Spacing.x / 2, -m_Spacing.y / 2);
                player3++;
            }
            if (m_Index == 4 && player4 == 0)
            {
                m_Transform.GetChild(3).localPosition = new Vector3(m_Spacing.x / 2, -m_Spacing.y / 2);
                player4++;
            }
        }
        else
        {
            Debug.LogWarning("Player Room List Out of Range" , gameObject);
        }
    }

    private void OnDisable()
    {
         player1 = 0;
         player2 = 0;
         player3 = 0;
         player4 = 0;
    }   
}
