using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TictactoeGridController : MonoBehaviour
{
    [SerializeField] private bool isUserX = true;
    
    private const int GridDimensionsX = 3;
    private const int GridDimensionsY = 3;
    private String[,] grid = new String[GridDimensionsY, GridDimensionsX];
    private bool[,] hasBeenHitGrid = new bool[GridDimensionsY, GridDimensionsX];
    
    private Dictionary<char, int> rowNameToIndex = new Dictionary<char, int>();
    // End states, 3 verticals, 3 horizontals, 2 diagonal = 8 possibilities
    Vector2[,] endStates = new Vector2[8, 3];
    
    
    // Start is called before the first frame update
    void Start()
    {
        InitRowNameToIndexDictionary();
        InitEndStates();
    }

    public bool GetIsUserX()
    {
        return isUserX;
    }
    
    void InitRowNameToIndexDictionary()
    {
        rowNameToIndex.Add('A', 0);
        rowNameToIndex.Add('B', 1);
        rowNameToIndex.Add('C', 2);
    }

    void InitEndStates()
    {
        // Horizontal
        endStates[0, 0] = new Vector2(0, 0); endStates[0, 1] = new Vector2(1, 0); endStates[0, 2] = new Vector2(2, 0);
        endStates[1, 0] = new Vector2(0, 1); endStates[1, 1] = new Vector2(1, 1); endStates[1, 2] = new Vector2(2, 1);
        endStates[2, 0] = new Vector2(0, 2); endStates[2, 1] = new Vector2(1, 2); endStates[2, 2] = new Vector2(2, 2);
        // Vertical
        endStates[3, 0] = new Vector2(0, 0); endStates[3, 1] = new Vector2(0, 1); endStates[3, 2] = new Vector2(0, 2);
        endStates[4, 0] = new Vector2(1, 0); endStates[4, 1] = new Vector2(1, 1); endStates[4, 2] = new Vector2(1, 2);
        endStates[5, 0] = new Vector2(2, 0); endStates[5, 1] = new Vector2(2, 1); endStates[5, 2] = new Vector2(2, 2);
        // Diagonal
        endStates[6, 0] = new Vector2(0, 0); endStates[6, 1] = new Vector2(1, 1); endStates[6, 2] = new Vector2(2, 2);
        endStates[7, 0] = new Vector2(0, 2); endStates[7, 1] = new Vector2(1, 1); endStates[7, 2] = new Vector2(2, 0);
    }

    public bool RegisterHit(String tileName, bool hitFromUserTile)
    {
        // y, x
        Tuple<int, int> tileCoordinates = ConvertTileNameToCoordinates(tileName);

        bool alreadyHit = hasBeenHitGrid[tileCoordinates.Item1, tileCoordinates.Item2];
        // if new hit
        if (!alreadyHit)
        {
            hasBeenHitGrid[tileCoordinates.Item1, tileCoordinates.Item2] = true;
            String userString = isUserX ? "X" : "O";
            String opponentString = isUserX ? "O" : "X";
            grid[tileCoordinates.Item1, tileCoordinates.Item2] = hitFromUserTile ? userString : opponentString;
            
            // check end state
            int endState = CheckEndState();
            if (endState != -1)
            {
                if (endState == 0)
                {
                    SceneManager.LoadScene(5);
                }
                else if (endState == 1)
                {
                    SceneManager.LoadScene(4);
                }
                else
                {
                    SceneManager.LoadScene(6);
                }
            }
                
        }
        else
        {
            Debug.Log("Repeat hit at " + tileName);
        }

        return alreadyHit;
    }
    
    Tuple<int, int> ConvertTileNameToCoordinates(String tileName)
    {
        char rowName = tileName[0];
        int rowNumber = rowNameToIndex[rowName];
        int tileNumber = int.Parse(tileName[1].ToString());
        return new Tuple<int, int>(rowNumber, tileNumber);
    }
    
    
    // -1 for unfinished , 0 for loss, 1 for win, 2 for draw
    public int CheckEndState()
    {
        String userString = isUserX ? "X" : "O";
        String opponentString = isUserX ? "O" : "X";
        
        // check through end states
        for (int i = 0; i < endStates.GetLength(0); i++)
        {
            // in current loop who is likely to win
            int state = -1;
            for (int j = 0; j < endStates.GetLength(1); j++)
            {
                Vector2 pointBeingInvestigated = endStates[i, j];
                String value = grid[(int)pointBeingInvestigated.y, (int)pointBeingInvestigated.x];
                // If not empty
                if (!string.IsNullOrEmpty(value))
                {
                    // If user
                    if (value == userString)
                    {
                        // if first point then assign state
                        if (j == 0) state = 1;
                        // If last point checked and same as state 
                        else if (state == 1 && j == endStates.GetLength(1) - 1)
                        {
                            return 1;
                        }
                        else if (state != 1) break;
                    }
                    // If opponent
                    else
                    {
                        // if first point then assign state
                        if (j == 0) state = 0;
                        // If last point checked and same as state 
                        else if (state == 0 && j == endStates.GetLength(1) - 1)
                        {
                            return 0;
                        }
                        else if (state != 0) break;
                    }
                }
            }
        }

        if (IsGridFull()) return 2;

        return -1;
    }

    bool IsGridFull()
    {
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                if (string.IsNullOrEmpty(grid[i, j])) return false;
            }
        }

        return true;
    }
}
