using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class AttackTriggerable : MonoBehaviour
{
    [HideInInspector] public int WeaponDamage;
    [HideInInspector] public float WeaponSpeed;
    [HideInInspector] public float WeaponRecoil;
    [HideInInspector] public float BeElasticity;
    [HideInInspector] public float WeaponScaleValue;
    [HideInInspector] public bool IsDontContinuous;
    [HideInInspector] public bool IsDontShootStraight;

    public bool IsSpeicalBool;

    private GameObject _bulletSpawnPos;
    private GameObject _bulletMidSpawnPos;
    private PlayerController _playerController;
    private ObjectsPool _objectsPool;
    private Bullet _bullet;
    private Katada _katada;

    private void Start()
    {
        _bulletSpawnPos = GameObject.FindGameObjectWithTag("BulletSpawnPos");
        _bulletMidSpawnPos = GameObject.FindGameObjectWithTag("MidPos");
        _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        _objectsPool = GameObject.FindGameObjectWithTag("ObjectPool").GetComponent<ObjectsPool>();
    }

    public void Fire()
    {
        GameObject _bulletObj = _objectsPool.SpawnFromPool("Bullet", _bulletSpawnPos.transform.position, _bulletSpawnPos.transform.rotation,null);
        _bullet = _bulletObj.GetComponent<Bullet>();
        _bullet.BulletSpeed = WeaponSpeed;
        _bullet.BulletDamage = WeaponDamage;
        _bullet.IsDontShootStraight = IsDontShootStraight;
        _bullet.BulletScaleValue = WeaponScaleValue;
        _bullet.BulletBeElasticity = BeElasticity;
        _playerController.PlayerRecoil(WeaponRecoil);
    }

    public void Charge()
    {
        _playerController.IsCharge = true;
        _playerController.PlayerRecoil(-WeaponSpeed);
        _playerController.BeElasticity = BeElasticity;
        _playerController.Damage = WeaponDamage;
        StartCoroutine(IsBoolChangeFalse());
    }

    public void Katada()
    {
        IsSpeicalBool = !IsSpeicalBool;
        GameObject _katadaObj = _objectsPool.SpawnFromPool("Katada", _bulletMidSpawnPos.transform.position, _bulletSpawnPos.transform.rotation, null);
        _katada = _katadaObj.GetComponent<Katada>();
        _katada.IsKatadaReverse = IsSpeicalBool;
        _katada.KatadaDamage = WeaponDamage;
        _katada.KatadaBeElasticity = BeElasticity;
        _katada.KatadaSpeed = WeaponSpeed;
    }

    public void Freeze()
    {
        _playerController.IsShootFreeze = true;
        _playerController.PlayerRecoil(WeaponRecoil);
        _objectsPool.SpawnFromPool("FreezeShoot", _bulletSpawnPos.transform.position, _bulletSpawnPos.transform.rotation, null);
        StartCoroutine(IsFreezeChangeFalse());
    }

    IEnumerator IsBoolChangeFalse()
    {
        yield return new WaitForSeconds(0.5f);
        _playerController.IsCharge = false;
    }

    IEnumerator IsFreezeChangeFalse()
    {
        yield return new WaitForSeconds(0.5f);
        _playerController.IsShootFreeze = false;
        _playerController.IsFreezeChange = false;
    }
}
