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

    private ShootTriggerable _shoot;

    public override void Initalize(GameObject _obj)
    {
        _shoot = _obj.GetComponent<ShootTriggerable>();
        _shoot.ShootDamage = ShootDamage;
        _shoot.ShootSpeed = ShootSpeed;
        _shoot.ShootRecoil = ShootRecoil;
        _shoot.BeShootElasticity = BeShootElasticity;
        _shoot.BulletScaleValue = BulletScaleValue;
        _shoot.IsDontContinuous = IsDontContinuous;
        _shoot.IsDontShootStraight = IsDontShootStraight;
    }

    public override void Activate()
    {
        _shoot.Fire();
    }
}
    
