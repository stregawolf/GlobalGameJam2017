using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPointTeamTriggerZone : MonoBehaviour
{
    protected void OnTriggerExit(Collider c)
    {
        Ball ball = c.GetComponent<Ball>();
        if (ball != null && ball.m_lastPlayer != null)
        {
            if (ball.transform.position.x < 0)
            {
                // ball crossed over from right to left
                ball.m_pointTeamId = Game.TeamId.TeamB;
            }
            else
            {
                // ball crossed over from left to right
                ball.m_pointTeamId = Game.TeamId.TeamA;
            }
        }
    }
}
