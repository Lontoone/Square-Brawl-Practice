using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class MenuButtonHandler : MonoBehaviour
{
    [SerializeField] private GameObject m_ButtonHandler;
    [SerializeField] private GameObject m_FirstSelectedButton;
    [SerializeField] private GameObject m_PlayButton;
    [SerializeField] private GameObject m_ButtonUp;
    [SerializeField] private GameObject m_ButtonDown;
    [SerializeField] private GameObject m_ButtonLeft;
    [SerializeField] private GameObject m_ButtonRight;
    [HideInInspector]
    public static int m_CurrentIndex;

    [Space (10)]
    [SerializeField] private float m_Moveduration = 0.5f;





    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private int CurrentSelectedButton(ButtonAction m_Button) 
    {
        Debug.Log("CurrentSelectedButton");
        return m_Button.m_ButtonIndex;
    }

    public void EnableButton()
    {
        m_ButtonUp.GetComponent<Button>().interactable = true;
        m_ButtonDown.GetComponent<Button>().interactable = true;
        m_ButtonLeft.GetComponent<Button>().interactable = true;
        m_ButtonRight.GetComponent<Button>().interactable = true;
        m_PlayButton.GetComponent<Button>().interactable = true;
        m_ButtonUp.SetActive(true);
        m_ButtonDown.SetActive(true);
        m_ButtonLeft.SetActive(true);
        m_ButtonRight.SetActive(true);
        m_PlayButton.SetActive(true);
    }
    public void DelayDisableButton() 
    {
        StartCoroutine(DelayDisable());
    }
    private IEnumerator DelayDisable()
    {
        m_ButtonUp.GetComponent<Button>().interactable = false;
        m_ButtonDown.GetComponent<Button>().interactable = false;
        m_ButtonLeft.GetComponent<Button>().interactable = false;
        m_ButtonRight.GetComponent<Button>().interactable = false;
        m_PlayButton.GetComponent<Button>().interactable = false;
        yield return new WaitForSeconds(m_Moveduration);
        m_ButtonUp.SetActive(false);
        m_ButtonDown.SetActive(false);
        m_ButtonLeft.SetActive(false);
        m_ButtonRight.SetActive(false);
        m_PlayButton.SetActive(false);
    }
}
