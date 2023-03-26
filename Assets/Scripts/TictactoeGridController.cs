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

    // TODO: Remove as not necessary at current time: only provides repeat hit feedback 
    public bool RegisterHit(String tileName)
    {
        // y, x
        Tuple<int, int> tileCoordinates = ConvertTileNameToCoordinates(tileName);
        // check status of tile
        bool isHit = CheckIfHit(tileCoordinates);

        bool alreadyHit = hasBeenHitGrid[tileCoordinates.Item1, tileCoordinates.Item2];
        // if new hit
        if (!alreadyHit && isHit);
        else
        {
            Debug.Log("Repeat hit at " + tileName);
        }

        return isHit;
    }
    
    bool CheckIfHit(Tuple<int, int> coordinates)
    {
        int y = coordinates.Item1;
        int x = coordinates.Item2;
        // if empty not a hit
        return !string.IsNullOrEmpty(grid[y, x]);
    }
    
    Tuple<int, int> ConvertTileNameToCoordinates(String tileName)
    {
        char rowName = tileName[0];
        int rowNumber = rowNameToIndex[rowName];
        int tileNumber = int.Parse(tileName[1].ToString());
        return new Tuple<int, int>(rowNumber, tileNumber);
    }
}
