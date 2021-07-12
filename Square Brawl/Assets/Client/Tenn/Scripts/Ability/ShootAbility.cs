using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/ShootAbility")]
public class ShootAbility : Ability
{
    public float BulletScaleValue;
    public bool IsDontContinuous;
    public bool IsDontShootStraight;

    public override void Initalize(GameObject _obj)
    {
        _attack = _obj.GetComponent<AttackTriggerable>();
        _attack.WeaponDamage = Damage;
        _attack.WeaponSpeed = Speed;
        _attack.WeaponRecoil = Recoil;
        _attack.BeElasticity = BeElasticity;
        _attack.WeaponScaleValue = BulletScaleValue;
        _attack.IsDontContinuous = IsDontContinuous;
        _attack.IsDontShootStraight = IsDontShootStraight;
    }

    public override void Activate()
    {
        _attack.Fire();
    }
}
    
