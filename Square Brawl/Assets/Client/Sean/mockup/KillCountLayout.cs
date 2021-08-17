using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class KillCountLayout : MonoBehaviour
{
    [SerializeField] private RectTransform m_Background;
    [SerializeField] private float m_Width;
    [SerializeField] private float m_Spacing;

    private void Start()
    {
        StartCoroutine(SetBackground());
    }

    private IEnumerator SetBackground()
    {
        yield return new WaitForEndOfFrame();

        var num = transform.childCount;
        if (num != 0)
        {
            m_Background.sizeDelta = new Vector2((num * m_Width) + (m_Spacing * (num - 1)), m_Background.sizeDelta.y);
        }
        else
        {
            Debug.LogError("None Player");
        }
        
    }
}