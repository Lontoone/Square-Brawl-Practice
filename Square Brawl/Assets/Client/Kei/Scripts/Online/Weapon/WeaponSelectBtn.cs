using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WeaponSelectBtn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler 
{
    public WeaponType weaponType;
    [SerializeField] private Button button;
    [SerializeField] private Image bg;
    [SerializeField] private Image icon;

    private Color color;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(delegate { SetWeapon(weaponType); });

        color = CustomPropertyCode.COLORS[(int)PhotonNetwork.LocalPlayer.CustomProperties[CustomPropertyCode.TEAM_CODE]];
    }
    public void SetWeapon(WeaponType _targetWeapon)
    {
        bool IsReady = (bool)PhotonNetwork.LocalPlayer.CustomProperties[CustomPropertyCode.READY];
        //check firsr weapon 1 is set?
        //  true =>set weapon 2
        //  false => set weapon1
        if (!IsReady)
        {
            object _tryData;
            WeaponType weapon1 = PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(CustomPropertyCode.WEAPON1CODE, out _tryData) ? (WeaponType)_tryData : WeaponType.None;
            WeaponType weapon2 = PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(CustomPropertyCode.WEAPON2CODE, out _tryData) ? (WeaponType)_tryData : WeaponType.None;

            if (weapon1 == _targetWeapon)
            {
                //unset weapon 1
                PhotonNetwork.LocalPlayer.SetCustomProperties(
                                   MyPhotonExtension.WrapToHash(
                                       new object[] { CustomPropertyCode.WEAPON1CODE, WeaponType.None }
                                   ));
            }
            else if (weapon2 == _targetWeapon)
            {
                //unset weapon 2
                PhotonNetwork.LocalPlayer.SetCustomProperties(
                                   MyPhotonExtension.WrapToHash(
                                       new object[] { CustomPropertyCode.WEAPON2CODE, WeaponType.None }
                                   ));
            }
            else if (weapon1 != WeaponType.None && weapon2 == WeaponType.None)
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

    private void ChangeColor()
    {
        bg.color = new Color(color.r, color.g, color.b ,0.25f);
        icon.color = color;
    }

    private void DefaultColor()
    {
        bg.color = new Color(1,1 ,1 , 0.25f);
        icon.color = new Color(1, 1, 1, 1);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ChangeColor();
    }

    public void OnSelect(BaseEventData eventData)
    {
        ChangeColor();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DefaultColor();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        DefaultColor();
    }
}
