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
    [SerializeField] private float m_Distance;
    [SerializeField] private Sprite[] m_Sprite; 

    private float distance;
    private Vector2 lastRes;
    private bool update = false;

    private float m_Angle;

    private Vector2 m_ObjectPos;
    private Vector2 m_MouseWorldPos;


    private void Start()
    {

    }

    private void FixedUpdate()
    {
        if(Screen.width != lastRes.x || Screen.height != lastRes.y)
        {
            update = true;
            lastRes.x = Screen.width;
            lastRes.y = Screen.height;
        }
        if (update)
        {
            update = false;
            distance = SceneHandler.WorldToCamera(m_Distance, 2);
        }
        m_ObjectPos = m_AimPos.transform.position;
        if (Mouse.current.wasUpdatedThisFrame)
        {
            AimToMouse();
        }
    }




    private void AimToMouse() 
    {
        m_MouseWorldPos = Mouse.current.position.ReadValue();
        Vector2 local_mouseWorldPos = m_MouseWorldPos - m_ObjectPos;

        float vector_length = Vector3.Magnitude(local_mouseWorldPos);
        m_Angle = Mathf.Atan2(local_mouseWorldPos.y, local_mouseWorldPos.x) * Mathf.Rad2Deg;
        m_AimObject.transform.position = new Vector3((local_mouseWorldPos.x / vector_length) * distance + m_ObjectPos.x, +(local_mouseWorldPos.y / vector_length) * distance + m_ObjectPos.y, 0);
        m_AimObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, m_Angle));
    }


    //Use in ButtonActions' Button Event Trigger 
    public void OnSelect(string direction)
    {
        switch (direction)
        {
            case "Up":
                Debug.Log("up");
                m_AimObject.transform.position = new Vector3(m_ObjectPos.x, m_ObjectPos.y + distance);
                m_AimObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                break;

            case "Down":
                Debug.Log("Down");
                m_AimObject.transform.position = new Vector3(m_ObjectPos.x, m_ObjectPos.y - distance);
                m_AimObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
                break;

            case "Left":
                Debug.Log("Left");
                m_AimObject.transform.position = new Vector3(m_ObjectPos.x - distance, m_ObjectPos.y);
                m_AimObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
                break;

            case "Right":
                Debug.Log("Right");
                m_AimObject.transform.position = new Vector3(m_ObjectPos.x + distance, m_ObjectPos.y);
                m_AimObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                break;

            default:
                Debug.LogError("Aim to mouse Error");
                break;
        }
    }

    private void ChangeSprite()
    {
        if (m_Angle < 45 && m_Angle > -45)
        {
            m_AimObject.GetComponent<Image>().sprite = m_Sprite[0];
        }
        else if (m_Angle > 45 && m_Angle < 135)
        {
            m_AimObject.GetComponent<Image>().sprite = m_Sprite[1];
        }
        else if (m_Angle > 135 || m_Angle < -135)
        {
            m_AimObject.GetComponent<Image>().sprite = m_Sprite[2];
        }
        else if (m_Angle < -45 && m_Angle > -135)
        {
            m_AimObject.GetComponent<Image>().sprite = m_Sprite[3];
        }


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
