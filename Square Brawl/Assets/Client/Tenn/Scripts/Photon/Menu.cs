using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Menu : MonoBehaviour
{
    public string menuName;



    //[HideInInspector]
    public bool open;

    public void Start()
    {
        open = gameObject.activeSelf;
    }

    public void Open()
    {
        Debug.Log("open" + menuName);
        open = true;
        SceneHandler.instance.EnterPage(gameObject.name);
    }
    public void Close()
    {
        Debug.Log("close" + gameObject.name);
        open = false;
        SceneHandler.instance.ExitPage(gameObject.name);
    }


}
