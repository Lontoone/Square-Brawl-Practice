using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGunBullet : MonoBehaviour
{
    public GameObject ExploreParticle;
    public Transform ExplorePoint;

    public int Damage;

    public float mSpeed = 100f;

    Vector3 mPrevPos;

    // Start is called before the first frame update
    void Start()
    {
        mPrevPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        mPrevPos = transform.position;

        transform.Translate(0.0f, 0.0f, mSpeed * Time.deltaTime);

        RaycastHit[] hits = Physics.RaycastAll(new Ray(mPrevPos, (transform.position - mPrevPos).normalized), (transform.position - mPrevPos).magnitude);

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.gameObject.name == "Player(Clone)")
            {
                Debug.Log("OK");
            }
        }
    }

}
