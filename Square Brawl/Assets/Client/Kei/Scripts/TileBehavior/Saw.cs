using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : MonoBehaviour
{
    public float speed = 100;
    private TileCell cell;
    private Transform cellHind;
    public void Start()
    {
        cellHind = cell.transform.GetChild(0);
    }
    public void FixedUpdate()
    {
        cellHind.Rotate(Vector3.forward, speed * Time.fixedDeltaTime);
    }
}
