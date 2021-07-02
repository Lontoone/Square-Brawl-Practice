using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class ShootTriggerable : MonoBehaviour
{
    [HideInInspector] public int ShootDamage;
    [HideInInspector] public float ShootSpeed;
    [HideInInspector] public float ShootRecoil;
    [HideInInspector] public float BeShootElasticity;
    [HideInInspector] public float BulletScaleValue;
    [HideInInspector] public bool IsDontContinuous;
    [HideInInspector] public bool IsDontShootStraight;

    [HideInInspector] public GameObject BulletSpawnPos;

    public Bullet _bullet;

    public void Fire()
    {
        BulletSpawnPos = GameObject.FindGameObjectWithTag("BulletSpawnPos");

        GameObject _bulletObj = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Bullet"), BulletSpawnPos.transform.position, BulletSpawnPos.transform.rotation);

        _bullet = _bulletObj.GetComponent<Bullet>();

        /*_bulletObj.GetComponent<Transform>().localScale = new Vector3(BulletScaleValue, BulletScaleValue, BulletScaleValue);

        _bulletObj.GetComponent<Bullet>().ShootSpeed = ShootSpeed;

        _bulletObj.GetComponent<Bullet>().ShootDamage = ShootDamage;

        _bulletObj.GetComponent<Bullet>().IsDontShootStraight = IsDontShootStraight;*/

        _bullet.ShootSpeed = ShootSpeed;
        _bullet.ShootDamage = ShootDamage;
        _bullet.IsDontShootStraight = IsDontShootStraight;
        _bullet.BulletScaleValue = BulletScaleValue;
        //GameObject _bulletObj = Instantiate(BulletPrefab, BulletSpawnPos.transform.position, BulletSpawnPos.transform.rotation);

        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().PlayerRecoil(ShootRecoil);
    }
}
