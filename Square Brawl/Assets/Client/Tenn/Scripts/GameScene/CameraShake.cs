using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
	public Transform CamTransform;

	public float ShakeTime;
	public float ShakePower;
	public float DecreaseNumber;

	Vector3 OriginalPos;

	public static CameraShake instance;
	void Awake()
	{
		instance = this;
		if (CamTransform == null)
		{
			CamTransform = GetComponent(typeof(Transform)) as Transform;
		}
	}

	void OnEnable()
	{
		OriginalPos = CamTransform.localPosition;
	}

	public void SetShakeValue(float _time,float _power,float _decrease)
    {
		ShakeTime = _time;
		ShakePower = _power;
		DecreaseNumber = _decrease;
	}

	void Update()
	{
		if (ShakeTime > 0)
		{
			CamTransform.localPosition = OriginalPos + Random.insideUnitSphere * ShakePower;

			ShakePower = Mathf.MoveTowards(ShakePower, 0f, 0.5f * Time.deltaTime* DecreaseNumber);
			ShakeTime -= Time.deltaTime* DecreaseNumber;
		}
		else
		{
			ShakeTime = 0f;
			CamTransform.localPosition = OriginalPos;
		}
	}
}
