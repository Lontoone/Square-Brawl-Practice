using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootTriggerable : MonoBehaviour
{
    [HideInInspector] public int ShootDamage;
    [HideInInspector] public float ShootSpeed;
    [HideInInspector] public float ShootRecoil;
    [HideInInspector] public float BeShootElasticity;
    [HideInInspector] public float BulletScaleValue;
    [HideInInspector] public bool IsCanContinuous;

    [HideInInspector] public GameObject BulletSpawnPos;

    public GameObject BulletPrefab;

    public void Fire()
    {
        BulletSpawnPos = GameObject.FindGameObjectWithTag("BulletSpawnPos");

        GameObject _bulletObj = Instantiate(BulletPrefab, BulletSpawnPos.transform.position, BulletSpawnPos.transform.rotation);

        PlayerController.Instance.PlayerRecoil(ShootRecoil);

        _bulletObj.GetComponent<Transform>().localScale = new Vector3(BulletScaleValue, BulletScaleValue, BulletScaleValue);

        _bulletObj.GetComponent<Bullet>().ShootSpeed = ShootSpeed;
    }
}
