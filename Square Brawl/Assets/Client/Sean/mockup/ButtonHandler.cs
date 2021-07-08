using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHandler : MonoBehaviour
{
    [SerializeField] private ButtonAction m_ButtonUp;
    [SerializeField] private ButtonAction m_ButtonDown;
    [SerializeField] private ButtonAction m_ButtonLeft;
    [SerializeField] private ButtonAction m_ButtonRight;
    [SerializeField] private ButtonAction m_PlayButton;






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



}
