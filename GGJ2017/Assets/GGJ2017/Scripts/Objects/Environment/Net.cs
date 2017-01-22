using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Net : MonoBehaviour {

	public float springStrength = 1000.0f;
	public float minDist = 0.2f;
	public float maxDist = 1.0f;
	public float damper = 0.2f;

	// Use this for initialization
	void Start () {
		SpringJoint[] joints = GetComponentsInChildren<SpringJoint>();
		foreach (SpringJoint joint in joints) {
			joint.spring = springStrength;
			joint.minDistance = minDist;
			joint.maxDistance = maxDist;
			joint.damper = damper;

			Rigidbody rbody = joint.GetComponent<Rigidbody>();
			rbody.freezeRotation = true;
			joint.gameObject.tag = gameObject.tag;
		}
	}
}
