using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    public void Awake()
    {
        instance = this;
        /*
        foreach (Menu _menu in FindObjectsOfType<Menu>())
        {
            menus.Add(_menu);
        }*/
        OpenMenu("title");
    }


    public List<Menu> menus = new List<Menu>();
    /*
    public void CallOpenMenuRpc(string _menuName)
    {
        //PhotonNetwork.RPC("OpenMenu", RpcTarget.All, _menuName);
    }*/
    [PunRPC]
    public void OpenMenu(string menuName)
    {
        for (int i = 0; i < menus.Count; i++)
        {
            if (menus[i].menuName.Equals(menuName))
            {
                menus[i].Open();
            }
            else if (menus[i].open)
            {
                CloseMenu(menus[i]);
            }
        }
    }


    public void OpenMenu(Menu menu)
    {
        for (int i = 0; i < menus.Count; i++)
        {
            if (menus[i].open)
            {
                CloseMenu(menus[i]);
            }
        }
        menu.Open();
    }
    public void CloseMenu(Menu menu)
    {
        menu.Close();
    }
}
