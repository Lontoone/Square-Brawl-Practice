using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;


public class MouseAimAction : MonoBehaviour
{
    [System.Serializable]
    public struct AimAction
    {
        [SerializeField] public GameObject m_AimPos;
        [SerializeField] public GameObject m_AimObject;
        [SerializeField] public Vector2 m_Area;
        [SerializeField] public float m_MoveDistance;
        [SerializeField] public float m_DetectDistance;
        [Space(10)]
        [SerializeField] public Easetype.Current_easetype.Easetype m_Easetype;
        [SerializeField] public float m_Duration;

        [HideInInspector]
        public Vector2 m_ObjectPos;
        [HideInInspector]
        public Vector2 m_MouseWorldPos;
    }

    [SerializeField] private AimAction m_Aim;

    private Easetype.Current_easetype m_CurrentEasetype;
    private Sequence m_IconMove;

    private bool gamepadUpdate = false;
    private string mode;//0 = keyboard & gamepad, 1 = mouse


    private void Start()
    {
        m_CurrentEasetype = new Easetype.Current_easetype();
    }

    private void FixedUpdate()
    {
        AimToMouse();
    }

    private void AimToMouse()
    {
        if (Gamepad.current != null)
        {
            if (Gamepad.current.wasUpdatedThisFrame)
            {
                gamepadUpdate = true;
            }
        }

        if (Mouse.current.wasUpdatedThisFrame)
        {
            mode = "Mouse";
            gamepadUpdate = false;
        }
        if (Keyboard.current.wasUpdatedThisFrame || gamepadUpdate)
        {
            mode = "Keyboard";
        }

        if (mode == "Keyboard")
        {
            m_Aim.m_MouseWorldPos = new Vector2(0, 0);
        }
        else
        {
            m_Aim.m_MouseWorldPos = Mouse.current.position.ReadValue();
        }

        //Move Icon
        var localVec = m_Aim.m_MouseWorldPos - new Vector2(m_Aim.m_AimPos.transform.position.x, m_Aim.m_AimPos.transform.position.y);
        var length = Vector3.Magnitude(localVec);
        Vector3 move;
        if (length <= m_Aim.m_DetectDistance)
        {
            move = (m_Aim.m_MoveDistance / m_Aim.m_DetectDistance) * localVec;
        }
        else
        {
            move = Vector3.zero;
        }
        m_IconMove.Kill();
        m_IconMove = DOTween.Sequence();
        m_IconMove.Append(m_Aim.m_AimObject.transform.DOLocalMove(move, m_Aim.m_Duration).SetEase(m_CurrentEasetype.GetEasetype(m_Aim.m_Easetype)));
    }
}

