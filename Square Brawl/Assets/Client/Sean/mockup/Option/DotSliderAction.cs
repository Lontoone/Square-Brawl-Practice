using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using BasicTools.ButtonInspector;
using TMPro;
using DG.Tweening;
namespace Easetype { }

namespace ToDotSlider
{
    public class DotSliderAction : MonoBehaviour
    {
        public enum MainColor {Green, Orange, Red, Blue}

        [System.Serializable]
        public struct DotSlider
        {
            [HideInInspector]
            public GameObject[] m_DotSlider;
            [HideInInspector]
            public GameObject m_SelectedDot;
            [SerializeField] public Transform m_Transform;
            [SerializeField] public Vector2 m_SelectedDotSize;
            [SerializeField] public Vector2 m_DotSize;
            [SerializeField] public Sprite m_DotImage;
            [SerializeField] public Sprite m_SelectedImage;
            [SerializeField] public MainColor m_MainColor;
            [HideInInspector] public Color32 m_Color;
            [SerializeField] public Color32 m_DefaultColor;
            [SerializeField] public int m_Series;
            [SerializeField] public Vector3 m_Distance;
            [HideInInspector] public bool onSelect;
            [SerializeField] public int m_SelectedIndex;
            [SerializeField] public int m_DefaultIndex;
            [HideInInspector] public bool m_IsChangebyDrag;
            [HideInInspector] public bool m_IsChangebyClick;
        }

        private Sequence m_Sequence;

        

       
        private bool firstInput = true;
        private bool press = false;

        public DotSlider SetUp(DotSlider dotSlider)
        {
            switch (dotSlider.m_MainColor)
            {
                case MainColor.Green:
                    dotSlider.m_Color = SceneHandler.green;
                    break;

                case MainColor.Orange:
                    dotSlider.m_Color = SceneHandler.orange;
                    break;

                case MainColor.Red:
                    dotSlider.m_Color = SceneHandler.red;
                    break;

                case MainColor.Blue:
                    dotSlider.m_Color = SceneHandler.blue;
                    break;

                default:
                    Debug.LogError("Null DotSlider color ");
                    break;
            }

            m_Sequence = DOTween.Sequence();
            dotSlider.m_DotSlider = Generate(dotSlider);

            //dotSlider.m_SelectedIndex = dotSlider.m_Series / 2;
            for (int i = 0; i <= dotSlider.m_DefaultIndex; i++)
            {
                Image[] comps = dotSlider.m_DotSlider[i].GetComponentsInChildren<Image>();
                comps[1].color = dotSlider.m_Color;
            }

            //Generate SelectedDot Image
            dotSlider.m_SelectedDot = new GameObject("Selected Dot");
            dotSlider.m_SelectedDot.AddComponent<Image>();
            dotSlider.m_SelectedDot.GetComponent<Image>().color = new Color32(dotSlider.m_DefaultColor.r, dotSlider.m_DefaultColor.g, dotSlider.m_DefaultColor.b,150);
            dotSlider.m_SelectedDot.GetComponent<RectTransform>().sizeDelta = dotSlider.m_SelectedDotSize;
            dotSlider.m_SelectedDot.AddComponent<CanvasGroup>();
            dotSlider.m_SelectedDot.AddComponent<DragHandler>();
            dotSlider.m_SelectedDot.GetComponent<DragHandler>().SetUp(dotSlider.m_SelectedDot,
                                                                      new Color32(dotSlider.m_Color.r, dotSlider.m_Color.g, dotSlider.m_Color.b ,150), 
                                                                      new Color32(dotSlider.m_DefaultColor.r, dotSlider.m_DefaultColor.g, dotSlider.m_DefaultColor.b, 150), 
                                                                      dotSlider.m_Series,
                                                                      dotSlider.m_DefaultIndex);
            dotSlider.m_SelectedDot = Instantiate(dotSlider.m_SelectedDot, dotSlider.m_DotSlider[dotSlider.m_DefaultIndex].transform.position, new Quaternion(0, 0, 0, 0), dotSlider.m_Transform);
            return dotSlider;
        }

        public GameObject[] Generate(DotSlider dotSlider)
        {
            GameObject[] Object = new GameObject[dotSlider.m_Series];
            GameObject[] m_Object = new GameObject[dotSlider.m_Series];
            GameObject image = new GameObject("dot image");
            image.AddComponent<Image>();
            image.GetComponent<Image>().color = dotSlider.m_DefaultColor;
            image.GetComponent<RectTransform>().sizeDelta = dotSlider.m_DotSize;

            for (int i = 0; i < dotSlider.m_Series; i++)
            {
                //Debug.Log(i);
                Object[i] = new GameObject();
                string index = (i+1).ToString();
                Object[i].name =  "Dot " + index;
                Object[i].AddComponent<Image>();
                Object[i].GetComponent<Image>().color = new Color32(0,0,0,0);
                Object[i].GetComponent<RectTransform>().sizeDelta = new Vector2(dotSlider.m_Distance.x,dotSlider.m_Distance.x);
                Object[i].AddComponent<DotEvent>();
                Object[i].GetComponent<DotEvent>().SetUp(Object[i], i,dotSlider.m_Color , dotSlider.m_DefaultColor);
                m_Object[i] = Instantiate(Object[i], dotSlider.m_Transform.position + dotSlider.m_Distance * ESC.instance.GetComponent<RectTransform>().lossyScale.x * i, new Quaternion(0,0,0,0), dotSlider.m_Transform);//TODO slider動態位置
                //Generate Small Dot
                Instantiate(image, m_Object[i].transform.position, new Quaternion(0, 0, 0, 0), m_Object[i].transform);
                Destroy(Object[i]);

            }
            Destroy(image);
            return m_Object;
        }

