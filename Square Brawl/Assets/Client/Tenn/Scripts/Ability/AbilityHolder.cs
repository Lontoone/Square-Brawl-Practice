using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class AbilityHolder : MonoBehaviour
{
    private float _curretCooldownTime;
    private float _activeTime;
    private float isCdAddCount;
    private float _coolDownTime;
    private int _abilityNum;

    public bool isWeapon01;
    protected bool _isFire01;
    protected bool _isFire02;

    public Ability[] ability;

    private PlayerController _playerController;
    private PlayerInputManager _inputAction;
    private PhotonView _pv;
    public Player _player;

    public static event Action<float,float,bool> OnCoolDownTime;

    public enum WeaponType2
    {
        None,
        Aevolver,
        BigBoom,
        Charge,
        CubeShoot,
        Katada,
        Sniper,
        Grenade,
        Pillar,
        Shield,
        Freeze,
        Bounce,
    }

    public WeaponType2 weapon1, weapon2;

    private enum AbilityState
    {
        Ready,
        Active,
        CoolDown
    }

    private AbilityState _state = AbilityState.Ready;

    void Awake()
    {
        _playerController = GetComponentInParent<PlayerController>();
        _pv = GetComponent<PhotonView>();
        _inputAction = new PlayerInputManager();
        ResultManager.OnDisableResult += OnDisableThis;
    }

    private void OnDisableThis()
    {
        enabled = false;
    }


    private void OnEnable()
    {
        _inputAction.Enable();
    }

    private void OnDisable()
    {
        _inputAction.Disable();
    }

    private void Start()
    {
        if (_pv.IsMine)
        {
            if (isWeapon01)
            {
                _abilityNum = (int)PhotonNetwork.LocalPlayer.CustomProperties[CustomPropertyCode.WEAPON1CODE];
                _inputAction.Player.Fire1.performed += _ => PlayerFire1Down();
                _inputAction.Player.Fire1.canceled += _ => PlayerFire1Up();
                SetWeapon();
            }
            else
            {
                _abilityNum = (int)PhotonNetwork.LocalPlayer.CustomProperties[CustomPropertyCode.WEAPON2CODE];
                _inputAction.Player.Fire2.performed += _ => PlayerFire2Down();
                _inputAction.Player.Fire2.canceled += _ => PlayerFire2Up();
                SetWeapon();
            }
        }
    }

    #region -- FireInputSystem --
    private void PlayerFire1Down()
    {
        _isFire01 = true;
    }

    private void PlayerFire1Up()
    {
        _isFire01 = false;
    }

    private void PlayerFire2Down()
    {
        _isFire02 = true;
    }

    private void PlayerFire2Up()
    {
        _isFire02 = false;
    }
    #endregion

    public void SetWeapon()
    {
        ability[_abilityNum].Initalize(gameObject);
        _activeTime = ability[_abilityNum].CoolDownTime;
    }

    void Update()
    {
        if (!_pv.IsMine || _playerController.IsBeFreeze)
        {
            return;
        }

        switch (_state)
        {
            case AbilityState.Ready:
                if (_isFire01 || _isFire02)
                {
                    ability[_abilityNum].Activate();

                    SpecialEvent();
                    
                    if(!ability[_abilityNum].isCdCanAdd && !ability[_abilityNum].isHaveTwoCd)
                    {
                         _coolDownTime = _curretCooldownTime = ability[_abilityNum].CoolDownTime;
                    }
                    
                    _state = AbilityState.CoolDown;
                }
                else
                {
                    if (ability[_abilityNum].isCdCanAdd)
                    {
                        if (_activeTime > ability[_abilityNum].CoolDownTime)
                        {
                            _activeTime -= Time.deltaTime / 5;
                        }
                    }
                }
                break;
            case AbilityState.CoolDown:
                if (_curretCooldownTime > 0)
                {
                    _curretCooldownTime -= Time.deltaTime;
                    OnCoolDownTime(_coolDownTime, _curretCooldownTime, isWeapon01);
                }
                else
                {
                    _state = AbilityState.Ready;
                }
                break;
        }
    }

    void SpecialEvent()
    {
        if (ability[_abilityNum].isCdCanAdd)//CubeShot and Katada
        {
            isCdAddCount += 1;
            if (isCdAddCount % 3 == 0)
            {
                _coolDownTime = _curretCooldownTime = _activeTime += 0.07f;
            }
            else
            {
                _coolDownTime = _curretCooldownTime = _activeTime;
            }
        }
        else if (ability[_abilityNum].isHaveTwoCd)//Aevolver
        {
            isCdAddCount += 1;

            if (_isFire01)
            {
                _isFire01 = false;
            }
            else if (_isFire02)
            {
                _isFire02 = false;
            }

            if (isCdAddCount % 6 == 0)
            {
                _coolDownTime = _curretCooldownTime = 2;
            }
            else
            {
                _coolDownTime = _curretCooldownTime = ability[_abilityNum].CoolDownTime;
            }
        }
    }
}
