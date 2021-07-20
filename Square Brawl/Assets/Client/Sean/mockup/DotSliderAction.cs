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
    public class DotSliderAction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, ISelectHandler, IDeselectHandler
    {
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
            [SerializeField] public Color32 m_DefaultColor;
            [SerializeField] public int m_Series;
            [SerializeField] public Vector3 m_Distance;
        }

        public void SetUp(DotSlider dotSlider)
        {
            dotSlider.m_DotSlider = Generate(dotSlider);

            dotSlider.m_SelectedDot = new GameObject();
            dotSlider.m_SelectedDot.AddComponent<Image>();
            dotSlider.m_SelectedDot.GetComponent<Image>().color = new Color32(dotSlider.m_DefaultColor.r, dotSlider.m_DefaultColor.g, dotSlider.m_DefaultColor.b,150);
            dotSlider.m_SelectedDot.GetComponent<RectTransform>().sizeDelta = dotSlider.m_SelectedDotSize;
            Instantiate(dotSlider.m_SelectedDot, dotSlider.m_Transform.position + (dotSlider.m_Distance * dotSlider.m_Series / 2), new Quaternion(0, 0, 0, 0), dotSlider.m_Transform);
        }

        public GameObject[] Generate(DotSlider dotSlider)
        {
            GameObject[] Object = new GameObject[dotSlider.m_Series];
            GameObject[] m_Object = new GameObject[dotSlider.m_Series];
            GameObject image = new GameObject();
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
                Object[i].AddComponent<EventTrigger>();
                //Object[i].GetComponent<EventTrigger>().OnPointerEnter();
                m_Object[i] = Instantiate(Object[i], dotSlider.m_Transform.position + dotSlider.m_Distance * i, new Quaternion(0,0,0,0), dotSlider.m_Transform);
                Instantiate(image, m_Object[i].transform.position, new Quaternion(0, 0, 0, 0), m_Object[i].transform);
                Object[i].SetActive(false);
            }
            image.SetActive(false);
            return m_Object;
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {

        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {

        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {

        }

        void ISelectHandler.OnSelect(BaseEventData eventData)
        {

        }

        void IDeselectHandler.OnDeselect(BaseEventData eventData)
        {

        }
    }
}

