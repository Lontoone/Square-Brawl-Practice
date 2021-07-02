using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviour
{
    public float ShootSpeed;
    public float ShootDamage;
    public float BulletScaleValue;
    public bool IsDontShootStraight;

    public GameObject ExploseEffectObj;

    private PhotonView _pv;
    // Start is called before the first frame update
    void Start()
    {
        _pv = GetComponent<PhotonView>();

        //gameObject.transform.localScale = new Vector3(BulletScaleValue, BulletScaleValue, BulletScaleValue);
        if (IsDontShootStraight)
        {
            transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z + Random.Range(-10, 11));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!_pv.IsMine)
        {
            return;
        }

        transform.Translate(Vector2.right * ShootSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            Instantiate(ExploseEffectObj, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
