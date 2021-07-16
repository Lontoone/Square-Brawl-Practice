using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using System.IO;
public class PlayerController : MonoBehaviour,IPunObservable
{
    [HeaderAttribute("Player Setting")]
    public float MoveSpeed;//Player Move Speed
    public float JumpForce;//Player Jump Force
    public float FlyForce;//Player Fly Force
    public float DownForce;//Player Down Force
    public float SpinForce;//Player Spin Force
    public float SpinInGroundForce;//Player Spin In Ground Force
    public float Damage;//Player Damage
    public float PlayerHp;//Player Hp
    public float Recoil;//Player Recoil
    public float BeElasticity;//Player BeElasticity
    public float DirX, DirY;//
    public float FrontSightAngle;
    private Vector3 _freezeLeftRay;
    private Vector3 _originPos;
    private Quaternion _originDir;
    private Vector2 _inputPos;//Keyboard Input Pos

    public bool IsShootFreeze;//Is Shoot Freeze?
    public bool IsFreezeChange;//Is Freeze Change?
    public bool IsBeFreeze;//Is Be Freeze?
    public bool IsCharge;//Is Change?
    private bool _isChargeChange;//Is Charge Change?
    private bool _canSpin;//Player Can Spin?

    [HeaderAttribute("GroundCheck Setting")]
    public float FootOffset;
    public float GroundDistance;
    public float PlayerWidth;

    private bool _isGround;//Player Is Ground?
    private bool _isWall;//Player Is Wall?
    private bool _isJump;//Player Is Jump?
    private bool _isCheckSpin;//Is Check Spin?

    public LayerMask GroundLayer;

    [HeaderAttribute("Other Setting")]
    public Transform FrontSightMidPos;//FrontSightMid Position
    public Transform FrontSightPos;//FrontSight Position
    private Rigidbody2D _rb;//Player Rigidbody
    public Rigidbody2D FrontSightRb;//FrontSight Rigidbody

    private Camera _camera;

    [HeaderAttribute("Sync Setting")]
    private float _newDirZ;

    private Vector2 _newPos;

    //private Quaternion _newDir;
    private Quaternion _newShootPointDir;

    public PhotonView Pv;
    private PlayerInputManager _inputAction;
    private PlayerManager _playerManager;

    public static PlayerController instance;
    private void Awake()
    {
        _inputAction = new PlayerInputManager();
        Pv = GetComponent<PhotonView>();
        _rb = GetComponent<Rigidbody2D>();
        _playerManager = PhotonView.Find((int)Pv.InstantiationData[0]).GetComponent<PlayerManager>();
        if (Pv.IsMine)
        {
            instance = this;
        }
    }

    private void OnEnable()
    {
        _inputAction.Enable();
    }

    private void OnDisable()
    {
        _inputAction.Disable();
    }

    void Start()
    {
        _camera = Camera.main;
        _canSpin = true;
        FrontSightMidPos = transform.GetChild(0).GetComponent<Transform>();
        if (Pv.IsMine)
        {
            //FrontSightMidPos = transform.GetChild(0).GetComponent<Transform>();
            FrontSightRb = transform.GetChild(1).GetComponent<Rigidbody2D>();
            FrontSightPos = FrontSightMidPos.transform.GetChild(0);

            _inputAction.Player.Jump.performed += _ => PlayerJumpDown();
            _inputAction.Player.Jump.canceled += _ => PlayerJumpUp();
            _inputAction.Player.Movement.performed += ctx => LimitInputValue(ctx.ReadValue<Vector2>());
            _inputAction.Player.MouseRotation.performed += ctx => MouseSpin(ctx.ReadValue<Vector2>());
            _inputAction.Player.GamePadRotation.performed += ctx => GamePadSpin(ctx.ReadValue<Vector2>());
        }
    }

    void Update()
    {
        if (!Pv.IsMine)
        {
            //_rb.position = Vector2.Lerp(_rb.position, _newPos, 10 * Time.deltaTime);
            //transform.rotation = Quaternion.Lerp(transform.rotation, _newDir, 15 * Time.deltaTime);
            FrontSightMidPos.transform.rotation = Quaternion.Lerp(FrontSightMidPos.transform.rotation, _newShootPointDir, 15 * Time.deltaTime);
        }
        else
        {
            ChargeEvent();//Ability Charge Event

            GroundCheckEvent();//Is Grounding?
        }

        FreezeEvent();//Ability Freeze Event
    }
    void FixedUpdate()
    {
        if (!Pv.IsMine)
        {
            _rb.position = Vector2.MoveTowards(_rb.position, _newPos, Time.fixedDeltaTime);
            _rb.rotation = Mathf.Lerp(_rb.rotation, _newDirZ, Time.fixedDeltaTime);
           // transform.rotation = Quaternion.Lerp(transform.rotation, _newDir, 15*Time.fixedDeltaTime);
           // ShootSpinMidPos.transform.rotation = Quaternion.Lerp(ShootSpinMidPos.transform.rotation, _newShootPointDir, 15*Time.fixedDeltaTime);
        }
        else if(!IsBeFreeze && Pv.IsMine)
        {
            PlayerMovement();//Player Move
            PlayerSpin();//Player Spin
        }
    }

