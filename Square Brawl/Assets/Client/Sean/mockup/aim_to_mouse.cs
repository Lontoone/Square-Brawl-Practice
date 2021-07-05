using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class aim_to_mouse : MonoBehaviour
{
    private PlayerInputManager _inputAction;
    [SerializeField] private GameObject aim_object;
    [SerializeField] private float distance;
    private Vector2 object_pos;
    private Vector2 _mouseWorldPos;


    private void Start()
    {
        object_pos = aim_object.transform.position;
    }

    void Update()
    {
        Aim_to_mouse();
        //MouseSpin();
    }

    
    private void Aim_to_mouse() 
    {
        Vector2 local_mouseWorldPos = new Vector2(_mouseWorldPos.x - 0.5f, _mouseWorldPos.y - 0.5f);//todo object_pos
        float vector_length = Vector3.Magnitude(local_mouseWorldPos);
        float _angle = Mathf.Atan2(local_mouseWorldPos.y, local_mouseWorldPos.x) * Mathf.Rad2Deg;

        _mouseWorldPos = Camera.main.ScreenToViewportPoint(Mouse.current.position.ReadValue());
        aim_object.transform.position = new Vector3((local_mouseWorldPos.x / vector_length) * distance + object_pos.x, +(local_mouseWorldPos.y / vector_length) * distance + object_pos.y, 0);
        aim_object.transform.rotation = Quaternion.Euler(new Vector3(0, 0, _angle));

        Debug.Log("aim_object.position = " + aim_object.transform.position + "\nvector_length = " + vector_length + "\nangle = " + _angle);
    }

    

    /*
    void MouseSpin()
    {
        Vector2 _mouseWorldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 _targetDir = _mouseWorldPos - new Vector2(aim_object.transform.position.x, aim_object.transform.position.y);
        float _angle = Mathf.Atan2(_targetDir.y, _targetDir.x) * Mathf.Rad2Deg;
        aim_object.transform.rotation = Quaternion.Euler(new Vector3(0, 0, _angle));

        Debug.Log("_mouseWorldPos = " + _mouseWorldPos + "\n_targetDir = " + _targetDir + "\n aim_object.transform.rotation " + aim_object.transform.rotation);
    }
    */

    /*
    
    テンのscript

    private PlayerInputManager _inputAction;
    publice Transform 你要轉的物件
    private void Awake()
        {
            _inputAction = new PlayerInputManager();
            _inputAction.Player.MouseRotation.performed += ctx => MouseSpin(ctx.ReadValue<Vector2>());
        }

    void MouseSpin(Vector2 _mousePos)
        {
       　   Vector2 _mouseWorldPos = Camera.main.ScreenToWorldPoint(_mousePos);
            Vector2 _targetDir = _mouseWorldPos - new Vector2(ShootSpinMidPos.position.x, ShootSpinMidPos.position.y);
            float _angle = Mathf.Atan2(_targetDir.y, _targetDir.x) * Mathf.Rad2Deg;
            你要轉的物件.rotation = Quaternion.Euler(new Vector3(0, 0, _angle));
        }
    */
}
