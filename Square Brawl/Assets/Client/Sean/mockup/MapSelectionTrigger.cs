using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSelectionTrigger : MonoBehaviour
{
    public static bool GridFinish = false;
    public static bool StyleFinish = false;
    public static bool MapFinish = false;
    public static bool AllFinish = false;

    private void OnDisable()
    {
        GridFinish = false;
        StyleFinish = false;
        MapFinish = false;
        AllFinish = false;
    }
}
