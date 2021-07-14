using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Grenade : Bullet, IPoolObject
{
    private bool isShoot;
    protected override void Start()
    {
        base.Start();
    }

    protected override void FixedUpdate()
    {
        if (!isShoot)
        {
            _rb.AddForce(BulletSpeed * transform.right);
            transform.eulerAngles = Vector3.zero;
            StartCoroutine(Boom());
            isShoot = true;
        }
    }

    IEnumerator Boom()
    {
        yield return new WaitForSeconds(1f);
        Explose();
        ObjectsPool.Instance.SpawnFromPool(ExploseEffectName, transform.position, transform.rotation, null);
        isShoot = false;
        _pv.RPC("Rpc_DisableObj", RpcTarget.All);
    }

    public void Explose()
    {

    }
}
