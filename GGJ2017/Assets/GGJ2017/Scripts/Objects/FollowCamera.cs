using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour {

	public GameObject ball;
	public float lookWeight = 0f;
	public float lookSpeed = 3.0f;
	public float moveWeight = 0f;
	public float moveSpeed = 3.0f;

	private Vector3 startPos;

	void Start()
	{
		startPos = transform.position;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (ball != null) {
			Vector3 ballPos = ball.transform.position;
			if (moveWeight != 0f) {
				float targetY = Mathf.Max(startPos.y, ballPos.y * moveWeight - 5.0f);
				Vector3 targetPosition = new Vector3(ballPos.x * moveWeight, targetY, transform.position.z);
				transform.position = Vector3.Lerp(transform.position, targetPosition, Time.fixedDeltaTime * moveSpeed);
			}
			if (lookWeight != 0f) {
				Vector3 lookAt = (ballPos * lookWeight - transform.position);
				lookAt = Vector3.ProjectOnPlane(lookAt.normalized, Vector3.up);
				Quaternion targetRotation = Quaternion.LookRotation(lookAt.normalized, Vector3.up);
				transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * lookSpeed);
			}
		}
	}
}
