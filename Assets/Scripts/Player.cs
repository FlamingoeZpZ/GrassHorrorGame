using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using Task = System.Threading.Tasks.Task;

public class Player : MonoBehaviour
{
	private Vector3 moveDirection;

	private Rigidbody rb;
	[SerializeField] private float moveSpeed = 6;
	[SerializeField] private float maxSpeed = 6;
	[SerializeField] private float yawSpeed;
	[SerializeField] private float pitchSpeed;
	private GPT_ImplGrassPainter gen;
		
	// Start is called before the first frame update
	void Awake()
	{
		PlayerControls.Init(this);
		rb = transform.parent.GetComponent<Rigidbody>();
		gen = GetComponent<GPT_ImplGrassPainter>();
		rb.maxLinearVelocity = maxSpeed;
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		HandleMovement();
	}

	void HandleMovement()
	{
		//Simple translation Movement
		print(moveDirection);
		rb.AddForce(transform.rotation * (moveDirection * moveSpeed));
	}

	public void SetMoveDirection(Vector3 readValue)
	{
		moveDirection = readValue;
	}

	public void SetWateringState(bool readValueAsButton)
	{
		gen.isWatering = readValueAsButton;
		//Also begin and/or end VFX.
	}


	public float minPitchAngle = -89f; // Minimum pitch angle (looking down)
	public float maxPitchAngle = 89f;  // Maximum pitch angle (looking up)

	public void MoveCamera(Vector2 readValue)
	{
		float dt = Time.deltaTime;

		// Calculate the target pitch and yaw rotations
		Quaternion currentRotation = transform.rotation;
		Quaternion yawRotation = Quaternion.AngleAxis(readValue.x * yawSpeed * dt, Vector3.up);
		Quaternion pitchRotation = Quaternion.AngleAxis(-readValue.y * pitchSpeed * dt, Vector3.right);

		// Apply the rotations
		Quaternion newRotation = currentRotation * yawRotation * pitchRotation;

		// Get the pitch angle from the new rotation
		float pitchAngle = QuaternionToPitchAngle(newRotation);

		// Clamp the pitch angle to prevent looking too far up or down
		float clampedPitchAngle = Mathf.Clamp(pitchAngle, minPitchAngle, maxPitchAngle);

		// Calculate the new rotation with clamped pitch angle
		Quaternion finalRotation = Quaternion.Euler(clampedPitchAngle, newRotation.eulerAngles.y, 0);

		// Apply the final rotation
		transform.rotation = finalRotation;
	}

	private float QuaternionToPitchAngle(Quaternion q)
	{
		float sinPitch = 2 * (q.w * q.x - q.y * q.z);
		return Mathf.Asin(Mathf.Clamp(sinPitch, -1, 1)) * Mathf.Rad2Deg;
	}





}
