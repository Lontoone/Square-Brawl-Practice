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
    private Vector2 _inputPosX;
    private Vector2 _inputPosY;

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
        _inputAction.Player.Jump.performed += _ => PlayerJumpTest();
        _inputAction.Player.Movement.performed += ctx => PlayerMoveTest(ctx.ReadValue<Vector2>());
        //_inputAction.Player.Rotation.performed += ctx => PlayerRotationTest(ctx.ReadValue<Vector2>());
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
        PlayerRotation();

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
        _rigidbody2D.AddForce(MoveSpeed * _inputPosX);
        //Player Spin
        _rigidbody2D.AddTorque(-5*_inputPosX.x);

        if (_inputPosY.y > 0)//Player Fly
        {
            _rigidbody2D.AddForce(FlyForce * new Vector2(0,_inputPosY.y));
        }
        else if(_inputPosY.y < 0)//Player Down
        {
            _rigidbody2D.AddForce(DownForce * new Vector2(0, _inputPosY.y));
        }
    }

    /*void OnMovement(InputValue value)
    {
        Vector2 _inputMovement = value.Get<Vector2>();
        _inputPosX = new Vector2(_inputMovement.x, 0);
        _inputPosY = new Vector2(0, _inputMovement.y);
    }*/

    /*void OnJump(InputValue value)
    {
        if (_isGround)
        {
            _rigidbody2D.AddForce(JumpForce * Vector3.up);
        }
    }*/

    //Ground Check
    void GroundCheckEvent()
    {
        RaycastHit2D leftCheck = Raycast(new Vector2(-PlayerWidth, FootOffset), Vector2.left, GroundDistance);
        RaycastHit2D rightCheck = Raycast(new Vector2(PlayerWidth, FootOffset), Vector2.right, GroundDistance);
        RaycastHit2D downCheck = Raycast(new Vector2(-PlayerWidth, FootOffset), Vector2.down, GroundDistance);
        RaycastHit2D upCheck = Raycast(new Vector2(PlayerWidth, FootOffset), Vector2.up, GroundDistance);
        if (leftCheck || rightCheck || downCheck|| upCheck)
        {
            _isGround = true;
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




    void PlayerMoveTest(Vector2 diretion)
    {
        Vector2 _inputMovement = diretion;
        _inputPosX = new Vector2(_inputMovement.x, 0);
        _inputPosY = new Vector2(0, _inputMovement.y);
    }
    private void PlayerJumpTest()
    {
        if (_isGround)
        {
            _rigidbody2D.AddForce(JumpForce * Vector3.up);
        }
    }

    void PlayerRotation()
    {
        Vector2 mousePos = _inputAction.Player.Rotation.ReadValue<Vector2>();
        Vector3 mouseWorldPos = _camera.ScreenToWorldPoint(mousePos);
        Vector3 targetDir = mouseWorldPos - ShootDir.localPosition;
        float angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;
        ShootDir.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

    }
    
}
