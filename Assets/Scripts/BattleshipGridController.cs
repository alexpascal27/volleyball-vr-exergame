using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Random = UnityEngine.Random;

public class BattleshipGridController : MonoBehaviour
{
    [SerializeField] private Vector3 gridOrigin = new Vector3(-4.5f, 0.0f, 9.1f);
    // if when moving From A to B, we are decreasing in Z value or not
    [SerializeField] private bool zDescending = true;
    // if when moving From A to B, we are increasing in X value or not
    [SerializeField] private bool xAscending = true;
    [SerializeField] private GameObject shipPrefab;
    [SerializeField] private float yShipSpawnOffset = 0.5f;
    
    
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
                    //Debug.Log(shipNames[i] + ": (" + x + ", " + y + ")");
                    PlaceShip(x, y, isHorizontal, i);
                    InstantiateShipPrefab(x, y, isHorizontal, i);
                    placed = true;
                }
            }
        }
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
        int shipSizeX = GetShipSizeXAndY(isHorizontal, shipIndex).Item1;
        int shipSizeY = GetShipSizeXAndY(isHorizontal, shipIndex).Item2;
        return x + shipSizeX - 1 < grid.GetLength(0) && y + shipSizeY - 1 < grid.GetLength(1);
    }

    bool IsOccupied(int x, int y, bool isHorizontal, int shipIndex)
    {
        // if not valid to put on board no need to go further, so occupied
        if (!IsValidPlacement(x, y, isHorizontal, shipIndex)) return true;
        int shipSizeX = GetShipSizeXAndY(isHorizontal, shipIndex).Item1;
        int shipSizeY = GetShipSizeXAndY(isHorizontal, shipIndex).Item2;
        
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
        int shipSizeX = GetShipSizeXAndY(isHorizontal, shipIndex).Item1;
        int shipSizeY = GetShipSizeXAndY(isHorizontal, shipIndex).Item2;
        
        for (int i = 0; i < shipSizeX; i++)
        {
            for (int j = 0; j < shipSizeY; j++)
            {
                grid[i+x, j+y] = shipNames[shipIndex];
            }
        }
    }

    Tuple<int, int> GetShipSizeXAndY(bool isHorizontal, int shipIndex)
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

        return new Tuple<int, int>(shipSizeX, shipSizeY);
    }

    // TODO: complete method to spawn ships
    void InstantiateShipPrefab(int z, int x, bool isHorizontal, int shipIndex)
    {
        float shipSizeZ = GetShipSizeXAndY(isHorizontal, shipIndex).Item1;
        float shipSizeX = GetShipSizeXAndY(isHorizontal, shipIndex).Item2;
        // Find origin point by doing (x + shipSizeX/2, z + shipSizeZ/2)
        float originX = xAscending ? gridOrigin.x + x + shipSizeX / 2 : gridOrigin.x - x - shipSizeX / 2;
        float originZ = zDescending ? gridOrigin.z - z - shipSizeZ / 2 : gridOrigin.z + z + shipSizeZ / 2;
        Vector3 origin = new Vector3(originX, yShipSpawnOffset, originZ);
        // Instantiate
        GameObject instantiatedShipPrefab = Instantiate(shipPrefab, origin, shipPrefab.transform.rotation);
        instantiatedShipPrefab.name = shipNames[shipIndex];
        // Rotate if vertical
        // TODO: when you add an actual model, not important now as just cube
        // Set scale based on shipSize
        instantiatedShipPrefab.transform.localScale = new Vector3(shipSizeX, instantiatedShipPrefab.transform.localScale.y, shipSizeZ);
    }
}
