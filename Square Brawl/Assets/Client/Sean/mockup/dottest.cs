using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ToDotSlider { }
public class dottest : MonoBehaviour
{
    [SerializeField]private GameObject m_Object;
    public ToDotSlider.DotSliderAction.DotSlider slider;
    [HideInInspector]
    public ToDotSlider.DotSliderAction DotSliderAction;
    // Start is called before the first frame update
    /*void Start()
    {
        DotSliderAction = m_Object.AddComponent<ToDotSlider.DotSliderAction>();
        DotSliderAction.SetUp(slider);
    }*/
}
