using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Umbrella : MonoBehaviour {

	Vector3 targetUp;
	Rigidbody rb;

	public float strength = 30.0f;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
		rb.centerOfMass = Vector3.zero;
		targetUp = transform.up;
	}
	
	// Update is called once per frame
	void Update () {
		var rot = Quaternion.FromToRotation(transform.up, Vector3.forward);
		rb.AddTorque(new Vector3(rot.x, rot.y, rot.z)*strength);
	}
}
