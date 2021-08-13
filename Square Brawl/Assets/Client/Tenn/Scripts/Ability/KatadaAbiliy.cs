using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/KatadaAbility")]
public class KatadaAbiliy : Ability
{
    public override void Initalize(GameObject _obj)
    {
        _attack = _obj.GetComponent<AttackTriggerable>();
        _attack.SoundName = Name;
        _attack.WeaponDamage = Damage;
        _attack.WeaponSpeed = Speed;
        _attack.BeElasticity = BeElasticity;
        _attack.ShootShakeValue = ShootShakeValue;
        _attack.BeShootShakeValue = BeShootShakeValue;
    }

    public override void Activate()
    {
        _attack.Katada();
    }
}
