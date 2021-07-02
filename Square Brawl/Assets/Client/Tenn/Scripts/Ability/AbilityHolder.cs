using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class AbilityHolder : MonoBehaviour
{
    private float _cooldownTime;
    private float _activeTime;
    private float isCdAddCount;

    public bool isSkill01;
    protected bool _isFire01;
    protected bool _isFire02;

    public Ability ability;

    private PlayerInputManager _inputAction;

    private PhotonView _pv;

    private enum AbilityState
    {
        Ready,
        Active,
        CoolDown
    }

    private AbilityState _state = AbilityState.Ready;

    void Awake()
    {
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
        ability.Initalize(gameObject);

        _activeTime = ability.CoolDownTime;

        if (isSkill01)
        {
            _inputAction.Player.Fire1.performed += _ => PlayerFire1Down();
            _inputAction.Player.Fire1.canceled += _ => PlayerFire1Up();
        }
        else
        {
            _inputAction.Player.Fire2.performed += _ => PlayerFire2Down();
            _inputAction.Player.Fire2.canceled += _ => PlayerFire2Up();
        }
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
        if (!_pv.IsMine)
        {
            return;
        }

        switch (_state)
        {
            case AbilityState.Ready:
                if (_isFire01||_isFire02)
                {
                    ability.Activate();

                    SpecialEvent();
                    
                    if(!ability.isCdCanAdd&&!ability.isHaveTwoCd)
                    {
                        _cooldownTime = ability.CoolDownTime;
                    }
                    
                    _state = AbilityState.CoolDown;
                }
                else
                {
                    if (ability.isCdCanAdd)
                    {
                        if (_activeTime > ability.CoolDownTime)
                        {
                            _activeTime -= Time.deltaTime/3;
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
            if (ability.isCdCanAdd)//CubeShot
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
            else if (ability.isHaveTwoCd)//Aevolver
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
                    _cooldownTime = ability.CoolDownTime;
                }
            }
        }
    }
}
