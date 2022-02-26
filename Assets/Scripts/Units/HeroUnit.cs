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
        //GameManager.Instance.ChangeState(++GameManager.Instance.currentState);
        else GameManager.Instance.ChangeState(GameManager.GameState.SmallEnemyPhase); // DEBUG
    }

    public void ActivateBait()
    {
        int x = currentGrid.IndexToVect().x;
        int y = currentGrid.IndexToVect().y;
        for(int dx = -2; dx <= 2; dx++)
        {
            Vector2Int gridToCheck = new Vector2Int(x + dx, y);
        }
    }
    /// <summary>
    /// Activate the hero specifc skill.
    /// </summary>
    public virtual void ActivateSkill()
    {
       //Each Hero has its own override ActivateSkill() 
    }

    /// <summary>
    /// Activate revive.
    /// </summary>
    public virtual void ActivateRevive()
    {
        Debug.Log("Clicked move..");
        StartCoroutine(Revive());
    }

    public IEnumerator Revive()
    {
        //Retrieve list of adjacent grids
        List<MapGrid> possibleGrids = GridManager.Instance.GetAdjacentGrids(this.currentGrid, true, true);
        
        //Find grids with with unconscious heros
        List<MapGrid> unconsciousHeroGrids = possibleGrids.FindAll(grid => grid.heroesOnGrid.Count > 0 && grid.heroesOnGrid[0].isConscious == false);
    
        if (unconsciousHeroGrids.Count == 0)
        {
            UIManager.Instance.ShowGameMessageText("No unconscious heroes nearby");
            Debug.Log("No unconscious heroes nearby");
            yield break;
        }

        // TODO: display valid Monster grids
        UIManager.Instance.ShowGameMessageText("Select Hero to revive");
        yield return StartCoroutine(GridManager.Instance.WaitForGridSelection());
        // Check if selected grid is a valid move
        while (!unconsciousHeroGrids.Contains(GridManager.Instance.confirmSelectedGrid))
        {
            UIManager.Instance.ShowGameMessageText("Select Hero to revive");
            yield return StartCoroutine(GridManager.Instance.WaitForGridSelection()); 
        }

        //Set selected unconscious hero isConscious to true
        GridManager.Instance.confirmSelectedGrid.heroesOnGrid[0].isConscious = true;
        Debug.Log("Saved" + GridManager.Instance.confirmSelectedGrid.heroesOnGrid[0].unitName);
        yield return new WaitForSeconds(1);
        EndTurn();
    }
}
