using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
namespace Easetype { }

namespace ToSplitChar
{
    public class SplitCharAction : MonoBehaviour
    {
        [HideInInspector]
        public enum Direction { GoUp = 0, GoDown = 1, GoLeft = 2, GoRight = 3 }
        private Easetype.Current_easetype m_current_easetype;
        [System.Serializable]
        public struct SplitChar
        {
            public TextMeshProUGUI m_Text;
            
            [Space(15)]
            public Direction m_Direction;
            public float ToX;
            public float ToY;

            
            [Space(15)]
            public Easetype.Current_easetype.Easetype m_Easetype;
            public float m_Duration;
            public float m_Outdistance;
        }

        [SerializeField] public SplitChar m_SplitChar;
        private TextMeshProUGUI[] textArray;
        private Sequence _moveSequence_char;
        private Vector3 pos;
        private Vector3[] charIdlePostion;


        public void SetUp()
        {
            Generate(m_SplitChar.m_Text, m_SplitChar.m_Text.transform);
            pos = m_SplitChar.m_Text.transform.localPosition;
            charIdlePostion = new Vector3[m_SplitChar.m_Text.text.Length];
        }

        public TextMeshProUGUI[] Generate(TextMeshProUGUI m_Text, Transform m_transform)
        {
            int length = m_Text.text.Length;
            string tmpstring = m_Text.text;
            char[] tmpchar = tmpstring.ToCharArray();
            TextMeshProUGUI[] textArray = new TextMeshProUGUI[length];
            TextMeshProUGUI[] m_textArray = new TextMeshProUGUI[length];

            for (int counter = 0; counter < length; counter++)
            {
                textArray[counter] = GetComponent<TextMeshProUGUI>();
                textArray[counter].text = tmpchar[counter].ToString();
                m_textArray[counter] =Instantiate(textArray[counter], m_Text.transform.position, m_Text.transform.rotation, m_transform);
            }
            return m_textArray;
        }
        /*   //todo
        public void HighlightedChar(SplitChar m_SplitChar)
        {
            //Debug.Log("HighlightedChar");
            _moveSequence_char.Kill();
            _moveSequence_char = DOTween.Sequence();

            for (int i = 0; i < m_SplitChar.m_Text.text.Length; i++)
            {
                textArray[i].transform.localPosition = pos;
                switch (m_SplitChar.m_Direction)
                {

                    case Direction.GoUp://todo
                        if (i <= (m_SplitChar.m_Text.text.Length - 1) / 2)
                        {
                            _moveSequence_char.Append(textArray[i].transform
                                                .DOLocalMove(new Vector3(pos.x + m_SplitChar.ToX, pos.y + m_SplitChar.ToY), m_SplitChar.m_Duration / 2)
                                                .SetEase(m_current_easetype.GetEasetype(m_SplitChar.m_Easetype)))

                                              .Join(textArray[(m_SplitChar.m_Text.text.Length - 1) - i].transform
                                                .DOLocalMove(new Vector3(pos.x + m_SplitChar.ToX, pos.y + m_SplitChar.ToY), m_SplitChar.m_Duration / 2)
                                                .SetEase(m_current_easetype.GetEasetype(m_SplitChar.m_Easetype)))

                                              .Append((textArray[i].transform
                                                .DOLocalMove(new Vector3(pos.x + m_SplitChar.ToX + (-(m_SplitChar.m_Text.characterSpacing * (((m_SplitChar.m_Text.text.Length - 1) / 2) - i + 0.5f))), pos.y + m_SplitChar.ToY), m_SplitChar.m_Duration))
                                                .SetEase(m_current_easetype.GetEasetype(m_SplitChar.m_Easetype)))

                                              .Join((textArray[(m_SplitChar.m_Text.text.Length - 1) - i].transform
                                                .DOLocalMove(new Vector3(pos.x + m_SplitChar.ToX + (m_SplitChar.m_Text.characterSpacing * (((m_SplitChar.m_Text.text.Length - 1) / 2) - i + 0.5f)), pos.y + m_SplitChar.ToY), m_SplitChar.m_Duration))
                                                .SetEase(m_current_easetype.GetEasetype(m_SplitChar.m_Easetype)));

                            charIdlePostion[i] = new Vector3(pos.x + m_SplitChar.ToX + (-(m_SplitChar.m_Text.characterSpacing * (((m_SplitChar.m_Text.text.Length - 1) / 2) - i + 0.5f))), pos.y + m_SplitChar.ToY);
                            charIdlePostion[(m_SplitChar.m_Text.text.Length - 1) - i] = new Vector3(pos.x + m_SplitChar.ToX + (m_SplitChar.m_Text.characterSpacing * (((m_SplitChar.m_Text.text.Length - 1) / 2) - i + 0.5f)), pos.y + m_SplitChar.ToY);
                        }
                        break;

                    case Direction.GoDown://todo
                        if (i <= (m_SplitChar.m_Text.text.Length - 1) / 2)
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

                            charIdlePostion[i] = new Vector3(pos.x + to_x + (-(m_button_text.characterSpacing * (((m_TextLength - 1) / 2) - i + 0.5f))), pos.y + to_y);
                            charIdlePostion[(m_TextLength - 1) - i] = new Vector3(pos.x + to_x + (m_button_text.characterSpacing * (((m_TextLength - 1) / 2) - i + 0.5f)), pos.y + to_y);
                        }
                        break;

                    case Direction.GoLeft:
                        _moveSequence_char.Append(textArray[i].transform
                                            .DOLocalMove(new Vector3(pos.x + to_x + (-(m_button_text.characterSpacing * (m_TextLength - 1 - i))), pos.y + to_y), char_duration)
                                            .SetEase(menu_current_easetype.GetEasetype(char_easetype)));

                        charIdlePostion[i] = new Vector3(pos.x + to_x + (-(m_button_text.characterSpacing * (m_TextLength - 1 - i))), pos.y + to_y);
                        break;

                    case Direction.GoRight:
                        _moveSequence_char.Append(textArray[(m_TextLength - 1) - i].transform
                                            .DOLocalMove(new Vector3(pos.x + to_x + (m_button_text.characterSpacing * ((m_TextLength - 1) - i)), pos.y + to_y), char_duration)
                                            .SetEase(menu_current_easetype.GetEasetype(char_easetype)));

                        charIdlePostion[(m_TextLength - 1) - i] = new Vector3(pos.x + to_x + (m_button_text.characterSpacing * ((m_TextLength - 1) - i)), pos.y + to_y);
                        break;

                    default:
                        break;
                }
            }
        }

        public void IdleChar()
        {
            //Debug.Log("IdleChar");
            _moveSequence_char.Kill();
            _moveSequence_char = DOTween.Sequence();

            for (int i = 0; i < m_TextLength; i++)
            {
                switch (direction)
                {
                    case Direction.GoUp:
                        if (i <= (m_TextLength - 1) / 2)
                        {
                            _moveSequence_char.Append(textArray[((m_TextLength - 1) / 2) - i].transform
                                                .DOLocalMove(new Vector3(charIdlePostion[((m_TextLength - 1) / 2) - i].x + to_x, charIdlePostion[((m_TextLength - 1) / 2) - i].y + to_y * stirng_outdistance), char_duration)
                                                .SetEase(menu_current_easetype.GetEasetype(char_easetype)))

                                              .Join(textArray[((m_TextLength - 1) + i) - ((m_TextLength - 1) / 2)].transform
                                                .DOLocalMove(new Vector3(charIdlePostion[((m_TextLength - 1) + i) - ((m_TextLength - 1) / 2)].x + to_x, charIdlePostion[((m_TextLength - 1) + i) - ((m_TextLength - 1) / 2)].y + to_y * stirng_outdistance), char_duration)
                                                .SetEase(menu_current_easetype.GetEasetype(char_easetype)));

                        }
                        break;

                    case Direction.GoDown:
                        if (i <= (m_TextLength - 1) / 2)
                        {
                            _moveSequence_char.Append(textArray[((m_TextLength - 1) / 2) - i].transform
                                                .DOLocalMove(new Vector3(charIdlePostion[((m_TextLength - 1) / 2) - i].x + to_x, charIdlePostion[((m_TextLength - 1) / 2) - i].y + to_y * stirng_outdistance), char_duration)
                                                .SetEase(menu_current_easetype.GetEasetype(char_easetype)))

                                              .Join(textArray[((m_TextLength - 1) + i) - ((m_TextLength - 1) / 2)].transform
                                                .DOLocalMove(new Vector3(charIdlePostion[((m_TextLength - 1) + i) - ((m_TextLength - 1) / 2)].x + to_x, charIdlePostion[((m_TextLength - 1) + i) - ((m_TextLength - 1) / 2)].y + to_y * stirng_outdistance), char_duration)
                                                .SetEase(menu_current_easetype.GetEasetype(char_easetype)));
                        }
                        break;

                    case Direction.GoLeft:
                        _moveSequence_char.Append(textArray[i].transform
                                            .DOLocalMove(new Vector3(charIdlePostion[i].x + (to_x * char_outdistance) + (-(m_button_text.characterSpacing * (m_TextLength - 1 - i))), charIdlePostion[i].y + to_y), char_duration)
                                            .SetEase(menu_current_easetype.GetEasetype(char_easetype)));
                        break;

                    case Direction.GoRight:
                        _moveSequence_char.Append(textArray[(m_TextLength - 1) - i].transform
                                            .DOLocalMove(new Vector3(charIdlePostion[(m_TextLength - 1) - i].x + (to_x * char_outdistance) + (m_button_text.characterSpacing * ((m_TextLength - 1) - i)), charIdlePostion[(m_TextLength - 1) - i].y + to_y), char_duration)
                                            .SetEase(menu_current_easetype.GetEasetype(char_easetype)));
                        break;

                    default:
                        break;
                }
            }
        }*/
    }
}