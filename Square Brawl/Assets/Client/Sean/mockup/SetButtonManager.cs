using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
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

    public static string readyTipText;
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

        readyTipText = "To Ready";
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
                SetReadyByType(child);

                //Set ready tip text
                if (!isReady)
                {
                    readyTipText = "To Ready";
                }
                else
                {
                    readyTipText = "To UnReady";
                }
                Debug.Log(child.player.NickName +" isReady == "+ isReady);
            }
        }
    }

    private void SetReadyByType(MenuReadyButton _btn)
    {
        switch (readyType)
        {
            case ReadyType.DefaultReady:
                isReady = !isReady;
                _btn.SetReady(isReady);
                break;

            case ReadyType.ColorReady:
                if (PhotonNetwork.LocalPlayer.CustomProperties[CustomPropertyCode.TEAM_CODE] != null)
                {
                    isReady = !isReady;
                }
                _btn.SetReady(isReady);
                break;

            case ReadyType.WeaponReady:
                if (((int)PhotonNetwork.LocalPlayer.CustomProperties[CustomPropertyCode.WEAPON1CODE] != 0
                  && (int)PhotonNetwork.LocalPlayer.CustomProperties[CustomPropertyCode.WEAPON2CODE] != 0))
                {
                    isReady = !isReady;
                }
                _btn.SetReady(isReady);
                break;
        }
    }

    private void BackByGamePad()
    {
        AudioSourcesManager.PlaySFX(1);
        GamePadBackEvent?.Invoke();
    }

    public void ResetButton()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(firstSelectedButton);
        }
    }
}
