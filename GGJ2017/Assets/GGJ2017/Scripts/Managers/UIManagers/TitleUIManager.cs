using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleUIManager : BaseUIManager {
    public string m_gameSceneName = string.Empty;

    public GameObject m_startButton;
    public RectTransform m_splashTitle;
    public RectTransform m_splashCharacter;

    protected bool m_isTransitioning = false;

    public void OnStartPressed()
    {
        if(m_isTransitioning)
        {
            return;
        }

        DoTransitionTo(m_splashCharacter, Vector3.down * Screen.height, 0.5f, 0.0f, LeanTweenType.easeInBack);
        //DoSpinTransitionOut(m_startButton, 0.5f, 2, 0.25f);
        //LeanTween.delayedCall(1.25f, StartGame);
        m_isTransitioning = true;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(m_gameSceneName);
    }
}
