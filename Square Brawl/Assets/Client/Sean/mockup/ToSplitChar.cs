using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ToSplitChar : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_Text;


    //Test
    public void SetUp() 
    {
        Generate(m_Text,m_Text.transform);
    }

    public void Generate(TextMeshProUGUI m_Text,Transform m_transform)
    {
        int length = m_Text.text.Length;
        string tmpstring = m_Text.text;
        char[] tmpchar = tmpstring.ToCharArray();
        TextMeshProUGUI[] textArray = new TextMeshProUGUI[length];

        for (int counter = 0; counter <length; counter++ )
        {
            textArray[counter] = GetComponent<TextMeshProUGUI>();
            textArray[counter].text = tmpchar[counter].ToString();
            Instantiate(textArray[counter], m_Text.transform.position, m_Text.transform.rotation, m_transform);
            //Debug.Log("tmpchar[" + counter + "]\t" + tmpchar[counter].ToString() + tmpchar[counter].ToString().GetType() +
            //      "\ttextArray[" + counter + "]\t" + textArray[counter].text + textArray[counter].text.GetType() + "\n");
        }
    }
}
