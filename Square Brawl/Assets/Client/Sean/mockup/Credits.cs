using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;

public class Credits : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler, IPointerClickHandler, ISubmitHandler
{
    private Button button;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Animator animator;

    private Sequence sequence;

    private void EnterAnimation()
    {
        sequence.Kill();
        sequence = DOTween.Sequence();
        sequence.Append(text
                    .DOColor(new Color32(90, 169, 246, 255), 0.3f)
                    .SetEase(Ease.OutCirc));
    }

    private void ExitAnimation()
    {
        sequence.Kill();
        sequence = DOTween.Sequence();
        sequence.Append(text
                    .DOColor(new Color32(183, 183, 183, 255), 0.3f)
                    .SetEase(Ease.OutCirc));
    }

    private void OnClickAnimation()
    {
        sequence.Kill();
        sequence = DOTween.Sequence();
        sequence.Append(text.transform
                    .DOPunchScale(new Vector2(0.05f, 0.05f), 0.5f)
                    .SetEase(Ease.OutCirc));
        if (OptionSetting.TRANSITIONANIMATION)
        {
            animator.Play("EnterCredit");
        }
        else
        {
            animator.Play("NoneCredit");
        }
    }

    public void OnBackAnimation()
    {
        if (OptionSetting.TRANSITIONANIMATION)
        {
            animator.Play("ExitCredit");
        }
        else
        {
            animator.Play("None");
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClickAnimation();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        EnterAnimation();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ExitAnimation();
    }

    public void OnSelect(BaseEventData eventData)
    {
        EnterAnimation();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        ExitAnimation();
    }

    public void OnSubmit(BaseEventData eventData)
    {
        OnClickAnimation();
    }
}
