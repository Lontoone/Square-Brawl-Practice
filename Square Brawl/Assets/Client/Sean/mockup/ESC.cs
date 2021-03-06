using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ESC : MonoBehaviour
{
    [SerializeField] private GameObject buttonGroup;
    [SerializeField] private GameObject option;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject roomPrefab;
    [SerializeField] private GameObject colorSelectionPrefab;
    [SerializeField] private GameObject weaponSelectionPrefab;

    public static ESC instance;
    private bool InOption = false;

    private GameObject lastSelectedObject;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Update()
    {
        var gamepadStartButtonWasPressed = false;
        var gamepadEastButtonWasPressed = false;

        
        if (Gamepad.current != null)
        {
            if (Gamepad.current.startButton.wasPressedThisFrame)
            {
                gamepadStartButtonWasPressed = true;
            }
            else if (Gamepad.current.buttonEast.wasPressedThisFrame)
            {
                gamepadEastButtonWasPressed = true;
            }
        }
        if ((Keyboard.current.escapeKey.wasPressedThisFrame || gamepadStartButtonWasPressed) 
            && buttonGroup.activeSelf == false 
            && SceneHandler.inOption == false)
        {
            lastSelectedObject = EventSystem.current.currentSelectedGameObject;
            buttonGroup.SetActive(true);
            AudioSourcesManager.PlaySFX(0);

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
        else if ((Keyboard.current.escapeKey.wasPressedThisFrame || gamepadEastButtonWasPressed) 
                && buttonGroup.activeSelf == true 
                && InOption ==false && animator.GetCurrentAnimatorClipInfo(0)[0].clip.name != "EnterESC" 
                && SceneHandler.inOption == false)
        {
            BackOnClick();
        }
    }


    public void BackOnClick()
    {
        AudioSourcesManager.PlaySFX(1);
        StartCoroutine(Back());
    }

    private IEnumerator Back()
    {
        EventSystem.current.SetSelectedGameObject(lastSelectedObject);
        if (OptionSetting.TRANSITIONANIMATION)
        {
            animator.Play("ExitESC");
            foreach (ESCButtonAction action in GetComponentsInChildren<ESCButtonAction>())
            {
                action.OffAnimation();
            }
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
        AudioSourcesManager.PlaySFX(1);
        AudioSourcesManager.StopBGM();
        PhotonNetwork.Disconnect();
        StartCoroutine(MenuBackAction());
    }

    public void EnterOption()
    {
        AudioSourcesManager.PlaySFX(0);
        StartCoroutine(EnterOptionAnimation());
    }

    public void ExitOption()
    {
        StartCoroutine(ExitOptionAnimation());
    }

    public void ExitGame()
    {
        AudioSourcesManager.PlaySFX(1);
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
        InOption = true;

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
        var btn = buttonGroup.GetComponentInChildren<Button>().gameObject;
        EventSystem.current.SetSelectedGameObject(btn);
        InOption = false;
    }
}
