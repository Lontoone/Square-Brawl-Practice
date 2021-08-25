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
    private float DirX, DirY;

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
    public DieEffect DieEffectObj;
    private Rigidbody2D _rb;//Player Rigidbody
    private Camera _camera;

    [HeaderAttribute("Sync Setting")]
    private float _newDirZ;

    private Vector2 _newPos;
    private Quaternion _newShootPointDir;

    public PhotonView Pv;
    private PlayerInputManager _inputAction;
    private PlayerManager _playerManager;
    private PlayerUIController _uiControl;

    public static PlayerController instance;
    private void Awake()
    {
        Pv = GetComponent<PhotonView>();
        _inputAction = new PlayerInputManager();
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

    /// <summary>
    /// 設定角色顏色，並同步到客戶端
    /// </summary>
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
        }

        GroundCheckEvent();//Is Grounding?
    }
    void FixedUpdate()
    {
        if (!Pv.IsMine)//客戶端同步位置
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
    /// <summary>
    /// Player Control
    /// </summary>
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

    /// <summary>
    /// Player Spin Event
    /// </summary>
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

            if ((IsGround && !_isCheckSpin)||_isWall)//在地面或牆上的角色旋轉
            {
                _rb.AddTorque(SpinInGroundForce * _inputPos.x);
            }
            /*else if (_isWall)
            {
                _rb.AddTorque(SpinInGroundForce * _inputPos.x);
            }*/
            else if (IsGround && _isCheckSpin)//點擊跳躍後但還在地面的角色旋轉
            {
                _rb.AddTorque(SpinForce * _inputPos.x);
                _isCheckSpin = false;
            }
            else if (!_isWall && !IsGround)//不再牆上或地上的角色旋轉
            {
                _rb.AddTorque(SpinForce * _inputPos.x);
            }
            _canSpin = false;
        }
    }
    #endregion

    /// <summary>
    /// Player InputSystem Control
    /// </summary>
    #region -- PlayerInputSystem Control --
    void LimitInputValue(Vector2 diretion)
    {
        if (!IsBeFreeze && _canLeftSticeSpin)//限制值要大於0.3，預防不小心觸發
        {
            //if (diretion.x >= 0.3f || diretion.x <= -0.3f || diretion.y >= 0.3f || diretion.y <= -0.3f)
            if (Mathf.Abs(diretion.x) >= 0.3f|| Mathf.Abs(diretion.y) >= 0.3f)
            {
                FrontSightAngle = Mathf.Atan2(diretion.y, diretion.x) * Mathf.Rad2Deg;
                FrontSightMidPos.rotation = Quaternion.Euler(new Vector3(0, 0, FrontSightAngle));
            }
        }

        Vector2 _inputMovement = diretion;
        //值大於0.5，強制設為1
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

    private void PlayerJumpDown()//按下Jump
    {
        _isJump = _isCheckSpin = true;
    }

    private void PlayerJumpUp()//放開Jump
    {
        _isJump = _isCheckSpin = false;
        CancelInvoke("IsJumpRecover");
    }

    void MouseSpin(Vector2 _mousePos)//滑鼠旋轉(瞄準點)
    {
        if (!IsBeFreeze)
        {
            Vector2 _mouseWorldPos = _camera.ScreenToWorldPoint(_mousePos);
            Vector2 _targetDir = _mouseWorldPos - new Vector2(FrontSightMidPos.position.x, FrontSightMidPos.position.y);
            FrontSightAngle = Mathf.Atan2(_targetDir.y, _targetDir.x) * Mathf.Rad2Deg;
            FrontSightMidPos.rotation = Quaternion.Euler(new Vector3(0, 0, FrontSightAngle));
        }
    }

    void GamePadSpin(Vector2 _mousePos)//GamePad蘑菇頭旋轉
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
            //if (_mousePos.x >= 0.3f || _mousePos.x <= -0.3f || _mousePos.y >= 0.3f || _mousePos.y <= -0.3f)
            if(Mathf.Abs(_mousePos.x)>=0.3f|| Mathf.Abs(_mousePos.y) >= 0.3f)
            {
                FrontSightAngle = Mathf.Atan2(_mousePos.y, _mousePos.x) * Mathf.Rad2Deg;
                FrontSightMidPos.rotation = Quaternion.Euler(new Vector3(0, 0, FrontSightAngle));
            }
        }
    }
    #endregion

    /// <summary>
    /// Player Damage And BeBounce
    /// </summary>
    #region -- Player Damage And BeBounce Event --
    public void DamageEvent(float _damage, float _beElasticity, float _dirX, float _dirY, Vector3 _beShootShake)
    {
        TakeDamage(_damage, _beShootShake.x, _beShootShake.y, _beShootShake.z);//傷害，並同步到客戶端
        BeBounce(_beElasticity, _dirX, _dirY);//被射到往後彈的力，並同步到客戶端
    }

    public bool IsKillAnyone()//是否殺到人?
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

    public void BeBounce(float _elasticty, float _dirX, float _dirY)//被射到往後彈的力，並同步到客戶端
    {
        Pv.RPC("Rpc_BeBounce", RpcTarget.All, _elasticty, _dirX, _dirY);
    }

    public void BeExplode(float _elasticty, Vector3 _pos, float _field)//被爆炸到的力，並同步到客戶端
    {
        Pv.RPC("Rpc_BeExplode", RpcTarget.All, _elasticty, _pos, _field);
    }

    public void TakeDamage(float _damage, float _shakeTime, float _shakePower, float _decrease)//傷害，並同步到客戶端
    {
        CameraShake.instance.SetShakeValue(_shakeTime, _shakePower, _decrease);
        Pv.RPC("Rpc_TakeDamage", RpcTarget.All, _damage, _shakeTime, _shakePower, _decrease);
    }
    #endregion

    /// <summary>
    /// Player Recoil
    /// </summary>
    #region -- Player Recoil Event --
    public void PlayerRecoil(float _recoil)//後座力
    {
        Recoil = _recoil;
        DirX = Mathf.Cos(FrontSightMidPos.eulerAngles.z * Mathf.PI / 180);//瞄準點的角度轉換成極座標X
        DirY = Mathf.Sin(FrontSightMidPos.eulerAngles.z * Mathf.PI / 180);//瞄準點的角度轉換成極座標Y

        _rb.AddForce(-Recoil * new Vector2(DirX, DirY));
    }
    #endregion

    /// <summary>
    /// Player Charge
    /// </summary>
    #region -- Player Charge Event --
    public void ChargeEvent(float _speed, float _elasticity, float _damage, Vector3 _beShotShake)//發動Charge
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

    /// <summary>
    /// Player Freeze
    /// </summary>
    #region -- Player Freeze Event --
    public void FreezeEvent(float _viewDistance, int viewCount, Vector3 _beShootShake)//發動Freeze
    {
        _beShootShakeValue = _beShootShake;

        _freezeLeftRay = Quaternion.Euler(0, 0, FrontSightPos.eulerAngles.z - 17.5f) * Vector2.right * _viewDistance;
        Vector3 _originPos = FrontSightPos.transform.position;

        for (int i = 0; i <= viewCount; i++)//扇形檢測區
        {
            Vector3 _freezeRay = Quaternion.Euler(0, 0, (35 / viewCount) * i) * _freezeLeftRay;
            int mask = LayerMask.GetMask("Player");
            int mask2 = LayerMask.GetMask("EditorView");
            RaycastHit2D FreezeHit = Physics2D.Raycast(_originPos, _freezeRay, _viewDistance, mask);
            RaycastHit2D GroundHit = Physics2D.Raycast(_originPos, _freezeRay, _viewDistance, mask2);
            Color color = FreezeHit ? Color.red : Color.green;
            if (FreezeHit)
            {
                PlayerController _playerController = FreezeHit.collider.gameObject.GetComponent<PlayerController>();
                if (!_playerController.Pv.IsMine)
                {
                    _playerController.BeFreezeEvent(_playerController.transform.position, _playerController.transform.rotation, _beShootShakeValue);
                }
            }else if (GroundHit)
            {
                Debug.Log("OK");
            }
            //Debug.DrawLine(_originPos, _originPos + _freezeRay, color);
        }
    }

    private void BeFreezeEvent(Vector3 _originPos, Quaternion _originDir, Vector3 _beShootAhake)//角色被凍結，並同步客戶端
    {
        Pv.RPC("Rpc_ChangeBeFreeze", RpcTarget.All, _originPos, _originDir, _beShootAhake);
        Invoke("StopBeFreeze", 2f);
    }

    public void StopBeFreeze()//角色被凍結結束，並同步客戶端
    {
        Pv.RPC("Rpc_StopBeFreeze", RpcTarget.All);
    }
    #endregion

    /// <summary>
    /// Bounce
    /// </summary>
    #region -- Bounce Event --
    public void IsBounceTrue()//被Bounce傷害中，並同步到客戶端
    {
        Pv.RPC("Rpc_IsBounceTrue", RpcTarget.All);
        Invoke("IsBounceFalse", 1f);
    }

    void IsBounceFalse()//被Bounce傷害結束，並同步到客戶端
    {
        Pv.RPC("Rpc_IsBounceFalse", RpcTarget.All);
    }
    #endregion

    /// <summary>
    /// Ground Check
    /// </summary>
    #region -- Ground Check Event --
    void GroundCheckEvent()//檢測是否在地上會牆上
    {
        RaycastHit2D leftCheck = Raycast(new Vector2(FootOffset, PlayerWidth), Vector2.left, GroundDistance);
        RaycastHit2D leftCheck2 = Raycast(new Vector2(FootOffset, -PlayerWidth), Vector2.left, GroundDistance);
        RaycastHit2D rightCheck = Raycast(new Vector2(FootOffset, PlayerWidth), Vector2.right, GroundDistance);
        RaycastHit2D rightCheck2 = Raycast(new Vector2(FootOffset, -PlayerWidth), Vector2.right, GroundDistance);
        RaycastHit2D downCheck = Raycast(new Vector2(PlayerWidth, FootOffset), Vector2.down, GroundDistance);
        RaycastHit2D downCheck2 = Raycast(new Vector2(-PlayerWidth, FootOffset), Vector2.down, GroundDistance);
        RaycastHit2D upCheck = Raycast(new Vector2(PlayerWidth, FootOffset), Vector2.up, GroundDistance);
        RaycastHit2D upCheck2 = Raycast(new Vector2(-PlayerWidth, FootOffset), Vector2.up, GroundDistance);
        if ((downCheck || downCheck2) && ((!leftCheck || !leftCheck2) && (!rightCheck || !rightCheck2)))
        {
            IsGround = _canSpin = true;
            _isWall = false;
        }
        else if (leftCheck || rightCheck || upCheck|| leftCheck2 || rightCheck2 || upCheck2)
        {
            _isWall = _canSpin = true;
            IsGround = false;
        }
        else
        {
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

    /// <summary>
    /// Shield
    /// </summary>
    #region -- ShieldEvent --
    public void IsShieldTrue()//發動Shield，並同步到客戶端
    {
        Pv.RPC("Rpc_IsShieldTrue", RpcTarget.All);
        Invoke("IsShieldFalse", 1f);
    }

    void IsShieldFalse()//結束Shield，並同步到客戶端
    {
        Pv.RPC("Rpc_IsShieldFalse", RpcTarget.All);
    }

    public void IsBeShieldTrue()//被Shield傷害中，並同步到客戶端
    {
        Pv.RPC("Rpc_IsBeShieldTrue", RpcTarget.All);
        Invoke("IsBeShieldFalse", 1f);
    }

    void IsBeShieldFalse()//被Shield傷害結束，並同步到客戶端
    {
        Pv.RPC("Rpc_IsBeShieldFalse", RpcTarget.All);
    }
    #endregion

    /// <summary>
    /// GamePad Shake
    /// </summary>
    #region -- GamePad Shake Event --
    void GamePadShakeEvent(float leftMotor,float rightMotor, float time)//搖桿震動
    {
        if (OptionSetting.CONTROLLER_RUMBLE)
        {
            GamePad.SetVibration(0, leftMotor, rightMotor);
            Invoke("StopGamePadShake", time);
        }
    }

    void StopGamePadShake()//關掉搖桿震動
    {
        GamePad.SetVibration(0, 0, 0);
    }
    #endregion

    /// <summary>
    /// Generate DieEffect Event
    /// </summary>
    public void GenerateDieEffect()//產生死亡特效與設定特效顏色，並同步到客戶端
    {
        DieEffect dieEffectObj = Instantiate(DieEffectObj, transform.position, Quaternion.identity);
        dieEffectObj.SetColor(DieEffectColor);
        Pv.RPC("Rpc_GenerateDieEffect", RpcTarget.Others, new Vector3(DieEffectColor.r, DieEffectColor.g, DieEffectColor.b));
    }

    /// <summary>
    /// Rebirth Event
    /// </summary>
    void Rebirth()//重生事件，並同步到客戶端
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

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && Pv.IsMine && _isCharge)//被Charge中的玩家撞到
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

        if (other.gameObject.CompareTag("Saw"))//碰到Saw
        {
            if (Pv.IsMine)
            {
                Vector2 dir = transform.position - other.gameObject.transform.position;
                DamageEvent(20, 1500, dir.x, dir.y, new Vector3(0.6f, 0.3f, 1));
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Katada") && !Pv.IsMine && !IsShield)//被Katana打到
        {
            Katada _katada = other.gameObject.GetComponent<Katada>();
            _katada.KatanaCollider(this);
        }
        else if (other.gameObject.CompareTag("Shield") && !Pv.IsMine && !IsBeShield)//被Shield打到
        {
            Shield _shield = other.gameObject.GetComponent<Shield>();
            _shield.ShieldCollider(this);
        }

        if (other.gameObject.CompareTag("Boundary"))//掉出邊界
        {
            if (Pv.IsMine)
            {
                CameraShake.instance.SetShakeValue(0.6f, 0.3f, 1);
                GamePadShakeEvent(0.5f, 0.5f, 0.5f);
                Pv.RPC("Rpc_OnBoundary", RpcTarget.All);
            }
        }
    }

    /// <summary>
    /// RPC
    /// </summary>
    #region -- RPC Event --
    [PunRPC]
    void ChangeColor(Vector3 color)//同步顏色
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
    void Rpc_BeBounce(float _elasticty, float _dirX, float _dirY)//同步被射到的力
    {
        _rb.AddForce(_elasticty * new Vector2(_dirX, _dirY));
    }

    [PunRPC]
    void Rpc_BeExplode(float _elasticty, Vector3 _pos, float _field)//同步被爆炸的力
    {
        _rb.AddExplosionForce(_elasticty, _pos, _field);
    }

    [PunRPC]
    void Rpc_TakeDamage(float _damage, float _shakeTime, float _shakePower, float _decrease)//同步傷害
    {
        _playerHp -= _damage;
        CameraShake.instance.SetShakeValue(_shakeTime, _shakePower, _decrease);
        GamePadShakeEvent(0.5f, 0.5f, 0.5f);
        _uiControl.ReduceHp(_playerHp);
        if (_playerHp <= 0)
        {
            GamePadShakeEvent(1f, 1f, 0.5f);
            Invoke("Rebirth", 3f);
            gameObject.SetActive(false);
        }
    }

    [PunRPC]
    void Rpc_OnBoundary()//同步掉出邊界後的事件
    {
        Invoke("Rebirth", 3f);
        gameObject.SetActive(false);
    }

    [PunRPC]
    void Rpc_GenerateDieEffect(Vector3 color)//同步產生死亡特效與同步特效顏色
    {
        DieEffect dieEffectObj = Instantiate(DieEffectObj, transform.position, Quaternion.identity);
        dieEffectObj.SetColor(new Color(color.x, color.y, color.z));
    }

    [PunRPC]
    void Rpc_Rebirth(Vector3 _pos)//同步重生事件
    {
        _newPos = _pos;
        transform.position = _pos;
        gameObject.SetActive(true);
    }

    [PunRPC]
    void Rpc_ChargeValue(bool _isCharge, float _elasticity, float _damage, float _dirX, float _dirY, Vector3 _beShootShake)//同步Charge發動後的數值
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
    void Rpc_ChangeBeFreeze(Vector3 _pos, Quaternion _dir, Vector3 _beShotAhake)//同步被凍結的事件
    {
        IsBeFreeze = true;
        transform.position = _pos;
        transform.rotation = _dir;
        _beShootShakeValue = _beShotAhake;
        if (Pv.IsMine)
        {
            CameraShake.instance.SetShakeValue(_beShotAhake.x, _beShotAhake.y, _beShotAhake.z);
        }
        _rb.bodyType = RigidbodyType2D.Kinematic;
        _rb.velocity=Vector2.zero;
    }

    [PunRPC]
    void Rpc_StopBeFreeze()//同步結束被凍結的事件
    {
        IsBeFreeze = false;
        _rb.bodyType = RigidbodyType2D.Dynamic;
    }
    #endregion

    /// <summary>
    /// 讓用戶自定義的component被Photon View監聽
    /// </summary>
    /// <param name="PhotonNetwork.Time">PhotonNetwork的時間，理論上同一個Room裡的玩家此數據是相同</param>
    /// <param name="info.SentServerTime">發送到伺服器的時間</param>
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)//發送方
        {
            stream.SendNext(_rb.position);
            stream.SendNext(_rb.rotation);
            stream.SendNext(FrontSightMidPos.transform.rotation);
            stream.SendNext(_rb.velocity);
            stream.SendNext(_rb.angularVelocity);
        }
        else //接收方
        {
            _newPos = (Vector2)stream.ReceiveNext();
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
