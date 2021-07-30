﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bounce : MonoBehaviour
{
    [HeaderAttribute("Bounce Setting")]
    public int BounceDamage;
    public float BounceBeElasticity;
    public float FieldExplose;//Explose Field

    public string BounceExploseEffectName;

    public Vector3 _beShotShakeValue;

    [HeaderAttribute("Laser Setting")]
    private int _laserDistance = 100; //max raycasting distance
    private int _laserLimit = 4; //the laser can be reflected this many times

    private float _laserTransparency;
    private float _laserWidth;

    private bool _isBounce;
    private bool _isChangeColor;

    public LineRenderer _laserRenderer; //the line renderer

    public LayerMask LayerToExplose;

    public Action<int, float, string,Vector2,Vector3> BounceFunc;

    private List<Vector2> HitGroundPos= new List<Vector2>();

    [HeaderAttribute("Syuc Setting")]
    private Vector2 _prevPos;
    private Vector2 _prevDir;

    private PhotonView _pv;
    private void Awake()
    {
        _pv = GetComponent<PhotonView>();
        _laserRenderer = GetComponent<LineRenderer>();
        BounceFunc = BounceEvent;
        if (_pv.IsMine)
        {
            SetColor();
           // PlayerController.instance.OnChangeColor += SetColor;
            _pv.RPC("Rpc_DisableObj", RpcTarget.All);
        }
    }

    private void SetColor()
    {
        Color _color = CustomPropertyCode.COLORS[(int)PhotonNetwork.LocalPlayer.CustomProperties[CustomPropertyCode.TEAM_CODE]];
        _laserRenderer.startColor = _laserRenderer.endColor = new Color(_color.r,_color.g,_color.b,0);

        _pv.RPC("ChangeColor", RpcTarget.Others, new Vector3(_color.r, _color.g, _color.b));
    }

    [PunRPC]
    void ChangeColor(Vector3 color)
    {
        Color _color = new Color(color.x, color.y, color.z);
        _laserRenderer.startColor = _laserRenderer.endColor = new Color(_color.r, _color.g, _color.b, 0);
    }

    private void BounceEvent (int _damage,float _beElasticity,string _effectName,Vector2 _dir,Vector3 _beShotShake)
    {
        _pv.RPC("Rpc_EnableObj", RpcTarget.All);
        BounceDamage = _damage;
        BounceBeElasticity = _beElasticity;
        BounceExploseEffectName = _effectName;
        _prevPos = transform.position;
        _prevDir = _dir;
        _beShotShakeValue = _beShotShake;
        ShootBounce(transform.position, _dir);
        _pv.RPC("Rpc_ShootBounce", RpcTarget.Others, _prevPos, _dir, BounceDamage, BounceBeElasticity, _beShotShakeValue);
    }

    void Update()
    {
        IsBounceEvent();
    }

    public void ShootBounce(Vector2 _pos,Vector2 _dir)
    {
        _isBounce = true;
        int vertexCounter = 1; 
        _laserRenderer.SetPosition(0, _pos);

        for (int i = 0; i < _laserLimit; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(_pos, _dir, _laserDistance, ~LayerToExplose);

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
                    }

                    /*for (int i = 0; i < _laserLimit; i++)
                    {
                        RaycastHit2D[] hits = Physics2D.RaycastAll(_prevPos, _prevDir, _laserDistance);

                        for (int j = 0; j < hits.Length; j++)
                        {
                            if (hits[i].collider.gameObject.CompareTag("Ground"))
                            {
                                Debug.Log("Line" + _prevPos);
                                _prevPos = hits[i].point;
                                _prevDir = Vector3.Reflect(_prevDir, hits[i].normal);
                                if (_pv.IsMine)
                                {
                                    _pv.RPC("Rpc_Explose", RpcTarget.All, _prevPos);
                                }
                            }

                            if (hits[i].collider.gameObject.CompareTag("Player"))
                            {
                                PlayerController _playerController = hits[i].collider.GetComponent<PlayerController>();
                                if (_pv.IsMine != _playerController.Pv.IsMine && _playerController.Pv.IsMine&&!_playerController.IsBounce)
                                {
                                    _playerController.DamageFunc(BounceDamage, BounceBeElasticity, _prevDir.x, _prevDir.y, _beShotShakeValue);
                                    _playerController.IsBounceEvent();
                                }
                            }
                        }
                    }*/

                    _isChangeColor = true;
                }
            }
            else if (_isChangeColor)
            {
                _laserTransparency -= Time.deltaTime * 1.5f;
                if (_laserWidth >= 0.05f)
                {
                    _laserWidth -= Time.deltaTime * 0.1f;
                }
                LaserColorAndWidth(_laserTransparency, _laserWidth);

                if (_laserTransparency <= 0)
                {
                    _isChangeColor = _isBounce = false;
                    HitGroundPos.Clear();
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

    private void Explose(Vector2 _originPos)
    {
        Collider2D[] objects = Physics2D.OverlapCircleAll(_originPos, FieldExplose, LayerToExplose);
        foreach (Collider2D obj in objects)
        {
            PlayerController _playerController = obj.GetComponent<PlayerController>();
            if (_pv.IsMine == _playerController.Pv.IsMine && _pv.IsMine && !_playerController.IsBounce)
            {
                _playerController.BeBounce(BounceBeElasticity, _prevDir.x, _prevDir.y);
                _playerController.IsBounceEvent();
            }

            if (_pv.IsMine != _playerController.Pv.IsMine && _playerController.Pv.IsMine&& !_playerController.IsBounce)
            {
                _playerController.DamageFunc(BounceDamage, BounceBeElasticity, _prevDir.x, _prevDir.y,_beShotShakeValue);
                _playerController.IsBounceEvent();
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
    void Rpc_ShootBounce(Vector2 _pos, Vector2 _dir, int _damage, float _beElasticity,Vector3 _beShotShake)
    {
        _prevPos = _pos;
        _prevDir = _dir;
        BounceDamage = _damage;
        BounceBeElasticity = _beElasticity;
        _beShotShakeValue = _beShotShake;
        ShootBounce(_pos, _dir);
    }
    [PunRPC]
    void Rpc_Explose(Vector2 _pos)
    {
        Explose(_pos);
    }
}
