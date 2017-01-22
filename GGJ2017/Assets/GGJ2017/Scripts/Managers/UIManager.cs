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

    public void DoSpinTransitionIn(GameObject obj, float displayTime, float transitionTime, Vector3 toScale, float spinMultiplier = 2, bool autoHide = true)
    {
        LeanTween.cancel(obj);
        obj.SetActive(true);

        obj.transform.localScale = Vector3.zero;
        obj.transform.rotation = Quaternion.identity;
        LeanTween.rotateZ(obj, 360 * spinMultiplier, transitionTime * 0.9f);
        LeanTween.scale(obj, toScale, transitionTime).setEase(LeanTweenType.easeSpring).setIgnoreTimeScale(true)
            .setOnComplete(() =>
            {
                if (autoHide)
                {
                    DoSpinTransitionOut(obj, transitionTime, spinMultiplier, displayTime);
                }
            });
    }

    public void DoSpinTransitionOut(GameObject obj, float transitionTime, float spinMultiplier = 2, float delay = 0.0f)
    {
        LeanTween.rotateZ(obj, 360 * spinMultiplier, transitionTime).setDelay(delay);
        LeanTween.scale(obj, Vector3.zero, transitionTime).setDelay(delay).setEase(LeanTweenType.easeSpring).setOnComplete(() =>
        {
            obj.SetActive(false);
        });
    }
}