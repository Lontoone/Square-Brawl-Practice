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
    [SerializeField] private GameObject m_PlayButton;
    private Button m_Button;
    private RectTransform rect;

    [SerializeField] private Image m_Shadow;
    [SerializeField] private Transform m_Background;
    [SerializeField] private Image m_Mask;
    [SerializeField] private Image m_Arror;
    private Easetype.Current_easetype m_PlayButtonCurrentEasetype;
    [SerializeField] Easetype.Current_easetype.Easetype easetype;
    [SerializeField] private float m_duration;
    
    [Space(10)]
    [Header("Aim Object")]
    [SerializeField] private GameObject m_AimObject;
    [SerializeField] Easetype.Current_easetype.Easetype m_AimEasetype;
    private Easetype.Current_easetype m_AimCurrentEasetype;
    [SerializeField] float m_aimduration = 0.3f;

    private Sequence menuButtonAnimation;
    private Sequence playAnimation;

    void Start()
    {
        m_Button = m_PlayButton.GetComponent<Button>();
        rect = m_PlayButton.GetComponent<RectTransform>();

        m_Arror.color = new Color32(0, 0, 0, 0);
        m_Shadow.color = new Color32(0, 0, 0, 0);
        m_PlayButtonCurrentEasetype = new Easetype.Current_easetype();
        m_AimCurrentEasetype = new Easetype.Current_easetype();
    }

    public void PlayAnimation()
    {
        playAnimation.Kill();
        playAnimation = DOTween.Sequence();
        playAnimation.Append(m_Mask.DOColor(new Color32(244, 244, 244, 255), 0.05f).SetEase(Ease.OutQuad))
                .AppendInterval(0.2f)
                .Append(m_Mask.DOColor(new Color32(244, 244, 244, 0), 0.3f).SetEase(Ease.OutQuad));
    }

    public void OnExitMenuAction()
    {
        m_Background.GetComponent<Animator>().enabled = false;
        foreach (Transform child in m_Background) 
        {
            child.gameObject.SetActive(false);
        }
        EventSystem.current.SetSelectedGameObject(m_Arror.gameObject);
    }


    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        AudioSourcesManager.PlaySFX(0);
        m_Arror.DOColor(new Color32(255, 255, 255, 255), m_aimduration).SetEase(m_PlayButtonCurrentEasetype.GetEasetype(m_AimEasetype));
        m_Shadow.DOColor(new Color32(255, 255, 255, 255), m_aimduration).SetEase(m_PlayButtonCurrentEasetype.GetEasetype(m_AimEasetype));
        AimAction.inButton = true;
        AimAction.AimFade(m_AimObject, m_aimduration, m_AimCurrentEasetype, m_AimEasetype);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        m_Arror.DOColor(new Color32(255, 255, 255, 0), m_aimduration).SetEase(m_PlayButtonCurrentEasetype.GetEasetype(m_AimEasetype));
        m_Shadow.DOColor(new Color32(255, 255, 255, 0), m_aimduration).SetEase(m_PlayButtonCurrentEasetype.GetEasetype(m_AimEasetype));
        AimAction.inButton = false;
        AimAction.AimUnFade(m_AimObject, m_aimduration, m_AimCurrentEasetype, m_AimEasetype);
    }

    public virtual void OnSelect(BaseEventData eventData)
    {
        AudioSourcesManager.PlaySFX(0);
        m_Arror.DOColor(new Color32(255, 255, 255, 255), m_aimduration).SetEase(m_PlayButtonCurrentEasetype.GetEasetype(m_AimEasetype));
        m_Shadow.DOColor(new Color32(255, 255, 255, 255), m_aimduration).SetEase(m_PlayButtonCurrentEasetype.GetEasetype(m_AimEasetype));
        AimAction.inButton = true;
        AimAction.AimFade(m_AimObject, m_aimduration, m_AimCurrentEasetype, m_AimEasetype);
    }

    public virtual void OnDeselect(BaseEventData eventData)
    {
        m_Arror.DOColor(new Color32(255, 255, 255, 0), m_aimduration).SetEase(m_PlayButtonCurrentEasetype.GetEasetype(m_AimEasetype));
        m_Shadow.DOColor(new Color32(255, 255, 255, 0), m_aimduration).SetEase(m_PlayButtonCurrentEasetype.GetEasetype(m_AimEasetype));
        AimAction.inButton = false;
        AimAction.AimUnFade(m_AimObject, m_aimduration, m_AimCurrentEasetype, m_AimEasetype);
    }
}
