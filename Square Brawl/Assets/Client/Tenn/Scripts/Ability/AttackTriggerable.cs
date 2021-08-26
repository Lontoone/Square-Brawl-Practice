using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class AttackTriggerable : MonoBehaviour
{
    [HideInInspector] public string Name;
    [HideInInspector] public int WeaponDamage;
    [HideInInspector] public float WeaponSpeed;
    [HideInInspector] public float WeaponRecoil;
    [HideInInspector] public float BeElasticity;
    [HideInInspector] public float WeaponScaleValue;
    [HideInInspector] public bool IsDontContinuous;
    [HideInInspector] public bool IsDontShootStraight;
    [HideInInspector] public bool IsSniper;
    [HideInInspector] public string LaunchEffectName;
    [HideInInspector] public string ExploseEffectName;
    [HideInInspector] public Vector3 BeShootShakeValue;
    [HideInInspector] public Vector3 ShootShakeValue;

    [System.Serializable]
    public class FireSound
    {
        public string tag;
        public AudioClip sound;
    }
    public List<FireSound> FireSounds;
    public Dictionary<string, AudioClip> soundsDictionary;

    public string SoundName;

    private bool _isKatadaReverse;

    private GameObject _bulletSpawnPos;
    private GameObject _bulletMidSpawnPos;
    private GameObject _grenadaSpawnPos;

    public AudioSource _audio;

    private PhotonView _pv;
    //private const byte PLAY_SOUND_EVENT=0;
    private void Start()
    {
        _bulletSpawnPos = GameObject.FindGameObjectWithTag("BulletSpawnPos");
        _bulletMidSpawnPos = GameObject.FindGameObjectWithTag("MidPos");
        _grenadaSpawnPos = GameObject.FindGameObjectWithTag("SpecicalFirePos");
        _pv = GetComponent<PhotonView>();
        _audio = GetComponent<AudioSource>();

        soundsDictionary = new Dictionary<string, AudioClip>();
        foreach (FireSound sound in FireSounds)
        {
            soundsDictionary.Add(sound.tag, sound.sound);
        }
    }

    public AudioClip PlaySound(string tag)
    {
        if (!soundsDictionary.ContainsKey(tag))
        {
            return null;
        }

        AudioClip sound = soundsDictionary[tag];

        return sound;
    }

    /*private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += NetClient;
    }

    private void OnDisble()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= NetClient;
    }*/

    void PlaySound()
    {
        _audio.PlayOneShot(PlaySound(SoundName), OptionSetting.SFXVOLUME);

        _pv.RPC("Rpc_PlaySound",RpcTarget.Others, SoundName);
       /* string _soundName = SoundName;

        object[] datas = new object[] { _soundName };
        PhotonNetwork.RaiseEvent(PLAY_SOUND_EVENT, datas, RaiseEventOptions.Default, SendOptions.SendUnreliable);
        _audio.PlayOneShot(PlaySound(SoundName),0.5f);*/
    }

    [PunRPC]
    void Rpc_PlaySound(string _soundName)
    {
        _audio.PlayOneShot(PlaySound(_soundName), OptionSetting.SFXVOLUME);
    }

    /*void NetClient(EventData obj)
    {
        if (obj.Code == PLAY_SOUND_EVENT)
        {
            object[]datas= (object[])obj.CustomData;
            string _soundName = (string)datas[0];
            _audio.PlayOneShot(PlaySound(_soundName),0.1f);
            Debug.Log("OK");
            Debug.Log(_audio);
            Debug.Log(GetComponent<PhotonView>().ViewID);
        }
    }*/

    public void Fire()
    {
        GameObject _bulletObj = ObjectsPool.Instance.SpawnFromPool("Bullet", _bulletMidSpawnPos.transform.position, _bulletSpawnPos.transform.rotation, null);
        Bullet _bullet = _bulletObj.GetComponent<Bullet>();
        if (!IsDontShootStraight)
        {
            ObjectsPool.Instance.SpawnFromPool(LaunchEffectName, _bulletSpawnPos.transform.position, _bulletSpawnPos.transform.rotation, null);
        }
        CameraShake.instance.SetShakeValue(ShootShakeValue.x, ShootShakeValue.y, ShootShakeValue.z);
        _bullet.ShootEvent(ExploseEffectName,WeaponSpeed, WeaponDamage, WeaponScaleValue, BeElasticity, IsDontShootStraight, IsSniper, BeShootShakeValue);
        PlayerController.instance.PlayerRecoil(WeaponRecoil);
        PlaySound();
    }

    public void ShotgunFire()
    {
        for (int i = 0; i <= 4; i++)
        {
            GameObject[] _bulletObj = new GameObject[5];
            _bulletObj[i]= ObjectsPool.Instance.SpawnFromPool("Bullet", _bulletMidSpawnPos.transform.position, _bulletSpawnPos.transform.rotation, null);
            Bullet _bullet = _bulletObj[i].GetComponent<Bullet>();
            _bullet.ShootEvent(ExploseEffectName, WeaponSpeed, WeaponDamage, WeaponScaleValue, BeElasticity, IsDontShootStraight, IsSniper, BeShootShakeValue);
        }
        ObjectsPool.Instance.SpawnFromPool(LaunchEffectName, _bulletSpawnPos.transform.position, _bulletSpawnPos.transform.rotation, null);
        CameraShake.instance.SetShakeValue(ShootShakeValue.x, ShootShakeValue.y, ShootShakeValue.z);
        PlayerController.instance.PlayerRecoil(WeaponRecoil);
        PlaySound();
    }

    public void Charge()
    {
        CameraShake.instance.SetShakeValue(ShootShakeValue.x, ShootShakeValue.y, ShootShakeValue.z);
        PlayerController.instance.ChargeEvent(-WeaponSpeed, BeElasticity, WeaponDamage, BeShootShakeValue);
        PlaySound();
    }

    public void Katada()
    {
        _isKatadaReverse = !_isKatadaReverse;
        GameObject _katadaObj = ObjectsPool.Instance.SpawnFromPool("Katada", _bulletMidSpawnPos.transform.position, _bulletSpawnPos.transform.rotation, null);
        Katada _katada = _katadaObj.GetComponent<Katada>();
        _katada.KatadaEvent(WeaponSpeed, WeaponDamage, BeElasticity, _isKatadaReverse, BeShootShakeValue);
        PlaySound();
    }

    public void Shield()
    {
        PlayerController.instance.IsShieldTrue();
        Shield _shield = PlayerController.instance.transform.GetChild(4).gameObject.GetComponent<Shield>();
        _shield.gameObject.SetActive(true);
        _shield.ShieldEvent(WeaponSpeed, WeaponDamage, BeElasticity, BeShootShakeValue);
        CameraShake.instance.SetShakeValue(ShootShakeValue.x, ShootShakeValue.y, ShootShakeValue.z);
        PlaySound();
    }

    public void Freeze()
    {
        PlayerController.instance.PlayerRecoil(WeaponRecoil);
        PlayerController.instance.FreezeEvent(2.5f, 5,BeShootShakeValue);
        ObjectsPool.Instance.SpawnFromPool("FreezeShoot", _bulletSpawnPos.transform.position, _bulletSpawnPos.transform.rotation, null);
        CameraShake.instance.SetShakeValue(ShootShakeValue.x, ShootShakeValue.y, ShootShakeValue.z);
        PlaySound();
    }

    public void GrenadeFire()
    {
        GameObject _grenadeObj = ObjectsPool.Instance.SpawnFromPool(Name, _grenadaSpawnPos.transform.position, _bulletSpawnPos.transform.rotation, null);
        Grenade _grenade = _grenadeObj.GetComponent<Grenade>();
        _grenade.GrenadeEvent(ExploseEffectName, WeaponSpeed, WeaponDamage, WeaponScaleValue, BeElasticity,BeShootShakeValue);
        CameraShake.instance.SetShakeValue(ShootShakeValue.x, ShootShakeValue.y, ShootShakeValue.z);
        PlayerController.instance.PlayerRecoil(WeaponRecoil);
        PlaySound();
    }

    public void Bounce()
    {
        GameObject _bounceObj = ObjectsPool.Instance.SpawnFromPool("Bounce", _bulletMidSpawnPos.transform.position, _bulletSpawnPos.transform.rotation, null);
        CameraShake.instance.SetShakeValue(ShootShakeValue.x, ShootShakeValue.y, ShootShakeValue.z);
        Bounce _bounce = _bounceObj.GetComponent<Bounce>();
        _bounce.BounceEvent(WeaponDamage, BeElasticity, ExploseEffectName, _bulletSpawnPos.transform.right, BeShootShakeValue);
        PlaySound();
    }
}
