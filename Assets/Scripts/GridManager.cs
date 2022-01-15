using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    //public Dictionary<MapGrid, Vector2> GridsDict = new Dictionary<MapGrid, Vector2>();
    public Dictionary<MapGrid, int> GridsDict = new Dictionary<MapGrid, int>();
    public MapGrid selectedGrid = null;
    public MapGrid confirmSelectedGrid = null;

    [SerializeField] private MapGrid gridPrefab;
    [SerializeField] private List<MapGrid> grids;    
    private int countlimit = 20;
    private int counter;

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
    public IEnumerator SetUpGridmap()
    {
        UIManager.Instance.ShowGameMessageText("Setting up Gridmap!");
        yield return new WaitForSeconds(2);
        int gridNumber = 0;
        foreach (MapGrid grid in grids)
        {
            GridsDict.Add(grid, gridNumber);
            gridNumber++;
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

    /// <summary>
    /// Updates the current selectedGrid. Displays the unit info on grid.
    /// </summary>
    /// <param name="grid">MapGrid to set as the selectedGrid</param>
    public void SetSelectedGrid(MapGrid grid)
    {
        selectedGrid = grid;
        if(grid.unitOnGrid == null)
        {
            UIManager.Instance.ShowMouseSelectionText("Beautiful Empty Grid");
        }
        else
        {
            UIManager.Instance.ShowMouseSelectionText(grid.unitOnGrid.unitName);
        }
    }

    /// <summary>
    /// Coroutine. Prompt user to select grid by clicking twice consecutively. Updates confirmSelectedGrid.
    /// </summary>
    /// <returns></returns>
    public IEnumerator WaitForGridSelection()
    {
        Debug.Log("Select tile! Click again to confirm!");
        confirmSelectedGrid = null;
        while (confirmSelectedGrid == null)
        {
            yield return new WaitForSeconds(0.1f);
        }
    }
}
