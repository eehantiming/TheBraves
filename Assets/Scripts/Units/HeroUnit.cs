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
        if (currentGrid.holdingHeart) GameManager.Instance.PlayerWin();
        //GameManager.Instance.ChangeState(++GameManager.Instance.currentState);
        else GameManager.Instance.ChangeState(GameManager.GameState.SmallEnemyPhase); // DEBUG
    }
}
