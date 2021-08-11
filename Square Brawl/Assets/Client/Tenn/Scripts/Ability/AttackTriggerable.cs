using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class AttackTriggerable : MonoBehaviour
{
    [HideInInspector] public string Name;
    [HideInInspector] public int WeaponDamage;
    [HideInInspector] public float WeaponSpeed;
    [HideInInspector] public float WeaponRecoil;
    [HideInInspector] public float BeElasticity;
    [HideInInspector] public float WeaponScaleValue;
    [HideInInspector] public bool IsDontContinuous;
    [HideInInspector] public bool IsDontShootStraight;
    [HideInInspector] public bool IsSniper;
    [HideInInspector] public string LaunchEffectName;
    [HideInInspector] public string ExploseEffectName;
    [HideInInspector] public Vector3 BeShootShakeValue;
    [HideInInspector] public Vector3 ShootShakeValue;


    private bool _isKatadaReverse;

    private GameObject _bulletSpawnPos;
    private GameObject _bulletMidSpawnPos;

    private void Start()
    {
        _bulletSpawnPos = GameObject.FindGameObjectWithTag("BulletSpawnPos");
        _bulletMidSpawnPos = GameObject.FindGameObjectWithTag("MidPos");
    }

    public void Fire()
    {
        GameObject _bulletObj = ObjectsPool.Instance.SpawnFromPool("Bullet", _bulletSpawnPos.transform.position, _bulletSpawnPos.transform.rotation, null);
        Bullet _bullet = _bulletObj.GetComponent<Bullet>();
        if (!IsDontShootStraight)
        {
            ObjectsPool.Instance.SpawnFromPool("Spark", _bulletSpawnPos.transform.position, _bulletSpawnPos.transform.rotation, null);
        }

        _bullet.IsSniper = IsSniper;
        CameraShake.instance.SetShakeValue(ShootShakeValue.x, ShootShakeValue.y, ShootShakeValue.z);
        _bullet.ShootEvent(ExploseEffectName,WeaponSpeed, WeaponDamage, WeaponScaleValue, BeElasticity, IsDontShootStraight, BeShootShakeValue);
        PlayerController.instance.PlayerRecoil(WeaponRecoil);
    }

    public void ScatterFire()
    {
        for (int i = 0; i <= 4; i++)
        {
            GameObject[] _bulletObj = new GameObject[5];
            _bulletObj[i]= ObjectsPool.Instance.SpawnFromPool("Bullet", _bulletSpawnPos.transform.position, _bulletSpawnPos.transform.rotation, null);
            Bullet _bullet = _bulletObj[i].GetComponent<Bullet>();
            _bullet.ShootEvent(ExploseEffectName, WeaponSpeed, WeaponDamage, WeaponScaleValue, BeElasticity, IsDontShootStraight, BeShootShakeValue);
        }
        ObjectsPool.Instance.SpawnFromPool("Spark", _bulletSpawnPos.transform.position, _bulletSpawnPos.transform.rotation, null);
        CameraShake.instance.SetShakeValue(ShootShakeValue.x, ShootShakeValue.y, ShootShakeValue.z);
        PlayerController.instance.PlayerRecoil(WeaponRecoil);
    }

    public void Charge()
    {
        CameraShake.instance.SetShakeValue(ShootShakeValue.x, ShootShakeValue.y, ShootShakeValue.z);
        PlayerController.instance.ChargeEvent(-WeaponSpeed, BeElasticity, WeaponDamage, BeShootShakeValue);
    }

    public void Katada()
    {
        _isKatadaReverse = !_isKatadaReverse;
        GameObject _katadaObj = ObjectsPool.Instance.SpawnFromPool("Katada", _bulletMidSpawnPos.transform.position, _bulletSpawnPos.transform.rotation, null);
        Katada _katada = _katadaObj.GetComponent<Katada>();
        _katada.KatadaEvent(WeaponSpeed, WeaponDamage, BeElasticity, _isKatadaReverse, BeShootShakeValue);
    }

    public void Shield()
    {
        PlayerController.instance.IsShieldTrue();
        Shield _shield = PlayerController.instance.transform.GetChild(4).gameObject.GetComponent<Shield>();
        _shield.gameObject.SetActive(true);
        _shield.ShieldEvent(WeaponSpeed, WeaponDamage, BeElasticity, BeShootShakeValue);
        CameraShake.instance.SetShakeValue(ShootShakeValue.x, ShootShakeValue.y, ShootShakeValue.z);
    }

    public void Freeze()
    {
        CameraShake.instance.SetShakeValue(ShootShakeValue.x, ShootShakeValue.y, ShootShakeValue.z);
        PlayerController.instance.PlayerRecoil(WeaponRecoil);
        PlayerController.instance.FreezeEvent(1.8f, 5,BeShootShakeValue);
        ObjectsPool.Instance.SpawnFromPool("FreezeShoot", _bulletSpawnPos.transform.position, _bulletSpawnPos.transform.rotation, null);
    }

    public void GrenadeFire()
    {
        GameObject _grenadeObj = ObjectsPool.Instance.SpawnFromPool(Name, _bulletSpawnPos.transform.position, _bulletSpawnPos.transform.rotation, null);
        Grenade _grenade = _grenadeObj.GetComponent<Grenade>();
        CameraShake.instance.SetShakeValue(ShootShakeValue.x, ShootShakeValue.y, ShootShakeValue.z);
        _grenade.GrenadeEvent(ExploseEffectName, WeaponSpeed, WeaponDamage, WeaponScaleValue, BeElasticity,BeShootShakeValue);
        PlayerController.instance.PlayerRecoil(WeaponRecoil);
    }

    public void Bounce()
    {
        GameObject _bounceObj = ObjectsPool.Instance.SpawnFromPool("Bounce", _bulletMidSpawnPos.transform.position, _bulletSpawnPos.transform.rotation, null);
        CameraShake.instance.SetShakeValue(ShootShakeValue.x, ShootShakeValue.y, ShootShakeValue.z);
        Bounce _bounce = _bounceObj.GetComponent<Bounce>();
        _bounce.BounceEvent(WeaponDamage, BeElasticity, ExploseEffectName, _bulletSpawnPos.transform.right, BeShootShakeValue);
    }
}
