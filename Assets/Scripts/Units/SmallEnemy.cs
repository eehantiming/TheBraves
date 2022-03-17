using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallEnemy : EnemyUnit
{
    /// <summary>
    /// Function to move small enemy towards bait, else throw dice and move randomly.   
    /// </summary>
    public override void DecideMovement()
    {
        Debug.Log($"{unitName} turn to move");
        MapGrid goalGrid;
        int roll;

        if (isBaited)
        {
            int goalGridIndex;
            var directionToMove = baitedTo.IndexToVect() - currentGrid.IndexToVect();
            if(directionToMove.x == 0) // move vertical
            {
                goalGridIndex = currentGrid.index + 4 * System.Math.Sign(directionToMove.y);
            }
            else if(directionToMove.y == 0) // move horizontal
            {
                goalGridIndex = currentGrid.index + System.Math.Sign(directionToMove.x);
            }
            else // two possible moves, roll dice to decide
            {
                // TODO: add visualization for which dice roll values correspond to each move
                List<int> possibleGridIndex = new List<int>()
                {
                    currentGrid.index + 4 * System.Math.Sign(directionToMove.y),
                    currentGrid.index + System.Math.Sign(directionToMove.x)
                };
                roll = DiceRoll.Instance.GenerateRoll();
                goalGridIndex = possibleGridIndex[(roll - 1) / 3];
            }
            goalGrid = GridManager.Instance.IndexToGrid[goalGridIndex];
            Move(goalGrid);
            LoseBait();
            return;
        }

        // If not baited
        var adjacentGrids = GridManager.Instance.GetAdjacentGrids(currentGrid, true, false, false, true); // can't move north
        FindNearestHero();
        // Roll dice and move based on roll outcome
        switch (adjacentGrids.Count)
        {
            case 0:
                UIManager.Instance.ShowGameMessageText($"{unitName} can't Move");
                break;
            case 1:
                UIManager.Instance.ShowGameMessageText($"{unitName} has only 1 Move");
                goalGrid = adjacentGrids[0];
                Move(goalGrid);
                break;
            case 2:
                Debug.Log("2 moves, rolling dice");
                roll = DiceRoll.Instance.GenerateRoll();
                goalGrid = adjacentGrids[(roll - 1) / 3];
                Move(goalGrid);
                break;
            case 3:
                Debug.Log("3 moves, rolling dice");
                roll = DiceRoll.Instance.GenerateRoll();
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
