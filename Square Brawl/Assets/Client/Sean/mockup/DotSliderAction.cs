using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BasicTools.ButtonInspector;
using TMPro;
using DG.Tweening;
namespace Easetype { }

namespace ToDotSlider
{
    public class DotSliderAction : MonoBehaviour
    {
        [System.Serializable]
        public struct DotSlider
        {
            [HideInInspector]
            public GameObject[] m_DotSlider;
            [SerializeField] public Transform m_Transform;
            [SerializeField] public Sprite m_Dot;
            [SerializeField] public Sprite m_SelectedDot;
            [SerializeField] public Color32 color;
            [SerializeField] public int m_Series;
            [SerializeField] public Vector3 m_Distance;
        }

        public void SetUp(DotSlider dotSlider)
        {
            dotSlider.m_DotSlider = Generate(dotSlider);
        }

        public GameObject[] Generate(DotSlider dotSlider)
        {
            GameObject[] Object = new GameObject[dotSlider.m_Series];
            GameObject[] m_Object = new GameObject[dotSlider.m_Series];
            for (int i = 0; i < dotSlider.m_Series; i++)
            {
                Debug.Log(i);
                Object[i].AddComponent<Image>();
                Object[i].GetComponent<Image>().sprite = dotSlider.m_Dot;
                Object[i].transform.localPosition += i * dotSlider.m_Distance;
                m_Object[i] =Instantiate(Object[i], dotSlider.m_Transform);
            }
            return m_Object;
        }
    }
}

