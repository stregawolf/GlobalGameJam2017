using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : BaseObject
{

    public float m_initialImpulse = 10.0f;
    public float m_contactImpulse = 1.0f;
    public float m_ballScale = 1f;
	public bool m_changeColors = false;

	public float m_windX = 0;
	public float m_windY = 0;
	public float m_windStrength = 0;

	public GameObject leftTree;
	public GameObject rightTree;
	public float m_treeOffset = 0.85f;

    public GameObject m_ground;

	public bool drawWindArea = false;

    public Rigidbody m_rigidbody;

    protected Vector3 m_originalPosition;
    protected Quaternion m_originalRotation;

    public Game.TeamId m_pointTeamId = Game.TeamId.Invalid;
    public Player m_lastPlayer { get; protected set; }

    private TrailRenderer m_trailRenderer;

    protected override void Awake()
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

        Show();
    }

    public void Show()
    {
        transform.localScale = Vector3.zero;
        LeanTween.scale(gameObject, Vector3.one * m_ballScale, 0.5f).setEase(LeanTweenType.easeSpring);
    }

    public void Hide(float delay = 0.0f)
    {
        transform.localScale = Vector3.one * m_ballScale;
        LeanTween.scale(gameObject, Vector3.zero, 1.0f).setDelay(delay).setEase(LeanTweenType.easeSpring);
    }

    public void TossBall(Vector3 force)
    {
        m_rigidbody.isKinematic = false;
        m_rigidbody.AddForce(force, ForceMode.Impulse);
    }

    public void SetPlayer(Player player)
    {
        if (Game.Instance.m_roundStarted && player != null)
        {
            player.ShowExpression(Player.Expression.Excited);
            m_lastPlayer = player;
            m_pointTeamId = Game.TeamId.Invalid;
			if(m_changeColors)
	            SetColor(m_lastPlayer.m_team.m_color);

            m_rigidbody.AddForce(Vector3.up * m_contactImpulse, ForceMode.Impulse);
        }
    }

	void FixedUpdate()
	{
		m_trailRenderer.startWidth = transform.localScale.x;
		bool inWindArea = false;
		if (m_windStrength > 0f) {
			if (Mathf.Abs(transform.position.x) + 0.5f * m_ballScale > m_windX) {
				if (Mathf.Sign(m_rigidbody.velocity.x) == Mathf.Sign(transform.position.x))
					m_rigidbody.AddForce(m_windStrength * Vector3.right * -Mathf.Sign(transform.position.x));
				inWindArea = true;
			}
			if (transform.position.y + 0.5f * m_ballScale > m_windY) {
				if (m_rigidbody.velocity.y > 0)
					m_rigidbody.AddForce(m_windStrength * -Vector3.up);
				inWindArea = true;
			}
		} else {
			Vector3 ballScreenPos = Camera.main.WorldToScreenPoint(transform.position + m_rigidbody.velocity * Time.fixedDeltaTime);
			Vector3 leftTreePos = Camera.main.WorldToScreenPoint(leftTree.transform.position + m_treeOffset * Camera.main.transform.right);
			Vector3 rightTreePos = Camera.main.WorldToScreenPoint(rightTree.transform.position - m_treeOffset * Camera.main.transform.right);
			if ((transform.position.x < 0 && ballScreenPos.x < leftTreePos.x && m_rigidbody.velocity.x < 0) || 
				(transform.position.x > 0 && ballScreenPos.x > rightTreePos.x && m_rigidbody.velocity.x > 0)) {
				m_rigidbody.velocity = new Vector3(-m_rigidbody.velocity.x, m_rigidbody.velocity.y, 0);
			}

		}

		if (drawWindArea) {
			Vector3 bl = new Vector3(-m_windX, 0, 0);
			Vector3 tl = new Vector3(-m_windX, m_windY, 0);
			Vector3 tr = new Vector3(m_windX, m_windY, 0);
			Vector3 br = new Vector3(m_windX, 0, 0);

			Color color = inWindArea ? Color.red : Color.green;


			Debug.DrawLine(bl, tl, color);
			Debug.DrawLine(tl, tr, color);
			Debug.DrawLine(tr, br, color);
			Debug.DrawLine(br, bl, color);
		}
	}

    public void OnCollisionEnter(Collision c)
    {
        SetPlayer(c.collider.GetComponentInParent<Player>());
        if (c.collider.gameObject == m_ground)
        {
            EventManager.OnBallHitGround.Dispatch();
        }
    }

    public void OnCollisionExit(Collision c)
    {
        
    }

}
