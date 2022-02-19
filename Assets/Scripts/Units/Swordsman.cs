using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swordsman : HeroUnit
{
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
        List<MapGrid> validGrids = GridManager.Instance.GetAdjacentGrids(UnitManager.Instance.activeUnit.currentGrid, false, true);
        Debug.Log(validGrids.Count);
        
        //validGrids contains both empty and enemy - remove grids without enemy from ValidGrids
        for (int i = 0; i < validGrids.Count; i++)
        {
            if (validGrids[i].unitsOnGrid.Count > 0)
            {
                bool hasEnemy = false;
                foreach(BaseUnit unit in validGrids[i].unitsOnGrid)
                {
                    // set hasEnemy to true if Enemy found
                    if (unit.faction == Faction.Enemy) hasEnemy = true;
                }
                if (!hasEnemy)
                {
                    validGrids.RemoveAt(i);
                    i--; // recheck at index which is a new grid since earlier grid was removed
                }
            }
        }
        Debug.Log(validGrids.Count);
        if (validGrids.Count == 0)
        {
            UIManager.Instance.ShowGameMessageText("No monsters nearby");
            Debug.Log("No nonsters nearby to use skill on");
            yield break;
        }
        // TODO: display valid Monster grids
        UIManager.Instance.ShowGameMessageText("Select Monster to Sword-Charge");
        yield return StartCoroutine(GridManager.Instance.WaitForGridSelection());
        // Check if selected grid is a valid move
        while (!validGrids.Contains(GridManager.Instance.confirmSelectedGrid))
        {
            UIManager.Instance.ShowGameMessageText("Select Monster to Sword-Charge");
            yield return StartCoroutine(GridManager.Instance.WaitForGridSelection());
        }
        //activeUnit.Move(GridManager.Instance.confirmSelectedGrid);
        Debug.Log("Charged successful");
        //reasign grid of monster and move it to the new grid
        yield return new WaitForSeconds(1);
        UnitManager.Instance.activeUnit.GetComponent<HeroUnit>().EndTurn();
    }
}
