using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Reflections : MonoBehaviour
{
    public int BounceDamage;
    public float BounceBeElasticity;
    public string BounceExploseEffectName;

    private int _laserDistance = 100; //max raycasting distance
    private int _laserLimit = 4; //the laser can be reflected this many times
    public LineRenderer _laserRenderer; //the line renderer
    public bool _isBounce;
    private bool _isChangeColor;
    private float _transparency;

    public EdgeCollider2D edgeCollider;

    public Action<int, float, string,Vector2> BounceFunc;

    private List<Vector2> HitGroundPos= new List<Vector2>();

    private Vector2 _originPos;
    private Vector2 _originDir;

    private PhotonView _pv;
    private void Start()
    {
        _pv = GetComponent<PhotonView>();
        edgeCollider = GetComponent<EdgeCollider2D>();
        _laserRenderer = GetComponent<LineRenderer>();
        BounceFunc = BounceEvent;
        _pv.RPC("Rpc_DisableObj", RpcTarget.All);
    }

    private void BounceEvent (int _damage,float _beElasticity,string _effectName,Vector2 _dir)
    {
        _pv.RPC("Rpc_EnableObj", RpcTarget.All);
        BounceDamage = _damage;
        BounceBeElasticity = _beElasticity;
        BounceExploseEffectName = _effectName;
        _originPos = transform.position;
        _originDir = _dir;
        _pv.RPC("Rpc_ShootBounce", RpcTarget.Others, _originPos, _dir);
        ShootBounce(transform.position, _dir);
    }

    void Update()
    {
        IsBounceEvent();
    }

    public void ShootBounce(Vector2 _pos,Vector2 _dir)
    {
        _laserRenderer.startColor = new Color(_laserRenderer.startColor.r, _laserRenderer.startColor.g, _laserRenderer.startColor.b, 0);
        _laserRenderer.endColor = new Color(_laserRenderer.endColor.r, _laserRenderer.endColor.g, _laserRenderer.endColor.b, 0);
        _isBounce = true;
        int vertexCounter = 1; 
        _laserRenderer.positionCount = 1;
        _laserRenderer.SetPosition(0, _pos);
        HitGroundPos.Add(_pos);

        for (int i = 0; i < _laserLimit; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(_pos, _dir, _laserDistance);

            if (hit.collider.gameObject.CompareTag("Ground"))
            {
                vertexCounter+=3;
                _laserRenderer.positionCount = vertexCounter;
                _laserRenderer.SetPosition(vertexCounter - 3, Vector3.MoveTowards(hit.point, _pos, 0.01f));
                _laserRenderer.SetPosition(vertexCounter - 2, hit.point);
                _laserRenderer.SetPosition(vertexCounter - 1, hit.point);
                _pos = hit.point;
                HitGroundPos.Add(_pos);
                _dir = Vector3.Reflect(_dir, hit.normal);
            }
        }
    }

    void IsBounceEvent()
    {
        if (_isBounce)
        {
            if (!_isChangeColor)
            {
                _transparency += Time.deltaTime;
                _laserRenderer.startColor = new Color(_laserRenderer.startColor.r, _laserRenderer.startColor.g, _laserRenderer.startColor.b, _transparency);
                _laserRenderer.endColor = new Color(_laserRenderer.endColor.r, _laserRenderer.endColor.g, _laserRenderer.endColor.b, _transparency);
                if (_transparency >= 1)
                {
                    if (_pv.IsMine)
                    {
                        for (int i = 0; i < HitGroundPos.Count; i++)
                        {
                            ObjectsPool.Instance.SpawnFromPool(BounceExploseEffectName, HitGroundPos[i], transform.rotation, null);
                        }

                        for (int j = 0; j < _laserLimit; j++)
                        {
                            RaycastHit2D hit = Physics2D.Raycast(_originPos, _originDir, _laserDistance);

                            if (hit.collider.gameObject.CompareTag("Ground"))
                            {
                                _originPos = hit.point;
                                _originDir = Vector3.Reflect(_originDir, hit.normal);
                            }
                            if (hit.collider.gameObject.CompareTag("Player"))
                            {
                                PlayerController _playerController = hit.collider.gameObject.GetComponent<PlayerController>();
                                if (!_playerController.Pv.IsMine)
                                {
                                    _playerController.TakeDamage(BounceDamage);
                                }
                            }
                        }
                    }
                    _isChangeColor = true;
                }
            }
            else if (_isChangeColor)
            {
                _transparency -= Time.deltaTime * 1.5f;
                _laserRenderer.startColor = new Color(_laserRenderer.startColor.r, _laserRenderer.startColor.g, _laserRenderer.startColor.b, _transparency);
                _laserRenderer.endColor = new Color(_laserRenderer.endColor.r, _laserRenderer.endColor.g, _laserRenderer.endColor.b, _transparency);
                if (_transparency <= 0)
                {
                    _isChangeColor = _isBounce = false;
                    HitGroundPos.Clear();
                    gameObject.SetActive(false);
                }
            }
        }
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
    void Rpc_ShootBounce(Vector2 _pos, Vector2 _dir)
    {
        ShootBounce(_pos, _dir);
    }
}
