using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PongGameManager : MonoBehaviour
{
    private const string PONG_USER_POINTS = "pongUserPoints";
    private const string PONG_OPPONENT_POINTS = "pongOpponentPoints";
    
    
    public int pointsToWin = 11;

    void Start()
    {
        ResetScores();
    }

    void Update()
    {
        CheckEndState();
    }

    void CheckEndState()
    {
        int userPoints = GetUserPoints();
        int opponentPoints = GetOpponentPoints();
        
        // Loss
        if (opponentPoints >= pointsToWin)
        {
            SceneManager.LoadScene(5);
            Debug.Log("LOST");
        }
        // Win
        if (userPoints >= pointsToWin)
        {
            SceneManager.LoadScene(4);
            Debug.Log("WON");
        }
    }

    public void IncrementUserPoints()
    {
        int userPoints = GetUserPoints();
        PlayerPrefs.SetInt(PONG_USER_POINTS, userPoints + 1);
        Debug.Log("U-" + (userPoints + 1) + " : O-" + GetOpponentPoints());
    }

    public void IncrementOpponentPoints()
    {
        int opponentPoints = GetOpponentPoints();
        PlayerPrefs.SetInt(PONG_OPPONENT_POINTS, opponentPoints + 1);
        Debug.Log("U-" + GetUserPoints() + " : O-" + (opponentPoints + 1));
    }

    private void OnApplicationQuit()
    {
        ResetScores();
    }

    public void ResetScores()
    {
        PlayerPrefs.SetInt(PONG_USER_POINTS, 0);
        PlayerPrefs.SetInt(PONG_OPPONENT_POINTS, 0);
        PlayerPrefs.Save();
    }

    public int GetUserPoints()
    {
        return PlayerPrefs.GetInt(PONG_USER_POINTS);
    }

    public int GetOpponentPoints()
    {
        return PlayerPrefs.GetInt(PONG_OPPONENT_POINTS);
    }
}
