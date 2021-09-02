using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;
using BasicTools.ButtonInspector;
using DG.Tweening;
using TMPro;
namespace Easetype { }

public class SettingGroupPrefabManager : MonoBehaviour//, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    [SerializeField] private GameObject m_SettingGroupPrefab;
    [SerializeField] public GameObject m_SettingList;
    [SerializeField] private GameObject[] m_SettingPrefab;
    private Easetype.Current_easetype m_CurrentEasetype;

    [HideInInspector]
    public enum AnimationType {LeftFlash, DownFlash, Fade}
    [Space(15)]
    [SerializeField] private AnimationType type;
    [SerializeField] private Easetype.Current_easetype.Easetype m_Easetype;
    [SerializeField] private float ToX;
    [SerializeField] private float m_duration;

    private Sequence m_Sequence;
    private Vector3[] pos;
    private Vector2 vec;
    private int selectedIndex = 0;

    public bool onSelect = false;
    public bool isSelected = false;
    public bool onPress;

    void Start()
    {
        m_CurrentEasetype = new Easetype.Current_easetype();
        vec = m_SettingGroupPrefab.GetComponent<RectTransform>().sizeDelta;

        switch (type)
        {
            case AnimationType.LeftFlash:
                pos = new Vector3[m_SettingPrefab.Length];
                for (int i = 0; i < m_SettingPrefab.Length; i++)
                {
                    pos[i] = new Vector3(-m_SettingPrefab[i].GetComponent<RectTransform>().sizeDelta.x, m_SettingPrefab[i].transform.localPosition.y);
                    m_SettingPrefab[i].transform.localPosition = pos[i];
                }
                break;

            case AnimationType.DownFlash:

                break;

            case AnimationType.Fade:

                break;
        }
        StartCoroutine(DisableSettingList());
        //m_SettingList.SetActive(false);
    }

    private IEnumerator DisableSettingList()
    {
        yield return new WaitForEndOfFrame();
        m_SettingList.SetActive(false);
    }

    public void SettingIn()
    {
        if (onPress == false)
        {
            m_SettingList.SetActive(true);
            m_Sequence.Kill();
            m_Sequence = DOTween.Sequence();
            switch (type)
            {
                case AnimationType.LeftFlash:
                    for (int i = 0; i < m_SettingPrefab.Length; i++)
                    {
                        pos[i] = new Vector3(-m_SettingPrefab[i].GetComponent<RectTransform>().sizeDelta.x, m_SettingPrefab[i].transform.localPosition.y);
                        m_SettingPrefab[i].transform.localPosition = pos[i];
                    }

                    for (int i = 0; i < m_SettingPrefab.Length; i++)
                    {
                        m_Sequence.Append(m_SettingPrefab[i].transform
                                        .DOLocalMoveX(pos[i].x + ToX, m_duration)
                                        .SetEase(m_CurrentEasetype.GetEasetype(m_Easetype)));
                    }
                    break;

                case AnimationType.DownFlash:
                    break;

                case AnimationType.Fade:
                    for (int i = 0; i < m_SettingPrefab.Length; i++)
                    {
                        m_SettingPrefab[i].GetComponent<RectTransform>().localScale = new Vector3(0,0);
                    }
                    m_Sequence.AppendInterval(0.2f);
                    for (int i = 0; i < m_SettingPrefab.Length; i++)
                    {
                        m_Sequence.Append(m_SettingPrefab[i].transform
                                        .DOScale(new Vector3(1,1), 0.2f)
                                        .SetEase(Ease.OutCirc));
                    }
                    break;
            }
            onPress = true;
        }
    }

    public void SettingOut()
    {
        if (onPress == true)
        {
            m_Sequence.Kill();
            m_Sequence = DOTween.Sequence();
            switch (type)
            {
                case AnimationType.LeftFlash:
                    for (int i = 0; i < m_SettingPrefab.Length; i++)
                    {
                        m_Sequence.Append(m_SettingPrefab[i].transform
                                        .DOLocalMoveX(pos[i].x + ToX * 2, m_duration)
                                        .SetEase(m_CurrentEasetype.GetEasetype(m_Easetype)));

                    }
                    break;

                case AnimationType.DownFlash:
                    break;

                case AnimationType.Fade:
                    break;
            }
            onPress = false;
            m_SettingList.SetActive(false);
        }
    }

    public void Selected()
    {
        m_SettingGroupPrefab.GetComponent<RectTransform>()
                                .DOSizeDelta(new Vector2(OptionManager.m_BarLength, vec.y), m_duration)
                                .SetEase(m_CurrentEasetype.GetEasetype(m_Easetype));
    }

    public void DeSelected()
    {
        m_SettingGroupPrefab.GetComponent<RectTransform>()
                               .DOSizeDelta(vec, m_duration)
                               .SetEase(m_CurrentEasetype.GetEasetype(m_Easetype));
    }

    public void SetSelect(int num)
    {
        selectedIndex += num;

        if (selectedIndex < 0)
        {
            selectedIndex = m_SettingPrefab.Length - 1;
        }
        else if (selectedIndex >= m_SettingPrefab.Length)
        {
            selectedIndex = 0;
        }

        for (int i = 0; i < m_SettingPrefab.Length; i++)
        {
            var settingPrefab = m_SettingPrefab[selectedIndex].GetComponent<SettingPrefabManager>();

            if (i == selectedIndex)
            {
                settingPrefab.OnSelected();
                Debug.Log("1");
            }
            else
            {
                settingPrefab.DeSelected();
                Debug.Log("0");
            }
        }

        EventSystem.current.SetSelectedGameObject(m_SettingPrefab[selectedIndex]);
    }

    /*
    public void OnPointerEnter(PointerEventData eventData)
    {
        onSelect = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        onSelect = false;
    }

    public virtual void OnSelect(BaseEventData eventData)
    {
        onSelect = true;

    }

    public virtual void OnDeselect(BaseEventData eventData)
    {
        onSelect = false;
    }*/
}

