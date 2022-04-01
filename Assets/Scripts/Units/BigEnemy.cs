using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigEnemy : EnemyUnit
{
    private bool moveTowardsPlayer = false;
    private bool moveTowardsSpawnPoint = false;
    private int movesTwice = 0; // this is either 0 or 1
    public void Start()
    {
        size = 2;
    }
    public override IEnumerator DecideMovement(bool keepBait)
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
        // Only remove bait after finishing movement and keepBait is false
        if(!keepBait & isBaited) LoseBait();
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
        yield break;
    }
}
