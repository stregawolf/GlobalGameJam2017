﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public static Game Instance { get; protected set; }

    public string m_uiSceneName;

    public int m_scoreToWinMatch = 10;
    public int m_matchesToWinGame = 3;

    public enum TeamId : int
    {
        Invalid = -1,
        TeamA = 0,
        TeamB = 1,
        TeamC = 2,
        TeamD = 3,
    }

    [System.Serializable]
    public class Team
    {
        public TeamId m_id = TeamId.Invalid;
        public string m_displayName = string.Empty;
        public int m_score = 0;
        public int m_matchPoints = 0;
        public Color m_color = Color.white;
        public Player[] m_players;
    }

    public Ball m_ball;
    public Team[] m_teams;
    public BaseObject[] m_environmentPieces;

    public bool m_gameStarted { get; protected set; }
    public bool m_gameCompleted { get; protected set; }
    public bool m_roundStarted { get; protected set; }

    protected Color m_environmentColor = Color.white;
    protected Team m_lastScoringTeam = null;

    protected void Awake()
    {
        Instance = this;
        EventManager.OnBallHitGround.Register(OnBallHitGround);

        m_gameStarted = false;
        m_roundStarted = false;
        m_gameCompleted = false;
        
        SceneManager.LoadScene(m_uiSceneName, LoadSceneMode.Additive);
    }

    protected void Start()
    {
        m_ball.Reset();
        InitPlayers();
    }

    protected void OnDestroy()
    {
        Instance = null;
        EventManager.OnBallHitGround.Unregister(OnBallHitGround);
    }
    
    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }


    public void InitPlayers()
    {
        for (int i = 0; i < m_teams.Length; ++i)
        {
            Team team = m_teams[i];
            for (int p = 0; p < team.m_players.Length; ++p)
            {
                team.m_players[p].Init(team);
            }
        }
    }

    public void StartGame()
    {
        m_gameStarted = true;
        m_gameCompleted = false;

        for (int i = 0; i < m_teams.Length; ++i)
        {
            Team team = m_teams[i];
            for (int p = 0; p < team.m_players.Length; ++p)
            {
                team.m_players[p].ShowDefaultExpression();
            }
        }

        ResetScore(true);
        EventManager.OnScoreChange.Dispatch();
        StartRound();
    }

    public void StartRound()
    {
        StartCoroutine(HandleStartRound());
    }


    protected IEnumerator HandleStartRound()
    {
        EventManager.StartCountDown.Dispatch(3, 4.25f);

        m_ball.Reset();

        if(m_environmentColor != Color.white)
        {
            Color startColor = m_environmentColor;
            LeanTween.value(0.0f, 1.0f, 1.0f)
                .setOnUpdate((float t) =>
                {
                    SetEnvironmentColor(Color.Lerp(startColor, Color.white, t));
                })
                .setOnComplete(()=>
                {
                    SetEnvironmentColor(Color.white);
                })
                .setEase(LeanTweenType.easeInOutSine);
        }

        yield return new WaitForSeconds(4.0f);

        m_roundStarted = true;
        Vector3 dir = m_ball.transform.right;
        if (m_lastScoringTeam != null && m_lastScoringTeam.m_id == TeamId.TeamA)
        {
            dir.x *= -1;
        }
        m_ball.TossBall(dir * m_ball.m_initialImpulse);
    }
    

    public void ResetScore(bool resetMatchPoints = false)
    {
        for (int i = 0; i < m_teams.Length; ++i)
        {
            m_teams[i].m_score = 0;
            if(resetMatchPoints)
            {
                m_teams[i].m_matchPoints = 0;
            }
        }
    }

    public float IncreaseScore(TeamId id)
    {
        int teamIndex = (int)id;
        if (teamIndex < 0 || teamIndex > m_teams.Length)
        {
            return 0.0f;
        }
        Team team = m_teams[teamIndex];

        m_lastScoringTeam = team;
        team.m_score++;
        if (team.m_score >= m_scoreToWinMatch)
        {
            ResetScore();
            team.m_matchPoints++;
            if (team.m_matchPoints >= m_matchesToWinGame)
            {
                // display "team wins the game!"
                ShowTeamExpression(Player.Expression.Excited, Player.Expression.Angry, team, 0.0f);
                EventManager.DisplayCenterText.Dispatch(string.Format("{0} wins!", team.m_displayName), 0.0f, 0.25f, Vector3.one * 1.25f);
                
                m_gameCompleted = true;
                EventManager.OnGameComplete.Dispatch();
                return -1;
            }
            else
            {
                // display "team wins the match!"
                ShowTeamExpression(Player.Expression.Excited, Player.Expression.Angry, team, 3.0f);
                EventManager.DisplayCenterText.Dispatch(string.Format("{0} wins the round!", team.m_displayName), 1.0f, 0.25f, Vector3.one * 1.25f);
                return 3.0f;
            }
        }
        else
        {
            // display "team scores!"
            ShowTeamExpression(Player.Expression.Excited, (Random.value > 0.5)?Player.Expression.Shocked:Player.Expression.Sad, team);
            EventManager.DisplayCenterText.Dispatch(string.Format("{0} scores!", team.m_displayName), 1.0f, 0.25f, Vector3.one * 1.25f);
            return 1.0f;
        }
    }

    public void ShowTeamExpression(Player.Expression winExpression, Player.Expression loseExpression, Team winningTeam, float duration = 2.0f)
	{
        for (int i = 0; i < m_teams.Length; ++i)
        {
            Team team = m_teams[i];
            for (int p = 0; p < team.m_players.Length; ++p)
			{
				
				team.m_players[p].MaybePlaySound((team == winningTeam) ? team.m_players[p].m_data.m_WinClips : team.m_players[p].m_data.m_LoseClips);
				team.m_players[p].ShowExpression((team == winningTeam)?winExpression:loseExpression, duration);
            }
        }
    }

    public void OnBallHitGround()
    {
        if (!m_roundStarted)
        {
            return;
        }

        float delay = 0.0f;

        if (m_ball.m_lastPlayer != null)
        {
            // determine winner
            if (m_ball.transform.position.x < 0)
            {
                if (m_ball.m_lastPlayer.m_team.m_id == TeamId.TeamA)
                {
                    // ball landed on A's side and last touched by A
                    delay = IncreaseScore(TeamId.TeamB);
                }
                else if (m_ball.m_lastPlayer.m_team.m_id == TeamId.TeamB)
                {
                    if (m_ball.m_pointTeamId == TeamId.TeamB)
                    {
                        // team B hit ball over net
                        delay = IncreaseScore(TeamId.TeamB);
                    }
                    else
                    {
                        // team B hit ball, but not over the net
                        delay = IncreaseScore(TeamId.TeamA);
                    }
                }
            }
            else
            {
                if (m_ball.m_lastPlayer.m_team.m_id == TeamId.TeamB)
                {
                    // ball landed on B's side and last touched by B
                    delay = IncreaseScore(TeamId.TeamA);
                }
                else if (m_ball.m_lastPlayer.m_team.m_id == TeamId.TeamA)
                {
                    if (m_ball.m_pointTeamId == TeamId.TeamA)
                    {
                        // team B hit ball over net
                        delay = IncreaseScore(TeamId.TeamA);
                    }
                    else
                    {
                        // team B hit ball, but not over the net
                        delay = IncreaseScore(TeamId.TeamB);
                    }
                }
            }
        }

        EventManager.OnScoreChange.Dispatch();
        EndRound();
        StartCoroutine(HandleCameraFlash(0.12f));
        StartCoroutine(HandleCameraShake(0.75f,1.0f));
        StartCoroutine(HandleCameraWrap(0.2f, -60.0f));

        StartCoroutine(HandleScoringFeedback(delay));
    }

    public void EndRound()
    {
        m_roundStarted = false;
    }

    public void SetEnvironmentColor(Color c)
    {
        m_environmentColor = c;
        for (int i = 0; i < m_environmentPieces.Length; ++i)
        {
            m_environmentPieces[i].SetColor(m_environmentColor);
        }
    }

    public IEnumerator HandleCameraFlash(float duration)
    { 
        var screenOverlay = Camera.main.GetComponent<UnityStandardAssets.ImageEffects.ScreenOverlay>();
        screenOverlay.enabled = true;
        yield return new WaitForSeconds(duration);
        screenOverlay.enabled = false;
    }

    public void WrapCamera(float intensity)
    {
        var lensAberration = Camera.main.GetComponent<UnityStandardAssets.CinematicEffects.LensAberrations>();

        lensAberration.distortion.amount = intensity;
    }

    public IEnumerator HandleCameraWrap(float duration, float intensity)
    {
        var lensAberration = Camera.main.GetComponent<UnityStandardAssets.CinematicEffects.LensAberrations>();
        lensAberration.enabled = true;        
        LeanTween.value(lensAberration.gameObject,WrapCamera,0.0f,intensity,duration).setLoopCount(2).setLoopPingPong();
        yield return new WaitForSeconds(duration*2);
        lensAberration.enabled = false;
    }

    //this works because we don't move the camera, probably has to be rethought otherwise
    public IEnumerator HandleCameraShake(float duration, float intensity)
    {
        float timer = 0.0f;
        Vector3 cameraPosition = Camera.main.transform.position;
        while(timer<duration)
        {
            yield return new WaitForEndOfFrame();
            timer += Time.deltaTime;

            float strength = (1.0f - timer / duration);
            float xoff = Random.Range(-intensity,intensity) * strength;
            float yoff = Random.Range(-intensity, intensity) * strength;

            Camera.main.transform.position = cameraPosition;
            Camera.main.transform.Translate(new Vector3(xoff, yoff, 0.0f), Space.Self);
        }

        Camera.main.transform.position = cameraPosition;
    }

    public IEnumerator HandleTimeFlux(float duration = 2.0f)
    {
        float timer = 0.0f;
        while(timer < duration)
        {
            yield return new WaitForEndOfFrame();
            timer += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(timer / duration);
            t = Mathf.Sin(t * Mathf.PI);
            Time.timeScale = Mathf.Lerp(1.0f, 0.25f, t);
        }

        Time.timeScale = 1.0f;
    }

    protected IEnumerator HandleScoringFeedback(float roundStartDelay = 0.0f)
    {
        m_ball.SetColor(m_lastScoringTeam.m_color);
        SetEnvironmentColor(m_lastScoringTeam.m_color);

        yield return StartCoroutine(HandleTimeFlux(1.0f));

        if(roundStartDelay >= 0.0f)
        {
            yield return new WaitForSeconds(roundStartDelay);
            if (!m_gameCompleted)
            {
                m_ball.Hide();
                yield return new WaitForSecondsRealtime(1.0f);

                StartRound();
            }
        }
    }

}
