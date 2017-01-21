using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class CharacterControls : MonoBehaviour {

    public XboxController controller;

    public Rigidbody m_head;
    public Rigidbody m_leftArm;
    public Rigidbody m_rightArm;
    public Rigidbody m_forcePoint;

    public float m_passiveForceHead = 50.0f;
    public float m_passiveForceArm = 25.0f;

    public float m_force = 50.0f;
    public float m_armForce = 20.0f;

    public KeyCode m_upkey = KeyCode.UpArrow;
    public KeyCode m_downKey = KeyCode.DownArrow;
    public KeyCode m_leftKey = KeyCode.LeftArrow;
    public KeyCode m_rightKey = KeyCode.RightArrow;
    
    protected void FixedUpdate()
    {
        m_head.AddForce(Vector3.up * m_passiveForceHead);
		if(m_leftArm != null)
	        m_leftArm.AddForce((Vector3.left + Vector3.up).normalized * m_passiveForceArm);
		if(m_rightArm != null)
	        m_rightArm.AddForce((Vector3.right + Vector3.up).normalized * m_passiveForceArm);

        Vector3 dir = Vector3.zero;
        if (XCI.GetNumPluggedCtrlrs() > 0 && XCI.IsPluggedIn((int)controller))
        {
            dir.x = XCI.GetAxis(XboxAxis.LeftStickX, controller);
            dir.y = XCI.GetAxis(XboxAxis.LeftStickY, controller);
        }
        else // Fallback to KB
        {
            if (Input.GetKey(m_upkey))
            {
                dir += Vector3.up;
            }
            if (Input.GetKey(m_leftKey))
            {
                dir += Vector3.left;
            }
            if (Input.GetKey(m_rightKey))
            {
                dir += Vector3.right;
            }
            if (Input.GetKey(m_downKey))
            {
                dir += Vector3.down;
            }
        }

        dir.Normalize();
        m_forcePoint.AddForce(dir * m_force);
		if(m_leftArm != null)
	        m_leftArm.AddForce(dir * m_armForce);
		if(m_rightArm != null)
	        m_rightArm.AddForce(dir * m_armForce);
    }
}
