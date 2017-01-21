using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPointTeamTriggerZone : MonoBehaviour {
    protected void OnTriggerExit(Collider c)
    {
        Ball ball = c.GetComponent<Ball>();
        if(ball != null && ball.m_lastPlayer != null)
        {
            ball.pointTeamId = (ball.transform.position.x > 0)? 0:1;
            Debug.Log(ball.pointTeamId);
        }
    }
}
