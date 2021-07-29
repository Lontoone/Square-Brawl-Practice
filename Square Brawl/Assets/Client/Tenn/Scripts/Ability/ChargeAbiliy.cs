using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/ChargeAbility")]
public class ChargeAbiliy : Ability
{
    public override void Initalize(GameObject _obj)
    {
        _attack = _obj.GetComponent<AttackTriggerable>();
        _attack.WeaponDamage = Damage;
        _attack.WeaponSpeed = Speed;
        _attack.BeElasticity = BeElasticity;
        _attack.ShotShakeValue = ShotShakeValue;
        _attack.BeShotShakeValue = BeShotShakeValue;
    }

    public override void Activate()
    {
        _attack.Charge();
    }
}
