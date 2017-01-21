using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager
{
    public static readonly CallbackEvent OnBallHitGround = new CallbackEvent();
    public static readonly CallbackEvent OnScoreChange = new CallbackEvent();
}
