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

    public bool IsSpeicalBool;

    private GameObject _bulletSpawnPos;
    private GameObject _bulletMidSpawnPos;
    private PlayerController _playerController;
    public GameObject[] gameObjects;
    //private ObjectsPool _objectsPool;
    //private Bullet _bullet;
    //private Katada _katada;

    private void Start()
    {
        _bulletSpawnPos = GameObject.FindGameObjectWithTag("BulletSpawnPos");
        _bulletMidSpawnPos = GameObject.FindGameObjectWithTag("MidPos");
        _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
       // _objectsPool = GameObject.FindGameObjectWithTag("ObjectPool").GetComponent<ObjectsPool>();
    }

    public void Fire()
    {
       // GameObject _bulletObj = _objectsPool.SpawnFromPool("Bullet", _bulletSpawnPos.transform.position, _bulletSpawnPos.transform.rotation,null);
        GameObject _bulletObj = ObjectsPool.Instance.SpawnFromPool("Bullet", _bulletSpawnPos.transform.position, _bulletSpawnPos.transform.rotation, null);
        Bullet _bullet = _bulletObj.GetComponent<Bullet>();
        _bullet.ExploseEffectName = ExploseEffectName;
        _bullet.BulletSpeed = WeaponSpeed;
        _bullet.BulletDamage = WeaponDamage;
        _bullet.IsDontShootStraight = IsDontShootStraight;
        _bullet.BulletScaleValue = WeaponScaleValue;
        _bullet.BulletBeElasticity = BeElasticity;
        _playerController.PlayerRecoil(WeaponRecoil);
    }

    public void ScatterFire()
    {
        for (int i = 0; i <= 4; i++)
        {
            GameObject[] _bulletObj = new GameObject[5];
            _bulletObj[i]= ObjectsPool.Instance.SpawnFromPool("Bullet", _bulletSpawnPos.transform.position, _bulletSpawnPos.transform.rotation, null);
            Bullet _bullet = _bulletObj[i].GetComponent<Bullet>();
            _bullet.ExploseEffectName = ExploseEffectName;
            _bullet.BulletSpeed = WeaponSpeed;
            _bullet.BulletDamage = WeaponDamage;
            _bullet.IsDontShootStraight = IsDontShootStraight;
            _bullet.BulletScaleValue = WeaponScaleValue;
            _bullet.BulletBeElasticity = BeElasticity;
        }
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
        //GameObject _katadaObj = _objectsPool.SpawnFromPool("Katada", _bulletMidSpawnPos.transform.position, _bulletSpawnPos.transform.rotation, null);
        GameObject _katadaObj = ObjectsPool.Instance.SpawnFromPool("Katada", _bulletMidSpawnPos.transform.position, _bulletSpawnPos.transform.rotation, null);
        Katada _katada = _katadaObj.GetComponent<Katada>();
        _katada.IsKatadaReverse = IsSpeicalBool;
        _katada.KatadaDamage = WeaponDamage;
        _katada.KatadaBeElasticity = BeElasticity;
        _katada.KatadaSpeed = WeaponSpeed;
    }

    public void Shield()
    {
        //GameObject _shieldObj = ObjectsPool.Instance.SpawnFromPool("Shield", _bulletMidSpawnPos.transform.position, Quaternion.identity, transform.parent);

        _playerController.transform.GetChild(4).gameObject.SetActive(true);
        Shield _shield = _playerController.transform.GetChild(4).gameObject.GetComponent<Shield>();
        _shield.ShieldSpeed = WeaponSpeed;
        _shield.ShieldDamage = WeaponDamage;
        _shield.ShieldBeElasticity = BeElasticity;

        IPoolObject pooledObj = _shield.GetComponent<IPoolObject>();
        if (pooledObj != null)
        {
            pooledObj.OnObjectSpawn();
        }
    }

    public void Freeze()
    {
        _playerController.IsShootFreeze = true;
        _playerController.PlayerRecoil(WeaponRecoil);
        ObjectsPool.Instance.SpawnFromPool("FreezeShoot", _bulletSpawnPos.transform.position, _bulletSpawnPos.transform.rotation, null);
        StartCoroutine(IsFreezeChangeFalse());
    }

    public void GrenadeFire()
    {
        GameObject _grenadeObj = ObjectsPool.Instance.SpawnFromPool(Name, _bulletSpawnPos.transform.position, _bulletSpawnPos.transform.rotation, null);
        Grenade _grenade = _grenadeObj.GetComponent<Grenade>();
        _grenade.BulletSpeed = WeaponSpeed;
        _grenade.BulletDamage = WeaponDamage;
        _grenade.BulletBeElasticity = BeElasticity;
        _grenade.ExploseEffectName = ExploseEffectName;
        _playerController.PlayerRecoil(WeaponRecoil);
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
