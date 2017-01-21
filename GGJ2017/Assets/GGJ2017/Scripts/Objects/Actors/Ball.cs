using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    public float m_initialImpulse = 10.0f;
    public float m_contactImpulse = 1.0f;

    public Rigidbody m_rigidbody;

    protected Vector3 m_originalPosition;
    protected Quaternion m_originalRotation;

	// Use this for initialization
	void Start () {

        m_originalPosition = transform.position;
        m_originalRotation = transform.rotation;
        Reset();
    }
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.Space))
        {
            Reset();
        }
	}

    public void Reset()
    {
        transform.position = m_originalPosition;
        transform.rotation = m_originalRotation;
        m_rigidbody.velocity = Vector3.zero;
        m_rigidbody.angularVelocity = Vector3.zero;
        m_rigidbody.AddForce(transform.forward * m_initialImpulse, ForceMode.Impulse);
    }

    
    public void OnCollisionExit(Collision c)
    {
        if(c.collider.gameObject.layer == LayerMask.NameToLayer("Character"))
        {
            m_rigidbody.AddForce(Vector3.up * m_contactImpulse, ForceMode.Impulse);
        }
    }
    
}
