using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    //public Dictionary<MapGrid, Vector2> GridToPosition = new Dictionary<MapGrid, Vector2>();
    public Dictionary<int, MapGrid> IndexToGrid = new Dictionary<int, MapGrid>();
    public MapGrid selectedGrid = null;
    public MapGrid confirmSelectedGrid = null;

    [SerializeField] private List<MapGrid> grids;    
    private int countlimit = 20;
    private int counter;

    private void Awake()
    {
        //create a global reference
        Instance = this;
    }

    /// <summary>
    /// Function to set up references to MapGrids
    /// </summary>
    public IEnumerator SetUpGridmap()
    {
        UIManager.Instance.ShowGameMessageText("Setting up Gridmap!");
        yield return new WaitForSeconds(1);
        int gridNumber = 0;
        foreach (MapGrid grid in grids)
        {
            grid.index = gridNumber;
            IndexToGrid.Add(gridNumber, grid);
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
        // int roll = XX.rollDice(); // TODO: create a function/coroutine somewhere to roll dice, run animation and return result
        Debug.Log("Roll: " + roll);
        int[] enemySpawnGrids = { 16, 20, 21, 22, 23, 19 };
        MapGrid spawnGrid = grids[enemySpawnGrids[roll-1]];
        counter = 0;
        while (spawnGrid.unitOnGrid != null && counter < countlimit)
        {
            roll = Random.Range(1, 7); // TODO: currently fixed to 6. use this value for dice throw
            // int roll = XX.rollDice(); // TODO: create a function somewhere to roll dice, run animation and return result
            Debug.Log("Roll: " + roll);
            spawnGrid = grids[enemySpawnGrids[roll-1]];
            counter++; //prevents infinite loop
        }
        UIManager.Instance.ShowGameMessageText($"Spawning Enemy on {roll}");
        return spawnGrid;
    }

    /// <summary>
    /// Function to get the available adjacent grids (up to 4) to the input MapGrid.
    /// </summary>
    /// <param name="centerGrid">The input MapGrid</param>
    /// <param name="acceptHero">Whether adjacent grid can contain a Hero</param>
    /// <param name="acceptEnemy">Whether adjacent grid can contain an Enemy</param>
    /// <returns></returns>
    public List<MapGrid> GetAdjacentGrids(MapGrid centerGrid, bool acceptHero=false, bool acceptEnemy=false)
    {
        //int gridIndex = GridToIndex[centerGrid];
        int gridIndex = centerGrid.index;
        List<MapGrid> adjacentGrids = new List<MapGrid>();
        // Add the 4 adjacent grids
        if (gridIndex > 3) adjacentGrids.Add(IndexToGrid[gridIndex - 4]); // Not on bottom row
        if (gridIndex % 4 != 0) adjacentGrids.Add(IndexToGrid[gridIndex - 1]); // Not on left edge
        if ((gridIndex + 1) % 4 != 0) adjacentGrids.Add(IndexToGrid[gridIndex + 1]); // Not on right edge
        if (gridIndex < 20) adjacentGrids.Add(IndexToGrid[gridIndex + 4]); // Not on top row
        // Remove occupied grids
        foreach (MapGrid adjacentGrid in adjacentGrids)
        {
            if (adjacentGrid.unitOnGrid != null)
            {
                if (!acceptHero && adjacentGrid.unitOnGrid.faction == Faction.Hero) adjacentGrids.Remove(adjacentGrid);
                if (!acceptEnemy && adjacentGrid.unitOnGrid.faction == Faction.Enemy) adjacentGrids.Remove(adjacentGrid);
            }
        }
        return adjacentGrids;
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
