using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WeaponSelectBtn : MonoBehaviour, IPointerEnterHandler, ISelectHandler, IPointerClickHandler, ISubmitHandler
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioSourcesManager.PlaySFX(2);
    }

    public void OnSelect(BaseEventData eventData)
    {
        //AudioSourcesManager.PlaySFX(2);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        AudioSourcesManager.PlaySFX(2);
    }

    public void OnSubmit(BaseEventData eventData)
    {
        AudioSourcesManager.PlaySFX(2);
    }
}
