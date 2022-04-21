using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroUnit : BaseUnit
{
    public bool isConscious = true;

    /// <summary>
    /// Ends current player turn, checks if on heart and move gamestate to next phase
    /// </summary>
    public void EndTurn()
    {
        if (currentGrid.isHoldingHeart) GameManager.Instance.PlayerWin();
        GameManager.Instance.ChangeState(++GameManager.Instance.currentState);
        //else GameManager.Instance.ChangeState(GameManager.GameState.SmallEnemyPhase); // DEBUG
    }

    /// <summary>
    /// Bait enemies within distance 2 of the hero.
    /// </summary>
    public IEnumerator ActivateBait()
    {
        UIManager.Instance.DisableButtons();
        int x = currentGrid.IndexToVect().x;
        int y = currentGrid.IndexToVect().y;
        bool baitSuccess = false;
        for (int dx = -2; dx <= 2; dx++)
        {
            for (int dy = -2; dy <= 2; dy++)
            {
                if (System.Math.Abs(dx) + System.Math.Abs(dy) > 2) // 2 units away diagonally
                    continue;
                if (dx == 0 && dy == 0)
                    continue;
                Vector2Int gridPosToCheck = new Vector2Int(x + dx, y + dy);
                if (!GridManager.Instance.CheckPosInBoard(gridPosToCheck)) // coordinates is out of board range.
                    continue;
                MapGrid gridToCheck = GridManager.Instance.GetGridFromPosition(gridPosToCheck);
                if (gridToCheck.enemiesOnGrid.Count > 0)
                {
                    foreach (EnemyUnit enemy in gridToCheck.enemiesOnGrid)
                        enemy.TakeBait(currentGrid);
                    baitSuccess = true;
                }
            }
        }
        yield return new WaitForSeconds(1);
        if(baitSuccess)
            EndTurn();
        else
        {
            Debug.Log("Nothing baited");
            UIManager.Instance.ShowGameMessageText("No units in range for bait!");
            UIManager.Instance.EnableButtons();
        }
    }

    /// <summary>
    /// Activate the hero specifc skill.
    /// </summary>
    public virtual void ActivateSkill()
    {
        UIManager.Instance.DisableButtons();
        //Each Hero has its own override ActivateSkill() 
    }

    public IEnumerator ActivateRevive()
    {
        UIManager.Instance.DisableButtons();
        //Retrieve list of adjacent grids
        List<MapGrid> possibleGrids = GridManager.Instance.GetAdjacentGrids(this.currentGrid, true, true);
        
        //Find grids with with unconscious heros
        List<MapGrid> unconsciousHeroGrids = possibleGrids.FindAll(grid => grid.heroesOnGrid.Count > 0 && grid.heroesOnGrid[0].isConscious == false);
    
        if (unconsciousHeroGrids.Count == 0)
        {
            UIManager.Instance.ShowGameMessageText("No unconscious heroes nearby");
            Debug.Log("No unconscious heroes nearby");
            UIManager.Instance.EnableButtons();
            yield break;
        }

        // TODO: display valid heroes grids
        UIManager.Instance.ShowGameMessageText("Select Hero to revive");
        yield return StartCoroutine(GridManager.Instance.WaitForGridSelection());
        // Check if selected grid is a valid move or if player has clicked outside to cancel revive
        while (!unconsciousHeroGrids.Contains(GridManager.Instance.confirmSelectedGrid) && !GridManager.Instance.clickedOutside)
        {
            UIManager.Instance.ShowGameMessageText("Select Hero to revive");
            yield return StartCoroutine(GridManager.Instance.WaitForGridSelection()); 
        }
        if (GridManager.Instance.clickedOutside == true) // Cancel revive
        {
            Debug.Log("Cancelled");
            UIManager.Instance.EnableButtons();
            yield break;
        }

        //Set selected unconscious hero isConscious to true
        GridManager.Instance.confirmSelectedGrid.heroesOnGrid[0].isConscious = true;
        Debug.Log("Saved" + GridManager.Instance.confirmSelectedGrid.heroesOnGrid[0].unitName);
        yield return new WaitForSeconds(1);
        EndTurn();
    }

    /// <summary>
    /// Coroutine. Prompt user to select grid, then moves active unit to that grid.
    /// </summary>
    /// <returns></returns>
    public IEnumerator ActivateMove() 
    {
        UIManager.Instance.DisableButtons();
        List<MapGrid> validGrids = GridManager.Instance.GetAdjacentGrids(currentGrid, false, false);
        if (validGrids.Count == 0)
        {
            UIManager.Instance.ShowGameMessageText("This unit can't move!");
            Debug.Log("There are no valid grids to move to");
            UIManager.Instance.EnableButtons();
            yield break;
        }
        // TODO: display valid move grids
        UIManager.Instance.ShowGameMessageText("Select Grid to move to");
        yield return StartCoroutine(GridManager.Instance.WaitForGridSelection());
        // Check if selected grid is a valid move or if player has clicked outside to cancel Move
        while (!validGrids.Contains(GridManager.Instance.confirmSelectedGrid) && !GridManager.Instance.clickedOutside)
        {
            UIManager.Instance.ShowGameMessageText("Select Valid Grid to move to");
            yield return StartCoroutine(GridManager.Instance.WaitForGridSelection());
        }
        if (GridManager.Instance.clickedOutside == true) // Cancel move
        {
            Debug.Log("Cancelled");
            UIManager.Instance.EnableButtons();
            yield break;
        }
        yield return StartCoroutine(MoveTo(GridManager.Instance.confirmSelectedGrid));
        EndTurn();
    }
}
