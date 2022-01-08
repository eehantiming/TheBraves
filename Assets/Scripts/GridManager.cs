using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    [SerializeField] private MapGrid gridPrefab;
    [SerializeField] private List<MapGrid> grids;
    
    private int countlimit = 20;
    private int counter;
    public Dictionary<Vector2, MapGrid> GridsDict = new Dictionary<Vector2, MapGrid>();

    private void Awake()
    {
        //create a global reference
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    /// <summary>
    /// Function to generate the gameboard, which consists of MapGrids
    /// </summary>
    public void SetUpGridmap()
    {
        foreach (MapGrid grid in grids)
        {
            GridsDict.Add(new Vector2(grid.transform.localPosition.x, grid.transform.localPosition.y), grid);
        }
        GameManager.Instance.ChangeState(GameManager.GameState.SetupEnemies);
    }

    /// <summary>
    /// Function that randomly picks an unoccupied enemy MapGrid.
    /// </summary>
    /// <returns>MapGrid object. Use grid.transform.position to get position.</returns>
    public MapGrid GetEnemySpawnGrid()
    {
        int roll = Random.Range(1, 7); // TODO: currently fixed to 6. use this value for dice throw
        // int roll = XX.rollDice(); // TODO: create a function somewhere to roll dice, run animation and return result
        Debug.Log("Roll: " + roll);
        MapGrid spawnGrid = grids[roll+17];
        counter = 0;
        while (spawnGrid.unitOnGrid != null && counter < countlimit)
        {
            roll = Random.Range(1, 7); // TODO: currently fixed to 6. use this value for dice throw
            // int roll = XX.rollDice(); // TODO: create a function somewhere to roll dice, run animation and return result
            Debug.Log("Roll: " + roll);
            spawnGrid = grids[roll + 17];
            counter++; //prevents infinite loop
        }
        return spawnGrid;
    }

}
