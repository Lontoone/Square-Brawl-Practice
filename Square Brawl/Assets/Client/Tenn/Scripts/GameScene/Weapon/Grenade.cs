using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Grenade : Bullet, IPoolObject
{
    protected bool isShoot;
    public float FieldExplose;
    public float ExploseForce;

    public LayerMask LayerToExplose;
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        ResetValue();//Reset Bullet Value
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
        yield return new WaitForSeconds(0.1f);
        _pv.RPC("Rpc_DisableObj", RpcTarget.All);
    }

    public void Explose()
    {
        Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, FieldExplose, LayerToExplose);
        foreach (Collider2D obj in objects)
        {
            PlayerController _playerController = obj.GetComponent<PlayerController>();
            if (_pv.IsMine != _playerController.Pv.IsMine && _playerController.Pv.IsMine)
            {
                Vector2 dir = obj.transform.position - transform.position;
                _playerController.TakeDamage(BulletDamage, BulletBeElasticity, dir.x, dir.y);
            }
        }
        isShoot = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, FieldExplose);
    }
}
