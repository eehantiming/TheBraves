using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallEnemy : EnemyUnit
{
    /// <summary>
    /// Function to throw dice and move Enemy.
    /// </summary>
    public override void DecideMovement()
    {
        if (isBaited)
        {
            var directionToMove = baitedTo.IndexToVect() - currentGrid.IndexToVect();
            if(directionToMove.x == 0) // move vertical
            {
                int goalGridIndex = currentGrid.index + 4 * System.Math.Sign(directionToMove.y);
            }
            else if(directionToMove.y == 0) // move horizontal
            {
                int goalGridIndex = currentGrid.index + System.Math.Sign(directionToMove.x);
            }
        }
        // Find available grids
        var adjacentGrids = GridManager.Instance.GetAdjacentGrids(currentGrid, true, false, false, true); // can't move north
        //foreach(MapGrid grid in adjacentGrids)
        //{
        //    Debug.Log(grid.index);
        //}
        // Roll dice and move based on roll outcome
        MapGrid goalGrid;
        switch (adjacentGrids.Count)
        {
            case 0:
                UIManager.Instance.ShowGameMessageText($"{unitName} can't Move");
                return;
            case 1:
                UIManager.Instance.ShowGameMessageText($"{unitName} has only 1 Move");
                goalGrid = adjacentGrids[0];
                Move(goalGrid);
                break;
            case 2:
                int roll = Random.Range(1, 7); // TODO: currently fixed to 6. use this value for dice throw
                // int roll = XX.rollDice(); // TODO: create a function/coroutine somewhere to roll dice, run animation and return result
                Debug.Log("Roll: " + roll);
                goalGrid = adjacentGrids[(roll - 1) / 3];
                UIManager.Instance.ShowGameMessageText($"Rolled {roll}. {unitName} moving to {goalGrid.index}");
                Move(goalGrid);
                break;
            case 3:
                roll = Random.Range(1, 7); // TODO: currently fixed to 6. use this value for dice throw
                // int roll = XX.rollDice(); // TODO: create a function/coroutine somewhere to roll dice, run animation and return result
                Debug.Log("Roll: " + roll);
                goalGrid = adjacentGrids[(roll - 1) / 2];
                Move(goalGrid);
                break;
            default:
                Debug.LogError("Invalid movement outcome");
                break;
        }
    }

    protected override void ActivateRage()
    {
        if (rageLevel == 1)
        {
            StartCoroutine(UnitManager.Instance.SpawnBigEnemy());
        }
        else if (rageLevel == 2)
        {
            UnitManager.Instance.smallEnemyCanDie = true;
        }
    }
}
