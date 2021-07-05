using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class ShootTriggerable : MonoBehaviour
{
     public int ShootDamage;
    [HideInInspector] public float ShootSpeed;
    [HideInInspector] public float ShootRecoil;
    [HideInInspector] public float BeShootElasticity;
    [HideInInspector] public float BulletScaleValue;
    [HideInInspector] public bool IsDontContinuous;
    [HideInInspector] public bool IsDontShootStraight;

    public GameObject BulletSpawnPos;

    public Bullet _bullet;

    public void Fire()
    {
        BulletSpawnPos = GameObject.FindGameObjectWithTag("BulletSpawnPos");

        //GameObject _bulletObj = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Bullet"), BulletSpawnPos.transform.position, BulletSpawnPos.transform.rotation);

        GameObject _bulletObj = GameObject.FindGameObjectWithTag("ObjectPool").GetComponent<ObjectsPool>().SpawnFromPool("Bullet", BulletSpawnPos.transform.position, BulletSpawnPos.transform.rotation,null);

        _bullet = _bulletObj.GetComponent<Bullet>();

        _bullet.ShootSpeed = ShootSpeed;
        _bullet.ShootDamage = ShootDamage;
        _bullet.IsDontShootStraight = IsDontShootStraight;
        _bullet.BulletScaleValue = BulletScaleValue;
        _bullet.BeShootElasticity = BeShootElasticity;

        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().PlayerRecoil(ShootRecoil);

    }
}
