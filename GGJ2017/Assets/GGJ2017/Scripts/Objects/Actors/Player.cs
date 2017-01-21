using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : BaseActor
{
    public Game.Team m_team;

    public void Init(Game.Team team)
    {
        m_team = team;
        DiscoverRenderers();
        SetColor(m_team.m_color);
    }
}
