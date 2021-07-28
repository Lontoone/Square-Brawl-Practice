using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testparent : MonoBehaviour
{
    [System.Serializable]
    public struct ggg
    {
        public int myindex;
        public int debugnum;
    }

    public ggg gg;

    private void Update()
    {
        Debug.Log("gg" + gg.myindex +"\t"+ gg.debugnum);
    }
}
