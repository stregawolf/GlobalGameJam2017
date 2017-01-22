using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : BaseActor
{
    public int m_playerIndex = 0;
    public bool m_pullDataFromSelection = false;

    public Game.Team m_team;
    public CharacterData m_data;

    public Head m_head;

    public CharacterControls m_controls;
    public PlayerBuilder m_builder;

	private AudioSource audio;

    public enum Expression
    {
        Default,
        Excited,
        Angry,
        Sad,
        Shocked,
    }

    public Expression m_currentExpression = Expression.Default;

    protected override void Awake()
    {
        base.Awake();

		audio = gameObject.AddComponent<AudioSource>();

        if(m_controls == null)
        {
            m_controls = GetComponent<CharacterControls>();
        }

        if(m_builder == null)
        {
            m_builder = GetComponent<PlayerBuilder>();
        }
        
        if(m_pullDataFromSelection && GlobalData.s_selectedCharacters[m_playerIndex] != null)
        {
            m_data = GlobalData.s_selectedCharacters[m_playerIndex];
        }
    }

    public void Init(Game.Team team = null)
    {
        if(team != null)
        {
            m_team = team;
            m_team.m_displayName = m_data.m_displayName;
            m_team.m_color = m_data.m_color;
        }

        m_builder.InitFromData(m_data);
        m_builder.BuildPlayer();
        m_head = GetComponentInChildren<Head>();

        ShowDefaultExpression();
        DiscoverRenderers();
        SetColor(m_data.m_color);
    }

    public void ShowDefaultExpression()
    {
        m_head.m_face.sprite = m_data.m_faceDefault;
        m_currentExpression = Expression.Default;
    }

    public void ShowExpression(Expression expression, float duration = 2.0f)
    {
        if(expression == m_currentExpression)
        {
            return;
        }

        m_currentExpression = expression;
        switch (expression)
        {
            case Expression.Default:
                ShowDefaultExpression();
                return;
			case Expression.Excited:
				m_head.m_face.sprite = m_data.m_faceExcited;
				MaybePlaySound(m_data.m_excitedClips);
                break;
            case Expression.Angry:
                m_head.m_face.sprite = m_data.m_faceAngry;
				MaybePlaySound(m_data.m_angryClips);
                break;
            case Expression.Sad:
                m_head.m_face.sprite = m_data.m_faceSad;
				MaybePlaySound(m_data.m_sadClips);
                break;
            case Expression.Shocked:
                m_head.m_face.sprite = m_data.m_faceShocked;
				MaybePlaySound(m_data.m_shockedClips);
                break;
        }

        LeanTween.cancel(gameObject);
        if (duration > 0.0f)
        {
            LeanTween.delayedCall(gameObject, duration, ShowDefaultExpression);
        }
    }

	public void MaybePlaySound(AudioClip[] clips)
	{
		if (Random.value > 0.7f && !audio.isPlaying) {
			if (clips.Length != 0) {
				AudioClip clip = clips[Random.Range(0, clips.Length)];
				audio.PlayOneShot(clip);
			}
		}
	}
}
