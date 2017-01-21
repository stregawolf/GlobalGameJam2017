using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObject : MonoBehaviour {
    protected Renderer[] m_renderers;

    protected virtual void Awake()
    {
        DiscoverRenderers();
    }

    public void DiscoverRenderers()
    {
        m_renderers = GetComponentsInChildren<Renderer>();
    }

    public void SetColor(Color c)
    {
        for(int i = 0; i < m_renderers.Length; ++i)
        {
            m_renderers[i].material.color = c;
        }
    }
}
