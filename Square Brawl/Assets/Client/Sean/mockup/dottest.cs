using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ToDotSlider { }
public class dottest : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    [SerializeField]private GameObject m_Object;
    public ToDotSlider.DotSliderAction.DotSlider slider;
    [HideInInspector]
    public ToDotSlider.DotSliderAction DotSliderAction;
    // Start is called before the first frame update
    void Start()
    {
        DotSliderAction = new ToDotSlider.DotSliderAction();
        //DotSliderAction = m_Object.AddComponent<ToDotSlider.DotSliderAction>();
        slider = DotSliderAction.SetUp(slider);
    }

    private void Update()
    {
        DotSliderAction.OnLoad(slider);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        slider.onSelect = true;
        Debug.Log("in");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        slider.onSelect = false;
        Debug.Log("out");
    }

    public void OnSelect(BaseEventData eventData)
    {
        slider.onSelect = true;
    }
    public void OnDeselect(BaseEventData eventData)
    {
        slider.onSelect = false;
    }
}
