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
    [SerializeField] private Image m_Icon;
    [SerializeField] private float m_Distance;
    [SerializeField] private Sprite[] m_Sprite; 

    private float distance;
    private Vector2 lastRes;
    private bool update = false;
    public static bool inButton;

    private float m_Angle;
    private string turnState = "None";

    private Vector2 m_ObjectPos;
    private Vector2 m_MouseWorldPos;

    private Sequence aimAnimation;

    private void Start()
    {
        m_Icon.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
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
            if (!inButton)
            {
                ChangeSprite();
            }
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

    public void OnExitMenuAction()
    {
        m_AimPos.SetActive(false);
    }

    public void OnEnterMenuAction()
    {
        m_AimPos.SetActive(true);
    }

    //Use in ButtonActions' Button Event Trigger 
    public void OnSelect(string direction)
    {
        switch (direction)
        {
            case "Up":
                m_AimObject.transform.position = new Vector3(m_ObjectPos.x, m_ObjectPos.y + distance);
                m_AimObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
                m_Icon.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -90));
                m_Icon.GetComponent<Image>().sprite = m_Sprite[1];
                aimAnimation.Kill();
                aimAnimation = DOTween.Sequence();
                aimAnimation.Append(m_Icon.GetComponent<Image>()
                               .DOColor(SceneHandler.orange, 0.2f)
                               .SetEase(Ease.OutQuad));
                break;

            case "Down":
                m_AimObject.transform.position = new Vector3(m_ObjectPos.x, m_ObjectPos.y - distance);
                m_AimObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
                m_Icon.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90));
                m_Icon.GetComponent<Image>().sprite = m_Sprite[3];
                aimAnimation.Kill();
                aimAnimation = DOTween.Sequence();
                aimAnimation.Append(m_Icon.GetComponent<Image>()
                               .DOColor(SceneHandler.red, 0.2f)
                               .SetEase(Ease.OutQuad));
                break;

            case "Left":
                m_AimObject.transform.position = new Vector3(m_ObjectPos.x - distance, m_ObjectPos.y);
                m_AimObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
                m_Icon.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 180));
                m_Icon.GetComponent<Image>().sprite = m_Sprite[2];
                aimAnimation.Kill();
                aimAnimation = DOTween.Sequence();
                aimAnimation.Append(m_Icon.GetComponent<Image>()
                               .DOColor(SceneHandler.blue, 0.2f)
                               .SetEase(Ease.OutQuad));
                break;

            case "Right":
                m_AimObject.transform.position = new Vector3(m_ObjectPos.x + distance, m_ObjectPos.y);
                m_AimObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                m_Icon.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                m_Icon.GetComponent<Image>().sprite = m_Sprite[0];
                aimAnimation.Kill();
                aimAnimation = DOTween.Sequence();
                aimAnimation.Append(m_Icon.GetComponent<Image>()
                               .DOColor(SceneHandler.green, 0.2f)
                               .SetEase(Ease.OutQuad));
                break;

            default:
                Debug.LogError("Aim to mouse Error");
                break;
        }
    }

    private void ChangeSprite()
    {
        if (m_Angle < 15 && m_Angle > -15)
        {
            if (turnState != "Right")
            {
                m_Icon.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                m_Icon.GetComponent<Image>().sprite = m_Sprite[0];
                aimAnimation.Kill();
                aimAnimation = DOTween.Sequence();
                aimAnimation.Append(m_Icon.GetComponent<Image>()
                               .DOColor(SceneHandler.green, 0.2f)
                               .SetEase(Ease.OutQuad));
            }
            turnState = "Right";
        }
        else if (m_Angle > 75 && m_Angle < 105)
        {
            if (turnState != "Up")
            {
                m_Icon.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -90));
                m_Icon.GetComponent<Image>().sprite = m_Sprite[1];
                aimAnimation.Kill();
                aimAnimation = DOTween.Sequence();
                aimAnimation.Append(m_Icon.GetComponent<Image>()
                               .DOColor(SceneHandler.orange, 0.2f)
                               .SetEase(Ease.OutQuad));
            }
            turnState = "Up";
        }
        else if (m_Angle > 165 || m_Angle < -165)
        {
            if (turnState != "Left")
            {
                m_Icon.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 180));
                m_Icon.GetComponent<Image>().sprite = m_Sprite[2];
                aimAnimation.Kill();
                aimAnimation = DOTween.Sequence();
                aimAnimation.Append(m_Icon.GetComponent<Image>()
                               .DOColor(SceneHandler.blue, 0.2f)
                               .SetEase(Ease.OutQuad));
            }
            turnState = "Left";
        }
        else if (m_Angle < -75 && m_Angle > -105)
        {
            if (turnState != "Down")
            {
                m_Icon.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 90));
                m_Icon.GetComponent<Image>().sprite = m_Sprite[3];
                aimAnimation.Kill();
                aimAnimation = DOTween.Sequence();
                aimAnimation.Append(m_Icon.GetComponent<Image>()
                               .DOColor(SceneHandler.red, 0.2f)
                               .SetEase(Ease.OutQuad));
            }
            turnState = "Down";
        }
        else
        {
                turnState = "None";
                m_Icon.GetComponent<Image>().sprite = null;
                aimAnimation.Kill();
                aimAnimation = DOTween.Sequence();
                aimAnimation.Append(m_Icon.GetComponent<Image>()
                                .DOColor(new Color32(225, 225, 225, 200), 0.2f)
                                .SetEase(Ease.OutQuad));
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
