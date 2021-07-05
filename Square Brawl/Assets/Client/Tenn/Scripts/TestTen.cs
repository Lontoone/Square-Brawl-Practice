using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTen : MonoBehaviour
{
    private PlayerInputManager _inputAction;
    public Rigidbody2D _rb;
    public float TestForce;
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

    // Update is called once per frame
    public void PlayerFire1Down()
    {
        _rb.AddForce(TestForce * Vector3.right);
    }
}
