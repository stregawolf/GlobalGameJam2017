using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolleyBallUIManager : BaseUIManager {
    public Text m_centerText;

    public void Awake()
    {
        EventManager.StartCountDown.Register(StartCountDown);
        EventManager.DisplayCenterText.Register(DisplayCenterText);
    }

    public void OnDestroy()
    {
        EventManager.StartCountDown.Unregister(StartCountDown);
        EventManager.DisplayCenterText.Unregister(DisplayCenterText);
    }

    public void StartCountDown(int startNumber, float totalDuration)
    {
        StartCoroutine(HandleCountDown(startNumber, totalDuration));
    }

    protected IEnumerator HandleCountDown(int startNumber, float totalDuration)
    {
        float timeStep = totalDuration / (startNumber + 1);
        while (startNumber > 0)
        {
            DisplayCenterText(startNumber.ToString(), timeStep * 0.5f, timeStep * 0.25f, Vector3.one);
            startNumber--;
            yield return new WaitForSecondsRealtime(timeStep);
        }

        DisplayCenterText("Start!", timeStep * 0.5f, timeStep * 0.25f, Vector3.one * 1.25f);
    }

    public void DisplayCenterText(string text, float displayTime, float transitionTime, Vector3 toScale)
    {
        m_centerText.text = text;
        DoSpinTransitionIn(m_centerText.gameObject, displayTime, transitionTime, toScale);
    }

}