using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class WeaponSelectPlayerItem : ColorSelectPlayerItemControl
{
    public Text weapon1Text, weapon2Text;
    [HideInInspector]
    public WeaponType weapon1, weapon2;
    public Image icon1, icon2;
    public Button ready;

    private Color color;
    private Sequence sequence;

    public void Start()
    {
        color = CustomPropertyCode.COLORS[(int)player.CustomProperties[CustomPropertyCode.TEAM_CODE]];
        SetColor(color);
    }

    public void SetWeapon1(WeaponType _targetWeapon)
    {
        weapon1 = _targetWeapon;
        weapon1Text.text = _targetWeapon.ToString();

        ChangeWeaponImage(icon1, _targetWeapon);
        SetReadyButton();
    }
    public void SetWeapon2(WeaponType _targetWeapon)
    {
        weapon2 = _targetWeapon;
        weapon2Text.text = _targetWeapon.ToString();
        
        ChangeWeaponImage(icon2, _targetWeapon);
        SetReadyButton();
    }

    private void ChangeWeaponImage(Image image, WeaponType _targetWeapon)
    {
        image.sprite = PlayerWeaponImage.instance.IconGroup[(int)_targetWeapon].sprite;

        if ((int)_targetWeapon != 0)
        {
            sequence.Kill();
            sequence = DOTween.Sequence();
            sequence.Append(image
                        .DOColor(new Color(color.r, color.g, color.b, 1), 0.3f)
                        .SetEase(Ease.OutCirc))
                    .Join(image.transform
                        .DOPunchScale(new Vector3(0.2f, 0.2f), 0.3f)
                        .SetEase(Ease.OutElastic));
        }
        else
        {
            sequence.Kill();
            sequence = DOTween.Sequence();
            sequence.Append(image
                        .DOColor(new Color(0.801f, 0.801f, 0.801f, 0.2705882f), 0.3f)
                        .SetEase(Ease.OutCirc));
        }
    }

    private void SetReadyButton()
    {
        if (weapon1 != WeaponType.None && weapon2 != WeaponType.None)
        {
            ready.interactable = true;
        }
        else 
        {
            ready.interactable = false;
        }
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
