﻿using UnityEngine;

public class TP_Camera : MonoBehaviour
{

	public static TP_Camera Instance;

	public Transform TargetLookAt;
	public float Distance = 5f;
	public float DistanceMin = 3f;
	public float DistanceMax = 10f;
	public float DistanceSmooth = 0.05f;
	public float X_MouseSensitivity = 5f;
	public float Y_MouseSensitivity = 5f;
	public float MouseWheelSensitivity = 5f;
	public float X_Smooth = 0.05f;
	public float Y_Smooth = 0.1f;
	public float Y_MinLimit = -40f;
	public float Y_MaxLimit = 80f;

	private float mouseX = 0f;
	private float mouseY = 0f;
	private float velX = 0f;
	private float velY = 0f;
	private float velZ = 0f;
	private float velDistance = 0f;
	private float startDistance = 0f;
	private Vector3 desiredPosition = Vector3.zero;
	private Vector3 position = Vector3.zero;
	private float desiredDistance = 0f;

	void Awake ()
	{
		Instance = this;	
	}

	void Start ()
	{
		Distance = Mathf.Clamp (Distance, DistanceMin, DistanceMax);
		startDistance = Distance;
		Reset ();
	}

	void LateUpdate ()
	{
		if (TargetLookAt == null) {
			return;
		}

		HandlePlayerInput ();
		CalculateDesiredPosition ();
		UpdatePosition ();
	}

	void HandlePlayerInput ()
	{
		var deadZone = 0.01f;

		if (Input.GetMouseButton (1)) {
			mouseX += Input.GetAxis ("Mouse X") * X_MouseSensitivity;
			mouseY -= Input.GetAxis ("Mouse Y") * Y_MouseSensitivity;
		}

		mouseY = ClampAngle (mouseY, Y_MinLimit, Y_MaxLimit);

		if (Input.GetAxis ("Mouse ScrollWheel") < -deadZone || Input.GetAxis ("Mouse ScrollWheel") > deadZone) {
			desiredDistance = Mathf.Clamp (
				Distance - Input.GetAxis ("Mouse ScrollWheel") * MouseWheelSensitivity,
				DistanceMin, DistanceMax);
		}
	}

	void CalculateDesiredPosition ()
	{
		Distance = Mathf.SmoothDamp (Distance, desiredDistance, ref velDistance, DistanceSmooth);

		desiredPosition = CalculatePosition (mouseY, mouseX, Distance);
	}

	Vector3 CalculatePosition (float rotationX, float rotationY, float distance)
	{
		Vector3 direction = new Vector3 (0, 0, -distance);
		Quaternion rotation = Quaternion.Euler (rotationX, rotationY, 0);

		return TargetLookAt.position + rotation * direction;
	}

	void UpdatePosition ()
	{
		var posX = Mathf.SmoothDamp (position.x, desiredPosition.x, ref velX, X_Smooth);
		var posY = Mathf.SmoothDamp (position.y, desiredPosition.y, ref velY, Y_Smooth);
		var posZ = Mathf.SmoothDamp (position.z, desiredPosition.z, ref velZ, X_Smooth);

		position = new Vector3 (posX, posY, posZ);

		transform.position = position;

		transform.LookAt (TargetLookAt);
	}

	public void Reset ()
	{
		mouseX = 0;
		mouseY = 10;
		Distance = startDistance;
		desiredDistance = Distance;
	}

	public static void UseExistingOrCreateNewMainCamera (GameObject target)
	{
		GameObject tempCamera;
		TP_Camera myCamera;

		if (Camera.main != null) {
			tempCamera = Camera.main.gameObject;
		} else {
			tempCamera = new GameObject ("Main Camera");
			tempCamera.AddComponent <Camera> ();
			tempCamera.tag = "MainCamera";
		}

		myCamera = tempCamera.GetComponent<TP_Camera> ();
		if (myCamera == null) {
			tempCamera.AddComponent <TP_Camera> ();

			myCamera = tempCamera.GetComponent<TP_Camera> ();
		}

		myCamera.TargetLookAt = target.transform;

	}

	private static float ClampAngle (float angle, float min, float max)
	{
		do {
			if (angle < -360) {
				angle += 360;
			}

			if (angle > 360) {
				angle -= 360;
			}
		} while (angle < -360 || angle > 360);

		return Mathf.Clamp (angle, min, max);
	}
}
