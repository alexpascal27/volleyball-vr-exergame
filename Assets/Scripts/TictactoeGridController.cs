using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TictactoeGridController : MonoBehaviour
{
    [SerializeField] private bool isUserX = true;
    
    private const int GridDimensionsX = 3;
    private const int GridDimensionsY = 3;
    private String[,] grid = new String[GridDimensionsY, GridDimensionsX];
    private bool[,] hasBeenHitGrid = new bool[GridDimensionsY, GridDimensionsX];
    
    private Dictionary<char, int> rowNameToIndex = new Dictionary<char, int>();
    
    // Start is called before the first frame update
    void Start()
    {
        InitRowNameToIndexDictionary();
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
}
