using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class PlayerController : MonoBehaviour
{
    [HeaderAttribute("Player Setting")]
    private Rigidbody2D _rigidbody2D;

    public float MoveSpeed;
    public float DownSpeed;
    public float JumpForce;
    private Vector3 _rotation;

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

        if (!_photonView.IsMine)
        {
            Destroy(_rigidbody2D);
        }
    }

    void Update()
    {
        if (!_photonView.IsMine) 
        {
            return;
        }
        PlayerJump();
        GroundCheckEvent();//Is Grounding?
    }
    void FixedUpdate()
    {
        if (!_photonView.IsMine)
        {
            return;
        }
        PlayerMoving();
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

    //Ground Check
    void GroundCheckEvent()
    {
        RaycastHit2D leftCheck = Raycast(new Vector2(-PlayerWidth, FootOffset), Vector2.down, GroundDistance);
        RaycastHit2D rightCheck = Raycast(new Vector2(PlayerWidth, FootOffset), Vector2.down, GroundDistance);
        if (leftCheck || rightCheck)
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
}
