using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/BounceAbility")]
public class BounceAbility : Ability
{
    public override void Initalize(GameObject _obj)
    {
        _attack = _obj.GetComponent<AttackTriggerable>();
        _attack.WeaponDamage = Damage;
        _attack.BeElasticity = BeElasticity;
        _attack.ExploseEffectName = ExploseEffectName;
        _attack.ShotShakeValue = ShotShakeValue;
        _attack.BeShotShakeValue = BeShotShakeValue;
    }
    public override void Activate()
    {
        _attack.Bounce();
    }
}
