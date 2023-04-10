using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuFunctions : MonoBehaviour
{
    public int mainMenuSceneIndex;
    public int battleshipSceneIndex;
    public int pongSceneIndex;
    public int tictactoeSceneIndex;
    
    // Go to main menu
    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public void StartBattleship()
    {
        SceneManager.LoadScene(1);
    }
    
    public void StartPong()
    {
        SceneManager.LoadScene(3);
    }
    
    public void StartTicTacToe()
    {
        SceneManager.LoadScene(2);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
