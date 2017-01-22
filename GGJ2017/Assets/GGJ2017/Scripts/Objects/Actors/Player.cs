using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : BaseActor
{
    public Game.Team m_team;
    public CharacterData m_data;

    public Head m_head;

    public PlayerBuilder m_builder;

    protected override void Awake()
    {
        base.Awake();
        if(m_builder == null)
        {
            m_builder = GetComponent<PlayerBuilder>();
        }
    }

    public void Init(Game.Team team)
    {
        m_team = team;
        m_team.m_displayName = m_data.m_displayName;
        m_team.m_color = m_data.m_color;

        m_builder.InitFromData(m_data);
        m_builder.BuildPlayer();
        m_head = GetComponentInChildren<Head>();

        DiscoverRenderers();
        SetColor(m_data.m_color);
    }
}
