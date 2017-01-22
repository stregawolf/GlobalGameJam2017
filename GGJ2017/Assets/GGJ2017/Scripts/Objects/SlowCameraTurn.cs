using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowCameraTurn : MonoBehaviour {

	public float speed = 1.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.rotation = Quaternion.Euler(0, Time.time * speed, 0);
	}
}
