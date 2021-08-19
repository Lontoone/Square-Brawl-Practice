using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MapSelectionButtonManager : MonoBehaviour
{
    [SerializeField] private Button m_PreviousStyle;
    [SerializeField] private Button m_NextStyle;
    [SerializeField] private Button m_PreviousMap;
    [SerializeField] private Button m_NextMap;

    [Space(15)]
    [SerializeField] private Animator animator;
    [SerializeField] private TillStyleLoader tillStyleLoader;
    [SerializeField] private MapSelectManager mapSelectManager;

    private void Start()
    {
        m_PreviousStyle.onClick.AddListener(delegate { StartCoroutine(ChangeStyle(-1)); });
        m_NextStyle.onClick.AddListener(delegate { StartCoroutine(ChangeStyle(1)); });
        m_PreviousMap.onClick.AddListener(delegate { StartCoroutine(ChangeMap(-1)); });
        m_NextMap.onClick.AddListener(delegate { StartCoroutine(ChangeMap(1)); });
    }

    private IEnumerator ChangeStyle(int index)
    {
        animator.Play("ChangeStyle");
        yield return new WaitForSeconds(0.3f);
        tillStyleLoader.Switch(index);
    }

    private IEnumerator ChangeMap(int index)
    {
        animator.Play("ChangeStyle");
        yield return new WaitForSeconds(0.3f);
        mapSelectManager.Switch(index);
    }

}
