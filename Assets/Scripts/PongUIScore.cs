using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PongUIScore : MonoBehaviour
{
    private const string PONG_USER_POINTS = "pongUserPoints";
    private const string PONG_OPPONENT_POINTS = "pongOpponentPoints";
    
    public TextMeshProUGUI userScoreText;
    public TextMeshProUGUI opponentScoreText;

    private void Update()
    {
        userScoreText.text = PlayerPrefs.GetInt(PONG_USER_POINTS).ToString();
        opponentScoreText.text = PlayerPrefs.GetInt(PONG_OPPONENT_POINTS).ToString();
    }
}
