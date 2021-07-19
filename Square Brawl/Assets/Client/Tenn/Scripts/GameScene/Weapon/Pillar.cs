using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Pillar : Grenade, IPoolObject
{
    public float _growY;
    public float GroundDistance;

    public bool _isGrow;
    private bool _isBoom;
    private bool _isCanAddForce;

    public Vector2 _colliderDir;
    public LayerMask GroundLayer;

    protected override void Start()
    {
        _growY = 1f;
        _childObj = transform.GetChild(0).gameObject;
        base.Start();
    }

    protected override void Update()
    {
        ResetValue();

        ColliderEvent();

        if (!_pv.IsMine)
        {
            _childObj.transform.localPosition = Vector3.Lerp(_childObj.transform.localPosition, _childObjnetworkPosition, 15 * Time.deltaTime);
            _childObj.transform.localScale = Vector3.Lerp(_childObj.transform.localScale, _childObjnetworkScale, 15 * Time.deltaTime);
            transform.rotation = _networkDir;
        }
    }

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
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
                    _isBoom = isShoot = _isGrow = false;
                    _pv.RPC("Rpc_RbDynamic", RpcTarget.All);
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground")&&!_isGrow)
        {
            Explose();
            if (_pv.IsMine)
            {
                _rb.bodyType = RigidbodyType2D.Static;
                _pv.RPC("Rpc_RbKinematic", RpcTarget.Others);
                GroundCheckEvent();
            }
            _isCanAddForce =_isGrow = _isBoom = true;
        }

        if (other.gameObject.CompareTag("Player")&& _isCanAddForce)
        {
            PlayerController _playerController = other.gameObject.GetComponent<PlayerController>();
            _playerController.BeBounce(BulletBeElasticity*0.5f, _colliderDir.x, _colliderDir.y);
        }
    }

    private void Explose()
    {
        Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, FieldExplose, LayerToExplose);
        foreach (Collider2D obj in objects)
        {
            PlayerController _playerController = obj.GetComponent<PlayerController>();

            if (_pv.IsMine == _playerController.Pv.IsMine && _pv.IsMine)
            {
                _playerController.BeBounce(BulletBeElasticity, _colliderDir.x, _colliderDir.y);
            }

            if (_pv.IsMine != _playerController.Pv.IsMine && _playerController.Pv.IsMine)
            {
                Debug.Log("OK2");
                _playerController.TakeDamage(BulletDamage, 0, 0, 0);
                _playerController.BeBounce(BulletBeElasticity, _colliderDir.x, _colliderDir.y);
            }
        }
    }

    IEnumerator Shorten()
    {
        yield return new WaitForSeconds(0.5f);
        _isCanAddForce = false;
        yield return new WaitForSeconds(2.5f);
        _isGrow = false;
    }

    //Ground Check
    void GroundCheckEvent()
    {
        RaycastHit2D leftCheck = Raycast(Vector2.zero, Vector2.left, GroundDistance);
        RaycastHit2D rightCheck = Raycast(Vector2.zero, Vector2.right, GroundDistance);
        RaycastHit2D downCheck = Raycast(Vector2.zero, Vector2.down, GroundDistance);
        RaycastHit2D upCheck = Raycast(Vector2.zero, Vector2.up, GroundDistance);
        if (leftCheck)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 270);
            _colliderDir = new Vector2(1, 0);
        }
        else if (rightCheck)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 90);
            _colliderDir = new Vector2(-1, 0);
        }
        else if (downCheck)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
            _colliderDir = new Vector2(0, 1);
        }
        else if (upCheck)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 180);
            _colliderDir = new Vector2(0, -1);
        }

        _pv.RPC("Rpc_SyncColliderDir", RpcTarget.Others,_colliderDir);
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
    void Rpc_RbKinematic()
    {
        _rb.bodyType = RigidbodyType2D.Kinematic;
    }

    [PunRPC]
    void Rpc_RbDynamic()
    {
        _rb.bodyType = RigidbodyType2D.Dynamic;
    }

    [PunRPC]
    void Rpc_SyncColliderDir(Vector2 _dir)
    {
        _colliderDir = _dir;
    }

}
