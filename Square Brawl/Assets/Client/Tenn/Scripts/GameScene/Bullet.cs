using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviour
{
    public float ShootSpeed;

    public GameObject ExploseEffectObj;

    private PhotonView _photonView;
    // Start is called before the first frame update
    void Start()
    {
        _photonView = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        /*if (!_photonView.IsMine)
        {
            return;
        }*/
        transform.Translate(Vector2.right* ShootSpeed * Time.deltaTime);
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
