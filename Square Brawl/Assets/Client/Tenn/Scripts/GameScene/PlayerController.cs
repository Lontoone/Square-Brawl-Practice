using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
public class PlayerController : MonoBehaviour
{
    [HeaderAttribute("Player Setting")]
    private Rigidbody2D _rigidbody2D;

    public float MoveSpeed;
    public float DownSpeed;
    public float JumpForce;
    public float FlyForce;
    public float DownForce;
    public Vector2 _inputPos;

    private bool _canSpin;

    [HeaderAttribute("GroundCheck Setting")]
    public float FootOffset;
    public float GroundDistance;
    public float PlayerWidth;
    
    private bool _isGround;

    public LayerMask GroundLayer;



    [HeaderAttribute("Other Setting")]
    public Transform ShootDir;

    private Camera _camera;

    private PhotonView _photonView;

    private PlayerInputManager _inputAction;

    private void Awake()
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

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _photonView = GetComponent<PhotonView>();
        _camera = Camera.main;

        _canSpin = true;

        _inputAction.Player.Jump.performed += _ => PlayerJump();
        _inputAction.Player.Movement.performed += ctx => GetMovementValue(ctx.ReadValue<Vector2>());
        _inputAction.Player.MouseRotation.performed += ctx => MouseSpin(ctx.ReadValue<Vector2>());
        _inputAction.Player.GamePadRotation.performed += ctx => GamePadSpin(ctx.ReadValue<Vector2>());
        /*if (!_photonView.IsMine)
        {
            Destroy(_rigidbody2D);
        }*/
    }

    void Update()
    {
        /*if (!_photonView.IsMine) 
        {
            return;
        }*/

        //Player Spin
        if (_canSpin && _inputPos.x != 0)
        {
            if (_inputPos.x != 0 && _inputPos.y != 0)
            {
                Physics2D.maxRotationSpeed = 12;
            }
            else
            {
                Physics2D.maxRotationSpeed = 6;
            }
            _rigidbody2D.AddTorque(-5 * _inputPos.x);
            _canSpin = false;
        }

        GroundCheckEvent();//Is Grounding?
    }
    void FixedUpdate()
    {
       /* if (!_photonView.IsMine)
        {
            return;
        }*/

        PlayerMovement();
    }

    void PlayerMovement()
    {
        //Player Move
        _rigidbody2D.AddForce(MoveSpeed * new Vector2(_inputPos.x, 0)) ;

        if (_inputPos.y > 0)//Player Fly
        {
            _rigidbody2D.AddForce(FlyForce * new Vector2(0,_inputPos.y));
        }
        else if(_inputPos.y < 0)//Player Down
        {
            _rigidbody2D.AddForce(DownForce * new Vector2(0, _inputPos.y));
        }
    }

    void GetMovementValue(Vector2 diretion)
    {
        Vector2 _inputMovement = diretion;
        _inputPos = new Vector2(_inputMovement.x, _inputMovement.y);
    }

    private void PlayerJump()
    {
        if (_isGround)
        {
            _rigidbody2D.AddForce(JumpForce * Vector3.up);
        }
    }

    void MouseSpin(Vector2 _mousePos)
    {
        Vector2 _mouseWorldPos = _camera.ScreenToWorldPoint(_mousePos);
        Vector2 _targetDir = _mouseWorldPos - new Vector2(ShootDir.position.x, ShootDir.position.y);
        float _angle = Mathf.Atan2(_targetDir.y, _targetDir.x) * Mathf.Rad2Deg;
        ShootDir.rotation = Quaternion.Euler(new Vector3(0, 0, _angle));
    }

    void GamePadSpin(Vector2 _mousePos)
    {
        float _angle = Mathf.Atan2(_mousePos.y, _mousePos.x) * Mathf.Rad2Deg;
        ShootDir.rotation = Quaternion.Euler(new Vector3(0, 0, _angle));
    }

    //Ground Check
    void GroundCheckEvent()
    {
        RaycastHit2D leftCheck = Raycast(new Vector2(-PlayerWidth, FootOffset), Vector2.left, GroundDistance);
        RaycastHit2D rightCheck = Raycast(new Vector2(PlayerWidth, FootOffset), Vector2.right, GroundDistance);
        RaycastHit2D downCheck = Raycast(new Vector2(-PlayerWidth, FootOffset), Vector2.down, GroundDistance);
        RaycastHit2D upCheck = Raycast(new Vector2(PlayerWidth, FootOffset), Vector2.up, GroundDistance);
        if (leftCheck || rightCheck || downCheck|| upCheck)
        {
            _isGround = _canSpin = true;
        }
        else
        {
            _isGround = false;
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
}
