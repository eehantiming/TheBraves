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
        List<MapGrid> validGrids = GridManager.Instance.GetAdjacentGrids(this.currentGrid, false, true);
        Debug.Log(validGrids.Count);
        
        //validGrids contains both empty and enemy - loop removes grids without enemy from ValidGrids
        for (int i = 0; i < validGrids.Count; i++)
        {
            //not need to check count()
            if (validGrids[i].unitsOnGrid.Count == 0)
            {
                validGrids.RemoveAt(i);
                i--; // recheck at index which is a new grid since earlier grid was removed
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
            UIManager.Instance.ShowGameMessageText("Select a grid with Monster to Sword-Charge");
            yield return StartCoroutine(GridManager.Instance.WaitForGridSelection());
        }
        Debug.Log("Swordcharged started");
        //reasign grid of monster and move it to the new grid
        //Sword charge displacment = monster current pos - hero pos
        Vector2 displacement = GridManager.Instance.confirmSelectedGrid.IndexToVect() - this.currentGrid.IndexToVect();
        //Monster final grid = monster current pos + displacement
        Vector2 monster_final_grid = GridManager.Instance.confirmSelectedGrid.IndexToVect() + displacement;

        List<MapGrid> monster_final_validGrids = GridManager.Instance.GetAdjacentGrids(GridManager.Instance.confirmSelectedGrid, true, true);
        //check if monster_final_grid is in one of the valid adjacentgrids
        for (int i = 0; i < monster_final_validGrids.Count; i++)
        {
            //not need to check count()
            if (monster_final_validGrids[i].IndexToVect() != monster_final_grid)
            {
                monster_final_validGrids.RemoveAt(i);
                i--; // recheck at index which is a new grid since earlier grid was removed
            }
        }
        //move monster to the monster_final_validGrids
        if (monster_final_validGrids.Count > 0) 
        {
            //Selects the first unit in the unitsOnGrid list = need to be able to select from multiple monsters
            BaseUnit target_monster = GridManager.Instance.confirmSelectedGrid.unitsOnGrid[0];
            target_monster.Move(monster_final_validGrids[0]);
        }
        Debug.Log("SwordCharged successful");
        yield return new WaitForSeconds(1);
        EndTurn();
    }
}
