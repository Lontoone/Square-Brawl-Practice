using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHandler : MonoBehaviour
{
    [SerializeField] private GameObject m_ButtonHandler;
    [SerializeField] private GameObject m_PlayButton;
    [SerializeField] private GameObject m_ButtonUp;
    [SerializeField] private GameObject m_ButtonDown;
    [SerializeField] private GameObject m_ButtonLeft;
    [SerializeField] private GameObject m_ButtonRight;
    [Space (10)]
    [SerializeField] private float duration = 0.5f;





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
        yield return new WaitForSeconds(duration);
        m_ButtonUp.SetActive(false);
        m_ButtonDown.SetActive(false);
        m_ButtonLeft.SetActive(false);
        m_ButtonRight.SetActive(false);
        m_PlayButton.SetActive(false);
    }



}
