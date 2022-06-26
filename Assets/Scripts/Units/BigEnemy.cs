using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigEnemy : EnemyUnit
{
    private bool moveTowardsPlayer = false;
    private bool moveTowardsSpawnPoint = false;
    private int movesTwice = 1; // this is either 0 or 1
    public void Start()
    {
        size = 2;
        extraText = "Info: Moves twice on its turn";
        inventory = rageLevel + "\nNo Effect\n\nOn Next Rage: Move towards the Nearest Player unless Baited";


    }
    public override IEnumerator DecideMovement()
    {
        for(int x = 0; x <= movesTwice; x++)
        {
            // End movement if stunned during turn
            if (isStunned)
            {
                break;
            }
            if (isBaited)
            {
                Debug.Log($"{unitName} move to baited");
                yield return StartCoroutine(MoveTowardsBait());
            }
            else if (moveTowardsPlayer)
            {
                Debug.Log($"{unitName} move to hero");
                var nearestHero = FindNearestHero();
                yield return StartCoroutine(MoveTowardsGrid(nearestHero));
            }
            else if (moveTowardsSpawnPoint)
            {
                Debug.Log($"{unitName} move to spawn point");
                var spawnPoint = FindNearestSpawnPoint();
                yield return StartCoroutine(MoveTowardsGrid(spawnPoint));
                //check if new currentGrid is a spawn point
                if(this.currentGrid.isEnemySpawnGrid)
                {
                    rageLevel = 0;
                    inventory = rageLevel + "\nOn Next Rage: Move towards the Nearest Player unless Baited";
                    moveTowardsPlayer = false;
                    moveTowardsSpawnPoint = false;
                }
            }
            else // move freely
            {
                Debug.Log($"{unitName} move freely");
                var adjacentGrids = GridManager.Instance.GetAdjacentGrids(currentGrid, true, true, false, true); // Moves onto monsters.
                //looks for grids whereby there are no monsters larger or equal to own size
                var safeGrids = adjacentGrids.FindAll(grid => grid.unitsOnGrid.TrueForAll(unit => unit.size < this.size));
                yield return StartCoroutine(RandomMovement(safeGrids));
            }
        }
        // Only remove bait after finishing movement
        if(isBaited) LoseBait();
        GameManager.Instance.ChangeState(++GameManager.Instance.currentState);
    }

    protected override IEnumerator ActivateRage()
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
        if(rageLevel == 1)
        {
            inventory = rageLevel + "\nMove towards the Nearest Player unless Baited\nOn Next Rage: Move towards Monster Spawn point. Rage returns to 0 after reaching a Monster Spawn Point";
        }

        if(rageLevel == 2)
        {
            inventory = rageLevel + " (max)" + "\n\nMove towards Monster Spawn point. Rage returns to 0 after reaching a Monster Spawn Point";
        }

        
        yield break;
    }
}
