using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTen : MonoBehaviour
{
    private PlayerInputManager _inputAction;
    public Rigidbody2D _rb;
    public float TestForce;
    public float ShootSpeed;
    public Vector3 _mPrevPos;
    // Start is called before the first frame update
    void Awake()
    {
        _inputAction = new PlayerInputManager();
        
    }

    private void Start()
    {
        _inputAction.Player.Fire1.performed += _ => PlayerFire1Down();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _mPrevPos = transform.position+new Vector3(transform.localScale.x/2,0,0);

        RaycastHit2D[] hits = Physics2D.RaycastAll(_mPrevPos, (transform.position - _mPrevPos).normalized, (transform.position - _mPrevPos).magnitude);

        //Debug.DrawLine(transform.position, _mPrevPos,Color.red, (transform.position - _mPrevPos).magnitude);

        for (int i = 0; i < hits.Length; i++)
        {

            if (hits[i].collider.gameObject.CompareTag("Ground"))
            {
                Debug.Log(hits[i].collider.gameObject.name);
            }
        }
    }

    private void FixedUpdate()
    {
        _rb.velocity = transform.right * ShootSpeed;
    }

    // Update is called once per frame
    public void PlayerFire1Down()
    {
        _rb.AddForce(TestForce * Vector3.right);
    }
}
