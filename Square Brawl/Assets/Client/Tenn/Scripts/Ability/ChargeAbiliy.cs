using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/ChargeAbility")]
public class ChargeAbiliy : Ability
{
    public int ChargeDamage;
    public float ChargeSpeed;
    public float BeElasticity;
    private AttackTriggerable _attack;

    public override void Initalize(GameObject _obj)
    {
        _attack = _obj.GetComponent<AttackTriggerable>();
        _attack.WeaponDamage = ChargeDamage;
        _attack.WeaponSpeed = ChargeSpeed;
        _attack.BeElasticity = BeElasticity;
    }

    public override void Activate()
    {
        _attack.Charge();
    }
}
