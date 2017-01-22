using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : BaseActor
{
    public Game.Team m_team;
    public CharacterData m_data;

    public Head m_head;

    public CharacterControls m_controls;
    public PlayerBuilder m_builder;

    public enum Expression
    {
        Default,
        Excited,
        Angry,
        Sad,
    }

    public Expression m_currentExpression = Expression.Default;

    protected override void Awake()
    {
        base.Awake();

        if(m_controls == null)
        {
            m_controls = GetComponent<CharacterControls>();
        }

        if(m_builder == null)
        {
            m_builder = GetComponent<PlayerBuilder>();
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
                break;
            case Expression.Angry:
                m_head.m_face.sprite = m_data.m_faceAngry;
                break;
            case Expression.Sad:
                m_head.m_face.sprite = m_data.m_faceSad;
                break;
        }

        LeanTween.cancel(gameObject);
        LeanTween.delayedCall(gameObject, duration, ShowDefaultExpression);
    }
}
