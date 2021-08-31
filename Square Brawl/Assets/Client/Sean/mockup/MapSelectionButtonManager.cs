using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
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

    private IEnumerator SetFirstSelection()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            yield return new WaitUntil(() => MapSelectionTrigger.GridFinish);
            tillStyleLoader.Switch(2);
            mapSelectManager.Switch(1);
            yield return new WaitUntil(() => MapSelectionTrigger.MapFinish);
            MapSelectionTrigger.AllFinish = true;
        }
        else
        {
            //Use a part of TileMapSetUpManager SetUpLevelCoro
            if (TileMapManager.instance == null)
            {
                yield break;
            }
            if (TileMapManager.instance.gridCells.Count < TileMapManager.instance.cellCount)
            {
                Debug.Log("Gernerate Grid");
                if (!TileMapManager.instance.generated)
                    TileMapManager.instance.GenerateGrid(false);

                yield return new WaitForSeconds(1.5f);
                Debug.Log("Gernerate Grid - finished");
            }
        }
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
