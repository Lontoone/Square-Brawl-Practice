using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ESC : MonoBehaviour
{
    [SerializeField] private GameObject buttonGroup;
    [SerializeField] private GameObject option;
    [SerializeField] private Animator animator;

    private void Update()
    {
        var gamepadStartButtonWasPressed = false;
        if (Gamepad.current != null)
        {
            if (Gamepad.current.startButton.wasPressedThisFrame)
            {
                gamepadStartButtonWasPressed = true;
            }
        }
        if ((Keyboard.current.escapeKey.wasPressedThisFrame || gamepadStartButtonWasPressed) && buttonGroup.activeSelf == false)
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
        else if ((Keyboard.current.escapeKey.wasPressedThisFrame || gamepadStartButtonWasPressed) && buttonGroup.activeSelf == true)
        {
            BackOnClick();
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

    public void BackToMenu()
    {
        PhotonNetwork.Disconnect();
        StartCoroutine(MenuBackAction());
    }

    public void EnterOption()
    {
        StartCoroutine(EnterOptionAnimation());
    }

    public void ExitOption()
    {
        StartCoroutine(ExitOptionAnimation());
    }

    public void ExitGame()
    {
        Debug.Log("Quitting");
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif

        Application.Quit();
    }

    private IEnumerator MenuBackAction()
    {
        yield return new WaitWhile(() => { return PhotonNetwork.IsConnected; });
        Debug.Log("Disconnected");
        SceneManager.LoadScene(0);
    }

    private IEnumerator EnterOptionAnimation()
    {
        var time = 0f;
        if (OptionSetting.TRANSITIONANIMATION)
        {
            time = 1f;
        }
        else
        {
            time = 0f;
        }
        option.SetActive(true);
        yield return null;
        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(null);
        buttonGroup.transform
            .DOLocalMove(new Vector3(buttonGroup.transform.localPosition.x - 1920, buttonGroup.transform.localPosition.y, 0), time)
            .SetEase(Ease.OutQuint);
        option.transform
            .DOLocalMove(new Vector3(0, option.transform.localPosition.y, 0), time)
            .SetEase(Ease.OutQuint);
        StartCoroutine(option.GetComponentInChildren<OptionManager>().EnterAnimation(time));
    }

    private IEnumerator ExitOptionAnimation()
    {
        var time = 0f;
        if (OptionSetting.TRANSITIONANIMATION)
        {
            time = 1f;
        }
        else
        {
            time = 0f;
        }
        EventSystem.current.SetSelectedGameObject(null);
        buttonGroup.transform
            .DOLocalMove(new Vector3(0, buttonGroup.transform.localPosition.y, 0), time)
            .SetEase(Ease.OutQuint);
        option.transform
            .DOLocalMove(new Vector3(1920, option.transform.localPosition.y, 0), time)
            .SetEase(Ease.OutQuint);
        option.GetComponentInChildren<OptionManager>().ExitAnimation();
        yield return new WaitForSeconds(time / 3);
        option.SetActive(false);
    }

}
