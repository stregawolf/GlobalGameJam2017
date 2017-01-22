using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager
{
    public static readonly CallbackEvent OnBallHitGround = new CallbackEvent();
    public static readonly CallbackEvent OnScoreChange = new CallbackEvent();

    /// <summary>
    /// int start number, float total duration of countdown
    /// </summary>
    public static readonly CallbackEvent<int, float> StartCountDown = new CallbackEvent<int, float>();

    /// <summary>
    /// string text to display, float display time, float transition time, vector3 toSize
    /// </summary>
    public static readonly CallbackEvent<string, float, float, Vector3> DisplayCenterText = new CallbackEvent<string, float, float, Vector3>();

    public static readonly CallbackEvent OnGameComplete = new CallbackEvent();
}
