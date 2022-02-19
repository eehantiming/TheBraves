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
        int numberOfTowns = 0;
        foreach (MapGrid grid in grids)
        {
            grid.index = gridNumber;
            IndexToGrid.Add(gridNumber, grid);
            gridNumber++;
            if (grid.isTownGrid) numberOfTowns++;
        }
        GameManager.Instance.numberOfTownsLeft = numberOfTowns;
        GameManager.Instance.ChangeState(GameManager.GameState.SetupEnemies);
    }

    /// <summary>
    /// Function that randomly picks an unoccupied enemy MapGrid.
    /// </summary>
    /// <returns>MapGrid object. Use grid.transform.position to get position.</returns>
    public MapGrid GetEnemySpawnGrid()
    {
        //int roll = Random.Range(1, 7); // TODO: currently fixed to 6. use this value for dice throw
        int roll = DiceRoll.Instance.GenerateRoll();
        // int roll = XX.rollDice(); // TODO: create a function/coroutine somewhere to roll dice, run animation and return result
        Debug.Log("Roll: " + roll);
        int[] enemySpawnGrids = { 16, 20, 21, 22, 23, 19 };
        MapGrid spawnGrid = grids[enemySpawnGrids[roll-1]];
        counter = 0;
        while (spawnGrid.unitsOnGrid.Count > 0 && counter < countlimit)
        {
            roll = DiceRoll.Instance.GenerateRoll();; // TODO: currently fixed to 6. use this value for dice throw
            // int roll = XX.rollDice(); // TODO: create a function somewhere to roll dice, run animation and return result
            Debug.Log("Re-Roll: " + roll);
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
    /// <param name="acceptNorth">Whether to include North grid</param>
    /// <param name="acceptSouth">Whether to include South grid</param>
    /// <returns>List of valid adjacent grids</returns>
    public List<MapGrid> GetAdjacentGrids(MapGrid centerGrid, bool acceptHero=false, bool acceptEnemy=false, bool acceptNorth=true, bool acceptSouth=true)
    {
        int gridIndex = centerGrid.index;
        List<MapGrid> adjacentGrids = new List<MapGrid>();
        // Add the 4 adjacent grids
        if (acceptSouth && gridIndex > 3) adjacentGrids.Add(IndexToGrid[gridIndex - 4]); // Not on bottom row
        if (gridIndex % 4 != 0) adjacentGrids.Add(IndexToGrid[gridIndex - 1]); // Not on left edge
        if ((gridIndex + 1) % 4 != 0) adjacentGrids.Add(IndexToGrid[gridIndex + 1]); // Not on right edge
        if (acceptNorth && gridIndex < 20) adjacentGrids.Add(IndexToGrid[gridIndex + 4]); // Not on top row
        // Remove occupied grids
        for (int i = 0; i < adjacentGrids.Count; i++)
        {
            if (adjacentGrids[i].unitsOnGrid.Count > 0)
            {
                bool hasHero = false;
                bool hasEnemy = false;
                foreach(BaseUnit unit in adjacentGrids[i].unitsOnGrid)
                {
                    // can break if both are true. however grid should contain at most 3 units which is not too expensive.
                    if (unit.faction == Faction.Hero) hasHero = true;
                    else if (unit.faction == Faction.Enemy) hasEnemy = true;
                }
                if (!acceptHero && hasHero || !acceptEnemy && hasEnemy) // && resolves before ||
                {
                    adjacentGrids.RemoveAt(i);
                    i--; // recheck at index which is a new grid since earlier grid was removed
                }
            }
        }
        //foreach (MapGrid adjacentGrid in adjacentGrids)
        //{
        //    if (adjacentGrid.unitsOnGrid != null)
        //    {
        //        if (!acceptHero && adjacentGrid.unitsOnGrid.faction == Faction.Hero) adjacentGrids.Remove(adjacentGrid);
        //        if (!acceptEnemy && adjacentGrid.unitsOnGrid.faction == Faction.Enemy) adjacentGrids.Remove(adjacentGrid);
        //    }
        //}
        return adjacentGrids;
    }

    /// <summary>
    /// Updates the current selectedGrid. Displays the unit info on grid.
    /// </summary>
    /// <param name="grid">MapGrid to set as the selectedGrid</param>
    public void SetSelectedGrid(MapGrid grid)
    {
        if(selectedGrid != null) selectedGrid.ToggleOverlay(false);
        selectedGrid = grid;
        if (selectedGrid == null) return; // if input grid is null
        selectedGrid.ToggleOverlay(true);
        if(grid.unitsOnGrid.Count == 0)
        {
            UIManager.Instance.ShowMouseSelectionText("Beautiful Empty Grid");
        }
        else
        {
            string names = "";
            foreach(BaseUnit unit in grid.unitsOnGrid)
            {
                names = names + unit.unitName + "\n";
            }
            UIManager.Instance.ShowMouseSelectionText(names);
        }
    }

    /// <summary>
    /// Coroutine. Prompt user to select grid by clicking twice consecutively. Updates confirmSelectedGrid.
    /// </summary>
    /// <returns></returns>
    public IEnumerator WaitForGridSelection()
    {
        //Debug.Log("Waiting for grid selection");
        confirmSelectedGrid = null;
        while (confirmSelectedGrid == null)
        {
            yield return new WaitForSeconds(0.1f);
        }
    }
}
