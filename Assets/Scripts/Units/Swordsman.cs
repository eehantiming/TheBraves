using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swordsman : HeroUnit
{
    public void Start()
    {
        size = 0;
        extraText = "Skill: Sword Charge - Push an adjacent monster back 1 space and increase Monster Rage by 1";
        inventory = "";
    }
    public override void ActivateSkill()
    {
        base.ActivateSkill();
        Debug.Log("SwordCharge");
        StartCoroutine(SwordCharge());
    }

/// <summary>
    /// Coroutine. Prompt user to select an enemy in adjacent square. Push this enemy back one space
    /// </summary>
    /// <returns></returns>
    public IEnumerator SwordCharge()
    {
        List<MapGrid> validGrids = GridManager.Instance.GetAdjacentGrids(this.currentGrid, true, true);
        
        //validGrids contains both empty, monsters and heroes - Find grid with enemiesOnGrid
        List<MapGrid> enemyGrids = validGrids.FindAll(grid => grid.enemiesOnGrid.Count > 0);

        if (enemyGrids.Count == 0)
        {
            UIManager.Instance.ShowGameMessageText("No monsters nearby");
            Debug.Log("No nonsters nearby to use skill on");
            UIManager.Instance.EnableButtons();
            yield break;
        }
        // Display valid Monster grids
        GridManager.Instance.ToggleActionDarken(true, enemyGrids);
        UIManager.Instance.ShowGameMessageText("Select Monster to Sword-Charge");
        yield return StartCoroutine(GridManager.Instance.WaitForGridSelection());
        // Check if selected grid is a valid move
        while (!enemyGrids.Contains(GridManager.Instance.confirmSelectedGrid) && !GridManager.Instance.clickedOutside)
        {
            UIManager.Instance.ShowGameMessageText("Select a grid with Monster to Sword-Charge");
            yield return StartCoroutine(GridManager.Instance.WaitForGridSelection()); 
        }
        if (GridManager.Instance.clickedOutside == true) // Cancel Sword-Charge
        {
            Debug.Log("Cancelled");
            UIManager.Instance.EnableButtons();
            yield break;
        }
        Debug.Log("Sword-Charge started");
        //reasign grid of monster and move it to the new grid
        //Sword charge displacment = monster current pos - hero pos
        Vector2Int displacement = GridManager.Instance.confirmSelectedGrid.IndexToVect() - this.currentGrid.IndexToVect();
        //MonstInter final grid = monster current pos + displacement
        Vector2Int monsterFinalGridPosition = GridManager.Instance.confirmSelectedGrid.IndexToVect() + displacement;

        List<MapGrid> monsterFinalValidGrids = GridManager.Instance.GetAdjacentGrids(GridManager.Instance.confirmSelectedGrid, true, true);
        
        //Find grids from monsterFinalValidGrids that is equal to monsterFinalGridPosition
        List<MapGrid> monsterPushBackGrid = monsterFinalValidGrids.FindAll(grid => grid.IndexToVect() == monsterFinalGridPosition);

        //Selects the first unit in the unitsOnGrid list
        EnemyUnit targetMonster = GridManager.Instance.confirmSelectedGrid.enemiesOnGrid[0];
        //move monster to the monsterFinalValidGrids
        if (monsterPushBackGrid.Count > 0) 
        {
            //move targetMonster back 1 space
            yield return StartCoroutine(targetMonster.MoveTo(GridManager.Instance.GetGridFromPosition(monsterFinalGridPosition)));
        }
        //increase targetMonster rage level
        yield return StartCoroutine(targetMonster.IncreaseRageLevel());

        Debug.Log("SwordCharged successful");
        //yield return new WaitForSeconds(1);
        GridManager.Instance.ToggleActionDarken(false, enemyGrids);

        EndTurn();
    }
}
