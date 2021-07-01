using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHolder : MonoBehaviour
{
    public Ability ability;
    private float _cooldownTime;
    private float _activeTime;

    public bool isSkill01;

    private bool _isFire01;
    private bool _isFire02;

    private PlayerInputManager _inputAction;
    private enum AbilityState
    {
        Ready,
        Active,
        CoolDown
    }

    private AbilityState _state = AbilityState.Ready;

    void Awake()
    {
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
        switch (_state)
        {
            case AbilityState.Ready:
                if (_isFire01||_isFire02)
                {
                    ability.Activate();
                    _state = AbilityState.Active;
                    _activeTime = ability.ActiveTime;
                }
                break;
            case AbilityState.Active:
                if (_activeTime > 0)
                {
                    _activeTime -= Time.deltaTime;
                }
                else
                {
                    _state = AbilityState.CoolDown;
                    _cooldownTime = ability.CoolDownTime;
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

    }
}
