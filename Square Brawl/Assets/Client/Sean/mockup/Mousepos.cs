using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Mousepos : MonoBehaviour
{
    private PlayerInputManager _inputAction;
    [SerializeField] private GameObject m_mouse;
    public Vector2 pos;

    void Awake()
    {
        
    }
    void Update()
    {
        pos = Camera.main.ScreenToViewportPoint(Mouse.current.position.ReadValue());
        //Debug.Log(pos);
        m_mouse.transform.position =new Vector3(pos.x*Screen.width , pos.y*Screen.height,0);
    }
}
