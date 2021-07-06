using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using System.IO;
public class PlayerController : MonoBehaviour
{
    [HeaderAttribute("Player Setting")]
    private Rigidbody2D _rigidbody2D;

    public float MoveSpeed;//Player Move Speed
    public float JumpForce;//Player Jump Force
    public float FlyForce;//Player Fly Force
    public float DownForce;//Player Down Force
    public float SpinForce;//Player Spin Force
    public float SpinInGroundForce;//Player Spin In Ground Force
    public float Damage;
    public float PlayerHp;
    public Vector2 _inputPos;//Keyboard Input Pos

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
    public Transform ShootSpinMidPos;//ShootSpinMid Object Pos
    public Transform ShootDir;//Shoot Point

    public GameObject ShootPointObj;//Shoot Point Prefab
    public GameObject _shootObj;

    private Camera _camera;

    private PhotonView _pv;

    private PlayerInputManager _inputAction;

    private PlayerManager _playerManager;
    private void Awake()
    {
        _inputAction = new PlayerInputManager();
        _pv = GetComponent<PhotonView>();
        _playerManager = PhotonView.Find((int)_pv.InstantiationData[0]).GetComponent<PlayerManager>();
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
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _camera = Camera.main;

        _shootObj = PhotonView.Find((int)_pv.InstantiationData[0]).GetComponent<PlayerManager>()._shootObj;
        //GameObject _ShootObj = Instantiate(ShootPointObj, Vector3.zero, Quaternion.identity);
        if (_pv.IsMine)
        {
            ShootSpinMidPos = _shootObj.transform;
            ShootDir = _shootObj.transform.GetChild(0);

            _inputAction.Player.Jump.performed += _ => PlayerJumpDown();
            _inputAction.Player.Jump.canceled += _ => PlayerJumpUp();
            _inputAction.Player.Movement.performed += ctx => GetMovementValue(ctx.ReadValue<Vector2>());
            _inputAction.Player.MouseRotation.performed += ctx => MouseSpin(ctx.ReadValue<Vector2>());
            _inputAction.Player.GamePadRotation.performed += ctx => GamePadSpin(ctx.ReadValue<Vector2>());
        }
        else
        {
            Destroy(_rigidbody2D);
        }

        _canSpin = true;
    }

    void Update()
    {
        if (!_pv.IsMine) 
        {
           return;
        }

        GroundCheckEvent();//Is Grounding?
    }
    void FixedUpdate()
    {
        if (!_pv.IsMine)
        {
            return;
        }

        PlayerMovement();//Player Move

        PlayerSpin();//Player Spin
    }

    void PlayerMovement()
    {
        //Player Move
        _rigidbody2D.AddForce(MoveSpeed * new Vector2(_inputPos.x, 0)) ;

        if (_inputPos.y > 0&&!_isWall&&!_isGround)//Player Fly
        {
            _rigidbody2D.AddForce(FlyForce * new Vector2(0,_inputPos.y));
        }
        else if(_inputPos.y < 0&&!_isGround)//Player Down
        {
            _rigidbody2D.AddForce(DownForce * new Vector2(0, _inputPos.y));
        }

        if ((_isGround || _isWall)&&_isJump)
        {
            _rigidbody2D.AddForce(JumpForce * Vector3.up);
            StartCoroutine(IsJumpRecover());
        }
    }

    IEnumerator IsJumpRecover()
    {
        _isJump = false;
        yield return new WaitForSeconds(0.3f);
        _isJump = true ;
    }

    void GetMovementValue(Vector2 diretion)
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
            if (_inputPos.x != 0 && _isCheckSpin)
            {
                Physics2D.maxRotationSpeed = 30;
            }
            else
            {
                Physics2D.maxRotationSpeed = 6;
            }

            if (_isGround && !_isCheckSpin)
            {
                _rigidbody2D.AddTorque(SpinInGroundForce * _inputPos.x);
                //Debug.Log("IsGround");
            }
            else if (_isWall)
            {
                _rigidbody2D.AddTorque(SpinInGroundForce * _inputPos.x);
                //Debug.Log("IsWall");
            }
            else if (_isGround && _isCheckSpin)
            {
                _rigidbody2D.AddTorque(SpinForce * _inputPos.x);
                _isCheckSpin = false;
                //Debug.Log("IsGroundJump");
            }
            else if (!_isWall&&!_isGround)
            {
                _rigidbody2D.AddTorque(SpinForce * _inputPos.x);
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
        Vector2 _mouseWorldPos = _camera.ScreenToWorldPoint(_mousePos);
        Vector2 _targetDir = _mouseWorldPos - new Vector2(ShootSpinMidPos.position.x, ShootSpinMidPos.position.y);
        float _angle = Mathf.Atan2(_targetDir.y, _targetDir.x) * Mathf.Rad2Deg;
        ShootSpinMidPos.rotation = Quaternion.Euler(new Vector3(0, 0, _angle));
    }

    void GamePadSpin(Vector2 _mousePos)
    {
        if (_mousePos.x >= 0.3f || _mousePos.x <= -0.3f|| _mousePos.y >= 0.3f || _mousePos.y <= -0.3f)
        {
            float _angle = Mathf.Atan2(_mousePos.y, _mousePos.x) * Mathf.Rad2Deg;
            ShootSpinMidPos.rotation = Quaternion.Euler(new Vector3(0, 0, _angle));
        }
    }

    public void PlayerRecoil(float _shootRecoil)
    {
        float x = Mathf.Cos(ShootSpinMidPos.eulerAngles.z * Mathf.PI / 180);
        float y = Mathf.Sin(ShootSpinMidPos.eulerAngles.z * Mathf.PI / 180);

        _rigidbody2D.AddForce(-_shootRecoil * new Vector2(x, y));
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            Bullet _bullet = other.gameObject.GetComponent<Bullet>();
            if (!_bullet._pv.IsMine)
            {
                if (_pv.IsMine)
                {
                    _bullet._pv.RPC("DisableObj", RpcTarget.All);
                    float x = Mathf.Cos(other.gameObject.transform.eulerAngles.z * Mathf.PI / 180);
                    float y = Mathf.Sin(other.gameObject.transform.eulerAngles.z * Mathf.PI / 180);
                    _rigidbody2D.AddForce(other.gameObject.GetComponent<Bullet>().BeShootElasticity * new Vector2(x, y));
                    TakeDamage(other.gameObject.GetComponent<Bullet>().ShootDamage);
                   // _bullet._pv.RPC("DisableObj", RpcTarget.All);
                }
                //other.gameObject.SetActive(false);
            }
        }
    }

    public void TakeDamage(float _damage)
    {
        _pv.RPC("Rpc_TakeDamage", RpcTarget.All, _damage);
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
}
