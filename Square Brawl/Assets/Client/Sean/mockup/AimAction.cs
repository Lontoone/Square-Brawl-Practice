using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using BasicTools.ButtonInspector;
using DG.Tweening;

public class AimAction : MonoBehaviour
{
    private PlayerInputManager _inputAction;
    [SerializeField] private GameObject m_AimPos;
    [SerializeField] public GameObject m_AimObject;
    [SerializeField] private float distance;

    private Vector2 m_ObjectPos;
    private Vector2 m_MouseWorldPos;


    private void Start()
    {
        
    }

    void Update()
    {
        m_ObjectPos = m_AimPos.transform.position;
        AimToMouse();
    }
    
    
    private void AimToMouse() 
    {
        m_MouseWorldPos = Mouse.current.position.ReadValue();
        Vector2 local_mouseWorldPos = m_MouseWorldPos - m_ObjectPos;

        float vector_length = Vector3.Magnitude(local_mouseWorldPos);
        float _angle = Mathf.Atan2(local_mouseWorldPos.y, local_mouseWorldPos.x) * Mathf.Rad2Deg;
        m_AimObject.transform.position = new Vector3((local_mouseWorldPos.x / vector_length) * distance + m_ObjectPos.x, +(local_mouseWorldPos.y / vector_length) * distance + m_ObjectPos.y, 0);
        m_AimObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, _angle));
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
