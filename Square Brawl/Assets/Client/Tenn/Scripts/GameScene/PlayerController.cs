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

    private PhotonView _photonView;

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _photonView = GetComponent<PhotonView>();

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
        //PlayerJump();
        GroundCheckEvent();//Is Grounding?
    }
    void FixedUpdate()
    {
       /* if (!_photonView.IsMine)
        {
            return;
        }*/
        //PlayerMoving();

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

    void OnMovement(InputValue value)
    {
        Vector2 _inputMovement = value.Get<Vector2>();
        _inputPosX = new Vector2(_inputMovement.x, 0);
        _inputPosY = new Vector2(0, _inputMovement.y);
    }

    void OnJump(InputValue value)
    {
        if (_isGround)
        {
            _rigidbody2D.AddForce(JumpForce * Vector3.up);
        }
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




    void PlayerMoving()
    {
        if (Input.GetKey(KeyCode.A))
        {
            _rigidbody2D.AddForce(MoveSpeed * Vector2.left);
            _rigidbody2D.AddTorque(5);
            Debug.Log(_rigidbody2D.angularVelocity);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            _rigidbody2D.AddForce(MoveSpeed * Vector2.right);
            _rigidbody2D.AddTorque(-5);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            _rigidbody2D.AddForce(DownSpeed * Vector2.down);
        }
    }

    void PlayerJump()
    {
        if (Input.GetKeyDown(KeyCode.W) && _isGround)
        {
            _rigidbody2D.AddForce(JumpForce * Vector3.up);
        }
        else if (Input.GetKey(KeyCode.W) && !_isGround)
        {
            _rigidbody2D.AddForce(3f * Vector3.up);
        }
    }
}
