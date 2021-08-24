using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Effect : MonoBehaviour,IPoolObject
{
    public bool isHaveSound;
    private bool _isChangeColor;

    private ParticleSystem _effect;
    private PhotonView _pv;
    private AudioSource _BombSound;

    void Awake()
    {
        _pv = GetComponent<PhotonView>();
        _effect = GetComponentInChildren<ParticleSystem>();
        if (isHaveSound)
        {
            _BombSound = GetComponent<AudioSource>();
        }

        if (_pv.IsMine)
        {
            SetColor();
        }
    }
    public void OnObjectSpawn()
    {
        if (_isChangeColor)
        {
            SetColor();
            _isChangeColor = false;
        }
        _pv.RPC("EnableObj", RpcTarget.All,transform.position,transform.rotation);
        _effect.Play();
        StartCoroutine(DisableObject());
    }


    public void SetColor()//設定Effect顏色，並同步到客戶端
    {
        ParticleSystem.MainModule main = _effect.main;
        Color _color = CustomPropertyCode.COLORS[(int)PhotonNetwork.LocalPlayer.CustomProperties[CustomPropertyCode.TEAM_CODE]];
        main.startColor = _color;

        _pv.RPC("Rpc_ChangeColor", RpcTarget.Others, new Vector3(_color.r, _color.g, _color.b));
        _pv.RPC("DisableObj", RpcTarget.All);
    }

    public void ChangeColor(Color _color)//反彈後爆炸的顏色(Shield)
    {
        ParticleSystem.MainModule main = _effect.main;
        main.startColor = _color;
        _isChangeColor = true;
        _pv.RPC("Rpc_ChangeColor", RpcTarget.Others, new Vector3(_color.r, _color.g, _color.b));
    }

    IEnumerator DisableObject()
    {
        yield return new WaitForSeconds(2f);
        _pv.RPC("DisableObj", RpcTarget.All);
    }

    /// <summary>
    /// RPC
    /// </summary>
    #region -- RPC Event --
    [PunRPC]
    void Rpc_ChangeColor(Vector3 color)//同步反彈後爆炸的顏色(Shield)
    {
        ParticleSystem.MainModule main = _effect.main;
        Color _color = new Color(color.x, color.y, color.z);
        main.startColor = _color;
    }

    [PunRPC]
    public void DisableObj()//同步關掉Effect
    {
        gameObject.SetActive(false);
    }

    [PunRPC]
    public void EnableObj(Vector3 _pos,Quaternion _dir)//同步開啟Effect，並同步Pos和Dir
    {
        gameObject.SetActive(true);
        if (_BombSound != null)
        {
            _BombSound.volume = OptionSetting.SFXVOLUME;
            _BombSound.Play();
        }
        transform.position = _pos;
        transform.rotation = _dir;
    }
    #endregion
}
