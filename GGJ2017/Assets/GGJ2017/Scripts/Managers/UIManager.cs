using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
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
        while(startNumber > 0)
        {
            DisplayCenterText(startNumber.ToString(), timeStep*0.5f, timeStep*0.25f, Vector3.one);
            startNumber--;
            yield return new WaitForSecondsRealtime(timeStep);
        }

        DisplayCenterText("Start!", timeStep * 0.5f, timeStep * 0.25f, Vector3.one*1.25f);
    }

    public void DisplayCenterText(string text, float displayTime, float transitionTime, Vector3 toScale)
    {
        LeanTween.cancel(m_centerText.gameObject);
        m_centerText.gameObject.SetActive(true);
        m_centerText.text = text;

        m_centerText.transform.localScale = Vector3.zero;
        LeanTween.rotateZ(m_centerText.gameObject, 360 * 2, transitionTime*0.9f);
        LeanTween.scale(m_centerText.gameObject, toScale, transitionTime).setEase(LeanTweenType.easeSpring).setIgnoreTimeScale(true)
            .setOnComplete(()=>
            {
                LeanTween.rotateZ(m_centerText.gameObject, 360 * 2, transitionTime).setDelay(displayTime);
                LeanTween.scale(m_centerText.gameObject, Vector3.zero, transitionTime).setDelay(displayTime).setEase(LeanTweenType.easeSpring).setOnComplete(()=>
                {
                    m_centerText.gameObject.SetActive(false);
                });
            });

    }
}
