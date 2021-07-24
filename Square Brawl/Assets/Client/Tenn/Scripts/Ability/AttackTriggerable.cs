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
        _bullet.ShootFunc(ExploseEffectName,WeaponSpeed, WeaponDamage, WeaponScaleValue, BeElasticity, IsDontShootStraight);
        PlayerController.instance.RecoilFunc(WeaponRecoil);
    }

    public void ScatterFire()
    {
        for (int i = 0; i <= 4; i++)
        {
            GameObject[] _bulletObj = new GameObject[5];
            _bulletObj[i]= ObjectsPool.Instance.SpawnFromPool("Bullet", _bulletSpawnPos.transform.position, _bulletSpawnPos.transform.rotation, null);
            Bullet _bullet = _bulletObj[i].GetComponent<Bullet>();
            _bullet.ShootFunc(ExploseEffectName, WeaponSpeed, WeaponDamage, WeaponScaleValue, BeElasticity, IsDontShootStraight);
        }
        PlayerController.instance.RecoilFunc(WeaponRecoil);
    }

    public void Charge()
    {
        PlayerController.instance.ChargeFunc(-WeaponSpeed, BeElasticity, WeaponDamage);
    }

    public void Katada()
    {
        _isKatadaReverse = !_isKatadaReverse;
        GameObject _katadaObj = ObjectsPool.Instance.SpawnFromPool("Katada", _bulletMidSpawnPos.transform.position, _bulletSpawnPos.transform.rotation, null);
        Katada _katada = _katadaObj.GetComponent<Katada>();
        _katada.KatadaFunc(WeaponSpeed, WeaponDamage, BeElasticity, _isKatadaReverse);
    }

    public void Shield()
    {
        PlayerController.instance.IsShield = true;
        Shield _shield = PlayerController.instance.transform.GetChild(4).gameObject.GetComponent<Shield>();
        _shield.gameObject.SetActive(true);
        _shield.ShieldFunc(WeaponSpeed, WeaponDamage, BeElasticity);
    }

    public void Freeze()
    {
        PlayerController.instance.RecoilFunc(WeaponRecoil);
        PlayerController.instance.FreezeFunc(3, 5);
        ObjectsPool.Instance.SpawnFromPool("FreezeShoot", _bulletSpawnPos.transform.position, _bulletSpawnPos.transform.rotation, null);
    }

    public void GrenadeFire()
    {
        GameObject _grenadeObj = ObjectsPool.Instance.SpawnFromPool(Name, _bulletSpawnPos.transform.position, _bulletSpawnPos.transform.rotation, null);
        Grenade _grenade = _grenadeObj.GetComponent<Grenade>();
        _grenade.GrenadeFunc(ExploseEffectName, WeaponSpeed, WeaponDamage, BeElasticity);
        PlayerController.instance.RecoilFunc(WeaponRecoil);
    }
}
