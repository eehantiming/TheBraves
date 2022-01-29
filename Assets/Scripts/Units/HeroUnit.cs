using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroUnit : BaseUnit
{
    public bool isConscious = true;

    /// <summary>
    /// Ends current player turn and move gamestate to next phase
    /// </summary>
    public void EndTurn()
    {
        GameManager.Instance.ChangeState(++GameManager.Instance.currentState);
    }
}
