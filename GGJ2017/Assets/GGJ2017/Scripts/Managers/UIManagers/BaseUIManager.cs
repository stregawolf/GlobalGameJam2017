using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseUIManager : MonoBehaviour {
    public RectTransform m_fader;

    public LTDescr FadeOut(float duration = 1.0f)
    {
        return LeanTween.color(m_fader, Color.white, duration).setEase(LeanTweenType.easeInOutSine);
    }

    public LTDescr FadeIn(float duration = 1.0f)
    {
        return LeanTween.color(m_fader, Color.clear, duration).setEase(LeanTweenType.easeInOutSine);
    }

    public LTDescr DoTransitionTo(RectTransform rect, Vector3 pos, float transitionTime, float delay = 0.0f, LeanTweenType ease = LeanTweenType.easeInOutSine, bool doCancel = false)
    {
        if(doCancel)
        {
            LeanTween.cancel(rect);
        }
        return LeanTween.move(rect, pos, transitionTime).setDelay(delay).setEase(ease);
    }

    public LTDescr DoTransitionTo(GameObject obj, Vector3 pos, float transitionTime, float delay = 0.0f, LeanTweenType ease = LeanTweenType.easeInOutSine, bool doCancel = false)
    {
        if (doCancel)
        {
            LeanTween.cancel(obj);
        }
        return LeanTween.move(obj, pos, transitionTime).setDelay(delay).setEase(ease);
    }
    public LTDescr DoSpinTransitionIn(GameObject obj, float displayTime, float transitionTime, Vector3 toScale, float delay = 0.0f, float spinMultiplier = 2)
    {
        LeanTween.cancel(obj);
        obj.SetActive(true);

        obj.transform.localScale = Vector3.zero;
        obj.transform.rotation = Quaternion.identity;
        LeanTween.rotateZ(obj, 360 * spinMultiplier, transitionTime * 0.9f).setDelay(delay).setIgnoreTimeScale(true);
        return LeanTween.scale(obj, toScale, transitionTime).setEase(LeanTweenType.easeSpring).setDelay(delay).setIgnoreTimeScale(true)
            .setOnComplete(() =>
            {
                if (displayTime > 0.0f)
                {
                    DoSpinTransitionOut(obj, transitionTime, spinMultiplier, displayTime);
                }
            });
    }

    public LTDescr DoSpinTransitionOut(GameObject obj, float transitionTime, float spinMultiplier = 2, float delay = 0.0f)
    {
        LeanTween.rotateZ(obj, 360 * spinMultiplier, transitionTime).setDelay(delay).setIgnoreTimeScale(true);
        return LeanTween.scale(obj, Vector3.zero, transitionTime).setDelay(delay).setEase(LeanTweenType.easeSpring).setIgnoreTimeScale(true)
            .setOnComplete(() =>
            {
                obj.SetActive(false);
            });
    }
}
