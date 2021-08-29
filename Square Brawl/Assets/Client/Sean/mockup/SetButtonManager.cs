using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class SetButtonManager : MonoBehaviour
{
    [SerializeField] private GameObject firstSelectedButton;

    public bool m_UseReadyButton = false;

    private PlayerInputManager input;
    //private MenuReadyButton localReadyButton;
    private bool isReady = false;

    private void Awake()
    {
        if (m_UseReadyButton == true)
        {
            input = new PlayerInputManager();
            input.UI.readyclick.performed += ctx => ReadyByKey();
            Debug.Log("Set key");
        }
    }

    private void OnEnable()
    {
        if (m_UseReadyButton == true)
        {
            input.Enable();
        }
        EventSystem.current.SetSelectedGameObject(firstSelectedButton); //todo text split error
    }

    private void OnDisable()
    {
        if (m_UseReadyButton == true)
        {
            input.Disable();
        }
    }

    private void ReadyByKey()
    {
        Debug.Log("Set ready");
        var obj = GetComponentsInChildren<MenuReadyButton>();
        foreach (MenuReadyButton child in obj)
        {
            if (child.isLocal == true)
            {
                isReady = !isReady;
                child.SetReady(isReady);
                Debug.Log(isReady);
            }
        }
    }
}
