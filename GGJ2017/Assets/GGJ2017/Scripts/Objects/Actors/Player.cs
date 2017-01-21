using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : BaseActor {
    public int teamId;
    public Color m_color = Color.white;

    protected override void Awake()
    {
        base.Awake();
        SetColor(m_color);
    }
}
