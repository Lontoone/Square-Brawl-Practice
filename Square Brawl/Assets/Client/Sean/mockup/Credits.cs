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
    [SerializeField] private GameObject credit;
    [SerializeField] private GameObject creditButton;

    private GameObject lastSelectObject;
    private Sequence sequence;

    private bool inCredit = false;

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

    private IEnumerator OnClickAnimation()
    {
        lastSelectObject = EventSystem.current.currentSelectedGameObject;
        EventSystem.current.SetSelectedGameObject(null);
        credit.SetActive(true);
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
        yield return new WaitUntil(() => animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Idle");
        inCredit = true;
        EventSystem.current.SetSelectedGameObject(creditButton);
    }

    public void BackToMenu()
    {
        if (inCredit == true)
        {
            StartCoroutine(OnBackAnimation());
        }
    }

    public IEnumerator OnBackAnimation()
    {
        var time = 0f;
        EventSystem.current.SetSelectedGameObject(null);

        if (OptionSetting.TRANSITIONANIMATION)
        {
            animator.Play("ExitCredit");
            time = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        }
        else
        {
            animator.Play("None");
        }
        yield return new WaitForSeconds(time);
        credit.SetActive(false);
        EventSystem.current.SetSelectedGameObject(lastSelectObject);

        inCredit = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (inCredit == false)
        {
            StartCoroutine(OnClickAnimation());
        }
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
        if (inCredit == false)
        {
            StartCoroutine(OnClickAnimation());
        }
    }
}
