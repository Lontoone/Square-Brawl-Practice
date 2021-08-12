using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
    public string Name = "New Ability";
    public string LaunchEffectName;
    public string ExploseEffectName;
    public float CoolDownTime;
    public int Damage;
    public float Speed;
    public float Recoil;
    public float BeElasticity;

    public Vector3 BeShootShakeValue;
    public Vector3 ShootShakeValue;

    public bool isCdCanAdd;
    public bool isHaveTwoCd;

    public string FireSound;

    protected AttackTriggerable _attack;

    public abstract void Initalize(GameObject _obj);
    public abstract void Activate();
}
