using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallEnemy : EnemyUnit
{
    /// <summary>
    /// Function to throw dice and move Enemy.
    /// </summary>
    public void DecideMovement()
    {
        // TODO: find available grids. if more than 1, roll dice. assign to dice result. move to that grid
        var adjacentGrids = GridManager.Instance.GetAdjacentGrids(currentGrid, true, false);
        adjacentGrids.Remove(GridManager.Instance.IndexToGrid[currentGrid.index + 4]); // Can't move North
        foreach(MapGrid grid in adjacentGrids)
        {
            Debug.Log(grid.index);
        }
        int roll = Random.Range(1, 7); // TODO: currently fixed to 6. use this value for dice throw
        // int roll = XX.rollDice(); // TODO: create a function/coroutine somewhere to roll dice, run animation and return result
        Debug.Log("Roll: " + roll);
        //Move(goalGrid);
    }
}
