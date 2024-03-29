using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Polybrush;
using Unity.Mathematics;
using Unity.VisualScripting;

public class AINavigation : MonoBehaviour
{
	private Coroutine moving;
	private Transform travelTarget;


	[SerializeField] private float speed = 5;
	[SerializeField] private float lookSpeed = 5;

	void Start()
	{
		travelTarget = GameObject.Find("NavPoint").transform;
	}

	void Update()
	{
		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(travelTarget.position - transform.position), lookSpeed * Time.deltaTime);

		transform.position += transform.forward * speed * Time.deltaTime;
	}

	public IEnumerator DelayCoroutine(float delay)
	{
		float restoreSpeed = speed;
		float restoreLookSpeed = lookSpeed;
		speed = 0;
		lookSpeed = 0;
		yield return new WaitForSeconds(delay);
		speed = restoreSpeed;
		lookSpeed = restoreLookSpeed;
	}
}
