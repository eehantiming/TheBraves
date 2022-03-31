using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallEnemy : EnemyUnit
{
    public void Start()
    {
        size = 1;
    }

    public void MoveDown() // DEBUG
    {
        Vector2Int targetGridPos = currentGrid.IndexToVect();
        targetGridPos.y--;
        Move(GridManager.Instance.GetGridFromPosition(targetGridPos));
    }

    /// <summary>
    /// Function to move small enemy towards bait, else throw dice and move randomly.   
    /// </summary>
    public override void DecideMovement(bool keepBait)
    {
        MapGrid goalGrid;
        int roll;

        if (isBaited)
        {
            Debug.Log($"{unitName} move towards bait");
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
        Debug.Log($"{unitName} move freely");
        var adjacentGrids = GridManager.Instance.GetAdjacentGrids(currentGrid, true, false, false, true); // can't move north
        // Roll dice and move based on roll outcome
        switch (adjacentGrids.Count)
        {
            case 0:
                UIManager.Instance.ShowGameMessageText($"{unitName} can't Move");
                Debug.Log("Small Monster can't move");
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

    protected override IEnumerator ActivateRage()
    {
        UIManager.Instance.ShowGameMessageText($"{unitName} RAGE!!");
        if (rageLevel == 1)
        {
            Debug.Log("Rage 1, spawning BigEnemy");
            yield return StartCoroutine(UnitManager.Instance.SpawnBigEnemy());
            Debug.Log("Spawned!");
        }
        else if (rageLevel == 2)
        {
            Debug.Log("Rage 2, Can now die");
            UnitManager.Instance.smallEnemyCanDie = true;
        }
    }

    /// <summary>
    /// Function to increase Rage level by 1. If you do, activates associated ability. If already at rage 2, enemy dies.
    /// </summary>
    public override void IncreaseRageLevel()
    {
        if (rageLevel == 2)
        {
            Debug.Log("small enemy dies from rage");
            UnitManager.Instance.DestroyUnit(this);
        }
        else base.IncreaseRageLevel();
    }
}
