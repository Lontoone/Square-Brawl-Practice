using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharArrayTest : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_Text;
    private string tmpstring;
    private char[] tmpchar;
    private int counter ;

    public TextMeshProUGUI[] textArray;
    // Start is called before the first frame update

    private void Awake()
    {

        Debug.Log("awake");
        //tmpchar = new char[tmpchar.Length];
        tmpstring = m_Text.text;
       
        tmpchar = tmpstring.ToCharArray();
        textArray = new TextMeshProUGUI[tmpchar.Length];
        for (counter = 0; counter < tmpchar.Length; counter++)
        {
            textArray[counter] = GetComponent<TextMeshProUGUI>();
        }
        

        for (counter = 0; counter < tmpchar.Length; counter++)
        {
            //Debug.Log("\t" + tmpchar[counter]);

            textArray[counter].SetText(tmpchar[counter].ToString());

            //char current;
            //current = tmpchar[counter];
            //textArray[counter].SetCharArray(new char[] { tmpchar[counter], ' ' });

            Debug.Log("textArray[" + counter + "]\t" + textArray[counter].text + "\n");
        }

    }
    void Start()
    {
        Debug.Log("start");
     
        
    }
    public void Setup() {



    }
    public void Generate()
    {
        Quaternion angle = Quaternion.Euler(0, 0, 0);
        for (counter = 0; counter < textArray.Length; counter++)
        {
            Debug.Log(counter + "\t");
            Instantiate(textArray[counter], new Vector3(0, 0, 0), angle);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
