using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class AbilityHolder : MonoBehaviour
{
    private float _cooldownTime;
    private float _activeTime;
    private float isCdAddCount;

    public bool isWeapon01;
    protected bool _isFire01;
    protected bool _isFire02;

    public Ability[] ability;
    public int _abilityNum;

    private PlayerController _playerController;
    private PlayerInputManager _inputAction;
    private PhotonView _pv;
    public Player player;

    /*public enum WeaponType
    {
        None,
        Aevolver,
        BigBoom,
        Charge,
        CubeShoot,
        Katada,
        Sniper
    }

    public WeaponType weapon1, weapon2;*/

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
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                {
                    //foreach player data:
                    player = PhotonNetwork.PlayerList[i];
                }
            }

            if (isWeapon01)
            {
                _abilityNum = (int)player.CustomProperties[CustomPropertyCode.WEAPON1CODE];
                //_abilityNum = (int)weapon1;
                _inputAction.Player.Fire1.performed += _ => PlayerFire1Down();
                _inputAction.Player.Fire1.canceled += _ => PlayerFire1Up();
                SetWeapon();
            }
            else
            {
                _abilityNum = (int)player.CustomProperties[CustomPropertyCode.WEAPON2CODE];
                //_abilityNum = (int)weapon2;
                _inputAction.Player.Fire2.performed += _ => PlayerFire2Down();
                _inputAction.Player.Fire2.canceled += _ => PlayerFire2Up();
                SetWeapon();
            }
        }
    }

    public void SetWeapon()
    {
        ability[_abilityNum].Initalize(gameObject);
        _activeTime = ability[_abilityNum].CoolDownTime;
    }


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

    // Update is called once per frame
    void Update()
    {
        if (!_pv.IsMine || _playerController.IsBeFreeze)
        {
            return;
        }

        switch (_state)
        {
            case AbilityState.Ready:
                if (_isFire01||_isFire02)
                {
                    ability[_abilityNum].Activate();

                    SpecialEvent();
                    
                    if(!ability[_abilityNum].isCdCanAdd&&!ability[_abilityNum].isHaveTwoCd)
                    {
                        _cooldownTime = ability[_abilityNum].CoolDownTime;
                    }
                    
                    _state = AbilityState.CoolDown;
                }
                else
                {
                    if (ability[_abilityNum].isCdCanAdd)
                    {
                        if (_activeTime > ability[_abilityNum].CoolDownTime)
                        {
                            _activeTime -= Time.deltaTime/5;
                        }
                    }
                }
                break;
            case AbilityState.CoolDown:
                if (_cooldownTime > 0)
                {
                    _cooldownTime -= Time.deltaTime;
                }
                else
                {
                    _state = AbilityState.Ready;
                }
                break;
        }


        void SpecialEvent()
        {
            if (ability[_abilityNum].isCdCanAdd)//CubeShot and Katada
            {
                isCdAddCount += 1;
                if (isCdAddCount % 3 == 0)
                {
                    _cooldownTime = _activeTime += 0.05f;
                }
                else
                {
                    _cooldownTime = _activeTime;
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
                    _cooldownTime = 2;
                }
                else
                {
                    _cooldownTime = ability[_abilityNum].CoolDownTime;
                }
            }
        }
    }
}
