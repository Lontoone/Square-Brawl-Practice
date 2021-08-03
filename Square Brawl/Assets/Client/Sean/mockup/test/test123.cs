using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using BasicTools.ButtonInspector;
using TMPro;
using DG.Tweening;
namespace Easetype { }
public class test123 : MonoBehaviour
{

    public static int index;

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(index);
    }
    public void setindex(int num)
    {
        this.GetComponentInParent<testparent>().gg.debugnum = num;
        Debug.Log("myparent" + this.GetComponentInParent<testparent>().gg.myindex);
    }
}
