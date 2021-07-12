using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/KatadaAbility")]
public class KatadaAbiliy : Ability
{
    // Start is called before the first frame update
    public override void Initalize(GameObject _obj)
    {
        _attack = _obj.GetComponent<AttackTriggerable>();
        _attack.WeaponDamage = Damage;
        _attack.WeaponSpeed = Speed;
        _attack.BeElasticity = BeElasticity;
    }

    public override void Activate()
    {
        _attack.Katada();
    }
}
