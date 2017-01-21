using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : BaseActor {
    public Game.TeamId teamId = Game.TeamId.Invalid;
    public Color m_color = Color.white;

    protected override void Awake()
    {
        base.Awake();
        SetColor(m_color);
    }
}
