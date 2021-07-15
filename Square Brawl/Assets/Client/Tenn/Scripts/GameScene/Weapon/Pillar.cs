using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pillar : Grenade, IPoolObject
{
    private bool _isGrow;

    private float _growY;

    public float FootOffset;
    public float GroundDistance;
    public float PlayerWidth;

    public LayerMask GroundLayer;
    /* private float _growY
     {
         get { return test; }
         set
         {
             if (value >= 10)
                 test = 10;
             else
                 test = value;
         }
     }*/
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        if (_isGrow)
        {
            // _growY += 20*Time.deltaTime;
            _growY = Mathf.Lerp(transform.localScale.y, 13, 10 * Time.deltaTime);
            transform.localScale = new Vector3(transform.localScale.x, _growY, transform.localScale.z);
            StartCoroutine(Shorten());
        }
        else
        {
            _growY = Mathf.Lerp(transform.localScale.y, 1,  10* Time.deltaTime);
            transform.localScale = new Vector3(transform.localScale.x, _growY, transform.localScale.z);
        }

        //GroundCheckEvent();

    }

    protected override void FixedUpdate()
    {
        if (!isShoot)
        {
            _rb.AddForce(BulletSpeed * transform.right);
            transform.eulerAngles = Vector3.zero;
            isShoot = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            _rb.bodyType = RigidbodyType2D.Static;
            _isGrow = true;
        }
    }

    IEnumerator Shorten()
    {
        yield return new WaitForSeconds(3f);
        _isGrow = false;
    }

    //Ground Check
    void GroundCheckEvent()
    {
        RaycastHit2D leftCheck = Raycast(new Vector2(PlayerWidth, FootOffset), Vector2.left, GroundDistance);
        RaycastHit2D rightCheck = Raycast(new Vector2(PlayerWidth, FootOffset), Vector2.right, GroundDistance);
        RaycastHit2D downCheck = Raycast(new Vector2(PlayerWidth, FootOffset), Vector2.down, GroundDistance);
        RaycastHit2D upCheck = Raycast(new Vector2(PlayerWidth, FootOffset), Vector2.up, GroundDistance);
        if (leftCheck)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 270);
            _rb.bodyType = RigidbodyType2D.Static;
            _isGrow = true;
        }
        else if (rightCheck)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 90);
            _rb.bodyType = RigidbodyType2D.Static;
            _isGrow = true;
        }
        else if (downCheck)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
            _rb.bodyType = RigidbodyType2D.Static;
            _isGrow = true;
        }
        else if (upCheck)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 180);
            _rb.bodyType = RigidbodyType2D.Static;
            _isGrow = true;
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
