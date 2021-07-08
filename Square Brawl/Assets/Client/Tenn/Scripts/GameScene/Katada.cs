using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Katada : MonoBehaviour
{
    public float Speed;
    public float Damage;
    public float BeShootElasticity;
    public bool IsKatadaReverse;
    void Start()
    {
        if (IsKatadaReverse)
        {
            transform.eulerAngles = new Vector3(0, 0, 45);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, -45);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (IsKatadaReverse)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0,0,-45), Time.deltaTime * Speed);
            Debug.Log(transform.eulerAngles.z);
        }
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 45), Time.deltaTime * Speed);
            Debug.Log(transform.eulerAngles.z);
        }

    }
}
