using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;
using BasicTools.ButtonInspector;
using DG.Tweening;
using TMPro;
namespace Easetype {}

public class ButtonAction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    [SerializeField] public int m_ButtonIndex;
    [SerializeField] private Button m_button;   
    [SerializeField] private TextMeshProUGUI m_button_text;
    private int m_TextLength;
    [SerializeField] private Transform m_transform;
    [Space(10)]
    [SerializeField] private GameObject m_AimObject;
    [Space(10)]
    [SerializeField] private float to_x;
    [SerializeField] private float to_y;
    private Vector3 pos;
    //
    
    private Sequence _moveSequence_stirng;
    private Sequence _moveSequence_char;
    private Vector3[] charIdlePostion;

    private Easetype.Current_easetype menu_current_easetype;
    
    private enum Dotweentype{ m_string = 0 , m_char = 1}
    [Space(10)]
    [SerializeField] private Dotweentype dotweentype;
    [Space(10)]
    [HeaderAttribute("String")]
    [SerializeField] Easetype.Current_easetype.Easetype string_easetype;
    [SerializeField] float string_duration = 1f;
    [SerializeField] private float stirng_outdistance = 5;

    
    private enum Direction { GoUp = 0, GoDown = 1 ,GoLeft = 2 , GoRight =3}
    [Space(10)]
    [HeaderAttribute("Char")]
    [SerializeField] private Direction direction;
    [SerializeField] Easetype.Current_easetype.Easetype char_easetype;
    [SerializeField] float char_duration = 1f;
    [SerializeField] private float char_outdistance = 5;

    //
    private bool m_MouseSelectedState = false;
    private bool m_KeySelectedState = false;
    TextMeshProUGUI[] textArray;

    void Start()
    {
        pos = m_button_text.transform.localPosition;
        menu_current_easetype = new Easetype.Current_easetype();
        m_TextLength = m_button_text.text.Length;
        charIdlePostion = new Vector3[m_TextLength]; 
        if (dotweentype == Dotweentype.m_char)
        {
            textArray = Generate(m_button_text, m_transform);
        }
        //Debug.Log("pos"+pos);
    }

    public void HighlightedString()
    {
        Debug.Log("HighlightedString");
        _moveSequence_stirng.Kill();
        _moveSequence_stirng = DOTween.Sequence();
        m_button_text.transform.localPosition = pos;
        _moveSequence_stirng.Append(
               m_button_text.transform.DOLocalMove(new Vector3(pos.x + to_x, pos.y + to_y), string_duration).SetEase(menu_current_easetype.GetEasetype(string_easetype))
           );
    }

    public void IdleString()
    {
        Debug.Log("IdleString");
        _moveSequence_stirng.Kill();
        _moveSequence_stirng = DOTween.Sequence();
        m_button_text.transform.localPosition = new Vector3(pos.x + to_x, pos.y + to_y, pos.z);
        _moveSequence_stirng.Append(
                m_button_text.transform.DOLocalMove(new Vector3(pos.x + to_x * stirng_outdistance, pos.y + to_y * stirng_outdistance), string_duration * stirng_outdistance).SetEase(menu_current_easetype.GetEasetype(string_easetype))
            );
    }



    public TextMeshProUGUI[] Generate(TextMeshProUGUI m_Text, Transform m_transform)
    {
        int length = m_TextLength;
        string tmpstring = m_Text.text;
        char[] tmpchar = tmpstring.ToCharArray();
        TextMeshProUGUI[] textArray = new TextMeshProUGUI[length];
        TextMeshProUGUI[] m_textArray = new TextMeshProUGUI[length];
        //Debug.Log(length + "\t" + tmpstring + "\t" + tmpchar + "\t" + textArray);
        for (int counter = 0; counter < length; counter++)
        {
            textArray[counter] = m_Text.GetComponent<TextMeshProUGUI>();
            textArray[counter].text = tmpchar[counter].ToString();
            m_textArray[counter] =Instantiate(textArray[counter], m_Text.transform.position, m_Text.transform.rotation, m_transform);
            //Debug.Log("tmpchar[" + counter + "]\t" + tmpchar[counter].ToString() + tmpchar[counter].ToString().GetType() +
            //      "\ttextArray[" + counter + "]\t" + textArray[counter].text + textArray[counter].text.GetType() + "\n");
        }
        return m_textArray;
    }
    public void HighlightedChar()
    {
        Debug.Log("HighlightedChar");
        _moveSequence_char.Kill();
        _moveSequence_char = DOTween.Sequence();
        
        for (int i=0;i<m_TextLength ;i++ ) 
        {
            //Debug.Log(i + "\t" + textArray[i].text);
           textArray[i].transform.localPosition = pos;
            //Debug.Log(i+"\t" + textArray[i].text +"\n" +textArray[i].transform.position +"& "+ m_button_text.characterSpacing + "\n " + (pos.x + to_x + (m_button_text.characterSpacing * i)));
            switch(direction)
            {

                case Direction.GoUp:
                    if (i <= (m_TextLength-1)/2)
                    {
                        _moveSequence_char.Append(textArray[i].transform
                                            .DOLocalMove(new Vector3(pos.x + to_x, pos.y + to_y), char_duration / 2)
                                            .SetEase(menu_current_easetype.GetEasetype(char_easetype)))

                                          .Join(textArray[(m_TextLength - 1) - i].transform
                                            .DOLocalMove(new Vector3(pos.x + to_x, pos.y + to_y), char_duration / 2)
                                            .SetEase(menu_current_easetype.GetEasetype(char_easetype)))

                                          .Append( (textArray[i].transform
                                            .DOLocalMove(new Vector3(pos.x + to_x + (-(m_button_text.characterSpacing * (((m_TextLength - 1) / 2) - i + 0.5f))), pos.y + to_y), char_duration))
                                            .SetEase(menu_current_easetype.GetEasetype(char_easetype)))

                                          .Join( (textArray[(m_TextLength-1) - i ].transform
                                            .DOLocalMove(new Vector3(pos.x + to_x + (m_button_text.characterSpacing * (((m_TextLength - 1) / 2) - i + 0.5f)), pos.y + to_y), char_duration))
                                            .SetEase(menu_current_easetype.GetEasetype(char_easetype)));
                    }
                    break;

                case Direction.GoDown:
                    if (i <= (m_TextLength - 1) / 2)
                    {
                        _moveSequence_char.Append(textArray[i].transform
                                            .DOLocalMove(new Vector3(pos.x + to_x, pos.y + to_y), char_duration / 2)
                                            .SetEase(menu_current_easetype.GetEasetype(char_easetype)))
                                          
                                          .Join(textArray[(m_TextLength - 1) - i].transform
                                            .DOLocalMove(new Vector3(pos.x + to_x, pos.y + to_y), char_duration / 2)
                                            .SetEase(menu_current_easetype.GetEasetype(char_easetype)))

                                          .Append((textArray[i].transform
                                            .DOLocalMove(new Vector3(pos.x + to_x + (-(m_button_text.characterSpacing * (((m_TextLength - 1) / 2) - i + 0.5f))), pos.y + to_y), char_duration))
                                            .SetEase(menu_current_easetype.GetEasetype(char_easetype)))

                                          .Join((textArray[(m_TextLength - 1) - i].transform
                                            .DOLocalMove(new Vector3(pos.x + to_x + (m_button_text.characterSpacing * (((m_TextLength - 1) / 2) - i + 0.5f)), pos.y + to_y), char_duration))
                                            .SetEase(menu_current_easetype.GetEasetype(char_easetype)));
                    }
                    break;

                case Direction.GoLeft:
                    Debug.Log("textArray[" + i + "].text" + textArray[i].text);
                    //Debug.Log(m_button_text.text + (m_TextLength - 1 - i) + m_button_text.characterSpacing);
                    _moveSequence_char.Append(textArray[i].transform.DOLocalMove(new Vector3(pos.x + to_x + (-(m_button_text.characterSpacing *(m_TextLength - 1 - i))), pos.y + to_y), char_duration)
                                    .SetEase(menu_current_easetype.GetEasetype(char_easetype)));
                    break;

                case Direction.GoRight:
                    _moveSequence_char.Append(textArray[(m_TextLength - 1) - i].transform.DOLocalMove(new Vector3(pos.x + to_x + (m_button_text.characterSpacing * ((m_TextLength - 1) - i)), pos.y + to_y), char_duration)
                                    .SetEase(menu_current_easetype.GetEasetype(char_easetype)));
                    break;

                default:
                    break;
            }
            Debug.Log("textArray[" + i + "].text" + textArray[i].text + "textArray[" + i + "].transform.localPosition" + textArray[i].transform.localPosition);
            charIdlePostion[i]= textArray[i].transform.localPosition;
        }
    }

    public void IdleChar()
    {
        Debug.Log("IdleChar");
        _moveSequence_char.Kill();
        _moveSequence_char = DOTween.Sequence();

        for (int i = 0; i < m_TextLength; i++)
        {
            //Debug.Log(i + "\t" + textArray[i].text);

            //textArray[i].transform.localPosition = charIdlePostion[i];
            //Debug.Log(i+"\t" + textArray[i].text +"\n" +textArray[i].transform.position +"& "+ m_button_text.characterSpacing + "\n " + (pos.x + to_x + (m_button_text.characterSpacing * i)));
            switch (direction)
            {

                case Direction.GoUp:
                    if (i <= (m_TextLength - 1) / 2)
                    {
                        _moveSequence_char.Append(textArray[i].transform
                                            .DOLocalMove(new Vector3(pos.x + to_x, pos.y + to_y), char_duration / 2)
                                            .SetEase(menu_current_easetype.GetEasetype(char_easetype)))

                                          .Join(textArray[(m_TextLength - 1) - i].transform
                                            .DOLocalMove(new Vector3(pos.x + to_x, pos.y + to_y), char_duration / 2)
                                            .SetEase(menu_current_easetype.GetEasetype(char_easetype)))

                                          .Append((textArray[i].transform
                                            .DOLocalMove(new Vector3(pos.x + to_x + (-(m_button_text.characterSpacing * (((m_TextLength - 1) / 2) - i + 0.5f))), pos.y + to_y), char_duration))
                                            .SetEase(menu_current_easetype.GetEasetype(char_easetype)))

                                          .Join((textArray[(m_TextLength - 1) - i].transform
                                            .DOLocalMove(new Vector3(pos.x + to_x + (m_button_text.characterSpacing * (((m_TextLength - 1) / 2) - i + 0.5f)), pos.y + to_y), char_duration))
                                            .SetEase(menu_current_easetype.GetEasetype(char_easetype)));
                    }
                    break;

                case Direction.GoDown:
                    if (i <= (m_TextLength - 1) / 2)
                    {
                        _moveSequence_char.Append(textArray[i].transform
                                            .DOLocalMove(new Vector3(pos.x + to_x, pos.y + to_y), char_duration / 2)
                                            .SetEase(menu_current_easetype.GetEasetype(char_easetype)))

                                          .Join(textArray[(m_TextLength - 1) - i].transform
                                            .DOLocalMove(new Vector3(pos.x + to_x, pos.y + to_y), char_duration / 2)
                                            .SetEase(menu_current_easetype.GetEasetype(char_easetype)))

                                          .Append((textArray[i].transform
                                            .DOLocalMove(new Vector3(pos.x + to_x + (-(m_button_text.characterSpacing * (((m_TextLength - 1) / 2) - i + 0.5f))), pos.y + to_y), char_duration))
                                            .SetEase(menu_current_easetype.GetEasetype(char_easetype)))

                                          .Join((textArray[(m_TextLength - 1) - i].transform
                                            .DOLocalMove(new Vector3(pos.x + to_x + (m_button_text.characterSpacing * (((m_TextLength - 1) / 2) - i + 0.5f)), pos.y + to_y), char_duration))
                                            .SetEase(menu_current_easetype.GetEasetype(char_easetype)));
                    }
                    break;

                case Direction.GoLeft:
                    _moveSequence_char.Append(textArray[i].transform.DOLocalMove(new Vector3(pos.x + (to_x * char_outdistance) + (-(m_button_text.characterSpacing * (m_TextLength - 1 - i))), pos.y + to_y), char_duration * char_outdistance)
                                    .SetEase(menu_current_easetype.GetEasetype(char_easetype)));
                    break;

                case Direction.GoRight:
                    _moveSequence_char.Append(textArray[(m_TextLength - 1) - i].transform.DOLocalMove(new Vector3(pos.x + (to_x) + (m_button_text.characterSpacing * ((m_TextLength - 1) - i)), pos.y + to_y), char_duration * char_outdistance)
                                    .SetEase(menu_current_easetype.GetEasetype(char_easetype)));
                    break;

                default:
                    break;
            }
        }
    }




    public void OnPointerEnter(PointerEventData eventData)
    {
        m_MouseSelectedState = true;
        if (m_KeySelectedState != true && m_MouseSelectedState == true)
        {
            switch (dotweentype) 
            {
                case Dotweentype.m_string:
                    HighlightedString();
                    break;

                case Dotweentype.m_char:
                    HighlightedChar();
                    break;

                default:
                    break;
            }
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        switch (dotweentype)
        {
            case Dotweentype.m_string:
                IdleString();
                break;

            case Dotweentype.m_char:
                IdleChar();
                break;

            default:
                break;
        }
        m_MouseSelectedState = false;
        m_KeySelectedState = false;
    }

    public virtual void OnSelect(BaseEventData eventData)
    {
        m_KeySelectedState = true;
        if (m_MouseSelectedState != true && m_KeySelectedState == true)
        {
            switch (dotweentype)
            {
                case Dotweentype.m_string:
                    HighlightedString();
                    break;

                case Dotweentype.m_char:
                    HighlightedChar();
                    break;

                default:
                    break;
            }
        }
    }

    public virtual void OnDeselect(BaseEventData eventData)
    {
        switch (dotweentype)
        {
            case Dotweentype.m_string:
                IdleString();
                break;

            case Dotweentype.m_char:
                IdleChar();
                break;

            default:
                break;
        }
        m_MouseSelectedState = false;
        m_KeySelectedState = false;
    }
}
