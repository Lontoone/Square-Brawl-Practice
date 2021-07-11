using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using BasicTools.ButtonInspector;
using DG.Tweening;

public class AimAction : MonoBehaviour
{
    private PlayerInputManager _inputAction;
    [SerializeField] private GameObject m_aim_pos;
    [SerializeField] public GameObject m_aim_object;
    [SerializeField] private float distance;

    private Vector2 object_pos;
    private Vector2 _mouseWorldPos;


    private void Start()
    {
        
    }

    void Update()
    {
        object_pos = m_aim_pos.transform.position;
        AimToMouse();
    }
    
    
    private void AimToMouse() 
    {
        _mouseWorldPos = Mouse.current.position.ReadValue();
        Vector2 local_mouseWorldPos = _mouseWorldPos - object_pos;

        float vector_length = Vector3.Magnitude(local_mouseWorldPos);
        float _angle = Mathf.Atan2(local_mouseWorldPos.y, local_mouseWorldPos.x) * Mathf.Rad2Deg;
        m_aim_object.transform.position = new Vector3((local_mouseWorldPos.x / vector_length) * distance + object_pos.x, +(local_mouseWorldPos.y / vector_length) * distance + object_pos.y, 0);
        m_aim_object.transform.rotation = Quaternion.Euler(new Vector3(0, 0, _angle));
    }

    public static void AimFade(GameObject m_aim_object, float m_duration, Easetype.Current_easetype aim_current_Easetype, Easetype.Current_easetype.Easetype easetype)
    {
        m_aim_object.GetComponent<Image>().DOFade(0,m_duration).SetEase(aim_current_Easetype.GetEasetype(easetype));
    }

    public static void AimUnFade(GameObject m_aim_object, float m_duration, Easetype.Current_easetype aim_current_Easetype, Easetype.Current_easetype.Easetype easetype)
    {
        m_aim_object.GetComponent<Image>().DOFade(1, m_duration).SetEase(aim_current_Easetype.GetEasetype(easetype));
    }

}
