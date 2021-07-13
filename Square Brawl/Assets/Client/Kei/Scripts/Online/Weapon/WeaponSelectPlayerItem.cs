using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WeaponSelectPlayerItem : ColorSelectPlayerItemControl
{
    public Text weapon1Text, weapon2Text;
    [HideInInspector]
    public WeaponType weapon1, weapon2;

    public void SetWeapon1(WeaponType _targetWeapon) {
        weapon1 = _targetWeapon;
        weapon1Text.text = _targetWeapon.ToString();
    }
    public void SetWeapon2(WeaponType _targetWeapon)
    {
        weapon2 = _targetWeapon;
        weapon2Text.text = _targetWeapon.ToString();
    }

    /*
    public void SetWeapon(WeaponType _targetWeapon)
    {
        //check firsr weapon 1 is set?
        //  true =>set weapon 2
        //  false => set weapon1
        if (weapon1 != WeaponType.None && weapon2 == WeaponType.None)
        {
            weapon2 = _targetWeapon;
            weapon2Text.text = _targetWeapon.ToString();
        }
        else if (weapon1 == WeaponType.None)
        {
            weapon1 = _targetWeapon;
            weapon1Text.text = _targetWeapon.ToString();
        }
    }
    */

}
