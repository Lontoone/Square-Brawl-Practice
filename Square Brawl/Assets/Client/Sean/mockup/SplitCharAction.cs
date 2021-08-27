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
            public Transform m_transform;
            [HideInInspector]
            public TextMeshProUGUI[] textArray;

            [Space(15)]
            public Direction m_Direction;
            public float ToX;
            public float ToY;

            
            [Space(15)]
            public Easetype.Current_easetype.Easetype m_Easetype;
            public float m_Duration;
            public float m_Outdistance;
        }

        //private TextMeshProUGUI[] m_SplitChar.textArray;
        private Sequence _moveSequence_char;
        private Vector3 pos;
        private Vector3[] charIdlePostion;
        private int m_TextLength;


        public SplitChar SetUp(SplitChar m_Text)
        {
            m_TextLength = m_Text.m_Text.text.Length;
            m_current_easetype = new Easetype.Current_easetype();
            m_Text.textArray = Generate(m_Text.m_Text, m_Text.m_transform);
            pos = m_Text.m_Text.transform.localPosition;
            charIdlePostion = new Vector3[m_TextLength];


            return m_Text;
            //Debug.Log(m_TextLength);
            /*
            for (int i = 0; i < m_TextLength; i++)
            {
                Debug.Log(m_SplitChar.m_Text.name + "\t" + i + m_SplitChar.textArray[i].text);
            }*/
        }

        private TextMeshProUGUI[] Generate(TextMeshProUGUI m_Text, Transform m_transform)
        {

            int length = m_Text.text.Length;
            string tmpstring = m_Text.text;
            char[] tmpchar = tmpstring.ToCharArray();

            TextMeshProUGUI[] textArray = new TextMeshProUGUI[length];
            TextMeshProUGUI[] m_textArray = new TextMeshProUGUI[length];
            TextMeshProUGUI reference = m_Text.GetComponent<TextMeshProUGUI>(); //TODO : 暫時解決方法
            string temp = m_Text.text; //TODO : 暫時解決方法
            for (int counter = 0; counter < length; counter++)
            {
                textArray[counter] = reference;
                textArray[counter].text = tmpchar[counter].ToString(); //TODO : 不知道為啥會影響到父輩物件
                m_textArray[counter] =Instantiate(textArray[counter], m_Text.transform.position, m_Text.transform.rotation, m_transform);
            }
            m_Text.text = temp; //TODO : 暫時解決方法
            return m_textArray;
        }

        public void HighlightedChar(SplitChar m_SplitChar)
        {

            //Debug.Log("HighlightedChar");
            _moveSequence_char.Kill();
            _moveSequence_char = DOTween.Sequence();

            for (int i = 0; i < m_TextLength; i++)
            {
                m_SplitChar.textArray[i].transform.localPosition = pos;
                switch (m_SplitChar.m_Direction)
                {

                    case Direction.GoUp://TODO: 上射動畫
                        if (i <= (m_TextLength - 1) / 2)
                        {
                            _moveSequence_char.Append(m_SplitChar.textArray[i].transform
                                                .DOLocalMove(new Vector3(pos.x + m_SplitChar.ToX, pos.y + m_SplitChar.ToY), m_SplitChar.m_Duration / 2)
                                                .SetEase(m_current_easetype.GetEasetype(m_SplitChar.m_Easetype)))

                                              .Join(m_SplitChar.textArray[(m_TextLength - 1) - i].transform
                                                .DOLocalMove(new Vector3(pos.x + m_SplitChar.ToX, pos.y + m_SplitChar.ToY), m_SplitChar.m_Duration / 2)
                                                .SetEase(m_current_easetype.GetEasetype(m_SplitChar.m_Easetype)))

                                              .Append((m_SplitChar.textArray[i].transform
                                                .DOLocalMove(new Vector3(pos.x + m_SplitChar.ToX + (-(m_SplitChar.m_Text.characterSpacing * (((m_TextLength - 1) / 2) - i + 0.5f))), pos.y + m_SplitChar.ToY), m_SplitChar.m_Duration))
                                                .SetEase(m_current_easetype.GetEasetype(m_SplitChar.m_Easetype)))

                                              .Join((m_SplitChar.textArray[(m_TextLength - 1) - i].transform
                                                .DOLocalMove(new Vector3(pos.x + m_SplitChar.ToX + (m_SplitChar.m_Text.characterSpacing * (((m_TextLength - 1) / 2) - i + 0.5f)), pos.y + m_SplitChar.ToY), m_SplitChar.m_Duration))
                                                .SetEase(m_current_easetype.GetEasetype(m_SplitChar.m_Easetype)));

                            charIdlePostion[i] = new Vector3(pos.x + m_SplitChar.ToX + (-(m_SplitChar.m_Text.characterSpacing * (((m_TextLength - 1) / 2) - i + 0.5f))), pos.y + m_SplitChar.ToY);
                            charIdlePostion[(m_TextLength - 1) - i] = new Vector3(pos.x + m_SplitChar.ToX + (m_SplitChar.m_Text.characterSpacing * (((m_TextLength - 1) / 2) - i + 0.5f)), pos.y + m_SplitChar.ToY);
                        }
                        break;

                    case Direction.GoDown://TODO: 下射動畫
                        if (i <= (m_TextLength - 1) / 2)
                        {
                            _moveSequence_char.Append(m_SplitChar.textArray[i].transform
                                                .DOLocalMove(new Vector3(pos.x + m_SplitChar.ToX, pos.y + m_SplitChar.ToY), m_SplitChar.m_Duration / 2)
                                                .SetEase(m_current_easetype.GetEasetype(m_SplitChar.m_Easetype)))

                                              .Join(m_SplitChar.textArray[(m_TextLength - 1) - i].transform
                                                .DOLocalMove(new Vector3(pos.x + m_SplitChar.ToX, pos.y + m_SplitChar.ToY), m_SplitChar.m_Duration / 2)
                                                .SetEase(m_current_easetype.GetEasetype(m_SplitChar.m_Easetype)))

                                              .Append((m_SplitChar.textArray[i].transform
                                                .DOLocalMove(new Vector3(pos.x + m_SplitChar.ToX + (-(m_SplitChar.m_Text.characterSpacing * (((m_TextLength - 1) / 2) - i + 0.5f))), pos.y + m_SplitChar.ToY), m_SplitChar.m_Duration))
                                                .SetEase(m_current_easetype.GetEasetype(m_SplitChar.m_Easetype)))

                                              .Join((m_SplitChar.textArray[(m_TextLength - 1) - i].transform
                                                .DOLocalMove(new Vector3(pos.x + m_SplitChar.ToX + (m_SplitChar.m_Text.characterSpacing * (((m_TextLength - 1) / 2) - i + 0.5f)), pos.y + m_SplitChar.ToY), m_SplitChar.m_Duration))
                                                .SetEase(m_current_easetype.GetEasetype(m_SplitChar.m_Easetype)));

                            charIdlePostion[i] = new Vector3(pos.x + m_SplitChar.ToX + (-(m_SplitChar.m_Text.characterSpacing * (((m_TextLength - 1) / 2) - i + 0.5f))), pos.y + m_SplitChar.ToY);
                            charIdlePostion[(m_TextLength - 1) - i] = new Vector3(pos.x + m_SplitChar.ToX + (m_SplitChar.m_Text.characterSpacing * (((m_TextLength - 1) / 2) - i + 0.5f)), pos.y + m_SplitChar.ToY);
                        }
                        break;

                    case Direction.GoLeft:
                        _moveSequence_char.Append(m_SplitChar.textArray[i].transform
                                            .DOLocalMove(new Vector3(pos.x + m_SplitChar.ToX + (-(m_SplitChar.m_Text.characterSpacing * (m_TextLength - 1 - i))), pos.y + m_SplitChar.ToY), m_SplitChar.m_Duration)
                                            .SetEase(m_current_easetype.GetEasetype(m_SplitChar.m_Easetype)));

                        charIdlePostion[i] = new Vector3(pos.x + m_SplitChar.ToX + (-(m_SplitChar.m_Text.characterSpacing * (m_TextLength - 1 - i))), pos.y + m_SplitChar.ToY);
                        break;

                    case Direction.GoRight:
                        _moveSequence_char.Append(m_SplitChar.textArray[(m_TextLength - 1) - i].transform
                                            .DOLocalMove(new Vector3(pos.x + m_SplitChar.ToX + (m_SplitChar.m_Text.characterSpacing * ((m_TextLength - 1) - i)), pos.y + m_SplitChar.ToY), m_SplitChar.m_Duration)
                                            .SetEase(m_current_easetype.GetEasetype(m_SplitChar.m_Easetype)));

                        charIdlePostion[(m_TextLength - 1) - i] = new Vector3(pos.x + m_SplitChar.ToX + (m_SplitChar.m_Text.characterSpacing * ((m_TextLength - 1) - i)), pos.y + m_SplitChar.ToY);
                        break;

                    default:
                        break;
                }
            }
        }

        public void IdleChar(SplitChar m_SplitChar)
        {
            //Debug.Log("IdleChar");
            _moveSequence_char.Kill();
            _moveSequence_char = DOTween.Sequence();

            for (int i = 0; i < m_TextLength; i++)
            {
                switch (m_SplitChar.m_Direction)
                {
                    case Direction.GoUp:
                        if (i <= (m_TextLength - 1) / 2)
                        {
                            _moveSequence_char.Append(m_SplitChar.textArray[((m_TextLength - 1) / 2) - i].transform
                                                .DOLocalMove(new Vector3(charIdlePostion[((m_TextLength - 1) / 2) - i].x + m_SplitChar.ToX, charIdlePostion[((m_TextLength - 1) / 2) - i].y + m_SplitChar.ToY * m_SplitChar.m_Outdistance), m_SplitChar.m_Duration)
                                                .SetEase(m_current_easetype.GetEasetype(m_SplitChar.m_Easetype)))

                                              .Join(m_SplitChar.textArray[((m_TextLength - 1) + i) - ((m_TextLength - 1) / 2)].transform
                                                .DOLocalMove(new Vector3(charIdlePostion[((m_TextLength - 1) + i) - ((m_TextLength - 1) / 2)].x + m_SplitChar.ToX, charIdlePostion[((m_TextLength - 1) + i) - ((m_TextLength - 1) / 2)].y + m_SplitChar.ToY * m_SplitChar.m_Outdistance), m_SplitChar.m_Duration)
                                                .SetEase(m_current_easetype.GetEasetype(m_SplitChar.m_Easetype)));

                        }
                        break;

                    case Direction.GoDown:
                        if (i <= (m_TextLength - 1) / 2)
                        {
                            _moveSequence_char.Append(m_SplitChar.textArray[((m_TextLength - 1) / 2) - i].transform
                                                .DOLocalMove(new Vector3(charIdlePostion[((m_TextLength - 1) / 2) - i].x + m_SplitChar.ToX, charIdlePostion[((m_TextLength - 1) / 2) - i].y + m_SplitChar.ToY * m_SplitChar.m_Outdistance), m_SplitChar.m_Duration)
                                                .SetEase(m_current_easetype.GetEasetype(m_SplitChar.m_Easetype)))

                                              .Join(m_SplitChar.textArray[((m_TextLength - 1) + i) - ((m_TextLength - 1) / 2)].transform
                                                .DOLocalMove(new Vector3(charIdlePostion[((m_TextLength - 1) + i) - ((m_TextLength - 1) / 2)].x + m_SplitChar.ToX, charIdlePostion[((m_TextLength - 1) + i) - ((m_TextLength - 1) / 2)].y + m_SplitChar.ToY * m_SplitChar.m_Outdistance), m_SplitChar.m_Duration)
                                                .SetEase(m_current_easetype.GetEasetype(m_SplitChar.m_Easetype)));
                        }
                        break;

                    case Direction.GoLeft:
                        _moveSequence_char.Append(m_SplitChar.textArray[i].transform
                                            .DOLocalMove(new Vector3(charIdlePostion[i].x + (m_SplitChar.ToX * m_SplitChar.m_Outdistance) + (-(m_SplitChar.m_Text.characterSpacing * (m_TextLength - 1 - i))), charIdlePostion[i].y + m_SplitChar.ToY), m_SplitChar.m_Duration)
                                            .SetEase(m_current_easetype.GetEasetype(m_SplitChar.m_Easetype)));
                        break;

                    case Direction.GoRight:
                        _moveSequence_char.Append(m_SplitChar.textArray[(m_TextLength - 1) - i].transform
                                            .DOLocalMove(new Vector3(charIdlePostion[(m_TextLength - 1) - i].x + (m_SplitChar.ToX * m_SplitChar.m_Outdistance) + (m_SplitChar.m_Text.characterSpacing * ((m_TextLength - 1) - i)), charIdlePostion[(m_TextLength - 1) - i].y + m_SplitChar.ToY), m_SplitChar.m_Duration)
                                            .SetEase(m_current_easetype.GetEasetype(m_SplitChar.m_Easetype)));
                        break;

                    default:
                        break;
                }
            }
        }
    }
}