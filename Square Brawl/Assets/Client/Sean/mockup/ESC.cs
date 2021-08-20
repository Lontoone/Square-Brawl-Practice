using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ESC : MonoBehaviour
{
    [SerializeField] private GameObject buttonGroup;
    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            buttonGroup.SetActive(true);
            Debug.Log("Esc Was Pressed");
        }
    }

    public void ExitGame()
    {
        Debug.Log("Quitting");

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif

        Application.Quit();
    }
}
