﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : BaseObject {

    public float m_initialImpulse = 10.0f;
    public float m_contactImpulse = 1.0f;

    public GameObject m_ground;

    public Rigidbody m_rigidbody;

    protected Vector3 m_originalPosition;
    protected Quaternion m_originalRotation;

    public Game.TeamId m_pointTeamId = Game.TeamId.Invalid;
    public Player m_lastPlayer { get; protected set; }

    private TrailRenderer m_trailRenderer;
    
	protected override void Awake ()
    {
        base.Awake();
        m_originalPosition = transform.position;
        m_originalRotation = transform.rotation;
        m_trailRenderer = GetComponent<TrailRenderer>();
    } 

    public void Reset()
    {
        m_rigidbody.isKinematic = true;
        m_rigidbody.velocity = Vector3.zero;
        m_rigidbody.angularVelocity = Vector3.zero;

        m_pointTeamId = Game.TeamId.Invalid;
        transform.position = m_originalPosition;
        transform.rotation = m_originalRotation;

        m_trailRenderer.Clear();
        SetColor(Color.white);
        m_lastPlayer = null;

        transform.localScale = Vector3.zero;
        LeanTween.scale(gameObject, Vector3.one, 0.5f).setEase(LeanTweenType.easeSpring);

    }

    public void TossBall(Vector3 force)
    {
        m_rigidbody.isKinematic = false;
        m_rigidbody.AddForce(force, ForceMode.Impulse);
    }

    public void SetPlayer(Player player)
    {
        if (player != null)
        {
            m_lastPlayer = player;
            m_pointTeamId = Game.TeamId.Invalid;
            SetColor(m_lastPlayer.m_color);

            m_rigidbody.AddForce(Vector3.up * m_contactImpulse, ForceMode.Impulse);
        }
    }
    
    public void OnCollisionEnter(Collision c)
    {
        if(c.collider.gameObject == m_ground)
        {
            EventManager.OnBallHitGround.Dispatch();
        }
    }

    public void OnCollisionExit(Collision c)
    {
        SetPlayer(c.collider.GetComponentInParent<Player>());
    }
    
}
