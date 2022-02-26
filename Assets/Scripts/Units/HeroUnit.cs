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
       //WIP
    }
}
