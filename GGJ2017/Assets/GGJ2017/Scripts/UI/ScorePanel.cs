using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScorePanel : MonoBehaviour {

    public Text m_scoreText;
    public Text m_matchText;

	protected void Awake ()
    {
        EventManager.OnScoreChange.Register(UpdateScore);	
	}

    protected void Start()
    {
        UpdateScore();
    }

    protected void OnDestroy()
    {
        EventManager.OnScoreChange.Unregister(UpdateScore);
    }

    public void UpdateScore()
    {
        m_scoreText.text = string.Format("{0}:{1}", Game.Instance.m_team0Score, Game.Instance.m_team1Score);
        m_matchText.text = string.Format("{0}:{1}", Game.Instance.m_team0MatchPoints, Game.Instance.m_team1MatchPoints);
    }
}
