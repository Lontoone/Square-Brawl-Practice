using UnityEngine;
using UnityEngine.InputSystem;

public class aim_to_mouse : MonoBehaviour
{
    private PlayerInputManager _inputAction;
    [SerializeField] private GameObject m_aim_pos;
    [SerializeField] private GameObject m_aim_object;
    [SerializeField] private float distance;
    private Vector2 object_pos;
    private Vector2 _mouseWorldPos;


    private void Start()
    {
        
    }

    void Update()
    {
        object_pos = m_aim_pos.transform.position;
        Aim_to_mouse();
    }
    
    
    private void Aim_to_mouse() 
    {
        _mouseWorldPos = Mouse.current.position.ReadValue();
        Vector2 local_mouseWorldPos = _mouseWorldPos - object_pos;

        float vector_length = Vector3.Magnitude(local_mouseWorldPos);
        float _angle = Mathf.Atan2(local_mouseWorldPos.y, local_mouseWorldPos.x) * Mathf.Rad2Deg;
        m_aim_object.transform.position = new Vector3((local_mouseWorldPos.x / vector_length) * distance + object_pos.x, +(local_mouseWorldPos.y / vector_length) * distance + object_pos.y, 0);
        m_aim_object.transform.rotation = Quaternion.Euler(new Vector3(0, 0, _angle));

        /*Debug.Log("\nlocal_mouseWorldPos = " + Mouse.current.position.ReadValue() 
                + "\nobject_pos = " + object_pos 
                + "\naim_object.position = " + aim_object.transform.position 
                + "\nvector_length = " + vector_length + "\nangle = " + _angle);*/
    }

}
