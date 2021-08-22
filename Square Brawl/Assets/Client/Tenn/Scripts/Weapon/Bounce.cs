using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bounce : MonoBehaviour,IPoolObject
{
    [HeaderAttribute("Bounce Setting")]
    public int BounceDamage;//Bounce Damage
    public float BounceBeElasticity;// Bounce BeElasticity
    public float FieldExplose;//Explose Field

    public string BounceExploseEffectName;

    private Vector3 _beShootShakeValue;

    [HeaderAttribute("Laser Setting")]
    private int _laserDistance = 100; //max raycasting distance
    private int _laserLimit = 4; //the laser can be reflected this many times

    private float _laserTransparency;//laser Transparency value
    private float _laserWidth;//laser Width value

    private bool _isBounce;
    private bool _isChangeColor;

    public EdgeCollider2D _edgeCollider;

    public LineRenderer _laserRenderer; //the line renderer

    public LayerMask LayerToExplose;

    private List<Vector2> HitGroundPos= new List<Vector2>();
    private List<Vector2> HitGroundLocalPos = new List<Vector2>();

    [HeaderAttribute("Syuc Setting")]
    private Vector2 _prevPos;
    private Vector2 _prevDir;

    private PhotonView _pv;
    private void Awake()
    {
        _pv = GetComponent<PhotonView>();
        _laserRenderer = GetComponent<LineRenderer>();
        _edgeCollider = GetComponent<EdgeCollider2D>();
        if (_pv.IsMine)
        {
            SetColor();
            _pv.RPC("Rpc_DisableObj", RpcTarget.All);
        }
    }

    public void OnObjectSpawn()
    {
        if (_pv.IsMine)
        {
            _pv.RPC("Rpc_EnableObj", RpcTarget.All);
        }
    }

    private void SetColor()
    {
        Color _color = CustomPropertyCode.COLORS[(int)PhotonNetwork.LocalPlayer.CustomProperties[CustomPropertyCode.TEAM_CODE]];
        _laserRenderer.startColor = _laserRenderer.endColor = new Color(_color.r,_color.g,_color.b,0);

        _pv.RPC("ChangeColor", RpcTarget.Others, new Vector3(_color.r, _color.g, _color.b));
    }

    public void BounceEvent (int _damage,float _beElasticity,string _effectName,Vector2 _dir,Vector3 _beShotShake)
    {
        BounceDamage = _damage;
        BounceBeElasticity = _beElasticity;
        BounceExploseEffectName = _effectName;
        _prevPos = transform.position;
        _prevDir = _dir;
        _beShootShakeValue = _beShotShake;
        ShootBounce(transform.position, _dir);
        _pv.RPC("Rpc_ResetPos", RpcTarget.Others, transform.position, transform.rotation);
        _pv.RPC("Rpc_ShootBounce", RpcTarget.Others, _prevPos, _dir, BounceDamage, BounceBeElasticity, _beShootShakeValue);
    }

    void Update()
    {
        IsBounceEvent();
    }

    public void ShootBounce(Vector2 _pos,Vector2 _dir)
    {
        Physics2D.queriesStartInColliders = false;
        _isBounce = true;
        int vertexCounter = 1; 
        _laserRenderer.SetPosition(0, _pos);
        HitGroundLocalPos.Add(gameObject.transform.InverseTransformPoint(_pos));

        for (int i = 0; i < _laserLimit; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(_pos, _dir, _laserDistance, ~LayerToExplose);

            if (hit.collider.gameObject.CompareTag("Ground")|| hit.collider.gameObject.CompareTag("Saw"))
            {
                vertexCounter+=3;
                _laserRenderer.positionCount = vertexCounter;
                _laserRenderer.SetPosition(vertexCounter - 3, Vector3.MoveTowards(hit.point, _pos, 0.01f));
                _laserRenderer.SetPosition(vertexCounter - 2, hit.point);
                _laserRenderer.SetPosition(vertexCounter - 1, hit.point);
                _pos = hit.point;
                HitGroundPos.Add(_pos);
                HitGroundLocalPos.Add(gameObject.transform.InverseTransformPoint(_pos));
                _dir = Vector3.Reflect(_dir, hit.normal);
            }
        }
        Physics2D.queriesStartInColliders = true;
    }

    void IsBounceEvent()
    {
        if (_isBounce)
        {
            if (!_isChangeColor)
            {
                _laserTransparency += Time.deltaTime;
                if (_laserWidth <= 0.1f)
                {
                    _laserWidth += Time.deltaTime*0.1f;
                }
                LaserColorAndWidth(_laserTransparency, _laserWidth);
                
                if (_laserTransparency >= 1)
                {
                    if (_pv.IsMine)
                    {
                        for (int i = 0; i < HitGroundPos.Count; i++)
                        {
                            ObjectsPool.Instance.SpawnFromPool(BounceExploseEffectName, HitGroundPos[i], transform.rotation, null);
                        }
                        CameraShake.instance.SetShakeValue(_beShootShakeValue.x, _beShootShakeValue.y, _beShootShakeValue.z);
                    }
                    _edgeCollider.points = HitGroundLocalPos.ToArray();
                    _isChangeColor = true;
                }
            }
            else if (_isChangeColor)
            {
                _laserTransparency -= Time.deltaTime * 1.5f;
                if (_laserWidth >= 0.5f)
                {
                    _laserWidth -= Time.deltaTime * 0.1f;
                }
                LaserColorAndWidth(_laserTransparency, _laserWidth);

                if(_laserTransparency <= 0.5f)
                {
                    for (int i = 0; i < HitGroundLocalPos.Count; i++)
                    {
                        HitGroundLocalPos[i] = Vector2.zero;
                    }
                    _edgeCollider.points = HitGroundLocalPos.ToArray();
                }

                if (_laserTransparency <= 0)
                {
                    _isChangeColor = _isBounce = false;
                    HitGroundPos.Clear();
                    HitGroundLocalPos.Clear();
                    gameObject.SetActive(false);
                }
            }
        }
    }

    void LaserColorAndWidth(float _laserTransparency, float _laserWidth)
    {
        _laserRenderer.startColor = new Color(_laserRenderer.startColor.r, _laserRenderer.startColor.g, _laserRenderer.startColor.b, _laserTransparency);
        _laserRenderer.endColor = new Color(_laserRenderer.endColor.r, _laserRenderer.endColor.g, _laserRenderer.endColor.b, _laserTransparency);
        _laserRenderer.startWidth = _laserWidth;
        _laserRenderer.endWidth = _laserWidth;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController _playerController = other.gameObject.GetComponent<PlayerController>();

            if (_pv.IsMine != _playerController.Pv.IsMine && !_playerController.Pv.IsMine && !_playerController.IsBounce)
            {
                _playerController.DamageEvent(BounceDamage, BounceBeElasticity, _prevDir.x, _prevDir.y, _beShootShakeValue);
                _playerController.IsBounceTrue();
                var IsKill = _playerController.IsKillAnyone();
                if (IsKill)
                {
                    PlayerKillCountManager.instance.SetKillCount();
                    _playerController.GenerateDieEffect();
                }
            }
        }
    }

    [PunRPC]
    void ChangeColor(Vector3 color)
    {
        Color _color = new Color(color.x, color.y, color.z);
        _laserRenderer.startColor = _laserRenderer.endColor = new Color(_color.r, _color.g, _color.b, 0);
    }

    [PunRPC]
    public void Rpc_EnableObj()
    {
        gameObject.SetActive(true);
    }

    [PunRPC]
    public void Rpc_DisableObj()
    {
        gameObject.SetActive(false);
    }

    [PunRPC]
    public void Rpc_ResetPos(Vector3 pos, Quaternion dir)
    {
        transform.position = pos;
        transform.rotation = dir;
    }

    [PunRPC]
    void Rpc_ShootBounce(Vector2 _pos, Vector2 _dir, int _damage, float _beElasticity,Vector3 _beShotShake)
    {
        _prevPos = _pos;
        _prevDir = _dir;
        BounceDamage = _damage;
        BounceBeElasticity = _beElasticity;
        _beShootShakeValue = _beShotShake;
        ShootBounce(_pos, _dir);
    }
}
