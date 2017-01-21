using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static Game Instance { get; protected set; }

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
        public int m_score = 0;
        public int m_matchPoints = 0;
        public Color m_color = Color.white;
        public Player[] m_players;
    }

    public Ball m_ball;
    public Team[] m_teams;
    public BaseObject[] m_environmentPieces;

    public bool m_roundStarted { get; protected set; }

    protected Color m_environmentColor = Color.white;
    protected Team m_lastScoringTeam = null;

    protected void Awake()
    {
        Instance = this;
        EventManager.OnBallHitGround.Register(OnBallHitGround);

        m_roundStarted = false;
    }

    protected void OnDestroy()
    {
        Instance = null;
        EventManager.OnBallHitGround.Unregister(OnBallHitGround);
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

    protected void Start()
    {
        InitPlayers();
        StartRound();
    }

    protected void StartRound()
    {
        StartCoroutine(HandleStartRound());
    }


    protected IEnumerator HandleStartRound()
    {
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

        yield return new WaitForSeconds(1.0f);

        m_roundStarted = true;
        Vector3 dir = m_ball.transform.right;
        if (m_lastScoringTeam != null && m_lastScoringTeam.m_id == TeamId.TeamA)
        {
            dir.x *= -1;
        }
        m_ball.TossBall(dir * m_ball.m_initialImpulse);
    }
    

    public void ResetScore()
    {
        for (int i = 0; i < m_teams.Length; ++i)
        {
            m_teams[i].m_score = 0;
        }
    }

    public void IncreaseScore(TeamId id)
    {
        int teamIndex = (int)id;
        if (teamIndex < 0 || teamIndex > m_teams.Length)
        {
            return;
        }
        Team team = m_teams[teamIndex];

        m_lastScoringTeam = team;
        team.m_score++;
        if (team.m_score >= m_scoreToWinMatch)
        {
            ResetScore();
            team.m_matchPoints++;
            if (team.m_matchPoints > m_matchesToWinGame)
            {
                // Team wins
            }
        }
    }

    public void OnBallHitGround()
    {
        if (!m_roundStarted)
        {
            return;
        }

        if (m_ball.m_lastPlayer != null)
        {
            // determine winner
            if (m_ball.transform.position.x < 0)
            {
                if (m_ball.m_lastPlayer.m_team.m_id == TeamId.TeamA)
                {
                    // ball landed on A's side and last touched by A
                    IncreaseScore(TeamId.TeamB);
                }
                else if (m_ball.m_lastPlayer.m_team.m_id == TeamId.TeamB)
                {
                    if (m_ball.m_pointTeamId == TeamId.TeamB)
                    {
                        // team B hit ball over net
                        IncreaseScore(TeamId.TeamB);
                    }
                    else
                    {
                        // team B hit ball, but not over the net
                        IncreaseScore(TeamId.TeamA);
                    }
                }
            }
            else
            {
                if (m_ball.m_lastPlayer.m_team.m_id == TeamId.TeamB)
                {
                    // ball landed on B's side and last touched by B
                    IncreaseScore(TeamId.TeamA);
                }
                else if (m_ball.m_lastPlayer.m_team.m_id == TeamId.TeamA)
                {
                    if (m_ball.m_pointTeamId == TeamId.TeamA)
                    {
                        // team B hit ball over net
                        IncreaseScore(TeamId.TeamA);
                    }
                    else
                    {
                        // team B hit ball, but not over the net
                        IncreaseScore(TeamId.TeamB);
                    }
                }
            }
        }

        EventManager.OnScoreChange.Dispatch();
        m_roundStarted = false;
        StartCoroutine(HandleScoringFeedback());
    }

    public void SetEnvironmentColor(Color c)
    {
        m_environmentColor = c;
        for (int i = 0; i < m_environmentPieces.Length; ++i)
        {
            m_environmentPieces[i].SetColor(m_environmentColor);
        }
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

    protected IEnumerator HandleScoringFeedback()
    {
        m_ball.SetColor(m_lastScoringTeam.m_color);
        SetEnvironmentColor(m_lastScoringTeam.m_color);

        yield return StartCoroutine(HandleTimeFlux(1.0f));
        m_ball.Hide();
        yield return new WaitForSecondsRealtime(1.0f);
        
        StartRound();
    }

}
