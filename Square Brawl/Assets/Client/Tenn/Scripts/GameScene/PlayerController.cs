using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using XInputDotNetPure;

public class PlayerController : MonoBehaviourPun, IPunObservable
{
    [HeaderAttribute("Player Setting")]
    [SerializeField]
    private float MoveSpeed;//Player Move Speed
    [SerializeField]
    private float JumpForce;//Player Jump Force
    [SerializeField]
    private float FlyForce;//Player Fly Force
    [SerializeField]
    private float DownForce;//Player Down Force
    [SerializeField]
    private float SpinForce;//Player Spin Force
    [SerializeField]
    private float SpinInGroundForce;//Player Spin In Ground Force

    public float FrontSightAngle;
    private float Damage;//Player Damage
    private float _playerHp;//Player Hp
    private float Recoil;//Player Recoil
    private float BeElasticity;//Player BeElasticity
    private float DirX, DirY;//

    private Vector3 _beShootShakeValue;
    private Vector3 _freezeLeftRay;
    private Vector2 _inputPos;//Keyboard Input Pos

    public bool IsBeFreeze;//Is Be Freeze?
    public bool _isCharge;//Is Change?
    public bool IsShield;
    public bool IsBeShield;
    public bool IsBounce;

    public bool IsGround;//Player Is Ground?
    private bool _isWall;//Player Is Wall?
    private bool _isJump;//Player Is Jump?
    private bool _isCheckSpin;//Is Check Spin?
    private bool _canSpin;//Player Can Spin?
    private bool _canLeftSticeSpin;

    public Color DieEffectColor;

    private SpriteRenderer _bodySprite;
    private Image _hpSprite;

    [HeaderAttribute("GroundCheck Setting")]
    public float FootOffset;
    public float GroundDistance;
    public float PlayerWidth;

    public LayerMask GroundLayer;

    [HeaderAttribute("Other Setting")]
    public Transform FrontSightMidPos;//FrontSightMid Position
    public Transform FrontSightPos;//FrontSight Position
    private Rigidbody2D _rb;//Player Rigidbody
    public DieEffect DieEffectObj;

    private Camera _camera;

    [HeaderAttribute("Sync Setting")]
    private float _newDirZ;

    private Vector2 _newPos;

    //private Quaternion _newDir;
    private Quaternion _newShootPointDir;

    public PhotonView Pv;
    private PlayerInputManager _inputAction;
    private PlayerManager _playerManager;
    private PlayerUIController _uiControl;

