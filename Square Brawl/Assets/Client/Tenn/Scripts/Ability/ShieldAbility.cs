using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/ShieldAbility")]
public class ShieldAbility : Ability
{
    public override void Initalize(GameObject _obj)
    {
        _attack = _obj.GetComponent<AttackTriggerable>();
        _attack.WeaponSpeed = Speed;
        _attack.WeaponDamage = Damage;
        _attack.BeElasticity = BeElasticity;
    }

    public override void Activate()
    {
        _attack.Shield();
    }
}
