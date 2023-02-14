using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Random = UnityEngine.Random;

public class BattleshipGridController : MonoBehaviour
{
    // Grid
    // if [][] = "" unallocated, if [][] = "{ship_name}" then allocated
    private String[,] grid = new String[9, 9];
    private String[] shipNames = new String[]{"Destroyer", "Submarine", "Cruiser", "Battleship", "Carrier"};
    private Tuple<int, int>[] shipSizes = new Tuple<int, int>[5]
    {
        new Tuple<int, int>(1, 2),
        new Tuple<int, int>(1, 3),
        new Tuple<int, int>(1, 3),
        new Tuple<int, int>(2, 4),
        new Tuple<int, int>(1, 5)
    };
    
    // Start is called before the first frame update
    void Start()
    {
        InitGrid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitGrid()
    {
        for (int i = 0; i < shipSizes.Length; i++)
        {
            bool placed = false;
            while (!placed)
            {
                int x = Random.Range(0, grid.GetLength(0));
                int y = Random.Range(0, grid.GetLength(1));
                bool isHorizontal = (Random.Range(0, 2) == 0);
                if (!IsOccupied(x, y, isHorizontal, i))
                {
                    Debug.Log("Placed " + shipNames[i] + (isHorizontal ? " horizontally" : " vertically") + " at (" + x + ", " + y + " | ship has size " + shipSizes[i].Item1 + " x " + shipSizes[i].Item2);
                    PlaceShip(x, y, isHorizontal, i);
                    InstantiateShipPrefab(x, y, isHorizontal, i);
                    placed = true;
                }
                else Debug.Log("Failed to spawn: " + shipNames[i] + (isHorizontal ? " horizontally" : " vertically") + " at (" + x + ", " + y + " | ship has size " + shipSizes[i].Item1 + " x " + shipSizes[i].Item2);
            }
        }

        Debug.Log("out of init for loop");
        PrintGrid();
    }
    
    void PrintGrid()
    {
        String arrayString = "Printing Grid:\n";
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                arrayString += grid[i, j] + ", ";
            }

            arrayString += "\n";
        }

        Debug.Log(arrayString);
    }

    // Take (x, y) as origin and work out if x + shipSizeX < gridLength AND y + shipSizeY < gridLength
    bool IsValidPlacement(int x, int y, bool isHorizontal, int shipIndex)
    {
        int shipSizeX, shipSizeY;
        if (isHorizontal)
        {
            shipSizeX = shipSizes[shipIndex].Item1;
            shipSizeY = shipSizes[shipIndex].Item2;
        }
        else
        {
            shipSizeY = shipSizes[shipIndex].Item1;
            shipSizeX = shipSizes[shipIndex].Item2;
        }
        return x + shipSizeX - 1 < grid.GetLength(0) && y + shipSizeY - 1 < grid.GetLength(1);
    }

    bool IsOccupied(int x, int y, bool isHorizontal, int shipIndex)
    {
        // if not valid to put on board no need to go further, so occupied
        if (!IsValidPlacement(x, y, isHorizontal, shipIndex)) return true;
        int shipSizeX, shipSizeY;
        if (isHorizontal)
        {
            shipSizeX = shipSizes[shipIndex].Item1;
            shipSizeY = shipSizes[shipIndex].Item2;
        }
        else
        {
            shipSizeY = shipSizes[shipIndex].Item1;
            shipSizeX = shipSizes[shipIndex].Item2;
        }
        
        for (int i = x; i < shipSizeX+x; i++)
        {
            for (int j = y; j < shipSizeY+y; j++)
            {
                // occupied if string not empty
                if (!string.IsNullOrEmpty(grid[i, j])) return true;
            }
        }

        return false;
    }

    void PlaceShip(int x, int y, bool isHorizontal, int shipIndex)
    {
        int shipSizeX, shipSizeY;
        if (isHorizontal)
        {
            shipSizeX = shipSizes[shipIndex].Item1;
            shipSizeY = shipSizes[shipIndex].Item2;
        }
        else
        {
            shipSizeY = shipSizes[shipIndex].Item1;
            shipSizeX = shipSizes[shipIndex].Item2;
        }
        
        for (int i = 0; i < shipSizeX; i++)
        {
            for (int j = 0; j < shipSizeY; j++)
            {
                grid[i+x, j+y] = shipNames[shipIndex];
                //Debug.Log(shipNames[shipIndex] + " at (" + (i+x) + ", " + (j+y) + ")");
            }
        }
    }

    // TODO: complete method to spawn ships
    void InstantiateShipPrefab(int x, int y, bool isHorizontal, int shipIndex)
    {
        // Find origin point by doing (x + shipSizeX/2, y + shipSizeY/2)
        // Instantiate
        // Set scale based on shipSize
    }
}
