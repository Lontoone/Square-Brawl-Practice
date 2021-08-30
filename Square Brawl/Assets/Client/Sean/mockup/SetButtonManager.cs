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

    [SerializeField] private bool m_UseReadyButton = false;
    [SerializeField] private ReadyType readyType;
    [SerializeField] private bool m_UseGamePadBackButton = false;

    [SerializeField] private Button backButton;
    public UnityEvent GamePadBackEvent;

    private PlayerInputManager input;
    //private MenuReadyButton localReadyButton;
    private bool isReady = false;

    public enum ReadyType
    { 
        DefaultReady,
        ColorReady,
        WeaponReady
    }

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
                SetReadyByType(child, isReady);
                Debug.Log(child.player.NickName +" isReady == "+ isReady);
            }
        }
    }

    private void SetReadyByType(MenuReadyButton _btn, bool _isReady)
    {
        switch (readyType)
        {
            case ReadyType.DefaultReady:
                _btn.SetReady(_isReady);
                break;

            case ReadyType.ColorReady:
                _btn.ColorSetReady(_isReady);
                break;

            case ReadyType.WeaponReady:
                _btn.WeaponSetReady(_isReady);
                break;
        }
    }

    private void BackByGamePad()
    {
        AudioSourcesManager.PlaySFX(1);
        GamePadBackEvent?.Invoke();
    }
}
