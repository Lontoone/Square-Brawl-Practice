using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : MonoBehaviour
{
    public float speed = 100;
    public void FixedUpdate()
    {
        transform.Rotate(Vector3.forward, speed * Time.fixedDeltaTime);
    }
}
