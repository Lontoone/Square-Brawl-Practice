using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;
using BasicTools.ButtonInspector;
using DG.Tweening;
using TMPro;
namespace Easetype {}

public class PlayButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    public int m_ButtonIndex;
    [Header ("Play Button")]
    [SerializeField] private GameObject play_button;
    private Button button_button;
    private RectTransform button_rect;
    private Easetype.Current_easetype m_PlayButtonCurrentEasetype;
    [SerializeField] Easetype.Current_easetype.Easetype easetype;
    [SerializeField] float m_duration =1f;
    
    [Space(10)]
    [Header("Aim Object")]
    [SerializeField] private GameObject m_AimObject;
    [SerializeField] Easetype.Current_easetype.Easetype m_AimEasetype;
    private Easetype.Current_easetype m_AimCurrentEasetype;
    [SerializeField] float m_aimduration = 0.3f;

    private Sequence menuButtonAnimation;

    void Start()
    {
        button_button = play_button.GetComponent<Button>();
        m_PlayButtonCurrentEasetype = new Easetype.Current_easetype();
        m_AimCurrentEasetype = new Easetype.Current_easetype();
    }

    public void EnterLobbyAnimation() 
    {
        menuButtonAnimation.Kill();
        menuButtonAnimation = DOTween.Sequence();
        menuButtonAnimation.Append(play_button.transform
                              .DOScale(new Vector3(10,10,0), m_duration)
                              .SetEase(m_PlayButtonCurrentEasetype.GetEasetype(easetype)));
    }

    public void ExitLobbyAnimation() 
    {
        menuButtonAnimation.Kill();
        menuButtonAnimation = DOTween.Sequence();
        menuButtonAnimation.Append(play_button.transform
                              .DOScale(new Vector3(1, 1, 0), m_duration)
                              .SetEase(m_PlayButtonCurrentEasetype.GetEasetype(easetype)));
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        AimAction.AimFade(m_AimObject, m_aimduration, m_AimCurrentEasetype, m_AimEasetype);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        AimAction.AimUnFade(m_AimObject, m_aimduration, m_AimCurrentEasetype, m_AimEasetype);
    }

    public virtual void OnSelect(BaseEventData eventData)
    {
        AimAction.AimFade(m_AimObject, m_aimduration, m_AimCurrentEasetype, m_AimEasetype);
    }

    public virtual void OnDeselect(BaseEventData eventData)
    {
        AimAction.AimUnFade(m_AimObject, m_aimduration, m_AimCurrentEasetype, m_AimEasetype);
    }
}
