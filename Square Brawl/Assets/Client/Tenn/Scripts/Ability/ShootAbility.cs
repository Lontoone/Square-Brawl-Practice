using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/ShootAbility")]
public class ShootAbility : Ability
{
    public float BulletScaleValue;
    public bool IsDontContinuous;
    public bool IsDontShootStraight;
    public bool IsShotgun;
    public bool IsGrenade;
    public bool IsSniper;

    public override void Initalize(GameObject _obj)
    {
        _attack = _obj.GetComponent<AttackTriggerable>();
        _attack.Name = _attack.SoundName = Name;
        _attack.WeaponDamage = Damage;
        _attack.WeaponSpeed = Speed;
        _attack.WeaponRecoil = Recoil;
        _attack.BeElasticity = BeElasticity;
        _attack.LaunchEffectName = LaunchEffectName;
        _attack.ExploseEffectName = ExploseEffectName;
        _attack.WeaponScaleValue = BulletScaleValue;
        _attack.IsDontContinuous = IsDontContinuous;
        _attack.IsDontShootStraight = IsDontShootStraight;
        _attack.IsSniper = IsSniper;
        _attack.ShootShakeValue = ShootShakeValue;
        _attack.BeShootShakeValue = BeShootShakeValue;
    }

    public override void Activate()
    {
        if (!IsShotgun && !IsGrenade)
        {
            _attack.Fire();
        }
        else if (IsShotgun)
        {
            _attack.ShotgunFire();
        }
        else if (IsGrenade)
        {
            _attack.GrenadeFire();
        }
    }
}
    
