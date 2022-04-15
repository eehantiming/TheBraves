using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trapper : HeroUnit
{
    public void Start()
    {
        size = 0;
        extraText = "Skill: Builds traps that can stun monsters walking into it for 1 turn";
    }

    [SerializeField] private int numOfTrapsLeft = 2; // TODO: add setter for this
    public int NumOfTrapsLeft
    {
        set
        {
            if (value > 2) numOfTrapsLeft = 2;
            else numOfTrapsLeft = value;
        }
        get { return numOfTrapsLeft; }
    }

    public override void ActivateSkill()
    {
        base.ActivateSkill();
        Debug.Log("Set Trap");
        StartCoroutine(SetTrap());
    }

    /// <summary>
    /// Coroutine. Adds a trap, if available, to the current grid if it doesn't already have a trap.
    /// </summary>
    /// <returns></returns>
    private IEnumerator SetTrap()
    {
        if (NumOfTrapsLeft == 0)
        {
            UIManager.Instance.ShowGameMessageText("No more traps left!");
            Debug.Log("can't set when numOfTrapsLeft = 0");
            yield break;
        }
        if (currentGrid.isHoldingTrap)
        {
            UIManager.Instance.ShowGameMessageText("There is already a trap here!");
            Debug.Log("current grid is already holding trap");
            yield break;
        }
        currentGrid.AddTrapToGrid();
        NumOfTrapsLeft--;
        yield return new WaitForSeconds(1);
        EndTurn();
    }
}
