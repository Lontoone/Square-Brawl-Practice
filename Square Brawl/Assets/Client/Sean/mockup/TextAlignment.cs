using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class TextAlignment : MonoBehaviour
{
    [SerializeField] private RectTransform m_AlignParent;
    [SerializeField] private RectTransform m_Text1;
    [SerializeField] private RectTransform m_Text2;
    [SerializeField] private float m_Spacing;

    private void Start()
    {
        if (m_Text1.gameObject.GetComponent<ContentSizeFitter>() == null)
        {
            m_Text1.gameObject.AddComponent<ContentSizeFitter>().horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        }
        if (m_Text2.gameObject.GetComponent<ContentSizeFitter>() == null)
        {
            m_Text2.gameObject.AddComponent<ContentSizeFitter>().horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        }
    }

    private void Update()
    {
        TextAlign();
    }

    private void TextAlign()
    { 
        var totalWidth = m_Text1.sizeDelta.x + m_Text2.sizeDelta.x + m_Spacing;

        m_AlignParent.sizeDelta = new Vector2(totalWidth, m_AlignParent.sizeDelta.y);

        m_Text1.pivot = new Vector2(0, 0.5f);
        m_Text2.pivot = new Vector2(1, 0.5f);
        m_Text1.localPosition = new Vector3(-totalWidth / 2, 0);
        m_Text2.localPosition = new Vector3(totalWidth / 2, 0);
    }
}
