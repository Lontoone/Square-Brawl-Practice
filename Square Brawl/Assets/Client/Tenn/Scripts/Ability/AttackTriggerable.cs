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
    [HideInInspector] public bool IsKatadaReverse;

    public GameObject BulletSpawnPos;
    public PlayerController _playerController;

    public Bullet _bullet;
    public Katada _katada;

    public void Fire()
    {
        BulletSpawnPos = GameObject.FindGameObjectWithTag("BulletSpawnPos");

        GameObject _bulletObj = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Bullet"), BulletSpawnPos.transform.position, BulletSpawnPos.transform.rotation);

        //GameObject _bulletObj = GameObject.FindGameObjectWithTag("ObjectPool").GetComponent<ObjectsPool>().SpawnFromPool("Bullet", BulletSpawnPos.transform.position, BulletSpawnPos.transform.rotation,null);

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
        _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
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
        BulletSpawnPos = GameObject.FindGameObjectWithTag("MidPos");
        GameObject _katadaObj = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Katada"), BulletSpawnPos.transform.position, BulletSpawnPos.transform.rotation);
        _katadaObj.transform.parent = transform.parent;
        _katada = _katadaObj.GetComponent<Katada>();
        _katada.IsKatadaReverse = IsKatadaReverse;
        _katada.Damage = WeaponDamage;
        _katada.BeShootElasticity = BeElasticity;
        _katada.Speed = WeaponSpeed;
    }

    IEnumerator ChargeIsFalse()
    {
        yield return new WaitForSeconds(0.5f);
        _playerController.IsCharge = false;
    }
}