        public int OnLoad(DotSlider dotSlider)
        {
            if (dotSlider.onSelect == true)
            {
                //dotSlider.m_SelectedDot.GetComponent<DragHandler>().UpdateSetUp(dotSlider.m_DotSlider[0], dotSlider.m_DotSlider[dotSlider.m_Series - 1]);
                MoveSelected(dotSlider);
                Debug.Log("dotSlider.m_IsChangebyClick" + dotSlider.m_IsChangebyClick);
                if (dotSlider.m_IsChangebyClick == true)
                {
                    Debug.Log("dotSlider.m_IsChangebyClick" + dotSlider.m_IsChangebyClick);
                    for (int i = 0; i < dotSlider.m_Series; i++)
                    {
                        Image[] comps = dotSlider.m_DotSlider[i].GetComponentsInChildren<Image>();
                        if (i <= dotSlider.m_SelectedIndex)
                        {
                            comps[1].color = dotSlider.m_Color;
                        }
                        else
                        {
                            comps[1].color = dotSlider.m_DefaultColor;
                        }
                    }
                    m_Sequence.Kill();
                    m_Sequence = DOTween.Sequence();
                    m_Sequence.Append(dotSlider.m_SelectedDot.transform
                                 .DOMoveX(dotSlider.m_DotSlider[dotSlider.m_SelectedIndex].transform.position.x, 0.05f)
                                 .SetEase(Ease.OutBounce))
                              .Join(dotSlider.m_SelectedDot.GetComponent<Image>()
                                 .DOColor(new Color32(dotSlider.m_Color.r, dotSlider.m_Color.g, dotSlider.m_Color.b, 150), 0.15f)
                                 .SetEase(Ease.OutBounce));
                    dotSlider.m_IsChangebyClick = false;
                    dotSlider.onSelect = false;
                }
                else if (dotSlider.m_IsChangebyDrag == true)
                {
                    m_Sequence.Kill();
                    m_Sequence = DOTween.Sequence();
                    for (int i = 0; i < dotSlider.m_Series; i++)
                    {
                        Image[] comps = dotSlider.m_DotSlider[i].GetComponentsInChildren<Image>();
                        if (i <= dotSlider.m_SelectedIndex)
                        {
                            comps[1].color = dotSlider.m_Color;
                        }
                        else
                        {
                            comps[1].color = dotSlider.m_DefaultColor;
                        }
                    }
                }
            }
            return dotSlider.m_SelectedIndex;
        }


        //Select by Keyboard or Gamepad
        private void MoveSelected(DotSlider dotSlider)
        {
            if (Gamepad.current != null)
            {
                Vector2 vec = Gamepad.current.leftStick.ReadValue();

                //Debug.Log(vec);
                if (press == true)
                {
                    if (vec.x >= -0.5f && vec.x <= 0.5f)
                    {
                        press = false;
                        //Debug.Log("pres false");
                    }
                }
                if (press == false)
                {
                    if (vec.x <= -0.5f || vec.x >= 0.5f)
                    {
                        firstInput = true;
                        press = true;
                        //Debug.Log("pres true");
                    }
                }

                while (firstInput == true)
                {
                    if (vec.x > 0.5f && dotSlider.m_SelectedIndex < dotSlider.m_Series)
                    {
                        dotSlider.m_SelectedIndex++;
                        if (dotSlider.m_SelectedIndex >= dotSlider.m_Series)
                        {
                            dotSlider.m_SelectedIndex = dotSlider.m_Series - 1;
                        }
                        //Debug.Log("++");
                        dotSlider.m_IsChangebyClick = true;
                    }
                    else if (vec.x < -0.5f && dotSlider.m_SelectedIndex >= 0)
                    {
                        dotSlider.m_SelectedIndex--;
                        if (dotSlider.m_SelectedIndex < 0)
                        {
                            dotSlider.m_SelectedIndex = 0;
                        }
                        //Debug.Log("--");
                        dotSlider.m_IsChangebyClick = true;
                    }
                    firstInput = false;
                }
            }
            if (Keyboard.current != null)
            {
                if ((Keyboard.current.rightArrowKey.wasPressedThisFrame || Keyboard.current.dKey.wasPressedThisFrame) && dotSlider.m_SelectedIndex < dotSlider.m_Series)
                {
                    dotSlider.m_SelectedIndex++;
                    if (dotSlider.m_SelectedIndex >= dotSlider.m_Series)
                    {
                        dotSlider.m_SelectedIndex = dotSlider.m_Series - 1;
                    }
                    //Debug.Log("++");
                    dotSlider.m_IsChangebyClick = true;
                }
                else if ((Keyboard.current.leftArrowKey.wasPressedThisFrame || Keyboard.current.aKey.wasPressedThisFrame) && dotSlider.m_SelectedIndex >= 0)
                {
                    dotSlider.m_SelectedIndex--;
                    if (dotSlider.m_SelectedIndex < 0)
                    {
                        dotSlider.m_SelectedIndex = 0;
                    }
                    //Debug.Log("--");
                    dotSlider.m_IsChangebyClick = true;
                }
            }
            Debug.LogError("dotSlider.m_IsChangebyClick" + dotSlider.m_IsChangebyClick);
        }
    }
}

