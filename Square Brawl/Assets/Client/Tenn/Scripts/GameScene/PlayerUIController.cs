using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{
    public Image PlayerHpImg;
    private float _playerHpValue;

    public void ReduceHp(float _hp)
    {
        _playerHpValue = _hp / 100;
        PlayerHpImg.rectTransform.localScale = new Vector3(_playerHpValue, _playerHpValue, _playerHpValue);
    }

}
