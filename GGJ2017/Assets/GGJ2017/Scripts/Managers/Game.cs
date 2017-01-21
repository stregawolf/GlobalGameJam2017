using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {
    public static Game Instance { get; protected set; }

    public int m_scoreToWinMatch = 10;
    public int m_matchesToWinGame = 3;

    public int m_team0Score = 0;
    public int m_team1Score = 0;
    public int m_team0MatchPoints = 0;
    public int m_team1MatchPoints = 0;

    public Player m_player1;
    public Player m_player2;

    public Ball m_ball;

    public BaseObject[] m_environmentPieces;

    protected void Awake()
    {
        Instance = this;
        EventManager.OnBallHitGround.Register(OnBallHitGround);
    }

    protected void Start()
    {
        m_ball.Reset();
    }

    protected void OnDestroy()
    {
        Instance = null;
        EventManager.OnBallHitGround.Unregister(OnBallHitGround);
    }

    public void ResetScore()
    {
        m_team0Score = 0;
        m_team1Score = 0;
    }

    public void IncreaseTeam0Score()
    {
        ++m_team0Score;
        if(m_team0Score >= m_scoreToWinMatch)
        {
            ResetScore();
            ++m_team0MatchPoints;
            if(m_team0MatchPoints > m_matchesToWinGame)
            {
                // player1 wins
            }
        }
    }

    public void IncreaseTeam1Score()
    {
        ++m_team1Score;
        if (m_team1Score >= m_scoreToWinMatch)
        {
            ResetScore();
            ++m_team1MatchPoints;
            if (m_team1MatchPoints > m_matchesToWinGame)
            {
                // player2 wins
            }
        }
    }

    public void OnBallHitGround()
    {
        if(m_ball.m_lastPlayer != null)
        {
            // determine winner
            if (m_ball.transform.position.x < 0)
            {
                // ball last hit by team 0 or ball last hit by team 1 and ball's pointTeam is team 1
                if (m_ball.m_lastPlayer.teamId == 0 || (m_ball.m_lastPlayer.teamId == 1 && m_ball.pointTeamId == 1))
                {
                    IncreaseTeam1Score();
                }
                else if (m_ball.m_lastPlayer.teamId == 1 && (m_ball.pointTeamId == 0 || m_ball.pointTeamId == -1))
                {
                    IncreaseTeam0Score();
                }
            }
            else
            {
                // ball last hit by team 0 or ball last hit by team 1 and ball's pointTeam is team 1
                if (m_ball.m_lastPlayer.teamId == 1 || (m_ball.m_lastPlayer.teamId == 0 && m_ball.pointTeamId == 0))
                {
                    IncreaseTeam0Score();
                }
                else if (m_ball.m_lastPlayer.teamId == 0 && (m_ball.pointTeamId == 1 || m_ball.pointTeamId == -1))
                {
                    IncreaseTeam1Score();
                }
            }
        }

        EventManager.OnScoreChange.Dispatch();

        m_ball.Reset();
    }
}
