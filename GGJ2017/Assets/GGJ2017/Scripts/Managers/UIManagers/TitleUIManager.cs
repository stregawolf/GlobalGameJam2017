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

    public Transform m_cameraStart;
    public Transform m_cameraEnd;

    public bool m_isTransitioning { get; protected set; }

    public enum TitleState
    {
        Splash,
        CharacterSelect,
        GameTransition,
    }

    public TitleState m_currentState { get; protected set; }

    public Player[] m_selectableCharacters;
    public CharacterSelector[] m_selectors;

    protected int[] m_selectionCount;
    protected bool m_countDownStarted = false;
    protected int m_currentCount = 3;
    protected float m_countDownTimer = 0.0f;

    protected void Awake()
    {
        m_selectionCount = new int[m_selectableCharacters.Length];
        m_isTransitioning = false;
        m_currentState = TitleState.Splash;
    }

    protected void Start()
    {
        for(int i = 0; i < m_selectableCharacters.Length; ++i)
        {
            m_selectableCharacters[i].Init();
        }
    }

    protected void Update()
    {
        if (m_isTransitioning)
        {
            return;
        }

        switch (m_currentState)
        {
            case TitleState.Splash:
                if (Input.anyKeyDown)
                {
                    OnStartPressed();
                }
                break;
            case TitleState.CharacterSelect:
                int confirmedCount = 0;
                for (int i = 0; i < m_selectors.Length; ++i)
                {
                    HandleSelectorInput(m_selectors[i]);
                    if(m_selectors[i].m_selectedPlayer != null)
                    {
                        confirmedCount++;
                    }
                }

                if(confirmedCount == m_selectors.Length)
                {

                }
                break;
        }
    }

    public void OnStartPressed()
    {
        if(m_isTransitioning)
        {
            return;
        }

        DoSpinTransitionOut(m_startButton, 0.5f, 2, 0.25f);
        DoTransitionTo(m_splashCharacter, Vector3.down * Screen.height, 0.5f, 0.5f, LeanTweenType.easeInBack);
        DoTransitionTo(m_splashTitle, Vector3.up * Screen.height, 0.5f, 1.0f, LeanTweenType.easeInBack);
        LeanTween.move(Camera.main.gameObject, m_cameraEnd.position, 1.5f).setDelay(1.25f).setEase(LeanTweenType.easeInOutSine);
        m_isTransitioning = true;

        LeanTween.delayedCall(3.0f, StartCharacterSelect);
    }

    public void StartCharacterSelect()
    {
        m_currentState = TitleState.CharacterSelect;
        m_isTransitioning = false;

        for(int i = 0; i < m_selectors.Length; ++i)
        {
            IncrementSelectionCount(m_selectors[i].m_playerIndex);
            UpdateSelector(m_selectors[i], true);
        }
    }

    public void HandleSelectorInput(CharacterSelector selector)
    {
        if (selector.m_selectedPlayer == null)
        {
            if (selector.LeftPressed())
            {
                DecrementSelectionCount(selector.m_playerIndex);
                selector.m_playerIndex = (selector.m_playerIndex - 1 + m_selectableCharacters.Length) % m_selectableCharacters.Length;
                IncrementSelectionCount(selector.m_playerIndex);
                UpdateSelector(selector);
            }
            else if (selector.RightPressed())
            {
                DecrementSelectionCount(selector.m_playerIndex);
                selector.m_playerIndex = (selector.m_playerIndex + 1) % m_selectableCharacters.Length;
                IncrementSelectionCount(selector.m_playerIndex);
                UpdateSelector(selector);
            }
            else if (selector.ConfirmPressed())
            {
                selector.SetPlayer(m_selectableCharacters[selector.m_playerIndex]);
            }
        }
        else
        {
            if (selector.CancelPressed())
            {
                selector.SetPlayer(null);
            }
        }

        if (selector.m_stackPos > m_selectionCount[selector.m_playerIndex])
        {
            UpdateSelector(selector);
        }
    }

    public void DecrementSelectionCount(int playerIndex)
    {
        m_selectionCount[playerIndex]--;
        if (m_selectionCount[playerIndex] <= 0)
        {
            m_selectableCharacters[playerIndex].m_controls.m_passiveForceHead = 0.0f;
        }
    }

    public void IncrementSelectionCount(int playerIndex)
    {
        m_selectionCount[playerIndex]++;
        if (m_selectionCount[playerIndex] > 0)
        {
            m_selectableCharacters[playerIndex].m_controls.m_passiveForceHead = 200.0f;
        }
    }

    public void UpdateSelector(CharacterSelector selector, bool instant = false)
    {
        Player selectedPlayer = m_selectableCharacters[selector.m_playerIndex];
        selector.m_stackPos = m_selectionCount[selector.m_playerIndex];
        if (instant)
        {
            selector.transform.position = GetScreenPosition(selectedPlayer, selector.m_stackPos);
        }
        else
        {
            DoTransitionTo(selector.gameObject, GetScreenPosition(selectedPlayer, selector.m_stackPos), 0.5f, 0.0f, LeanTweenType.easeInOutSine, true);
        }
    }

    public Vector3 GetScreenPosition(Player player, int stackPos = 0)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(player.transform.position + Vector3.up * 14.0f);
        screenPos.y += 50 * stackPos-1;
        return screenPos;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(m_gameSceneName);
    }
}
