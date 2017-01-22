using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour {

	public GameObject ball;
	public bool look = true;
	public float lookSpeed = 3.0f;
	public bool move = true;
	public float moveSpeed = 3.0f;
	
	// Update is called once per frame
	void FixedUpdate () {
		if (ball != null) {
			Vector3 ballPos = ball.transform.position;
			if (move) {
				Vector3 targetPosition = new Vector3(ballPos.x, transform.position.y, transform.position.z);
				transform.position = Vector3.Lerp(transform.position, targetPosition, Time.fixedDeltaTime * moveSpeed);
			}
			if (look) {
				Vector3 lookAt = ballPos - transform.position;
				lookAt = Vector3.ProjectOnPlane(lookAt, Vector3.up);
				Quaternion targetRotation = Quaternion.LookRotation(lookAt, Vector3.up);
				transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * lookSpeed);
			}
		}
	}
}
