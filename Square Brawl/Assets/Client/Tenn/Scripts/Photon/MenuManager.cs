using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;
    private Stack<string> previousMenu = new Stack<string>();

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

    public void Update()
    {
        //press "B" to go back previous menu:
        if (Keyboard.current.bKey.wasPressedThisFrame)
        {
            BackToPreviousMenu();
        }
    }

    public void BackToPreviousMenu()
    {
        Debug.Log(previousMenu.Count);
        if (previousMenu.Count > 1)
        {
            //Clear the current:
            if (previousMenu.Pop().Equals("room"))
            {
                previousMenu.Clear();
                return;
            };

            //get the previous:
            string _menuName = previousMenu.Pop();
            Debug.Log("Back " + _menuName);

            //[Hard code] the locked menu 
            if (!_menuName.Equals("room"))
            {
                OpenMenu(_menuName);
            }
            else
            {
                previousMenu.Clear();
            }
        }
    }

    [PunRPC]
    public void OpenMenu(string menuName)
    {
        for (int i = 0; i < menus.Count; i++)
        {
            if (menus[i].menuName.Equals(menuName))
            {
                menus[i].Open();
                previousMenu.Push(menuName);
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
