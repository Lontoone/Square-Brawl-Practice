using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/ShootAbility")]
public class ShootAbility : Ability
{
    public int ShootDamage;
    public float ShootSpeed;
    public float ShootRecoil;
    public float BeShootElasticity;
    public float BulletScaleValue;
    public bool IsDontContinuous;
    public bool IsDontShootStraight;

    private AttackTriggerable _attack;

    public override void Initalize(GameObject _obj)
    {
        _attack = _obj.GetComponent<AttackTriggerable>();
        _attack.WeaponDamage = ShootDamage;
        _attack.WeaponSpeed = ShootSpeed;
        _attack.WeaponRecoil = ShootRecoil;
        _attack.BeElasticity = BeShootElasticity;
        _attack.WeaponScaleValue = BulletScaleValue;
        _attack.IsDontContinuous = IsDontContinuous;
        _attack.IsDontShootStraight = IsDontShootStraight;
    }

    public override void Activate()
    {
        _attack.Fire();
    }
}
    
