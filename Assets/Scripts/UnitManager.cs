using System;
using System.Collections;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance;
    [SerializeField] private BaseUnit smallEnemyPrefab;
    [SerializeField] private BaseUnit swordsmanPrefab, trapperPrefab, magicianPrefab;

    public Swordsman swordsman = null;
    public Trapper trapper = null;
    public Magician magician = null;

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
    BaseUnit SpawnUnit(BaseUnit prefab, MapGrid grid)
    {
        BaseUnit spawnedUnit = Instantiate(prefab, grid.transform.position, Quaternion.identity);
        grid.unitOnGrid = spawnedUnit;
        spawnedUnit.currentGrid = grid;
        return spawnedUnit;
    }

    /// <summary>
    /// Spawns a small enemy on an unoccupied enemy spawn grid and add it to the small enemies data structure.
    /// </summary>
    public IEnumerator SpawnSmallEnemy()
    {
        UIManager.Instance.ShowGameMessageText("Spawning Small Enemy");
        yield return new WaitForSeconds(2);
        SpawnUnit(smallEnemyPrefab, GridManager.Instance.GetEnemySpawnGrid()); // TODO: add smallenemies data structure and add this
        GameManager.Instance.ChangeState(GameManager.GameState.SetupSwordsman);
    }

    /// <summary>
    /// Coroutine. Prompts user to select grid, then spawn Swordsman there. TODO: combine into SpawnHeroes(heroType)
    /// </summary>
    /// <returns>Changes game state once done.</returns>
    public IEnumerator SpawnSwordsman()
    {
        UIManager.Instance.ShowGameMessageText("Select Grid to spawn Swordsman");
        // TODO: display the valid grids selection
        yield return StartCoroutine(GridManager.Instance.WaitForGridSelection()); //yield return required to wait for this to complete
        while(!GridManager.Instance.confirmSelectedGrid.isHeroSpawnGrid || GridManager.Instance.confirmSelectedGrid.unitOnGrid != null)
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
        UIManager.Instance.ShowGameMessageText("Select Grid to spawn Trapper");
        // TODO: display the valid grids selection
        yield return StartCoroutine(GridManager.Instance.WaitForGridSelection()); //yield return required to wait for this to complete
        while (!GridManager.Instance.confirmSelectedGrid.isHeroSpawnGrid || GridManager.Instance.confirmSelectedGrid.unitOnGrid != null)
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
        UIManager.Instance.ShowGameMessageText("Select Grid to spawn Magician");
        // TODO: display the valid grids selection
        yield return StartCoroutine(GridManager.Instance.WaitForGridSelection()); //yield return required to wait for this to complete
        while (!GridManager.Instance.confirmSelectedGrid.isHeroSpawnGrid || GridManager.Instance.confirmSelectedGrid.unitOnGrid != null)
        {
            Debug.Log("Invalid grid");
            UIManager.Instance.ShowGameMessageText("Choose a valid grid!");
            yield return StartCoroutine(GridManager.Instance.WaitForGridSelection()); //yield return required to wait for this to complete
        }
        magician = (Magician)SpawnUnit(magicianPrefab, GridManager.Instance.confirmSelectedGrid);
        UIManager.Instance.ShowGameMessageText(null);
        GameManager.Instance.ChangeState(GameManager.GameState.SwordsmanPhase);
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
    // Wrapper for PlayerMove Coroutine so that button can access it
    public void ButtonPlayerMove()
    {
        StartCoroutine(PlayerMove());
    }

    /// <summary>
    /// Coroutine. Prompt user to select grid, then moves active unit to that grid.
    /// </summary>
    /// <returns></returns>
    public IEnumerator PlayerMove()
    {
        // TODO: display valid move grids
        UIManager.Instance.ShowGameMessageText("Select Grid to move Swordsman");
        yield return StartCoroutine(GridManager.Instance.WaitForGridSelection());
        // TOOD: check if grid is valid
        activeUnit.Move(GridManager.Instance.confirmSelectedGrid);
    }

}
