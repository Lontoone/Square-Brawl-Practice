using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class ReadyTipAction : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    private Animator animator;
    private bool gamePadOn = false;

    public static bool isReady = false;

    private void OnEnable()
    {
        animator = GetComponent<Animator>();
        animator.Play("Idle");
    }

    void Update()
    {
        SetAnimation();
    }

    private void SetAnimation()
    {
        if (isReady == true)
        {
            text.text = SetButtonManager.readyTipText;
            if (Gamepad.current != null)
            {
                if (Gamepad.current.wasUpdatedThisFrame)
                {
                    gamePadOn = true;
                }
                else if (Keyboard.current.wasUpdatedThisFrame)
                {
                    gamePadOn = false;
                }
            }
            else
            {
                gamePadOn = false;
            }

            if (gamePadOn == true)
            {
                GamePadAnimation();
            }
            else
            {
                KeyBoardAnimation();
            }
        }
    }

    private void GamePadAnimation()
    {
        animator.Play("GamePad");
    }

    private void KeyBoardAnimation()
    {
        animator.Play("KeyBoard");
    }
}
