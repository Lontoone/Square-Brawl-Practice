using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ESC : MonoBehaviour
{
    [SerializeField] private GameObject buttonGroup;
    [SerializeField]private Animator animator;

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame && buttonGroup.activeSelf == false)
        {
            buttonGroup.SetActive(true);

            if (OptionSetting.TRANSITIONANIMATION)
            {
                animator.Play("EnterESC");
            }
            else
            {
                animator.Play("NoneESC");
            }
            Debug.Log("Esc Was Pressed");
        }
    }

    public void BackOnClick()
    {
        StartCoroutine(Back());
    }

    private IEnumerator Back()
    {
        if (OptionSetting.TRANSITIONANIMATION)
        {
            animator.Play("ExitESC");
        }
        else
        {
            animator.Play("None");
        }

        var time = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        yield return new WaitForSeconds(time);
        buttonGroup.SetActive(false);
    }

    public void ExitGame()
    {
        Debug.Log("Quitting");
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif

        Application.Quit();
    }
}
