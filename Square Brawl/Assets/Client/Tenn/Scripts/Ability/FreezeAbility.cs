using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/FreezeAbility")]
public class FreezeAbility : Ability
{
    public override void Initalize(GameObject _obj)
    {
        _attack = _obj.GetComponent<AttackTriggerable>();
        _attack.SoundName = Name;
        _attack.WeaponRecoil = Recoil;
        _attack.ShootShakeValue = ShootShakeValue;
        _attack.BeShootShakeValue = BeShootShakeValue;
    }

    public override void Activate()
    {
        _attack.Freeze();
    }
}
