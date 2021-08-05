using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class MenuButtonHandler : MonoBehaviour
{
    [SerializeField] private GameObject m_ButtonHandler;
    [SerializeField] public GameObject m_FirstSelectedButton;
    [SerializeField] private GameObject m_PlayButton;
    [SerializeField] private GameObject m_ButtonUp;
    [SerializeField] private GameObject m_ButtonDown;
    [SerializeField] private GameObject m_ButtonLeft;
    [SerializeField] private GameObject m_ButtonRight;
    [HideInInspector]
    public static int m_CurrentIndex;


    private int CurrentSelectedButton(ButtonAction m_Button)
    {
        Debug.Log("CurrentSelectedButton");
        return m_Button.m_ButtonIndex;
    }

    public void OnExitMenuAction()
    {
        m_ButtonUp.SetActive(false);
        m_ButtonDown.SetActive(false);
        m_ButtonLeft.SetActive(false);
        m_ButtonRight.SetActive(false);
        m_PlayButton.GetComponent<PlayButton>().OnExitMenuAction();
    }

    public void OnEnterMenuAction()
    {
        m_ButtonUp.SetActive(true);
        m_ButtonDown.SetActive(true);
        m_ButtonLeft.SetActive(true);
        m_ButtonRight.SetActive(true);
        EventSystem.current.SetSelectedGameObject(m_FirstSelectedButton);
    }

    public IEnumerator EnableButton(float duration)
    {
        m_ButtonUp.SetActive(true);
        m_ButtonDown.SetActive(true);
        m_ButtonLeft.SetActive(true);
        m_ButtonRight.SetActive(true);
        m_PlayButton.SetActive(true);
        yield return new WaitForSeconds(duration);
        m_ButtonUp.GetComponent<Button>().interactable = true;
        m_ButtonDown.GetComponent<Button>().interactable = true;
        m_ButtonLeft.GetComponent<Button>().interactable = true;
        m_ButtonRight.GetComponent<Button>().interactable = true;
        m_PlayButton.GetComponent<Button>().interactable = true;
    }

    public void DisableButton()
    {
        m_ButtonUp.GetComponent<Button>().interactable = false;
        m_ButtonDown.GetComponent<Button>().interactable = false;
        m_ButtonLeft.GetComponent<Button>().interactable = false;
        m_ButtonRight.GetComponent<Button>().interactable = false;
        m_PlayButton.GetComponent<Button>().interactable = false;
    }
}
