using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XboxCtrlrInput;

public class CharacterSelector : MonoBehaviour {
    public Image m_background;

    public int m_playerIndex = 0;
    public int m_stackPos = 0;
    public Player m_selectedPlayer;

#if !UNITY_WEBGL
    public XboxController controller;
#endif
    public KeyCode m_upkey = KeyCode.UpArrow;
    public KeyCode m_downKey = KeyCode.DownArrow;
    public KeyCode m_leftKey = KeyCode.LeftArrow;
    public KeyCode m_rightKey = KeyCode.RightArrow;

    public void SetPlayer(Player player)
    {
        m_selectedPlayer = player;
        if(player == null)
        {
            m_background.color = Color.white;
        }
        else
        {
            m_background.color = player.m_data.m_color;
        }

        LeanTween.scale(gameObject, Vector3.one * 1.25f, 0.25f).setEase(LeanTweenType.punch);
    }

    public bool ShouldUseControllerInput()
    {
#if UNITY_WEBGL
        return false;
#else
        return XCI.GetNumPluggedCtrlrs() > 0 && Input.GetJoystickNames().Length != 0 && XCI.IsPluggedIn((int)(controller == XboxController.All ? XboxController.First : controller));
#endif
    }

    public bool LeftPressed()
    {
        if (ShouldUseControllerInput())
        {
#if !UNITY_WEBGL
            return XCI.GetButtonDown(XboxButton.DPadLeft, controller);
#else
            return false;
#endif
        }

        return Input.GetKeyDown(m_leftKey);
    }

    public bool RightPressed()
    {
        if (ShouldUseControllerInput())
        {
#if !UNITY_WEBGL
            return XCI.GetButtonDown(XboxButton.DPadRight, controller);
#else
            return false;
#endif
        }

        return Input.GetKeyDown(m_rightKey);
    }

    public bool ConfirmPressed()
    {
        if (ShouldUseControllerInput())
        {
#if !UNITY_WEBGL
            return XCI.GetButtonDown(XboxButton.DPadDown, controller);
#else
            return false;
#endif
        }

        return Input.GetKeyDown(m_downKey);
    }

    public bool CancelPressed()
    {
        if (ShouldUseControllerInput())
        {
#if !UNITY_WEBGL
            return XCI.GetButtonDown(XboxButton.DPadUp, controller);
#else
            return false;
#endif
        }

        return Input.GetKeyDown(m_upkey);
    }
}
