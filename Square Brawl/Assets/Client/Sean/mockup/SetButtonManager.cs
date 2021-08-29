using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class SetButtonManager : MonoBehaviour
{
    [SerializeField] private GameObject firstSelectedButton;

    public bool m_UseReadyButton = false;
    public bool m_UseGamePadBackButton = false;

    public Button backButton;
    public UnityEvent GamePadBackEvent;

    private PlayerInputManager input;
    //private MenuReadyButton localReadyButton;
    private bool isReady = false;

    private void Awake()
    {
        if (m_UseReadyButton == true || m_UseGamePadBackButton == true)
        {
            input = new PlayerInputManager();
            if (m_UseReadyButton == true)
            {
                input.UI.readyclick.performed += ctx => ReadyByKey();
            }
            else if (m_UseGamePadBackButton == true)
            {
                input.UI.gamepadbackclick.performed += ctx => BackByGamePad();
            }
        }
    }

    private void OnEnable()
    {
        if (m_UseReadyButton == true || m_UseGamePadBackButton == true)
        {
            input.Enable();
        }
        EventSystem.current.SetSelectedGameObject(firstSelectedButton); //todo text split error
    }

    private void OnDisable()
    {
        if (m_UseReadyButton == true || m_UseGamePadBackButton == true)
        {
            input.Disable();
        }
    }

    private void ReadyByKey()
    {
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

    private void BackByGamePad()
    {
        AudioSourcesManager.PlaySFX(1);
        GamePadBackEvent?.Invoke();
    }
}
