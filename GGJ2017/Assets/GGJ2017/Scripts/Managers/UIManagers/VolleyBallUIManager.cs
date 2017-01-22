using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VolleyBallUIManager : BaseUIManager {
    public Text m_centerText;

    public GameObject m_pauseMenu;

    public GameObject m_rematchButton;
    public GameObject m_quitButton;

    protected Coroutine m_countDownCoroutine;

    public void Awake()
    {
        EventManager.StartCountDown.Register(StartCountDown);
        EventManager.DisplayCenterText.Register(DisplayCenterText);
        EventManager.OnGameComplete.Register(OnGameComplete);
    }

    public void Start()
    {
        FadeIn().setOnComplete(Game.Instance.StartGame);
    }

    public void Update()
    {
        if (Game.Instance.m_gameCompleted)
        {
            return;
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (m_pauseMenu.activeSelf)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
        
    }

    public void OnDestroy()
    {
        EventManager.StartCountDown.Unregister(StartCountDown);
        EventManager.DisplayCenterText.Unregister(DisplayCenterText);
        EventManager.OnGameComplete.Unregister(OnGameComplete);
    }

    public void OnGameComplete()
    {
        DoSpinTransitionIn(m_rematchButton, 0.0f, 0.25f, Vector3.one, 1.0f);
        DoSpinTransitionIn(m_quitButton, 0.0f, 0.25f, Vector3.one, 1.5f);
    }

    public void OnRematchPressed()
    {
        DoSpinTransitionOut(m_rematchButton, 0.25f);
        DoSpinTransitionOut(m_quitButton, 0.25f);

        DoRematchTransition();
    }

    public void DoRematchTransition()
    {
        if (!m_centerText.gameObject.activeSelf)
        {
            LeanTween.delayedCall(1.0f, Game.Instance.StartGame);
            return;
        }

        DoSpinTransitionOut(m_centerText.gameObject, 0.25f).setOnComplete(() =>
        {
            m_centerText.gameObject.SetActive(false);
            Game.Instance.StartGame();
        });
    }

    public void OnQuitPressed()
    {
        DoSpinTransitionOut(m_rematchButton, 0.25f);
        DoSpinTransitionOut(m_quitButton, 0.25f);

        DoTitleTransition();
    }

    public void DoTitleTransition()
    {
        if(m_countDownCoroutine != null)
        {
            StopCoroutine(m_countDownCoroutine);
        }

        FadeOut(1.0f);
        Game.Instance.EndRound();
        Camera.main.GetComponent<FollowCamera>().enabled = false;
        LeanTween.move(Camera.main.gameObject, Camera.main.transform.position + Vector3.up * 30.0f, 1.5f).setEase(LeanTweenType.easeInOutSine);
        LeanTween.delayedCall(2.0f, GoToTitle);
    }

    public void GoToTitle()
    {
        SceneManager.LoadScene(0);
    }
    public void StartCountDown(int startNumber, float totalDuration)
    {
        m_countDownCoroutine = StartCoroutine(HandleCountDown(startNumber, totalDuration));
    }

    protected IEnumerator HandleCountDown(int startNumber, float totalDuration)
    {
        float timeStep = totalDuration / (startNumber + 1);
        
        while (startNumber > 0)
        {
            if(!m_pauseMenu.activeSelf)
            {
                DisplayCenterText(startNumber.ToString(), timeStep * 0.5f, timeStep * 0.25f, Vector3.one);
                startNumber--;
                yield return new WaitForSecondsRealtime(timeStep);
            }
            else
            {
                yield return new WaitForEndOfFrame();
            }
        }

        DisplayCenterText("Start!", timeStep * 0.5f, timeStep * 0.25f, Vector3.one * 1.25f);
    }

    public void DisplayCenterText(string text, float displayTime, float transitionTime, Vector3 toScale)
    {
        m_centerText.text = text;
        DoSpinTransitionIn(m_centerText.gameObject, displayTime, transitionTime, toScale);
    }


    public void PauseGame()
    {
        Time.timeScale = 0.0f;
        DoSpinTransitionIn(m_pauseMenu, 0.0f, 0.25f, Vector3.one);
    }

    public void OnPauseRematchPressed()
    {
        ResumeGame();
        DoRematchTransition();
    }

    public void OnPauseQuitPressed()
    {
        ResumeGame();
        DoTitleTransition();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1.0f;
        DoSpinTransitionOut(m_pauseMenu, 0.25f);
    }
}