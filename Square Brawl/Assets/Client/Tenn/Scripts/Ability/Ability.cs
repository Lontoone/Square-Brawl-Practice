using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
    public string Name = "New Ability";
    public AudioClip Sound;
    public float CoolDownTime;
    public int Damage;
    public float Speed;
    public float Recoil;
    public float BeElasticity;

    public bool isCdCanAdd;
    public bool isHaveTwoCd;

    protected AttackTriggerable _attack;

    public abstract void Initalize(GameObject _obj);
    public abstract void Activate();
}
