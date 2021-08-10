using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviourPunCallbacks
{
    public Image PlayerHpImg;
    public Image WeaponCold01TimeImg;
    public Image WeaponCold02TimeImg;

    private float _playerHpValue;

    private PhotonView _pv;

    private void Start()
    {
        _pv = GetComponent<PhotonView>();
        if (_pv.IsMine)
        {
            AbilityHolder.OnColdTime += ColdTime;
        }
        SetColor();
    }

    private void OnDestroy()
    {
        AbilityHolder.OnColdTime -= ColdTime;
    }

    private void SetColor()
    {
        Color _color = 
            CustomPropertyCode.COLORS[(int)PhotonNetwork.LocalPlayer.CustomProperties[CustomPropertyCode.TEAM_CODE]];

        WeaponCold01TimeImg.color = WeaponCold02TimeImg.color = new Color(_color.r, _color.g, _color.b, 0.4f);
    }

    public void ReduceHp(float _hp)
    {
        _playerHpValue = _hp / 100;
        PlayerHpImg.rectTransform.localScale = new Vector3(_playerHpValue, _playerHpValue, _playerHpValue);
    }

    void ColdTime(float _coldTime,float _curretTime,bool _isWeapon01)
    {
        if (_isWeapon01)
        {
            WeaponCold01TimeImg.fillAmount = _curretTime / _coldTime;
        }
        else
        {
            WeaponCold02TimeImg.fillAmount = _curretTime / _coldTime;
        }
    }

}
