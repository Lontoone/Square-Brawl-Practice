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
    public bool IsKatadaReverse;

    private GameObject _bulletSpawnPos;
    private GameObject _karataSpawnPos;
    private PlayerController _playerController;
    private Bullet _bullet;
    private Katada _katada;

    private void Start()
    {
        _bulletSpawnPos = GameObject.FindGameObjectWithTag("BulletSpawnPos");
        _karataSpawnPos = GameObject.FindGameObjectWithTag("MidPos");
        _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public void Fire()
    {

        //GameObject _bulletObj = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Bullet"), BulletSpawnPos.transform.position, BulletSpawnPos.transform.rotation);

        GameObject _bulletObj = GameObject.FindGameObjectWithTag("ObjectPool").GetComponent<ObjectsPool>().SpawnFromPool("Bullet", _bulletSpawnPos.transform.position, _bulletSpawnPos.transform.rotation,null);

        _bullet = _bulletObj.GetComponent<Bullet>();

        _bullet.ShootSpeed = WeaponSpeed;
        _bullet.ShootDamage = WeaponDamage;
        _bullet.IsDontShootStraight = IsDontShootStraight;
        _bullet.BulletScaleValue = WeaponScaleValue;
        _bullet.BeShootElasticity = BeElasticity;

        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().PlayerRecoil(WeaponRecoil);
    }

    public void Charge()
    {
        //_playerController.MoveSpeed = ShootSpeed;
        _playerController.IsCharge = true;
        _playerController.PlayerRecoil(-WeaponSpeed);
        _playerController.BeElasticity = BeElasticity;
        _playerController.Damage = WeaponDamage;
        StartCoroutine(ChargeIsFalse());
    }

    public void Katada()
    {
        IsKatadaReverse = !IsKatadaReverse;
        //GameObject _katadaObj = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Katada"), BulletSpawnPos.transform.position, BulletSpawnPos.transform.rotation);
        GameObject _katadaObj = GameObject.FindGameObjectWithTag("ObjectPool").GetComponent<ObjectsPool>().SpawnFromPool("Katada", _karataSpawnPos.transform.position, _bulletSpawnPos.transform.rotation, null);
        _katada = _katadaObj.GetComponent<Katada>();
        _katada.IsKatadaReverse = IsKatadaReverse;
        _katada.Damage = WeaponDamage;
        _katada.BeAttackElasticity = BeElasticity;
        _katada.Speed = WeaponSpeed;
    }

    IEnumerator ChargeIsFalse()
    {
        yield return new WaitForSeconds(0.5f);
        _playerController.IsCharge = false;
    }
}