    void PlayerMovement()
    {
        //Player Move
        _rb.AddForce(MoveSpeed * new Vector2(_inputPos.x, 0)) ;

        if (_inputPos.y > 0&&!_isWall&&!_isGround)//Player Fly
        {
            _rb.AddForce(FlyForce * new Vector2(0,_inputPos.y));
        }
        else if(_inputPos.y < 0&&!_isGround)//Player Down
        {
            _rb.AddForce(DownForce * new Vector2(0, _inputPos.y));
        }

        if ((_isGround || _isWall)&&_isJump)//Player Jump
        {
            _rb.AddForce(JumpForce * Vector3.up);
            StartCoroutine(IsJumpRecover());
        }
    }

    IEnumerator IsJumpRecover()
    {
        _isJump = false;
        yield return new WaitForSeconds(0.3f);
        _isJump = true ;
    }

    void LimitInputValue(Vector2 diretion)
    {
        Vector2 _inputMovement = diretion;
        if (_inputMovement.x > 0.5)
        {
            _inputMovement.x = 1;
        }
        else if(_inputMovement.x < -0.5)
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

    void PlayerSpin()
    {
        if (_canSpin && _inputPos.x != 0)
        {
            if (_inputPos.x != 0 && _isCheckSpin)//Limit Player Rotation Speed
            {
                Physics2D.maxRotationSpeed = 30;
            }
            else
            {
                Physics2D.maxRotationSpeed = 6;
            }

            if (_isGround && !_isCheckSpin)
            {
                _rb.AddTorque(SpinInGroundForce * _inputPos.x);
                //Debug.Log("IsGround");
            }
            else if (_isWall)
            {
                _rb.AddTorque(SpinInGroundForce * _inputPos.x);
                //Debug.Log("IsWall");
            }
            else if (_isGround && _isCheckSpin)
            {
                _rb.AddTorque(SpinForce * _inputPos.x);
                _isCheckSpin = false;
                //Debug.Log("IsGroundJump");
            }
            else if (!_isWall&&!_isGround)
            {
                _rb.AddTorque(SpinForce * _inputPos.x);
                //Debug.Log("IsNotGroundWall");
            }
            _canSpin = false;
        }
    }

    private void PlayerJumpDown()
    {
        _isJump = _isCheckSpin = true;    
    }

    private void PlayerJumpUp()
    {
        _isJump = _isCheckSpin = false;
        StopAllCoroutines();
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
        if (!IsBeFreeze) 
        {
            if (_mousePos.x >= 0.3f || _mousePos.x <= -0.3f || _mousePos.y >= 0.3f || _mousePos.y <= -0.3f)
            {
                FrontSightAngle = Mathf.Atan2(_mousePos.y, _mousePos.x) * Mathf.Rad2Deg;
                FrontSightMidPos.rotation = Quaternion.Euler(new Vector3(0, 0, FrontSightAngle));
            }
        }
    }

    public void PlayerRecoil(float _recoil)
    {
        Recoil = _recoil;
        DirX = Mathf.Cos(FrontSightMidPos.eulerAngles.z * Mathf.PI / 180);
        DirY = Mathf.Sin(FrontSightMidPos.eulerAngles.z * Mathf.PI / 180);

        _rb.AddForce(-Recoil * new Vector2(DirX, DirY));
    }

    void ChargeEvent()
    {
        if (_isChargeChange != IsCharge)
        {
            DirX = Mathf.Cos(FrontSightMidPos.eulerAngles.z * Mathf.PI / 180);
            DirY = Mathf.Sin(FrontSightMidPos.eulerAngles.z * Mathf.PI / 180);
            Pv.RPC("Rpc_ChargeValue", RpcTarget.All, IsCharge, BeElasticity, Damage, DirX, DirY);
            _isChargeChange = IsCharge;
        }
    }

    void FreezeEvent()
    {
        if (IsShootFreeze)
        {
            PlayerFreezeRaycast(3, 5);
        }

        if (IsBeFreeze)
        {
            Pv.RPC("Rpc_ChangeBeFreeze", RpcTarget.All,_originPos,_originDir);
            StartCoroutine(StopBeFreeze());
        }
    }

    void PlayerFreezeRaycast(float _viewDistance, int viewCount)
    {
        if (!IsFreezeChange)
        {
            _freezeLeftRay = Quaternion.Euler(0, 0, FrontSightPos.eulerAngles.z - 17.5f) * Vector2.right * _viewDistance;
            _originPos = FrontSightPos.transform.position;
            IsFreezeChange = true;
        }

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
                    _playerController.IsBeFreeze = true;
                    _playerController._originPos = _playerController.transform.position;
                    _playerController._originDir = _playerController.transform.rotation;
                }
            }
            Debug.DrawLine(_originPos, _originPos + _freezeRay, color);
        }
    }

    IEnumerator StopBeFreeze()
    {
        yield return new WaitForSeconds(2f);
        Pv.RPC("Rpc_StopBeFreeze",RpcTarget.All);
    }

    //Ground Check
    void GroundCheckEvent()
    {
        RaycastHit2D leftCheck = Raycast(new Vector2(-PlayerWidth, FootOffset), Vector2.left, GroundDistance);
        RaycastHit2D rightCheck = Raycast(new Vector2(PlayerWidth, FootOffset), Vector2.right, GroundDistance);
        RaycastHit2D downCheck = Raycast(new Vector2(-PlayerWidth, FootOffset), Vector2.down, GroundDistance);
        RaycastHit2D upCheck = Raycast(new Vector2(PlayerWidth, FootOffset), Vector2.up, GroundDistance);
        if (downCheck&&(!leftCheck && !rightCheck))
        {
            _isGround = _canSpin = true;
            _isWall = false;
        }
        else if(leftCheck || rightCheck || upCheck)
        {
            _isWall = _canSpin = true;
            _isGround = false;
        }
        else
        {
            _isGround = false;
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

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player")&&Pv.IsMine)
        {
            PlayerController _playerController = other.gameObject.GetComponent<PlayerController>();
            if (!other.gameObject.GetComponent<PhotonView>().IsMine && other.gameObject.GetComponent<PlayerController>().IsCharge)
            {
                TakeDamage(_playerController.Damage, _playerController.BeElasticity, _playerController.DirX, _playerController.DirY);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Katada")&&Pv.IsMine)
        {
            Katada _katada = other.gameObject.GetComponent<Katada>();
            if (!_katada._pv.IsMine)
            {
                _katada._pv.RPC("DisableObj", RpcTarget.All);
                float x = Mathf.Cos(_katada.BeElasticityDir * Mathf.PI / 180);
                float y = Mathf.Sin(_katada.BeElasticityDir * Mathf.PI / 180);
                TakeDamage(_katada.KatadaDamage, _katada.KatadaBeElasticity,x,y);
            }
        }
    }

    public void BeBounce(float _elasticty,float _dirX, float _dirY)
    {
        Debug.Log(_elasticty);
        _rb.AddForce(_elasticty * new Vector2(_dirX, _dirY));
    }

    public void TakeDamage(float _damage,float _elasticty,float _bullletDirX,float _bullletDirY)
    {
        _rb.AddForce(_elasticty * new Vector2(_bullletDirX, _bullletDirY));
        Pv.RPC("Rpc_TakeDamage", RpcTarget.All, _damage);
    }

    [PunRPC]
    void Rpc_TakeDamage(float _damage)
    {
        PlayerHp -= _damage;
        if (PlayerHp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        _playerManager.Die();
    }

    [PunRPC]
    void Rpc_ChargeValue(bool _isCharge,float _elasticity,float _damage,float _dirX,float _dirY)
    {
        IsCharge = _isCharge;
        BeElasticity = _elasticity;
        Damage = _damage;
        DirX = _dirX;
        DirY = _dirY;
    }

    [PunRPC]
    void Rpc_ChangeBeFreeze(Vector3 _pos,Quaternion _dir)
    {
        IsBeFreeze = true;
        _originPos = _pos;
        _originDir = _dir;
        _rb.bodyType = RigidbodyType2D.Static;
        transform.position = _originPos;
        transform.rotation = _originDir;
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
        }
        else
        {
            _newPos = (Vector2)stream.ReceiveNext();
            //_newDir = (Quaternion)stream.ReceiveNext();
            _newDirZ = (float)stream.ReceiveNext();
            _newShootPointDir = (Quaternion)stream.ReceiveNext();
            _rb.velocity = (Vector2)stream.ReceiveNext();
            _rb.angularVelocity = (float)stream.ReceiveNext();

            float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime)) + (float)(PhotonNetwork.GetPing()*0.001f);
            _newPos += (_rb.velocity * lag);
            _newDirZ += (_rb.angularVelocity * lag);
        }
    }
}
