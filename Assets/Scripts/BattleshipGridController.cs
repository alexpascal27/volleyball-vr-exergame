using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class BattleshipGridController : MonoBehaviour
{
    private const string BATTLESHIP_USER_SHIP_HITS = "battleshipUserShipHits";
    private const string BATTLESHIP_OPPONENT_SHIP_HITS = "battleshipOpponentShipHit";
    
    public bool isUser;
    [SerializeField] private Vector3 gridOrigin = new Vector3(-4.5f, 0.0f, 9.1f);
    // if when moving From A to B, we are decreasing in Z value or not
    [SerializeField] private bool zDescending = true;
    // if when moving From A to B, we are increasing in X value or not
    [SerializeField] private bool xAscending = true;
    [SerializeField] private Material shipSinkMaterial;
    [SerializeField] private GameObject[] shipModelPrefabs;

    public GameObject hitExplosionVFXPrefab;
    public GameObject sinkExplosionVFXPrefab;
    
    // if [][] = "" unallocated, if [][] = "{ship_name}" then allocated
    private const int GridDimensionsX = 9;
    private const int GridDimensionsY = 9;
    private String[,] grid = new String[GridDimensionsY, GridDimensionsX];
    private bool[,] hasBeenHitGrid = new bool[GridDimensionsY, GridDimensionsX];
    private String[] shipNames = new String[]{"Destroyer", "Submarine", "Cruiser", "Battleship", "Carrier"};
    private int[] shipTileCount = new int[] { 2, 3, 3, 8, 5 };
    private GameObject[] shipPrefabs = new GameObject[5];
    private Tuple<int, int>[] shipSizes = new Tuple<int, int>[]
    {
        new Tuple<int, int>(1, 2), // https://www.cgtrader.com/free-3d-models/watercraft/industrial-watercraft/polygrunt-low-poly-boat-or-water-craft-or-sea-vehicle
        new Tuple<int, int>(1, 3), // https://www.cgtrader.com/free-3d-models/watercraft/military-watercraft/war-ship-ae93c071-d5d0-44a3-9100-eae86b5b5182
        new Tuple<int, int>(1, 3), // ^
        new Tuple<int, int>(2, 4), // https://www.cgtrader.com/free-3d-models/watercraft/industrial-watercraft/toon-ship
        new Tuple<int, int>(1, 5) // https://www.cgtrader.com/free-3d-models/watercraft/industrial-watercraft/low-poly-cargo-ship-688d0170-b2eb-4198-9f09-22b6aee72249
    };

    private Dictionary<char, int> rowNameToIndex = new Dictionary<char, int>();

    void Start()
    {
        InitGrid();
        InitRowNameToIndexDictionary();
    }

    void Update()
    {
        // check if all sunk
        bool allShipsSunk = shipTileCount.Sum() == 0;
        if (allShipsSunk)
        {
            if (isUser)
            {
                SceneManager.LoadScene(4);
            }
            else
            {
                SceneManager.LoadScene(5);
            }
        }
    }

    void InitGrid()
    {
        for (int i = 0; i < shipSizes.Length; i++)
        {
            bool placed = false;
            while (!placed)
            {
                int y = Random.Range(0, grid.GetLength(0));
                int x = Random.Range(0, grid.GetLength(1));
                bool isHorizontal = (Random.Range(0, 2) == 0);
                if (!IsOccupied(y, x, isHorizontal, i))
                {
                    //Debug.Log(shipNames[i] + ": (" + x + ", " + y + ")");
                    PlaceShip(y, x, isHorizontal, i);
                    InstantiateShipPrefab(y, x, isHorizontal, i);
                    placed = true;
                }
            }
        }
        //PrintGrid();
    }

    void InitRowNameToIndexDictionary()
    {
        rowNameToIndex.Add('A', 0);
        rowNameToIndex.Add('B', 1);
        rowNameToIndex.Add('C', 2);
        rowNameToIndex.Add('D', 3);
        rowNameToIndex.Add('E', 4);
        rowNameToIndex.Add('F', 5);
        rowNameToIndex.Add('G', 6);
        rowNameToIndex.Add('H', 7);
        rowNameToIndex.Add('I', 8);
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
    bool IsValidPlacement(int y, int x, bool isHorizontal, int shipIndex)
    {
        int shipSizeY = GetShipSizeYAndX(isHorizontal, shipIndex).Item1;
        int shipSizeX = GetShipSizeYAndX(isHorizontal, shipIndex).Item2;
        return x + shipSizeX - 1 < grid.GetLength(1) && y + shipSizeY - 1 < grid.GetLength(0);
    }

    bool IsOccupied(int y, int x, bool isHorizontal, int shipIndex)
    {
        // if not valid to put on board no need to go further, so occupied
        if (!IsValidPlacement(y, x, isHorizontal, shipIndex)) return true;
        int shipSizeY = GetShipSizeYAndX(isHorizontal, shipIndex).Item1;
        int shipSizeX = GetShipSizeYAndX(isHorizontal, shipIndex).Item2;
        
        for (int i = y; i < shipSizeY+y; i++)
        {
            for (int j = x; j < shipSizeX+x; j++)
            {
                // occupied if string not empty
                if (!string.IsNullOrEmpty(grid[i, j])) return true;
            }
        }

        return false;
    }

    void PlaceShip(int y, int x, bool isHorizontal, int shipIndex)
    {
        int shipSizeY = GetShipSizeYAndX(isHorizontal, shipIndex).Item1;
        int shipSizeX = GetShipSizeYAndX(isHorizontal, shipIndex).Item2;
        
        for (int i = 0; i < shipSizeY; i++)
        {
            for (int j = 0; j < shipSizeX; j++)
            {
                grid[i+y, j+x] = shipNames[shipIndex];
            }
        }
    }

    Tuple<int, int> GetShipSizeYAndX(bool isHorizontal, int shipIndex)
    {
        int shipSizeX, shipSizeY;
        if (isHorizontal)
        {
            shipSizeY = shipSizes[shipIndex].Item1;
            shipSizeX = shipSizes[shipIndex].Item2;
        }
        else
        {
            shipSizeX = shipSizes[shipIndex].Item1;
            shipSizeY = shipSizes[shipIndex].Item2;
        }

        return new Tuple<int, int>(shipSizeY, shipSizeX);
    }
    
   void InstantiateShipPrefab(int z, int x, bool isHorizontal, int shipIndex)
    {
        float shipSizeZ = GetShipSizeYAndX(isHorizontal, shipIndex).Item1;
        float shipSizeX = GetShipSizeYAndX(isHorizontal, shipIndex).Item2;
        // Find origin point by doing (x + shipSizeX/2, z + shipSizeZ/2)
        float originX = xAscending ? gridOrigin.x + x + shipSizeX / 2 : gridOrigin.x - x - shipSizeX / 2;
        float originZ = zDescending ? gridOrigin.z - z - shipSizeZ / 2 : gridOrigin.z + z + shipSizeZ / 2;
        Vector3 origin = new Vector3(originX, 0f, originZ);
        // Instantiate
        // Rotate if horizontal
        Vector3 defaultShipPrefabRotation = shipModelPrefabs[shipIndex].transform.rotation.eulerAngles;
        GameObject instantiatedShipPrefab = Instantiate(shipModelPrefabs[shipIndex], origin + transform.position, isHorizontal ? Quaternion.Euler(new Vector3(defaultShipPrefabRotation.x, defaultShipPrefabRotation.y - 90f ,defaultShipPrefabRotation.z))  : Quaternion.Euler(defaultShipPrefabRotation));
        
        // weird offset that needs to be applied only to 5th ship
        if (shipIndex == 4) instantiatedShipPrefab.transform.position = new Vector3(isHorizontal ? origin.x - 1f : origin.x, origin.y, isHorizontal ? origin.z : origin.z + 1f) + transform.position;
        instantiatedShipPrefab.name = shipNames[shipIndex];
        shipPrefabs[shipIndex] = instantiatedShipPrefab;
    }
   
   
   // Bool represents ifSuccessfulHit (true if hit, false if not hit)
   public bool RegisterHit(String tileName, Vector3 tilePosition)
   {
       // y, x
       Tuple<int, int> tileCoordinates = ConvertTileNameToCoordinates(tileName);
       // check status of tile
       bool isHit = CheckIfHit(tileCoordinates);

       bool alreadyHit = hasBeenHitGrid[tileCoordinates.Item1, tileCoordinates.Item2];
       // if new hit
       if (!alreadyHit && isHit) RegisterHitToShip(tileCoordinates, tilePosition);
       else
       {
           //Debug.Log("Repeat hit at " + tileName);
       }

       return isHit;
   }

   Tuple<int, int> ConvertTileNameToCoordinates(String tileName)
   {
       char rowName = tileName[0];
       int rowNumber = rowNameToIndex[rowName];
       int tileNumber = int.Parse(tileName[1].ToString());
       return new Tuple<int, int>(rowNumber, tileNumber);
   }

   bool CheckIfHit(Tuple<int, int> coordinates)
   {
       int y = coordinates.Item1;
       int x = coordinates.Item2;
       // if empty not a hit
       return !string.IsNullOrEmpty(grid[y, x]);
   }

   void RegisterHitToShip(Tuple<int, int> coordinates, Vector3 tilePosition)
   {
       int y = coordinates.Item1;
       int x = coordinates.Item2;

       int shipIndex = GetShipIndex(grid[y, x]);
       if(shipIndex==-1) Debug.LogError("Failure to get ship index");
       shipTileCount[shipIndex] = shipTileCount[shipIndex] - 1;
       hasBeenHitGrid[y, x] = true;
       if (isUser) IncrementUserShipHits();
       else IncrementOpponentShipHits();
       Instantiate(hitExplosionVFXPrefab, tilePosition, hitExplosionVFXPrefab.transform.rotation);
       
       bool isWholeShipSunk = shipTileCount[shipIndex] == 0;

       if (isWholeShipSunk)
       {
           ChangeShipMaterial(shipIndex);
           Instantiate(sinkExplosionVFXPrefab, shipPrefabs[shipIndex].transform.position,
               sinkExplosionVFXPrefab.transform.rotation);
           //ChangeTileMaterial(y, x);
       }
       
   }

   int GetShipIndex(String shipName)
   {
       for (int i = 0; i < shipNames.GetLength(0); i++)
       {
           if (shipName == shipNames[i]) return i;
       }

       return -1;
   }

   void ChangeShipMaterial(int shipIndex)
   {
       GameObject currentShipPrefab = shipPrefabs[shipIndex];
       Renderer[] renderers = currentShipPrefab.GetComponentsInChildren<Renderer>();
       foreach (Renderer renderer in renderers)
       {
           renderer.material = shipSinkMaterial;
       }
   }

   void ChangeTileMaterial(int y, int x)
   {
       GameObject tilePrefab = gameObject.transform.GetChild(y).GetChild(x).gameObject;
       BattleshipTileController battleshipTileController = tilePrefab.GetComponent<BattleshipTileController>();
       battleshipTileController.ChangeTileMaterialShipSunk();
   }

   private void IncrementUserShipHits()
   {
       int userHits = GetUserShipHits();
       PlayerPrefs.SetInt(BATTLESHIP_USER_SHIP_HITS, userHits + 1);
       PlayerPrefs.Save();
   }

   private void IncrementOpponentShipHits()
   {
       int opponentHits = GetOpponentShipHits();
       PlayerPrefs.SetInt(BATTLESHIP_OPPONENT_SHIP_HITS, opponentHits + 1);
       PlayerPrefs.Save();
   }

   private int GetUserShipHits()
   {
       return PlayerPrefs.GetInt(BATTLESHIP_USER_SHIP_HITS);
   }

   private int GetOpponentShipHits()
   {
       return PlayerPrefs.GetInt(BATTLESHIP_OPPONENT_SHIP_HITS);
   }
   
}
