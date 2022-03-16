using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigEnemy : EnemyUnit
{
    private bool moveTowardsPlayer = false;
    private bool moveTowardsSpawnPoint = false;

    public override void DecideMovement()
    {
        Debug.Log($"{unitName} turn to move");
        MapGrid goalGrid;
        int roll;
        if (isBaited)
        {
            //using code from smallEnemy
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
        else if (moveTowardsPlayer)
        {
            //using code from isBaited
            int goalGridIndex;
            //need to change baitedTo into a mapgrid that contains the nearest hero
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
            return;
        }
        else if (moveTowardsSpawnPoint)
        {
            // moves up until. check if reach spawn
        }
        else // move freely
        {
            var adjacentGrids = GridManager.Instance.GetAdjacentGrids(currentGrid, true, false, false, true); // can't move north
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
    }

    protected override void ActivateRage()
    {
        if (rageLevel == 1)
        {
            moveTowardsPlayer = true;
        }
        else if (rageLevel == 2)
        {
            moveTowardsPlayer = false;
            moveTowardsSpawnPoint = true;
        }
    }
}
