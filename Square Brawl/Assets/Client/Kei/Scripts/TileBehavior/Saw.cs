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
}