    public static PlayerController instance;
    private void Awake()
    {
        _inputAction = new PlayerInputManager();
        Pv = GetComponent<PhotonView>();
        _rb = GetComponent<Rigidbody2D>();
        _bodySprite = transform.GetChild(1).GetComponent<SpriteRenderer>();
        _hpSprite = transform.GetChild(6).GetChild(0).GetComponent<Image>();
        _playerManager = PhotonView.Find((int)Pv.InstantiationData[0]).GetComponent<PlayerManager>();
        _uiControl = transform.GetChild(6).GetComponent<PlayerUIController>();
        if (Pv.IsMine)
        {
            instance = this;
        }
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

    public void OnDestroy()
    {
        ResultManager.OnDisableResult -= OnDisableThis;
    }

    void Start()
    {
        _bodySprite.sprite = Resources.Load<Sprite>("PlayerStyle/" + TillStyleLoader.s_StyleName + "/PlayerBody");
        _hpSprite.sprite = Resources.Load<Sprite>("PlayerStyle/" + TillStyleLoader.s_StyleName + "/PlayerUI");

        _playerHp = 100;
        _canSpin = true;
        _camera = Camera.main;
        FrontSightMidPos = transform.GetChild(0).GetComponent<Transform>();
        if (Pv.IsMine)
        {
            FrontSightPos = FrontSightMidPos.transform.GetChild(0);

            _inputAction.Player.Jump.performed += _ => PlayerJumpDown();
            _inputAction.Player.Jump.canceled += _ => PlayerJumpUp();
            _inputAction.Player.Movement.performed += ctx => LimitInputValue(ctx.ReadValue<Vector2>());
            _inputAction.Player.MouseRotation.performed += ctx => MouseSpin(ctx.ReadValue<Vector2>());
            _inputAction.Player.GamePadRotation.performed += ctx => GamePadSpin(ctx.ReadValue<Vector2>());
            SetColor();
        }
    }

    private void SetColor()
    {
        Color _color = DieEffectColor = transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().color =
            CustomPropertyCode.COLORS[(int)PhotonNetwork.LocalPlayer.CustomProperties[CustomPropertyCode.TEAM_CODE]];

        transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(_color.r, _color.g, _color.b, 0.5f);
        _uiControl.PlayerHpImg.color = _color;

        Pv.RPC("ChangeColor", RpcTarget.Others, new Vector3(_color.r, _color.g, _color.b));
    }

    void Update()
    {
        if (!Pv.IsMine)
        {
            FrontSightMidPos.transform.rotation = Quaternion.Lerp(FrontSightMidPos.transform.rotation, _newShootPointDir, 15 * Time.deltaTime);
            /*_rb.position = Vector2.MoveTowards(_rb.position, _newPos, Time.deltaTime);
            _rb.rotation = Mathf.Lerp(_rb.rotation, _newDirZ, 5 * Time.deltaTime);*/
        }

        GroundCheckEvent();//Is Grounding?
    }
    void FixedUpdate()
    {
        if (!Pv.IsMine)
        {
            _rb.position = Vector2.MoveTowards(_rb.position, _newPos, 2*Time.fixedDeltaTime);
            _rb.rotation = Mathf.Lerp(_rb.rotation, _newDirZ, 3*Time.fixedDeltaTime);
        }
        if (!IsBeFreeze && Pv.IsMine)
        {
            PlayerMovement();//Player Move
            PlayerSpin();//Player Spin
        }
    }

    #region -- Player Control --
    void PlayerMovement()
    {
        //Player Move
        _rb.AddForce(MoveSpeed * new Vector2(_inputPos.x, 0));

        if (_inputPos.y > 0 && !_isWall && !IsGround)//Player Fly
        {
            _rb.AddForce(FlyForce * new Vector2(0, _inputPos.y));
        }
        else if (_inputPos.y < 0 && !IsGround)//Player Down
        {
            _rb.AddForce(DownForce * new Vector2(0, _inputPos.y));
        }

        if ((IsGround || _isWall) && _isJump)//Player Jump
        {
            _rb.AddForce(JumpForce * Vector3.up);
            _isJump = false;
            Invoke("IsJumpRecover", 0.3f);
        }
    }

    void IsJumpRecover()
    {
        _isJump = true;
    }

    void PlayerSpin()
    {
        if (_canSpin && _inputPos.x != 0)
        {
            if (_inputPos.x != 0 && _isCheckSpin)//Limit Player Rotation Speed
            {
                Physics2D.maxRotationSpeed = 20;
            }
            else
            {
                Physics2D.maxRotationSpeed = 6;
            }

            if (IsGround && !_isCheckSpin)
            {
                _rb.AddTorque(SpinInGroundForce * _inputPos.x);
                //Debug.Log("IsGround");
            }
            else if (_isWall)
            {
                _rb.AddTorque(SpinInGroundForce * _inputPos.x);
                //Debug.Log("IsWall");
            }
            else if (IsGround && _isCheckSpin)
            {
                _rb.AddTorque(SpinForce * _inputPos.x);
                _isCheckSpin = false;
                //Debug.Log("IsGroundJump");
            }
            else if (!_isWall && !IsGround)
            {
                _rb.AddTorque(SpinForce * _inputPos.x);
                //Debug.Log("IsNotGroundWall");
            }
            _canSpin = false;
        }
    }
    #endregion


    #region -- PlayerInputSystem Control --
    void LimitInputValue(Vector2 diretion)
    {
        if (!IsBeFreeze && _canLeftSticeSpin)
        {
            if (diretion.x >= 0.3f || diretion.x <= -0.3f || diretion.y >= 0.3f || diretion.y <= -0.3f)
            {
                FrontSightAngle = Mathf.Atan2(diretion.y, diretion.x) * Mathf.Rad2Deg;
                FrontSightMidPos.rotation = Quaternion.Euler(new Vector3(0, 0, FrontSightAngle));
            }
        }

        Vector2 _inputMovement = diretion;
        if (_inputMovement.x > 0.5)
        {
            _inputMovement.x = 1;
        }
        else if (_inputMovement.x < -0.5)
        {
            _inputMovement.x = -1;
        }
        else
        {
            _inputMovement.x = 0;
        }

        if (_inputMovement.y > 0.5)
        {
            _inputMovement.y = 1;
        }
        else if (_inputMovement.y < -0.5)
        {
            _inputMovement.y = -1;
        }
        else
        {
            _inputMovement.y = 0;
        }

        _inputPos = new Vector2(_inputMovement.x, _inputMovement.y);
    }

    private void PlayerJumpDown()
    {
        _isJump = _isCheckSpin = true;
    }

    private void PlayerJumpUp()
    {
        _isJump = _isCheckSpin = false;
        CancelInvoke("IsJumpRecover");
    }

    void MouseSpin(Vector2 _mousePos)
    {
        if (!IsBeFreeze)
        {
            Vector2 _mouseWorldPos = _camera.ScreenToWorldPoint(_mousePos);
            Vector2 _targetDir = _mouseWorldPos - new Vector2(FrontSightMidPos.position.x, FrontSightMidPos.position.y);
            FrontSightAngle = Mathf.Atan2(_targetDir.y, _targetDir.x) * Mathf.Rad2Deg;
            FrontSightMidPos.rotation = Quaternion.Euler(new Vector3(0, 0, FrontSightAngle));
        }
    }

    void GamePadSpin(Vector2 _mousePos)
    {
        if (_mousePos.x != 0 && _mousePos.y != 0)
        {
            _canLeftSticeSpin = false;
        }
        else
        {
            _canLeftSticeSpin = true;
        }

        if (!IsBeFreeze)
        {
            if (_mousePos.x >= 0.3f || _mousePos.x <= -0.3f || _mousePos.y >= 0.3f || _mousePos.y <= -0.3f)
            {
                FrontSightAngle = Mathf.Atan2(_mousePos.y, _mousePos.x) * Mathf.Rad2Deg;
                FrontSightMidPos.rotation = Quaternion.Euler(new Vector3(0, 0, FrontSightAngle));
            }
        }
    }
    #endregion


    #region -- Player Damage And BeBounce Event --
    public void DamageEvent(float _damage, float _beElasticity, float _dirX, float _dirY, Vector3 _beShootShake)
    {
        TakeDamage(_damage, _beShootShake.x, _beShootShake.y, _beShootShake.z);
        BeBounce(_beElasticity, _dirX, _dirY);
    }

    public bool IsKillAnyone()
    {
        if (_playerHp <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void BeBounce(float _elasticty, float _dirX, float _dirY)
    {
        Pv.RPC("Rpc_BeBounce", RpcTarget.All, _elasticty, _dirX, _dirY);
    }

    public void BeExplode(float _elasticty, Vector3 _pos, float _field)
    {
        Pv.RPC("Rpc_BeExplode", RpcTarget.All, _elasticty, _pos, _field);
    }

    public void TakeDamage(float _damage, float _shakeTime, float _shakePower, float _decrease)//,float _elasticty,float _bullletDirX,float _bullletDirY)
    {
        CameraShake.instance.SetShakeValue(_shakeTime, _shakePower, _decrease);
        Pv.RPC("Rpc_TakeDamage", RpcTarget.All, _damage, _shakeTime, _shakePower, _decrease);
    }
    #endregion


    #region -- Player Recoil Event --
    public void PlayerRecoil(float _recoil)
    {
        Recoil = _recoil;
        DirX = Mathf.Cos(FrontSightMidPos.eulerAngles.z * Mathf.PI / 180);
        DirY = Mathf.Sin(FrontSightMidPos.eulerAngles.z * Mathf.PI / 180);

        _rb.AddForce(-Recoil * new Vector2(DirX, DirY));
    }
    #endregion


    #region -- Player Charge Event --
    public void ChargeEvent(float _speed, float _elasticity, float _damage, Vector3 _beShotShake)
    {
        PlayerRecoil(_speed);
        BeElasticity = _elasticity;
        Damage = _damage;
        _beShootShakeValue = _beShotShake;
        _isCharge = true;
        DirX = Mathf.Cos(FrontSightMidPos.eulerAngles.z * Mathf.PI / 180);
        DirY = Mathf.Sin(FrontSightMidPos.eulerAngles.z * Mathf.PI / 180);
        Pv.RPC("Rpc_ChargeValue", RpcTarget.All, _isCharge, BeElasticity, Damage, DirX, DirY, _beShootShakeValue);
    }
    #endregion


    #region -- Player Freeze Event --
    public void FreezeEvent(float _viewDistance, int viewCount, Vector3 _beShootShake)
    {
        _beShootShakeValue = _beShootShake;

        _freezeLeftRay = Quaternion.Euler(0, 0, FrontSightPos.eulerAngles.z - 17.5f) * Vector2.right * _viewDistance;
        Vector3 _originPos = FrontSightPos.transform.position;

        for (int i = 0; i <= viewCount; i++)
        {
            Vector3 _freezeRay = Quaternion.Euler(0, 0, (35 / viewCount) * i) * _freezeLeftRay;
            int mask = LayerMask.GetMask("Player");
            RaycastHit2D FreezeHit = Physics2D.Raycast(_originPos, _freezeRay, _viewDistance, mask);
            Color color = FreezeHit ? Color.red : Color.green;
            if (FreezeHit)
            {
                PlayerController _playerController = FreezeHit.collider.gameObject.GetComponent<PlayerController>();
                if (!_playerController.Pv.IsMine)
                {
                    _playerController.BeFreezeEvent(_playerController.transform.position, _playerController.transform.rotation, _beShootShakeValue);
                }
            }
            //Debug.DrawLine(_originPos, _originPos + _freezeRay, color);
        }
    }

    private void BeFreezeEvent(Vector3 _originPos, Quaternion _originDir, Vector3 _beShotAhake)
    {
        Pv.RPC("Rpc_ChangeBeFreeze", RpcTarget.All, _originPos, _originDir, _beShotAhake);
        //CameraShake.instance.SetShakeValue(_beShotAhake.x, _beShotAhake.y, _beShotAhake.z);
        Invoke("StopBeFreeze", 2f);
    }

    public void StopBeFreeze()
    {
        Pv.RPC("Rpc_StopBeFreeze", RpcTarget.All);
    }
    #endregion


    #region -- Bounce Event --
    public void IsBounceTrue()
    {
        Pv.RPC("Rpc_IsBounceTrue", RpcTarget.All);
        Invoke("IsBounceFalse", 1f);
    }

    void IsBounceFalse()
    {
        Pv.RPC("Rpc_IsBounceFalse", RpcTarget.All);
    }
    #endregion


    #region -- Ground Check Event --
    void GroundCheckEvent()
    {
        RaycastHit2D leftCheck = Raycast(new Vector2(FootOffset, PlayerWidth), Vector2.left, GroundDistance);
        RaycastHit2D leftCheck2 = Raycast(new Vector2(FootOffset, -PlayerWidth), Vector2.left, GroundDistance);
        RaycastHit2D rightCheck = Raycast(new Vector2(FootOffset, PlayerWidth), Vector2.right, GroundDistance);
        RaycastHit2D rightCheck2 = Raycast(new Vector2(FootOffset, -PlayerWidth), Vector2.right, GroundDistance);
        RaycastHit2D downCheck = Raycast(new Vector2(PlayerWidth, FootOffset), Vector2.down, GroundDistance);
        RaycastHit2D downCheck2 = Raycast(new Vector2(-PlayerWidth, FootOffset), Vector2.down, GroundDistance);
        RaycastHit2D upCheck = Raycast(new Vector2(PlayerWidth, FootOffset), Vector2.up, GroundDistance);
        RaycastHit2D upCheck2 = Raycast(new Vector2(-PlayerWidth, FootOffset), Vector2.up, GroundDistance);
        //if (downCheck && (!leftCheck && !rightCheck))
        if ((downCheck || downCheck2) && ((!leftCheck || !leftCheck2) && (!rightCheck || !rightCheck2)))
        {
            //Physics2D.IgnoreLayerCollision(11, 12, false);
            IsGround = _canSpin = true;
            _isWall = false;
        }
        else if (leftCheck || rightCheck || upCheck|| leftCheck2 || rightCheck2 || upCheck2)
        {
            //Physics2D.IgnoreLayerCollision(11, 12, false);
            _isWall = _canSpin = true;
            IsGround = false;
        }
        else
        {
            //Physics2D.IgnoreLayerCollision(11, 12, true);
            IsGround = false;
            _isWall = false;
        }
    }

    //Ground Raycast
    private RaycastHit2D Raycast(Vector2 offset, Vector2 rayDirection, float lengh)
    {
        Vector2 pos = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDirection, lengh, GroundLayer);
        Color color = hit ? Color.red : Color.green;
        Debug.DrawRay(pos + offset, rayDirection * lengh, color);
        return hit;
    }
    #endregion


    #region -- ShieldEvent --
    public void IsShieldTrue()
    {
        Pv.RPC("Rpc_IsShieldTrue", RpcTarget.All);
        Invoke("IsShieldFalse", 1f);
    }

    void IsShieldFalse()
    {
        Pv.RPC("Rpc_IsShieldFalse", RpcTarget.All);
    }

    public void IsBeShieldTrue()
    {
        Pv.RPC("Rpc_IsBeShieldTrue", RpcTarget.All);
        Invoke("IsBeShieldFalse", 1f);
    }

    void IsBeShieldFalse()
    {
        Pv.RPC("Rpc_IsBeShieldFalse", RpcTarget.All);
    }
    #endregion

    void GamePadShakeEvent(float leftMotor,float rightMotor, float time)
    {
        if (OptionSetting.CONTROLLER_RUMBLE)
        {
            GamePad.SetVibration(0, leftMotor, rightMotor);
            Invoke("StopGamePadShake", time);
        }
    }

    void StopGamePadShake()
    {
        GamePad.SetVibration(0, 0, 0);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && Pv.IsMine && _isCharge)
        {
            PlayerController _playerController = other.gameObject.GetComponent<PlayerController>();
            if (!_playerController.Pv.IsMine && !_playerController.IsShield)
            {
                _playerController.DamageEvent(Damage, BeElasticity, DirX, DirY, _beShootShakeValue);
                var IsKill = _playerController.IsKillAnyone();
                if (IsKill)
                {
                    PlayerKillCountManager.instance.SetKillCount();
                    _playerController.GenerateDieEffect();
                }
                _isCharge = false;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Katada") && !Pv.IsMine && !IsShield)
        {
            Katada _katada = other.gameObject.GetComponent<Katada>();
            _katada.KatadaCollider(this);
        }
        else if (other.gameObject.CompareTag("Shield") && !Pv.IsMine && !IsBeShield)
        {
            Shield _shield = other.gameObject.GetComponent<Shield>();
            _shield.ShieldCollider(this);
        }

        if (other.gameObject.CompareTag("Boundary"))
        {
            if (Pv.IsMine)
            {
                CameraShake.instance.SetShakeValue(0.6f, 0.3f, 1);
                GamePadShakeEvent(0.5f, 0.5f, 0.5f);
                Pv.RPC("Rpc_OnBoundary", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    void ChangeColor(Vector3 color)
    {
        Color _color = DieEffectColor = new Color(color.x, color.y, color.z);
        transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().color = _color;
        transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(_color.r, _color.g, _color.b, 0.5f);
        _uiControl.PlayerHpImg.color = _color;
    }

    [PunRPC]
    void Rpc_IsBounceTrue()
    {
        IsBounce = true;
    }

    [PunRPC]
    void Rpc_IsBounceFalse()
    {
        IsBounce = false;
    }

    [PunRPC]
    void Rpc_IsShieldTrue()
    {
        IsShield = true;
    }

    [PunRPC]
    void Rpc_IsShieldFalse()
    {
        IsShield = false;
    }

    [PunRPC]
    void Rpc_IsBeShieldTrue()
    {
        IsBeShield = true;
    }

    [PunRPC]
    void Rpc_IsBeShieldFalse()
    {
        IsBeShield = false;
    }

    [PunRPC]
    void Rpc_BeBounce(float _elasticty, float _dirX, float _dirY)
    {
        _rb.AddForce(_elasticty * new Vector2(_dirX, _dirY));
    }

    [PunRPC]
    void Rpc_BeExplode(float _elasticty, Vector3 _pos, float _field)
    {
        _rb.AddExplosionForce(_elasticty, _pos, _field);
    }

    [PunRPC]
    void Rpc_TakeDamage(float _damage, float _shakeTime, float _shakePower, float _decrease)
    {
        _playerHp -= _damage;
        CameraShake.instance.SetShakeValue(_shakeTime, _shakePower, _decrease);
        GamePadShakeEvent(0.5f, 0.5f, 0.5f);
        _uiControl.ReduceHp(_playerHp);
        if (_playerHp <= 0)
        {
            GamePadShakeEvent(1f, 1f, 0.5f);
            Invoke("Rebirth", 3f);
            /*DieEffect dieEffectObj = Instantiate(DieEffectObj, transform.position, Quaternion.identity);
            dieEffectObj.SetColor(DieEffectColor);*/
            gameObject.SetActive(false);
        }
    }

    [PunRPC]
    void Rpc_OnBoundary()
    {
        Invoke("Rebirth", 3f);
        gameObject.SetActive(false);
    }

    public void GenerateDieEffect()
    {
        //Color color = CustomPropertyCode.COLORS[(int)PhotonNetwork.LocalPlayer.CustomProperties[CustomPropertyCode.TEAM_CODE]];
        DieEffect dieEffectObj = Instantiate(DieEffectObj, transform.position, Quaternion.identity);
        dieEffectObj.SetColor(DieEffectColor);
        Pv.RPC("Rpc_GenerateDieEffect", RpcTarget.Others, new Vector3(DieEffectColor.r, DieEffectColor.g, DieEffectColor.b));
    }

    [PunRPC]
    void Rpc_GenerateDieEffect(Vector3 color)
    {
        DieEffect dieEffectObj = Instantiate(DieEffectObj, transform.position, Quaternion.identity);
        dieEffectObj.SetColor(new Color(color.x, color.y, color.z));
    }

    void Rebirth()
    {
        if (Pv.IsMine)
        {
            gameObject.SetActive(true);
            transform.position = new Vector3(UnityEngine.Random.Range(-5, 6), 0, 0);
            Pv.RPC("Rpc_Rebirth", RpcTarget.Others, transform.position);
        }
        _playerHp = 100;
        _uiControl.ReduceHp(_playerHp);
    }

    [PunRPC]
    void Rpc_Rebirth(Vector3 _pos)
    {
        _newPos = _pos;
        transform.position = _pos;
        gameObject.SetActive(true);
    }

    [PunRPC]
    void Rpc_ChargeValue(bool _isCharge, float _elasticity, float _damage, float _dirX, float _dirY, Vector3 _beShootShake)
    {
        this._isCharge = _isCharge;
        BeElasticity = _elasticity;
        Damage = _damage;
        DirX = _dirX;
        DirY = _dirY;
        _beShootShakeValue = _beShootShake;
        Invoke("IsChargeChangeFalse", 0.5f);
    }

    void IsChargeChangeFalse()
    {
        _isCharge = false;
    }

    [PunRPC]
    void Rpc_ChangeBeFreeze(Vector3 _pos, Quaternion _dir, Vector3 _beShotAhake)
    {
        IsBeFreeze = true;
        transform.position = _pos;
        transform.rotation = _dir;
        _beShootShakeValue = _beShotAhake;
        if (Pv.IsMine)
        {
            CameraShake.instance.SetShakeValue(_beShotAhake.x, _beShotAhake.y, _beShotAhake.z);
        }
        _rb.bodyType = RigidbodyType2D.Static;
    }

    [PunRPC]
    void Rpc_StopBeFreeze()
    {
        IsBeFreeze = false;
        _rb.bodyType = RigidbodyType2D.Dynamic;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(_rb.position);
            //stream.SendNext(transform.rotation);
            stream.SendNext(_rb.rotation);
            stream.SendNext(FrontSightMidPos.transform.rotation);
            stream.SendNext(_rb.velocity);
            stream.SendNext(_rb.angularVelocity);
            stream.SendNext(IsBeShield);
            stream.SendNext(IsBounce);
        }
        else
        {
            _newPos = (Vector2)stream.ReceiveNext();
            //_newDir = (Quaternion)stream.ReceiveNext();
            _newDirZ = (float)stream.ReceiveNext();
            _newShootPointDir = (Quaternion)stream.ReceiveNext();
            _rb.velocity = (Vector2)stream.ReceiveNext();
            _rb.angularVelocity = (float)stream.ReceiveNext();

            float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime)) + (float)(PhotonNetwork.GetPing() * 0.0001f);
            _newPos += (_rb.velocity * lag);
            _newDirZ += (_rb.angularVelocity * lag);
        }
    }
}
