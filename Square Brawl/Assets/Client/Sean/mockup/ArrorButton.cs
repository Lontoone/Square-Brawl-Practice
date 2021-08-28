using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class ArrorButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    private Animator animator;
    private Button button;
    private bool inButton = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        button = GetComponent<Button>();
        button.onClick.AddListener(delegate { OnPress(); });
    }

    private void OnPress()
    {
        AudioSourcesManager.PlaySFX(2);

        animator.Play("Pressed");
    }

    private void Highlighted()
    {
        animator.Play("Highlighted");
    }

    private IEnumerator Idle()
    {
        //yield return new WaitUntil(() => animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Pressed");
        yield return null;
        animator.Play("Exit");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioSourcesManager.PlaySFX(2);

        inButton = true;
        Highlighted();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StartCoroutine(Idle());
        inButton = false;
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void OnSelect(BaseEventData eventData)
    {
        inButton = true;
        Highlighted();
    }
    public void OnDeselect(BaseEventData eventData)
    {
        StartCoroutine(Idle());
        inButton = false;
    }
}
