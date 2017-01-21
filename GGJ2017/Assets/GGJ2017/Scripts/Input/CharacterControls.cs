using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControls : MonoBehaviour {
    public Rigidbody m_head;
    public Rigidbody m_leftArm;
    public Rigidbody m_rightArm;
    public Rigidbody m_forcePoint;

    public Color m_playerColor = Color.white;

    public float m_passiveForceHead = 50.0f;
    public float m_passiveForceArm = 25.0f;

    public float m_force = 50.0f;

    public KeyCode m_upkey = KeyCode.UpArrow;
    public KeyCode m_downKey = KeyCode.DownArrow;
    public KeyCode m_leftKey = KeyCode.LeftArrow;
    public KeyCode m_rightKey = KeyCode.RightArrow;

    protected void Start()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        for(int i = 0; i < renderers.Length; ++i)
        {
            renderers[i].material.color = m_playerColor;
        }
    }
    
    protected void Update()
    {
        m_head.AddForce(Vector3.up * m_passiveForceHead);
        m_leftArm.AddForce((Vector3.left + Vector3.up).normalized * m_passiveForceArm);
        m_rightArm.AddForce((Vector3.right+Vector3.up).normalized * m_passiveForceArm);

        Vector3 dir = Vector3.zero;
        if(Input.GetKey(m_upkey))
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
        if(Input.GetKey(m_downKey))
        {
            dir += Vector3.down;
        }

        dir.Normalize();
        m_forcePoint.AddForce(dir * m_force);
    }
}
