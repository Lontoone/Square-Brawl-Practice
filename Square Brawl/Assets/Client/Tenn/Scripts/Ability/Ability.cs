using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
    public string Name = "New Ability";
    public AudioClip Sound;
    public float ActiveTime;
    public float CoolDownTime;

    public abstract void Initalize(GameObject _obj);
    public abstract void Activate();
}
