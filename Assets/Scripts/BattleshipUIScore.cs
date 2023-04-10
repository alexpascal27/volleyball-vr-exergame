using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleshipUIScore : MonoBehaviour
{
    private const string BATTLESHIP_USER_SHIP_HITS = "battleshipUserShipHits";
    private const string BATTLESHIP_OPPONENT_SHIP_HITS = "battleshipOpponentShipHit";
    
    public TextMeshProUGUI userScoreText;
    public TextMeshProUGUI opponentScoreText;

    private void Start()
    {
        ResetScores();
    }

    private void Update()
    {
        userScoreText.text = PlayerPrefs.GetInt(BATTLESHIP_USER_SHIP_HITS).ToString();
        opponentScoreText.text = PlayerPrefs.GetInt(BATTLESHIP_OPPONENT_SHIP_HITS).ToString();
    }
    
    public void ResetScores()
    {
        PlayerPrefs.SetInt(BATTLESHIP_USER_SHIP_HITS, 0);
        PlayerPrefs.SetInt(BATTLESHIP_OPPONENT_SHIP_HITS, 0);
        PlayerPrefs.Save();
    }
}
