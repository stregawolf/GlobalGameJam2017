using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VolleyBallUIManager : BaseUIManager {
    public Text m_centerText;

    public GameObject m_rematchButton;
    public GameObject m_quitButton;

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
        DoSpinTransitionOut(m_centerText.gameObject, 0.25f, 2.0f).setOnComplete(()=>
        {
            m_centerText.gameObject.SetActive(false);
            Game.Instance.StartGame();
        });

        DoSpinTransitionOut(m_rematchButton, 0.25f);
        DoSpinTransitionOut(m_quitButton, 0.25f);
    }

    public void OnQuitPressed()
    {
        FadeOut(1.0f);

        DoSpinTransitionOut(m_rematchButton, 0.25f);
        DoSpinTransitionOut(m_quitButton, 0.25f);
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