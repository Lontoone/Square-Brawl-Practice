using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Pillar : Grenade, IPoolObject
{
    protected float _groundDistance;
    private float _growY;

    private bool _isGrow;
    private bool _isBoom;
    private bool _isCanAddForce;

    private Vector2 _colliderDir;
    private Vector2 _colliderSpawnPos;

    private BoxCollider2D _collider2D;
    protected AudioSource _pillarSound;

    public LayerMask GroundLayer;

    protected override void Start()
    {
        _growY = 1f;
        _childObj = transform.GetChild(0).gameObject;

        _rb = GetComponent<Rigidbody2D>();
        _pv = GetComponent<PhotonView>();
        _pillarSound = GetComponent<AudioSource>();
        _collider2D = transform.GetChild(0).GetComponent<BoxCollider2D>();
        if (_pv.IsMine)
        {
            SetColor();
        }
    }

    private void SetColor()//設定顏色
    {
        Color _color = transform.GetChild(0).GetComponent<SpriteRenderer>().color =
            CustomPropertyCode.COLORS[(int)PhotonNetwork.LocalPlayer.CustomProperties[CustomPropertyCode.TEAM_CODE]];

        _pv.RPC("ChangeColor", RpcTarget.Others, new Vector3(_color.r, _color.g, _color.b));
        _pv.RPC("Rpc_DisableObj", RpcTarget.All);
    }

    public new void OnObjectSpawn()
    {
        if (_pv.IsMine)
        {
            _groundDistance = 0.2f;
            _pv.RPC("Rpc_EnableObj", RpcTarget.All);
            _pv.RPC("Rpc_ResetPos", RpcTarget.Others, transform.position, transform.rotation);
        }
    }

    private void Update()
    {
        GrowEvent();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (!_pv.IsMine)
        {
            _childObj.transform.localPosition = Vector3.Lerp(_childObj.transform.localPosition, _childObjnetworkPosition, 15 * Time.fixedDeltaTime);
            _childObj.transform.localScale = Vector3.Lerp(_childObj.transform.localScale, _childObjnetworkScale, 15 * Time.fixedDeltaTime);
            transform.rotation = _networkDir;
        }
    }

    void GrowEvent()//長出Pillar事件
    {
        if (_isBoom&&_pv.IsMine)
        {
            if (_isGrow)//生長中
            {
                _growY = Mathf.Lerp(_growY, 10f, 10 * Time.deltaTime);
                _childObj.transform.localPosition = new Vector3(_childObj.transform.localPosition.x, (_growY / 2) - 0.5f, _childObj.transform.localPosition.z);
                _childObj.transform.localScale = new Vector3(_childObj.transform.localScale.x, _growY, _childObj.transform.localScale.z);
                StartCoroutine(Shorten());
            }
            else //縮小回去
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
        if (other.gameObject.CompareTag("Ground") && !_isGrow)//碰到地面
        {
            _pillarSound.volume = (OptionSetting.SFXVOLUME * 0.7f);
            _pillarSound.Play();
            if (_pv.IsMine)
            {
                ObjectsPool.Instance.SpawnFromPool(ExploseEffectName, transform.position, transform.rotation, null);
                _pv.RPC("Rpc_RbStatic", RpcTarget.All,transform.position);
                GroundCheckEvent();
                _pv.RPC("Rpc_Explode", RpcTarget.All);
            }
        }

        if (other.gameObject.CompareTag("Player")&& _isCanAddForce)//生長中碰到玩家
        {
            PlayerController _playerController = other.gameObject.GetComponent<PlayerController>();
            if (isMaster == _playerController.Pv.IsMine && isMaster)//玩家碰到自己發射的Pillar
            {
                _playerController.BeBounce(GrenadeBeElasticity, _colliderDir.x, _colliderDir.y);
                _isCanAddForce = false;
            }
            else if (isMaster != _playerController.Pv.IsMine && isMaster)//玩家碰到別人發射的Pillar
            {
                _playerController.DamageEvent(GrenadeDamage, GrenadeBeElasticity, _colliderDir.x, _colliderDir.y, _cameraShakeValue);
                _isCanAddForce = false;
                var IsKill = _playerController.IsKillAnyone();
                if (IsKill)
                {
                    PlayerKillCountManager.instance.SetKillCount();
                    _playerController.GenerateDieEffect();
                }
            }
        }
    }

    private void Explose()//爆炸事件
    {
        Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, FieldExplose, LayerToExplose);
        foreach (Collider2D obj in objects)
        {
            PlayerController _playerController = obj.GetComponent<PlayerController>();

            if (isMaster == _playerController.Pv.IsMine && isMaster)//自己發射的Pillar
            {
                _playerController.BeBounce(GrenadeBeElasticity, _colliderDir.x, _colliderDir.y);
            }

            if (isMaster != _playerController.Pv.IsMine && !_playerController.Pv.IsMine)//別人發射的Pillar
            {
                GroundCheckEvent();
                _playerController.DamageEvent(GrenadeDamage, GrenadeBeElasticity, _colliderDir.x, _colliderDir.y, _cameraShakeValue);
                var IsKill = _playerController.IsKillAnyone();
                if (IsKill)
                {
                    PlayerKillCountManager.instance.SetKillCount();
                }
            }
        }
    }

    IEnumerator Shorten()
    {
        yield return new WaitForSeconds(0.3f);//0.3秒後生長完成
        _pv.RPC("Rpc_CanAddForceFalse", RpcTarget.All);
        yield return new WaitForSeconds(2.7f);//3秒後結束生長，縮小回去
        _isGrow = false;
    }

    //Ground Check
    void GroundCheckEvent()
    {
        _isGrow = _isBoom = true;
        RaycastHit2D leftCheck = Raycast(Vector2.zero, Vector2.left, _groundDistance);
        RaycastHit2D rightCheck = Raycast(Vector2.zero, Vector2.right, _groundDistance);
        RaycastHit2D downCheck = Raycast(Vector2.zero, Vector2.down, _groundDistance);
        RaycastHit2D upCheck = Raycast(Vector2.zero, Vector2.up, _groundDistance);
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
        else
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
            _colliderDir = new Vector2(0, 1);
            _colliderSpawnPos = new Vector3(transform.position.x, transform.position.y - 0.15f, transform.position.z);
        }
        _pv.RPC("Rpc_SyncColliderDir", RpcTarget.Others,_colliderDir,transform.eulerAngles);
    }
    
    /*void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay((Vector2)transform.position+ Vector2.zero, Vector2.left* _groundDistance);
        Gizmos.DrawRay((Vector2)transform.position + Vector2.zero, Vector2.right * _groundDistance);
        Gizmos.DrawRay((Vector2)transform.position + Vector2.zero, Vector2.down * _groundDistance);
        Gizmos.DrawRay((Vector2)transform.position + Vector2.zero, Vector2.up * _groundDistance);
    }*/

    //Ground Raycast
    private RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float lengh)
    {
        Vector2 pos = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDirection, lengh, GroundLayer);
        Color color = hit ? Color.red : Color.green;
        Debug.DrawRay(pos + offset, rayDirection * lengh, color);
        return hit;
    }


    /// <summary>
    /// RPC
    /// </summary>
    #region -- RPC Event --
    [PunRPC]
    void ChangeColor(Vector3 color)//同步顏色
    {
        Color _color = new Color(color.x, color.y, color.z);
        transform.GetChild(0).GetComponent<SpriteRenderer>().color = _color;
    }

    [PunRPC]
    void Rpc_RbStatic(Vector3 _pos)//同步生長中狀態
    {
        transform.position = _pos;
        _rb.bodyType = RigidbodyType2D.Static;
        _collider2D.enabled = true;
        _isCanAddForce = true;
    }

    [PunRPC]
     void Rpc_Explode()//同步爆炸
     {
        Explose();
     }

    [PunRPC]
    void Rpc_CanAddForceFalse()
    {
        _isCanAddForce = false;
    }

    [PunRPC]
    void Rpc_RbDynamic()//同步生長結束縮小後狀態
    {
        _isGrow = _isBoom = _isCanAddForce = false;
        _collider2D.enabled = false;
        _rb.bodyType = RigidbodyType2D.Dynamic;
    }

    [PunRPC]
    void Rpc_SyncColliderDir(Vector2 _pos, Vector3 _dir)//同步Pos Dir
    {
        _colliderDir = _pos;
        transform.eulerAngles = _dir;
    }
    #endregion
}
