using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance;
    [SerializeField] private BaseUnit smallEnemyPrefab, bigEnemyPrefab, giantEnemyPrefab, heartPrefab;
    [SerializeField] private BaseUnit swordsmanPrefab, trapperPrefab, magicianPrefab;
    private int smallEnemyCount = 0; 

    public Swordsman swordsman = null;
    public Trapper trapper = null;
    public Magician magician = null;
    public List<SmallEnemy> smallEnemies;
    public bool smallEnemyCanDie = false; //dies if move into player or trap
    public BigEnemy bigEnemy = null;
    public GiantEnemy giantEnemy = null;
    public BaseUnit heart = null;

    public BaseUnit activeUnit = null;
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Spawns unit on Grid.
    /// </summary>
    /// <param name="prefab">Unit prefab to spawn</param>
    /// <param name="grid">Grid to spawn in</param>
    /// <param name="unitName">String to set as unit name</param>
    /// <returns>BaseUnit spawned</returns>
    BaseUnit SpawnUnit(BaseUnit prefab, MapGrid grid, string unitName=null)
    {
        BaseUnit spawnedUnit = Instantiate(prefab, grid.transform.position, Quaternion.identity);
        if (unitName!=null)
        {
            spawnedUnit.unitName = unitName;
        }
        grid.unitsOnGrid.Add(spawnedUnit);
        spawnedUnit.currentGrid = grid;
        return spawnedUnit;
    }

    /// <summary>
    /// Spawns a small enemy on an unoccupied enemy spawn grid and add it to the small enemies data structure.
    /// </summary>
    public IEnumerator SpawnSmallEnemy()
    {
        UIManager.Instance.ShowGameMessageText("Small Monster Appears!");
        yield return new WaitForSeconds(1);
        smallEnemyCount++;
        smallEnemies.Add((SmallEnemy)SpawnUnit(smallEnemyPrefab, GridManager.Instance.GetEnemySpawnGrid(), $"Small Monster {smallEnemyCount}")); // TODO: add spawn animation
        smallEnemies.Add((SmallEnemy)SpawnUnit(smallEnemyPrefab, GridManager.Instance.IndexToGrid[8])); // DEBUG
        //smallEnemies.Add((SmallEnemy)SpawnUnit(smallEnemyPrefab, GridManager.Instance.IndexToGrid[16])); // DEBUG
        //GameManager.Instance.ChangeState(GameManager.GameState.EnemyPhase); // DEBUG
        GameManager.Instance.ChangeState(GameManager.GameState.SetupSwordsman);
    }

    /// <summary>
    /// Coroutine. Prompts user to select grid, then spawn Swordsman there. TODO: combine into SpawnHeroes(heroType)
    /// </summary>
    /// <returns>Changes game state once done.</returns>
    public IEnumerator SpawnSwordsman()
    {
        UIManager.Instance.ShowGameMessageText("Double click Grid to spawn Swordsman");
        // TODO: display the valid grids selection
        yield return StartCoroutine(GridManager.Instance.WaitForGridSelection()); //yield return required to wait for this to complete
        while(!GridManager.Instance.confirmSelectedGrid.isHeroSpawnGrid || GridManager.Instance.confirmSelectedGrid.unitsOnGrid.Count > 0)
        {
            Debug.Log("Invalid grid");
            UIManager.Instance.ShowGameMessageText("Choose a valid grid!");
            yield return StartCoroutine(GridManager.Instance.WaitForGridSelection()); //yield return required to wait for this to complete
        }
        swordsman = (Swordsman)SpawnUnit(swordsmanPrefab, GridManager.Instance.confirmSelectedGrid);
        UIManager.Instance.ShowGameMessageText(null);
        GameManager.Instance.ChangeState(GameManager.GameState.SetupTrapper);
    }

    /// <summary>
    /// Coroutine. Prompts user to select grid, then spawn Trapper there.
    /// </summary>
    /// <returns>Changes game state once done.</returns>
    public IEnumerator SpawnTrapper()
    {
        UIManager.Instance.ShowGameMessageText("Double click Grid to spawn Trapper");
        // TODO: display the valid grids selection
        yield return StartCoroutine(GridManager.Instance.WaitForGridSelection()); //yield return required to wait for this to complete
        while (!GridManager.Instance.confirmSelectedGrid.isHeroSpawnGrid || GridManager.Instance.confirmSelectedGrid.unitsOnGrid.Count > 0)
        {
            Debug.Log("Invalid grid");
            UIManager.Instance.ShowGameMessageText("Choose a valid grid!");
            yield return StartCoroutine(GridManager.Instance.WaitForGridSelection()); //yield return required to wait for this to complete
        }
        trapper = (Trapper)SpawnUnit(trapperPrefab, GridManager.Instance.confirmSelectedGrid);
        UIManager.Instance.ShowGameMessageText(null);
        GameManager.Instance.ChangeState(GameManager.GameState.SetupMagician);
    }

    /// <summary>
    /// Coroutine. Prompts user to select grid, then spawn Magician there.
    /// </summary>
    /// <returns>Changes game state once done.</returns>
    public IEnumerator SpawnMagician()
    {
        UIManager.Instance.ShowGameMessageText("Double click Grid to spawn Magician");
        // TODO: display the valid grids selection
        yield return StartCoroutine(GridManager.Instance.WaitForGridSelection()); //yield return required to wait for this to complete
        while (!GridManager.Instance.confirmSelectedGrid.isHeroSpawnGrid || GridManager.Instance.confirmSelectedGrid.unitsOnGrid.Count > 0)
        {
            Debug.Log("Invalid grid");
            UIManager.Instance.ShowGameMessageText("Choose a valid grid!");
            yield return StartCoroutine(GridManager.Instance.WaitForGridSelection()); //yield return required to wait for this to complete
        }
        magician = (Magician)SpawnUnit(magicianPrefab, GridManager.Instance.confirmSelectedGrid);
        UIManager.Instance.ShowGameMessageText(null);
        GameManager.Instance.ChangeState(GameManager.GameState.SwordsmanPhase);
    }

    public IEnumerator SpawnBigEnemy()
    {
        UIManager.Instance.ShowGameMessageText("Big Monster Appears!");
        yield return new WaitForSeconds(1);
        bigEnemy = (BigEnemy)SpawnUnit(bigEnemyPrefab, GridManager.Instance.GetEnemySpawnGrid()); // TODO: add spawn animation
    }

    public IEnumerator SpawnGiantEnemy()
    {
        UIManager.Instance.ShowGameMessageText("Giant Monster Appears!!");
        yield return new WaitForSeconds(1);
        giantEnemy = (GiantEnemy)SpawnUnit(giantEnemyPrefab, GridManager.Instance.GetEnemySpawnGrid()); // TODO: add spawn animation
    }

    public IEnumerator SpawnHeart()
    {
        UIManager.Instance.ShowGameMessageText("Monster Heart Appears!!");
        yield return new WaitForSeconds(1);
        MapGrid spawnGrid = GridManager.Instance.GetEnemySpawnGrid();
        heart = Instantiate(heartPrefab, spawnGrid.transform.position, Quaternion.identity);
        heart.currentGrid = spawnGrid;
        spawnGrid.isHoldingHeart = true;
        //heart = SpawnUnit(heartPrefab, GridManager.Instance.GetEnemySpawnGrid()); // TODO: add spawn animation
        yield return new WaitForSeconds(1);
    }

    /// <summary>
    /// Sets current active unit. Displays this info
    /// </summary>
    /// <param name="unit">BaseUnit to set</param>
    public void SetActiveUnit(BaseUnit unit)
    {
        activeUnit = unit;
        if(activeUnit == null)
        {
            UIManager.Instance.ShowActiveUnitText(null);
        }
        else
        {
            UIManager.Instance.ShowActiveUnitText(activeUnit.unitName);
        }
    }
    // Wrapper for UseSkill Coroutine so that button can access it
    public void ButtonUseSkill()
    {
        Debug.Log("Clicked skill..");
        activeUnit.ActivateSkill();
    }



    // Wrapper for PlayerMove Coroutine so that button can access it
    public void ButtonPlayerMove()
    {
        Debug.Log("Clicked move..");
        StartCoroutine(PlayerMove());
    }

    /// <summary>
    /// Coroutine. Prompt user to select grid, then moves active unit to that grid.
    /// </summary>
    /// <returns></returns>
    public IEnumerator PlayerMove()
    {
        List<MapGrid> validGrids = GridManager.Instance.GetAdjacentGrids(activeUnit.currentGrid, false, false);
        if (validGrids.Count == 0)
        {
            UIManager.Instance.ShowGameMessageText("This unit can't move!");
            Debug.Log("There are no valid grids to move to");
            yield break;
        }
        // TODO: display valid move grids
        UIManager.Instance.ShowGameMessageText("Select Grid to move to");
        yield return StartCoroutine(GridManager.Instance.WaitForGridSelection());
        // Check if selected grid is a valid move
        while (!validGrids.Contains(GridManager.Instance.confirmSelectedGrid))
        {
            UIManager.Instance.ShowGameMessageText("Select Valid Grid to move to");
            yield return StartCoroutine(GridManager.Instance.WaitForGridSelection());
        }
        activeUnit.Move(GridManager.Instance.confirmSelectedGrid);
        yield return new WaitForSeconds(1);
        activeUnit.GetComponent<HeroUnit>().EndTurn();
    }

    /// <summary>
    /// Checks if any hero is conscious. if not, call PlayerLose
    /// </summary>
    /// <returns>false if all are unconscious</returns>
    public bool CheckConsciousness()
    {
        if (!swordsman.isConscious && !trapper.isConscious && !magician.isConscious)
        {
            Debug.Log("All heroes are unconscious");
            UIManager.Instance.ShowGameMessageText("All heroes are unconscious!");
            GameManager.Instance.PlayerLose();
            return (false);
        }
        return true;
    }
}
