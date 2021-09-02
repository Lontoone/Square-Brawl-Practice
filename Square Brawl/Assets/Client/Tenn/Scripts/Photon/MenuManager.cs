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
        //OpenMenu("title");
    }


    public List<Menu> menus = new List<Menu>();

    public void BackToPreviousMenu()
    {
        Debug.Log(previousMenu.Count);

        if (previousMenu.Count > 1)
        {
            //Clear the current:
            string _currentMenu = previousMenu.Peek();

            if (_currentMenu.Equals("title"))
            {
                previousMenu.Clear();
                return;
            }
            else if (_currentMenu.Equals("room"))
            {
                return;
            }
            else
            {
                previousMenu.Pop();
            }

            string _backMenu = previousMenu.Pop();

            //get the previous:
            Debug.Log("Back " + _backMenu);

            //[Hard code] the locked menu 
            if (!_backMenu.Equals("title"))
            {
                OpenMenu(_backMenu);
                //sync menu
                PhotonNetwork.CurrentRoom.SetCustomProperties(MyPhotonExtension.WrapToHash(
                                                                       new object[] {
                                                                            CustomPropertyCode.ROOM_MENU,
                                                                            _backMenu
                                                                       }));
            }
            else
            {
                //previousMenu.Clear();
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
                if (!menuName.Equals("loading"))
                {
                    previousMenu.Push(menuName);
                }
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

    public void BackToCharacterSelection()
    {
        previousMenu.Push("title");
        previousMenu.Push("room");
        previousMenu.Push("characterselection");
        Debug.Log(PhotonNetwork.CurrentRoom.Name);
    }
}
