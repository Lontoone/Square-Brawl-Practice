using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;

public class ESCButtonAction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    [SerializeField] private TextMeshProUGUI text;

    private Sequence sequence;

    private void OnAnimation()
    {
        sequence.Kill();
        sequence = DOTween.Sequence();
        sequence.Append(text.DOFade(0.6f,0.3f));
    }

    public void OffAnimation()
    {
        sequence.Kill();
        sequence = DOTween.Sequence();
        sequence.Append(text.DOFade(0, 0.3f));
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnAnimation();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OffAnimation();
    }

    public void OnSelect(BaseEventData eventData)
    {
        OnAnimation();
    }
    public void OnDeselect(BaseEventData eventData)
    {
        OffAnimation();
    }
}
