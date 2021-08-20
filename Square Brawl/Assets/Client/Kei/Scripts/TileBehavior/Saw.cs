using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : MonoBehaviour
{
    private float speed = 100;
    /*
    private TileCell cell;
    private Transform cellHind;
    public void Start()
    {
        cell = GetComponent<TileCell>();
        cellHind = cell.transform.GetChild(0);
    }*/
    public void FixedUpdate()
    {
        //cellHind.Rotate(Vector3.forward, speed * Time.fixedDeltaTime);
        transform.Rotate(Vector3.forward, speed * Time.fixedDeltaTime);
    }
    private void OnDestroy()
    {
        transform.eulerAngles = Vector3.zero;
    }

    /*private void OnCollisionEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
            if (playerController.Pv.IsMine)
            {
                Debug.Log("OK");
                Vector2 dir = other.gameObject.transform.position - transform.position;
                playerController.DamageEvent(20, 1500, dir.x, dir.y, new Vector3(0.6f, 0.3f, 1));
            }
        }
    }*/
}
