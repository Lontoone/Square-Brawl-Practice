using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/ShootAbility")]
public class ShootAbility : Ability
{
    public float BulletScaleValue;
    public bool IsDontContinuous;
    public bool IsDontShootStraight;
    public bool IsScatterShot;
    public bool IsGrenade;
    public override void Initalize(GameObject _obj)
    {
        _attack = _obj.GetComponent<AttackTriggerable>();
        _attack.WeaponDamage = Damage;
        _attack.WeaponSpeed = Speed;
        _attack.WeaponRecoil = Recoil;
        _attack.BeElasticity = BeElasticity;
        _attack.LaunchEffectName = LaunchEffectName;
        _attack.ExploseEffectName = ExploseEffectName;
        _attack.WeaponScaleValue = BulletScaleValue;
        _attack.IsDontContinuous = IsDontContinuous;
        _attack.IsDontShootStraight = IsDontShootStraight;
    }

    public override void Activate()
    {
        if (!IsScatterShot&&!IsGrenade)
        {
            _attack.Fire();
        }
        else if (IsScatterShot)
        {
            _attack.ScatterFire();
        }
        else if (IsGrenade)
        {
            _attack.GrenadeFire();
        }
    }
}
    
