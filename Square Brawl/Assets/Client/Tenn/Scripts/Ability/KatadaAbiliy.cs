using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/KatadaAbility")]
public class KatadaAbiliy : Ability
{
    public int KatadaDamage;
    public float KatadaSpeed;
    public float BeElasticity;
    private AttackTriggerable _attack;
    // Start is called before the first frame update
    public override void Initalize(GameObject _obj)
    {
        _attack = _obj.GetComponent<AttackTriggerable>();
        _attack.WeaponDamage = KatadaDamage;
        _attack.WeaponSpeed = KatadaSpeed;
        _attack.BeElasticity = BeElasticity;
    }

    public override void Activate()
    {
        _attack.Katada();
    }
}
