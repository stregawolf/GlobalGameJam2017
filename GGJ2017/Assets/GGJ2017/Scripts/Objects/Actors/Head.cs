using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head : MonoBehaviour {
    public Transform m_pivot;
    public SpriteRenderer m_face;

    public void LateUpdate()
    {
        Vector3 dir = m_pivot.transform.position - Camera.main.transform.position;
        dir.Normalize();
        m_pivot.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(dir, m_pivot.up), m_pivot.up);
    }
}
