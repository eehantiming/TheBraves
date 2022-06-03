using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    const int MapHeight = 6;
    const int MapWidth = 4;
    public static GridManager Instance;
    public Dictionary<int, MapGrid> IndexToGrid = new Dictionary<int, MapGrid>();
    public MapGrid selectedGrid = null;
    public MapGrid confirmSelectedGrid = null;
    public GameObject bait;
    public List<MapGrid> townGrids = new List<MapGrid>();
    public bool clickedOutside;

    [SerializeField] private GameObject baitPrefab;
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
            if (grid.isTownGrid)
            {
                townGrids.Add(grid);
            }
        }
        GameManager.Instance.ChangeState(GameManager.GameState.SetupEnemies);
    }

    /// <summary>
    /// Function that randomly picks an unoccupied enemy MapGrid.
    /// </summary>
    /// <returns>MapGrid object. Use grid.transform.position to get position.</returns>
    public MapGrid GetEnemySpawnGrid()
    {
        Debug.Log("Rolling for enemy spawn grid");
        int roll = DiceRoll.Instance.GenerateRoll();
        int[] enemySpawnGrids = {16, 20, 21, 22, 23, 19};
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
                //bool hasHero = false;
                //bool hasEnemy = false;
                //if (adjacentGrids[i].heroesOnGrid.Count > 0) hasHero = true;
                //if (adjacentGrids[i].enemiesOnGrid.Count > 0) hasEnemy = true;
                //foreach (BaseUnit unit in adjacentGrids[i].unitsOnGrid)
                //{
                //    // can break if both are true. however grid should contain at most 3 units which is not too expensive.
                //    if (unit.faction == Faction.Hero) hasHero = true;
                //    else if (unit.faction == Faction.Enemy) hasEnemy = true;
                //}
                if (!acceptHero && (adjacentGrids[i].heroesOnGrid.Count > 0) || !acceptEnemy && (adjacentGrids[i].enemiesOnGrid.Count > 0)) // && resolves before ||
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

        string gridInfo = "";
        string invInfo = "";
        if(grid.unitsOnGrid.Count == 0)
        {
            if(grid.isTownGrid)
            {
                gridInfo = "Peaceful town: \nDo not let the monsters reach both towns";
            }
            else if(grid.isEnemySpawnGrid)
            {
                gridInfo = "Monster cave: \nMonsters spawns here base on dice roll";
            }
            else
            {
                gridInfo = "Nothing here";

            }
            UIManager.Instance.ShowMouseSelectionText("Beautiful Empty Grid");
            UIManager.Instance.ShowMouseSelectionExtraInfo(gridInfo);
            UIManager.Instance.ShowMouseSelectionInv(invInfo);

        }
        else
        {
            string names = "";
            string extraInfo = "";
            string inventory = "";
            foreach(BaseUnit unit in grid.unitsOnGrid)
            {
                names = names + unit.unitName + "\n";
                extraInfo = extraInfo + unit.extraText + "\n";
                if(unit.unitName == "Trapper")
                {
                    inventory = "Traps available: " + unit.inventory;
                }
            }
            UIManager.Instance.ShowMouseSelectionText(names);
            UIManager.Instance.ShowMouseSelectionExtraInfo(extraInfo);
            UIManager.Instance.ShowMouseSelectionInv(inventory);

        }
    }

    /// <summary>
    /// Coroutine. Prompt user to select grid by clicking grid twice consecutively. Updates confirmSelectedGrid.
    /// </summary>
    /// <param name="cancellable">Whether this wait can be cancelled by clicking outside the gridmap.</param>
    /// <returns></returns>
    public IEnumerator WaitForGridSelection(bool cancellable=true)
    {
        //Debug.Log("Waiting for grid selection");
        confirmSelectedGrid = null;
        clickedOutside = false;
        while (confirmSelectedGrid == null)
        {
            if (cancellable && clickedOutside) // Player cancelled this action.
                {
                    GridManager.Instance.ToggleActionDarken(false, grids);
                    break;
                }
            yield return new WaitForSeconds(0.1f);
        }
    }

    /// <summary>
    /// Get the grid at given coordinates e.g. (3,4).
    /// </summary>
    /// <param name="position">Input grid coordinates (x,y).</param>
    /// <returns>The MapGrid at this coordinate.</returns>
    public MapGrid GetGridFromPosition(Vector2Int position)
    {
        int index = position.y * 4 + position.x; 
        return IndexToGrid[index];
    }

    /// <summary>
    /// Checks if coordinate is within the board.
    /// </summary>
    /// <param name="pos">Vector2Int(x,y) to check.</param>
    /// <returns>True if it is within the board.</returns>
    public bool CheckPosInBoard(Vector2Int pos)
    {
        if (pos.x < 0 || pos.x >= MapWidth || pos.y < 0 || pos.y >= MapHeight)
            return false;
        return true;
    }

    /// <summary>
    /// Gets all grids which are linearly 2 units away from the input MapGrid
    /// </summary>
    /// <param name="centerGrid">The input MapGrid</param>
    /// <returns>List of valid MapGrids</returns>
    public List<MapGrid> GetTeleportGrids(MapGrid centerGrid)
    {
        List<MapGrid> ValidGrids = new List<MapGrid>();
        if (centerGrid.IndexToVect().y >= 2)
            ValidGrids.Add(GetGridFromPosition(new Vector2Int(centerGrid.IndexToVect().x, centerGrid.IndexToVect().y - 2)));
        if (centerGrid.IndexToVect().y <= MapHeight - 3)
            ValidGrids.Add(GetGridFromPosition(new Vector2Int(centerGrid.IndexToVect().x, centerGrid.IndexToVect().y + 2)));
        if (centerGrid.IndexToVect().x >= 2)
            ValidGrids.Add(GetGridFromPosition(new Vector2Int(centerGrid.IndexToVect().x - 2, centerGrid.IndexToVect().y)));
        if (centerGrid.IndexToVect().x <= MapWidth - 3)
            ValidGrids.Add(GetGridFromPosition(new Vector2Int(centerGrid.IndexToVect().x + 2, centerGrid.IndexToVect().y)));
        ValidGrids = ValidGrids.FindAll(grid => grid.unitsOnGrid.Count == 0);
        return ValidGrids;
    }

    /// <summary>
    /// Function to darken or revert every grid that is not a hero spawn grid
    /// </summary>
    /// <param name="darken">whether to darken or remove darkness</param>
    public void ToggleDarken(bool darken)
    {
        foreach(MapGrid grid in grids)
        {
            if (!grid.isHeroSpawnGrid)
            {
                grid.ToggleDarken(darken);
            }
        }
    }

    public void ToggleActionDarken(bool darken, List<MapGrid> inputGrids)
    {
        foreach(MapGrid grid in grids)
        {
            if (inputGrids.Contains(grid))
            {
                grid.ToggleDarken(darken);
            }
        }
    }
}
