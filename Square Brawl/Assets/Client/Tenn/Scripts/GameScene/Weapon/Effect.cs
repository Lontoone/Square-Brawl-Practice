using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Effect : MonoBehaviour,IPoolObject
{
    private ParticleSystem _effect;
    private PhotonView _pv;
    public void OnObjectSpawn()
    {
        _pv.RPC("EnableObj", RpcTarget.All,transform.position,transform.rotation);
        _effect.Play();
        StartCoroutine(DisableObject());
    }

    void Start()
    {
        _pv = GetComponent<PhotonView>();
        _effect = GetComponentInChildren<ParticleSystem>();

        if (_pv.IsMine)
        {
            SetColor();
           // PlayerController.instance.OnChangeColor += SetColor;
            _pv.RPC("DisableObj", RpcTarget.All);
        }
    }

    private void SetColor()
    {
        ParticleSystem.MainModule main = _effect.main;
        Color _color = CustomPropertyCode.COLORS[(int)PhotonNetwork.LocalPlayer.CustomProperties[CustomPropertyCode.TEAM_CODE]];
        main.startColor = _color;

        _pv.RPC("ChangeColor", RpcTarget.Others, new Vector3(_color.r, _color.g, _color.b));
    }

    [PunRPC]
    void ChangeColor(Vector3 color)
    {
        ParticleSystem.MainModule main = _effect.main;
        Color _color = new Color(color.x, color.y, color.z);
        main.startColor = _color;
    }

    IEnumerator DisableObject()
    {
        yield return new WaitForSeconds(2f);
        _pv.RPC("DisableObj", RpcTarget.All);
    }

    [PunRPC]
    public void DisableObj()
    {
        gameObject.SetActive(false);
    }

    [PunRPC]
    public void EnableObj(Vector3 _pos,Quaternion _dir)
    {
        gameObject.SetActive(true);
        transform.position = _pos;
        transform.rotation = _dir;
    }
}
