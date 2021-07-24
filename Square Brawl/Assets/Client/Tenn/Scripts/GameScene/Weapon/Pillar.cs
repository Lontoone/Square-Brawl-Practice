﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Pillar : Grenade, IPoolObject
{
    public float GroundDistance;
    private float _growY;

    public bool _isGrow;
    public bool _isBoom;
    public bool _isCanAddForce;

    private Vector2 _colliderDir;
    private Vector2 _colliderSpawnPos;
    public LayerMask GroundLayer;

    protected override void Start()
    {
        _growY = 1f;
        _childObj = transform.GetChild(0).gameObject;
        base.Start();
    }

    public void OnObjectSpawn()
    {
        if (_pv.IsMine)
        {
            GroundDistance = 0.2f;
            _pv.RPC("Rpc_EnableObj", RpcTarget.All);
            _pv.RPC("Rpc_ResetPos", RpcTarget.Others, transform.position, transform.rotation);
        }
    }

    private void Update()
    {
        ColliderEvent();
    }

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
        if (!_pv.IsMine)
        {
            _childObj.transform.localPosition = Vector3.Lerp(_childObj.transform.localPosition, _childObjnetworkPosition, 15 * Time.deltaTime);
            _childObj.transform.localScale = Vector3.Lerp(_childObj.transform.localScale, _childObjnetworkScale, 15 * Time.deltaTime);
            transform.rotation = _networkDir;
        }
    }

    void ColliderEvent()
    {
        if (_isBoom&&_pv.IsMine)
        {
            if (_isGrow)
            {
                _growY = Mathf.Lerp(_growY, 15f, 10 * Time.deltaTime);
                _childObj.transform.localPosition = new Vector3(_childObj.transform.localPosition.x, (_growY / 2) - 0.5f, _childObj.transform.localPosition.z);
                _childObj.transform.localScale = new Vector3(_childObj.transform.localScale.x, _growY, _childObj.transform.localScale.z);
                StartCoroutine(Shorten());
            }
            else
            {
                _growY = Mathf.Lerp(_growY, 0f, 8 * Time.deltaTime);
                _childObj.transform.localPosition = new Vector3(_childObj.transform.localPosition.x, (_growY / 2) - 0.5f, _childObj.transform.localPosition.z);
                _childObj.transform.localScale = new Vector3(_childObj.transform.localScale.x, _growY, _childObj.transform.localScale.z);
                if (_growY <= 0.1f)
                {
                    _pv.RPC("Rpc_DisableObj", RpcTarget.All);
                    _childObj.transform.localPosition = new Vector3(_childObj.transform.localPosition.x, _childObj.transform.localPosition.y + 0.5f, _childObj.transform.localPosition.z);
                    _childObj.transform.localScale = new Vector3(_childObj.transform.localScale.x, 1, _childObj.transform.localScale.z);
                    _pv.RPC("Rpc_RbDynamic", RpcTarget.All);
                }
            }
        }
    }
    protected override void OnCollisionEnter2D(Collision2D other)
    {
        base.OnCollisionEnter2D(other);
        if (other.gameObject.CompareTag("Ground") && !_isGrow)
        {
            if (_pv.IsMine)
            {
                ObjectsPool.Instance.SpawnFromPool(ExploseEffectName, transform.position, transform.rotation, null);
                _pv.RPC("Rpc_RbStatic", RpcTarget.All,transform.position);
                GroundCheckEvent();
                _pv.RPC("Rpc_Explode", RpcTarget.All);
            }
        }
        else if (other.gameObject.CompareTag("Player") && _isCanAddForce)
        {
            PlayerController _playerController = other.gameObject.GetComponent<PlayerController>();
            if (isMaster == _playerController.Pv.IsMine && isMaster)
            {
                _playerController.BeBounce(GrenadeBeElasticity, _colliderDir.x, _colliderDir.y);
                _isCanAddForce = false;
            }
        }
        else if (other.gameObject.CompareTag("Player") && !_isCanAddForce &&!_isGrow)
        {
            PlayerController _playerController = other.gameObject.GetComponent<PlayerController>();
            if (isMaster != _playerController.Pv.IsMine && !isMaster && !_playerController.IsShield)
            {
                GroundDistance = 0.65f;
                GroundCheckEvent();
                _playerController.BeBounce(GrenadeBeElasticity * 2, _colliderDir.x, _colliderDir.y);
            }

            if (isMaster != _playerController.Pv.IsMine && isMaster && !_playerController.IsShield)
            {
                GroundDistance = 0.65f;
                transform.position = other.gameObject.transform.position;
                GroundCheckEvent();
                transform.position = _colliderSpawnPos;
                _pv.RPC("Rpc_RbStatic", RpcTarget.All, transform.position);
                _pv.RPC("Rpc_Explode", RpcTarget.All);
            }
        }
    }

    private void Explose()
    {
        Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, FieldExplose, LayerToExplose);
        foreach (Collider2D obj in objects)
        {
            PlayerController _playerController = obj.GetComponent<PlayerController>();

            if (isMaster == _playerController.Pv.IsMine && isMaster)
            {
                _playerController.BeBounce(GrenadeBeElasticity, _colliderDir.x, _colliderDir.y);
            }

            if (isMaster != _playerController.Pv.IsMine && _playerController.Pv.IsMine)
            {
                GroundCheckEvent();
                _playerController.TakeDamage(GrenadeDamage);
                _playerController.BeBounce(GrenadeBeElasticity, _colliderDir.x, _colliderDir.y);
            }
        }
    }

    IEnumerator Shorten()
    {
        yield return new WaitForSeconds(0.3f);
        _pv.RPC("Rpc_CanAddForceFalse", RpcTarget.All);
        yield return new WaitForSeconds(2.7f);
        _isGrow = false;
    }

    //Ground Check
    void GroundCheckEvent()
    {
        _isGrow = _isBoom = true;
        RaycastHit2D leftCheck = Raycast(Vector2.zero, Vector2.left, GroundDistance);
        RaycastHit2D rightCheck = Raycast(Vector2.zero, Vector2.right, GroundDistance);
        RaycastHit2D downCheck = Raycast(Vector2.zero, Vector2.down, GroundDistance);
        RaycastHit2D upCheck = Raycast(Vector2.zero, Vector2.up, GroundDistance);
        if (leftCheck)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 270);
            _colliderDir = new Vector2(1, 0);
            _colliderSpawnPos = new Vector3(transform.position.x - 0.15f, transform.position.y, transform.position.z);
        }
        else if (rightCheck)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 90);
            _colliderDir = new Vector2(-1, 0);
            _colliderSpawnPos = new Vector3(transform.position.x + 0.15f, transform.position.y, transform.position.z);
        }
        else if (downCheck)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
            _colliderDir = new Vector2(0, 1);
            _colliderSpawnPos = new Vector3(transform.position.x, transform.position.y - 0.15f, transform.position.z);
        }
        else if (upCheck)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 180);
            _colliderDir = new Vector2(0, -1);
            _colliderSpawnPos = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        }
        _pv.RPC("Rpc_SyncColliderDir", RpcTarget.Others,_colliderDir,transform.eulerAngles);
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay((Vector2)transform.position+ Vector2.zero, Vector2.left* GroundDistance);
        Gizmos.DrawRay((Vector2)transform.position + Vector2.zero, Vector2.right * GroundDistance);
        Gizmos.DrawRay((Vector2)transform.position + Vector2.zero, Vector2.down * GroundDistance);
        Gizmos.DrawRay((Vector2)transform.position + Vector2.zero, Vector2.up * GroundDistance);
    }
    //Ground Raycast
    private RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float lengh)
    {
        Vector2 pos = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDirection, lengh, GroundLayer);
        Color color = hit ? Color.red : Color.green;
        Debug.DrawRay(pos + offset, rayDirection * lengh, color);
        return hit;
    }

    [PunRPC]
    void Rpc_RbStatic(Vector3 _pos)
    {
        transform.position = _pos;
        _rb.bodyType = RigidbodyType2D.Static;
        _isCanAddForce = true;
    }

    [PunRPC]
     void Rpc_Explode()
     {
        Explose();
     }

    [PunRPC]
    void Rpc_Test(PlayerController playerController)
    {
        playerController.BeBounce(GrenadeBeElasticity * 2, _colliderDir.x, _colliderDir.y);
    }

    [PunRPC]
    void Rpc_CanAddForceFalse()
    {
        _isCanAddForce = false;
    }

    [PunRPC]
    void Rpc_RbDynamic()
    {
        _isGrow = _isBoom = _isCanAddForce = false;
        _rb.bodyType = RigidbodyType2D.Dynamic;
    }

    [PunRPC]
    void Rpc_SyncColliderDir(Vector2 _pos, Vector3 _dir)
    {
        _colliderDir = _pos;
        transform.eulerAngles = _dir;
    }
}