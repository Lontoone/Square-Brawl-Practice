using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelectBtn : MonoBehaviour
{
    public WeaponType weaponType;
    private Button button;
    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(delegate { SetWeapon(weaponType); });
    }  
    public void SetWeapon(WeaponType _targetWeapon)
    {
        //check firsr weapon 1 is set?
        //  true =>set weapon 2
        //  false => set weapon1
        WeaponType weapon1 = (WeaponType)PhotonNetwork.LocalPlayer.CustomProperties[CustomPropertyCode.WEAPON1CODE];
        WeaponType weapon2 = (WeaponType)PhotonNetwork.LocalPlayer.CustomProperties[CustomPropertyCode.WEAPON2CODE];
        if (weapon1 != WeaponType.None && weapon2 == WeaponType.None)
        {
            //weapon2 = _targetWeapon;
            PhotonNetwork.LocalPlayer.SetCustomProperties(
                               MyPhotonExtension.WrapToHash(
                                   new object[] { CustomPropertyCode.WEAPON2CODE, _targetWeapon }
                               ));
        }
        else if (weapon1 == WeaponType.None)
        {
            //weapon1 = _targetWeapon;
            PhotonNetwork.LocalPlayer.SetCustomProperties(
                              MyPhotonExtension.WrapToHash(
                                  new object[] { CustomPropertyCode.WEAPON1CODE, _targetWeapon }
                              ));
        }
    }
}
