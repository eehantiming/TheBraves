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

    [SerializeField] public ActiveMarker activeMarker;

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
        grid.AddUnitToGrid(spawnedUnit);
        spawnedUnit.currentGrid = grid;
        UIManager.Instance.ShowGameMessageText($"{unitName} spawned on {grid.IndexToVect()}!");
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
        //smallEnemies.Add((SmallEnemy)SpawnUnit(smallEnemyPrefab, GridManager.Instance.IndexToGrid[8], $"Small Monster 2")); // DEBUG
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
        swordsman = (Swordsman)SpawnUnit(swordsmanPrefab, GridManager.Instance.confirmSelectedGrid, "Reiner");
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
        magician = (Magician)SpawnUnit(magicianPrefab, GridManager.Instance.confirmSelectedGrid, "Wanda");
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
        giantEnemy = (GiantEnemy)SpawnUnit(giantEnemyPrefab, GridManager.Instance.GetEnemySpawnGrid(), "Godzilla"); // TODO: add spawn animation
        //giantEnemy = (GiantEnemy)SpawnUnit(giantEnemyPrefab, GridManager.Instance.IndexToGrid[8], "Godzilla"); // DEBUG
    }

    public IEnumerator SpawnHeart()
    {
        UIManager.Instance.ShowGameMessageText("Monster Heart Appears!!");
        Debug.Log("spawning heart");
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
        activeUnit = unit; // TODO: add visualization for active unit
        if(activeUnit == null)
        {
            UIManager.Instance.ShowActiveUnitText(null);
        }
        else
        {
            UIManager.Instance.ShowActiveUnitText(activeUnit.unitName);
        }
        if (activeUnit.faction == Faction.Hero)
        {
            UIManager.Instance.EnableButtons();
        }
        SetActiveMarker();
    }

    // Wrapper for ActivateRevive coroutine so that button can access it
    public void ButtonUseRevive()
    {
        Debug.Log("Clicked revive..");
        StartCoroutine(activeUnit.GetComponent<HeroUnit>().ActivateRevive());
    }

    // Wrapper for ActivateSkill for consistency
    public void ButtonUseSkill()
    {
        Debug.Log("Clicked skill..");
        activeUnit.GetComponent<HeroUnit>().ActivateSkill();
    }

    // Wrapper for ActivateBait Coroutine so that button can access it
    public void ButtonUseBait()
    {
        Debug.Log("Clicked bait..");
        StartCoroutine(activeUnit.GetComponent<HeroUnit>().ActivateBait());
    }

    // Wrapper for ActivateMove Coroutine so that button can access it
    public void ButtonPlayerMove()
    {
        Debug.Log("Clicked move..");
        StartCoroutine(activeUnit.GetComponent<HeroUnit>().ActivateMove());
    }


    /// <summary>
    /// Coroutine which moves each small enemy one after another. Switches to next state after all movement are completed
    /// </summary>
    /// <returns></returns>
    public IEnumerator MoveSmallEnemies()
    {
        foreach (SmallEnemy smallEnemy in smallEnemies) 
        {
            // small enemy will be null and still in list if killed out of turn (e.g. by trap)
            if (smallEnemy != null && !smallEnemy.isStunned)
            {
                SetActiveUnit(smallEnemy);
                yield return StartCoroutine(smallEnemy.DecideMovement());
                //yield return StartCoroutine(smallEnemy.MoveDown()); // DEBUG
            }
            else
            {
                if (smallEnemy.isStunned)
                {
                    UIManager.Instance.ShowGameMessageText($"{smallEnemy.unitName} is stunned!");
                    Debug.Log($"{smallEnemy.unitName} stunned, skipping.");
                    smallEnemy.LoseStun();
                }
                if (!smallEnemy.isAlive) Debug.Log($"{smallEnemy.unitName} is Dead");
            }
        }
        // Remove enemies that are dead. have to do this after finishing iterating thru the list.
        smallEnemies = smallEnemies.FindAll(units => units.isAlive);
        GameManager.Instance.ChangeState(++GameManager.Instance.currentState);
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

    /// <summary>
    /// Function to destroy unit's gameobject and unitmanager's refernce for the unit.
    /// </summary>
    /// <param name="unit">The baseunit to be destroyed.</param>
    public void DestroyUnit(BaseUnit unit)
    {
        if (unit.GetComponent<SmallEnemy>())
        {
            // Sets a flag to remove small enemy from the list later
            unit.GetComponent<SmallEnemy>().isAlive = false;
        }
        else if(unit.GetComponent<BigEnemy>())
        {
            bigEnemy = null;
        }
        else if (unit.GetComponent<BigEnemy>())
        {
            giantEnemy = null;
        }
        // Destroys the game object. any references to it will be "missing" and treated as null
        Destroy(unit.gameObject, 0.5f); //TODO: add animation for this
        UIManager.Instance.ShowGameMessageText($"{unit.unitName} dies!");
        Debug.Log($"{unit.unitName} is destroyed");
    }
    
    /// <summary>
    /// Function to add a ActiveMarker to active unit
    /// </summary>
    public void SetActiveMarker()
    {
        ActiveMarker marker = Instantiate(activeMarker, activeUnit.transform.position, Quaternion.identity);
        marker.transform.parent = activeUnit.transform;
        marker.transform.position = new Vector3(activeUnit.transform.position.x + 0.4f, activeUnit.transform.position.y + 0.4f, activeUnit.transform.position.z);

    }
}
