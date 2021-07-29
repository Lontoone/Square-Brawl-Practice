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
    [HideInInspector] public string LaunchEffectName;
    [HideInInspector] public string ExploseEffectName;
    [HideInInspector] public Vector3 BeShotShakeValue;
    [HideInInspector] public Vector3 ShotShakeValue;

    private bool _isKatadaReverse;

    private GameObject _bulletSpawnPos;
    private GameObject _bulletMidSpawnPos;
    public GameObject[] gameObjects;

    public event Action test;
    private void Start()
    {
        _bulletSpawnPos = GameObject.FindGameObjectWithTag("BulletSpawnPos");
        _bulletMidSpawnPos = GameObject.FindGameObjectWithTag("MidPos");
    }

    public void Fire()
    {
        GameObject _bulletObj = ObjectsPool.Instance.SpawnFromPool("Bullet", _bulletSpawnPos.transform.position, _bulletSpawnPos.transform.rotation, null);
        Bullet _bullet = _bulletObj.GetComponent<Bullet>();
        CameraShake.instance.SetShakeValue(ShotShakeValue.x, ShotShakeValue.y, ShotShakeValue.z);
        _bullet.ShootFunc(ExploseEffectName,WeaponSpeed, WeaponDamage, WeaponScaleValue, BeElasticity, IsDontShootStraight, BeShotShakeValue);
        PlayerController.instance.RecoilFunc(WeaponRecoil);
    }

    public void ScatterFire()
    {
        for (int i = 0; i <= 4; i++)
        {
            GameObject[] _bulletObj = new GameObject[5];
            _bulletObj[i]= ObjectsPool.Instance.SpawnFromPool("Bullet", _bulletSpawnPos.transform.position, _bulletSpawnPos.transform.rotation, null);
            Bullet _bullet = _bulletObj[i].GetComponent<Bullet>();
            _bullet.ShootFunc(ExploseEffectName, WeaponSpeed, WeaponDamage, WeaponScaleValue, BeElasticity, IsDontShootStraight, BeShotShakeValue);
        }
        CameraShake.instance.SetShakeValue(ShotShakeValue.x, ShotShakeValue.y, ShotShakeValue.z);
        PlayerController.instance.RecoilFunc(WeaponRecoil);
    }

    public void Charge()
    {
        CameraShake.instance.SetShakeValue(ShotShakeValue.x, ShotShakeValue.y, ShotShakeValue.z);
        PlayerController.instance.ChargeFunc(-WeaponSpeed, BeElasticity, WeaponDamage, BeShotShakeValue);
    }

    public void Katada()
    {
        _isKatadaReverse = !_isKatadaReverse;
        GameObject _katadaObj = ObjectsPool.Instance.SpawnFromPool("Katada", _bulletMidSpawnPos.transform.position, _bulletSpawnPos.transform.rotation, null);
        Katada _katada = _katadaObj.GetComponent<Katada>();
        _katada.KatadaFunc(WeaponSpeed, WeaponDamage, BeElasticity, _isKatadaReverse, BeShotShakeValue);
    }

    public void Shield()
    {
        PlayerController.instance.IsShield = true;
        Shield _shield = PlayerController.instance.transform.GetChild(4).gameObject.GetComponent<Shield>();
        _shield.gameObject.SetActive(true);
        _shield.ShieldFunc(WeaponSpeed, WeaponDamage, BeElasticity, BeShotShakeValue);
        CameraShake.instance.SetShakeValue(ShotShakeValue.x, ShotShakeValue.y, ShotShakeValue.z);
    }

    public void Freeze()
    {
        CameraShake.instance.SetShakeValue(ShotShakeValue.x, ShotShakeValue.y, ShotShakeValue.z);
        PlayerController.instance.RecoilFunc(WeaponRecoil);
        PlayerController.instance.FreezeFunc(3, 5,BeShotShakeValue);
        ObjectsPool.Instance.SpawnFromPool("FreezeShoot", _bulletSpawnPos.transform.position, _bulletSpawnPos.transform.rotation, null);
    }

    public void GrenadeFire()
    {
        GameObject _grenadeObj = ObjectsPool.Instance.SpawnFromPool(Name, _bulletSpawnPos.transform.position, _bulletSpawnPos.transform.rotation, null);
        Grenade _grenade = _grenadeObj.GetComponent<Grenade>();
        CameraShake.instance.SetShakeValue(ShotShakeValue.x, ShotShakeValue.y, ShotShakeValue.z);
        _grenade.GrenadeFunc(ExploseEffectName, WeaponSpeed, WeaponDamage, BeElasticity,BeShotShakeValue);
        PlayerController.instance.RecoilFunc(WeaponRecoil);
    }

    public void Bounce()
    {
        CameraShake.instance.SetShakeValue(ShotShakeValue.x, ShotShakeValue.y, ShotShakeValue.z);
        Bounce _bounce = PlayerController.instance.transform.GetChild(5).gameObject.GetComponent<Bounce>();
        _bounce.BounceFunc(WeaponDamage, BeElasticity, ExploseEffectName, _bulletSpawnPos.transform.right, BeShotShakeValue);
    }
}
