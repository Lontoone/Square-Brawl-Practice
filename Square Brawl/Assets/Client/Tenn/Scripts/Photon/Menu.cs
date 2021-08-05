using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Menu : MonoBehaviour
{
    public string menuName;

    

    [HideInInspector]
    public bool open;

    public void Start()
    {
        open = gameObject.activeSelf;
    }

    public void Open()
    {
        open = true;
        SceneHandler.instance.EnterPage(gameObject.name);
    }
    public void Close()
    {
        open = false;
        SceneHandler.instance.ExitPage(gameObject.name);
    }


}
