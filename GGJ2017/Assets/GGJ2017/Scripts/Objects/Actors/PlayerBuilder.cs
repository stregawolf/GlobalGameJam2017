using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterControls))]
public class PlayerBuilder : MonoBehaviour {

	public int bodySegments = 5;
	public int armSegments = 5;
	public int bodySegmentForArms = 4;

	public GameObject bodySegment;
	public GameObject baseSegment;
	public GameObject armSegment;

	public Vector3 bodySegmentScale = Vector3.one;
	public Vector3 armSegmentScale = Vector3.one;

	public AnimationCurve bodyCurve;
	public AnimationCurve armCurve;

	public SpringJoint springSettings;

	private CharacterControls controls;

	void Awake()
	{
		controls = gameObject.GetComponent<CharacterControls>();
		BuildPlayer();
	}

	void BuildPlayer()
	{
		GameObject armLink = null;
		GameObject lastSegment = null;
		// Build body
		GameObject bodyChain = new GameObject("Body Chain");
		bodyChain.transform.parent = transform;
		bodyChain.transform.localPosition = Vector3.zero;
		bodyChain.transform.localRotation = Quaternion.identity;
		bodyChain.transform.localScale = Vector3.one;
		for (int i = bodySegments - 1; i >= 0; i--) {
			GameObject currentSegment = Instantiate(i == 0 ? baseSegment : bodySegment);
			currentSegment.name = "Body Segment(" + i + ")";
			currentSegment.transform.parent = bodyChain.transform;
			currentSegment.transform.localRotation = Quaternion.identity;
			Vector3 scale = bodySegmentScale;
			if (bodyCurve != null) {
				scale *= bodyCurve.Evaluate((float)i / (float)bodySegments);
			}
			currentSegment.transform.localScale = scale;
			currentSegment.transform.localPosition = Vector3.up * bodySegmentScale.y * i;
			Rigidbody rb = currentSegment.GetComponent<Rigidbody>();
			rb.isKinematic = i == 0;

			if (lastSegment != null) {
				SpringJoint spring = lastSegment.AddComponent<SpringJoint>();
				CopySpring(spring, springSettings);
				spring.connectedBody = rb;
				Physics.IgnoreCollision(lastSegment.GetComponentInChildren<Collider>(), currentSegment.GetComponentInChildren<Collider>());
			} else {
				controls.m_forcePoint = controls.m_head = currentSegment.GetComponent<Rigidbody>();
			}

			if (i+1 == bodySegmentForArms)
				armLink = currentSegment;

			lastSegment = currentSegment;
		}

		if (armLink == null || armSegments == 0)
			return;

		// Build right arm
		lastSegment = null;
		GameObject rightArmChain = new GameObject("Right Arm Chain");
		rightArmChain.transform.parent = armLink.transform;
		rightArmChain.transform.localPosition = Vector3.zero;
		rightArmChain.transform.localRotation = Quaternion.Euler(0, 0, -90);
		rightArmChain.transform.localPosition= armLink.transform.right / bodySegmentScale.x * 0.5f;
		rightArmChain.transform.parent = transform;
		rightArmChain.transform.localScale = Vector3.one;
		for (int i = armSegments - 1; i >= 0; i--) {
			GameObject currentSegment = Instantiate(armSegment);
			currentSegment.name = "Arm Segment(" + i + ")";
			currentSegment.transform.parent = rightArmChain.transform;
			currentSegment.transform.localRotation = Quaternion.identity;

			Vector3 scale = armSegmentScale;
			if (armCurve != null) {
				scale *= armCurve.Evaluate((float)i / (float)armSegments);
			}
			currentSegment.transform.localScale = scale;
			currentSegment.transform.localPosition = Vector3.up * armSegmentScale.y * i;
			Rigidbody rb = currentSegment.GetComponent<Rigidbody>();

			if (lastSegment != null) {
				SpringJoint spring = lastSegment.AddComponent<SpringJoint>();
				CopySpring(spring, springSettings);
				spring.connectedBody = rb;
				Physics.IgnoreCollision(lastSegment.GetComponentInChildren<Collider>(), currentSegment.GetComponentInChildren<Collider>());
			} else {
				controls.m_rightArm = currentSegment.GetComponent<Rigidbody>();
			}
			if (i==0) {
				SpringJoint spring = currentSegment.AddComponent<SpringJoint>();
				spring.connectedBody = armLink.GetComponent<Rigidbody>();
				Physics.IgnoreCollision(armLink.GetComponentInChildren<Collider>(), currentSegment.GetComponentInChildren<Collider>());
				CopySpring(spring, springSettings);
			}
			lastSegment = currentSegment;
		}

		// Build right arm
		lastSegment = null;
		GameObject leftArmChain = new GameObject("Left Arm Chain");
		leftArmChain.transform.parent = armLink.transform;
		leftArmChain.transform.localPosition = Vector3.zero;
		leftArmChain.transform.localRotation = Quaternion.Euler(0, 0, 90);
		leftArmChain.transform.localPosition= -armLink.transform.right / bodySegmentScale.x * 0.5f;
		leftArmChain.transform.parent = transform;
		leftArmChain.transform.localScale = Vector3.one;
		for (int i = armSegments - 1; i >= 0; i--) {
			GameObject currentSegment = Instantiate(armSegment);
			currentSegment.name = "Arm Segment(" + i + ")";
			currentSegment.transform.parent = leftArmChain.transform;
			currentSegment.transform.localRotation = Quaternion.identity;

			Vector3 scale = armSegmentScale;
			if (armCurve != null) {
				scale *= armCurve.Evaluate((float)i / (float)armSegments);
			}
			currentSegment.transform.localScale = scale;
			currentSegment.transform.localPosition = Vector3.up * armSegmentScale.y * i;
			Rigidbody rb = currentSegment.GetComponent<Rigidbody>();

			if (lastSegment != null) {
				SpringJoint spring = lastSegment.AddComponent<SpringJoint>();
				CopySpring(spring, springSettings);
				spring.connectedBody = rb;
				Physics.IgnoreCollision(lastSegment.GetComponentInChildren<Collider>(), currentSegment.GetComponentInChildren<Collider>());
			} else {
				controls.m_leftArm = currentSegment.GetComponent<Rigidbody>();
			}

			if (i==0) {
				SpringJoint spring = currentSegment.AddComponent<SpringJoint>();
				spring.connectedBody = armLink.GetComponent<Rigidbody>();
				Physics.IgnoreCollision(armLink.GetComponentInChildren<Collider>(), currentSegment.GetComponentInChildren<Collider>());
				CopySpring(spring, springSettings);
			}
			lastSegment = currentSegment;
		}
	}

	void CopySpring(SpringJoint spring, SpringJoint source)
	{
		spring.autoConfigureConnectedAnchor = true;
		spring.spring = source.spring;
		spring.minDistance = source.minDistance;
		spring.maxDistance = source.maxDistance;
		spring.damper = source.damper;
		spring.tolerance = source.tolerance;
		spring.breakForce = source.breakForce;
		spring.breakTorque = source.breakTorque;
	}
}
