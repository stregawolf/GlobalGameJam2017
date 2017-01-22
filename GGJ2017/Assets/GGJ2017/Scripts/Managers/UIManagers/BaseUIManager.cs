using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUIManager : MonoBehaviour {

    public LTDescr DoTransitionTo(RectTransform obj, Vector3 pos, float transitionTime, float delay = 0.0f, LeanTweenType ease = LeanTweenType.easeInOutSine)
    {
        return LeanTween.move(obj, pos, transitionTime).setDelay(delay).setEase(ease);
    }

    public LTDescr DoSpinTransitionIn(GameObject obj, float displayTime, float transitionTime, Vector3 toScale, float spinMultiplier = 2, bool autoHide = true)
    {
        LeanTween.cancel(obj);
        obj.SetActive(true);

        obj.transform.localScale = Vector3.zero;
        obj.transform.rotation = Quaternion.identity;
        LeanTween.rotateZ(obj, 360 * spinMultiplier, transitionTime * 0.9f);
        return LeanTween.scale(obj, toScale, transitionTime).setEase(LeanTweenType.easeSpring).setIgnoreTimeScale(true)
            .setOnComplete(() =>
            {
                if (autoHide)
                {
                    DoSpinTransitionOut(obj, transitionTime, spinMultiplier, displayTime);
                }
            });
    }

    public LTDescr DoSpinTransitionOut(GameObject obj, float transitionTime, float spinMultiplier = 2, float delay = 0.0f)
    {
        LeanTween.rotateZ(obj, 360 * spinMultiplier, transitionTime).setDelay(delay);
        return LeanTween.scale(obj, Vector3.zero, transitionTime).setDelay(delay).setEase(LeanTweenType.easeSpring).setOnComplete(() =>
        {
            obj.SetActive(false);
        });
    }
}
