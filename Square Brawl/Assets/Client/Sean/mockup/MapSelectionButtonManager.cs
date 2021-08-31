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
    [SerializeField] public Animator animator;
    [SerializeField] private TillStyleLoader tillStyleLoader;
    [SerializeField] private MapSelectManager mapSelectManager;

    public static MapSelectionButtonManager instance;
    public static bool mapSetUpReady = false;

    private void Start()
    {
        m_PreviousStyle.onClick.AddListener(delegate { StartCoroutine(ChangeStyle(-1,0.3f)); });
        m_NextStyle.onClick.AddListener(delegate { StartCoroutine(ChangeStyle(1, 0.3f)); });
        m_PreviousMap.onClick.AddListener(delegate { StartCoroutine(ChangeMap(-1, 0.3f)); });
        m_NextMap.onClick.AddListener(delegate { StartCoroutine(ChangeMap(1, 0.3f)); });
    }

    private void OnEnable()
    {
        StartCoroutine(SetFirstSelection());
        /*StartCoroutine(ChangeStyle(0, 1));
        StartCoroutine(ChangeMap(0, 1));*/
    }

    private void OnDisable()
    {
        mapSetUpReady = false;
    }

    private IEnumerator SetFirstSelection()
    {
        yield return new WaitUntil(() => TileMapManager.mapIsReady);
        mapSelectManager.Switch(1);
        yield return new WaitForSeconds(1f);
        tillStyleLoader.Switch(2);
        yield return new WaitForSeconds(1f);
        mapSetUpReady = true;
    }

    private IEnumerator ChangeStyle(int index,float time)
    {
        animator.Play("ChangeStyle");
        yield return new WaitForSeconds(time);
        tillStyleLoader.Switch(index);
    }

    private IEnumerator ChangeMap(int index, float time)
    {
        animator.Play("ChangeStyle");
        yield return new WaitForSeconds(time);
        mapSelectManager.Switch(index);
    }

    //TODO 動畫沒同步到非房主玩家畫面
}
