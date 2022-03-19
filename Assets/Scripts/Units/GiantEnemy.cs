using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantEnemy : EnemyUnit
{
    private bool moveTowardsTown = true;
    private bool movesTwice = false;

    public void Start()
    {
        size = 3;
    }


    /// <summary>
    /// Function to find and move towards nearest town, which may have 1 or 2 possible paths.
    /// </summary>
    protected void MoveTowardsNearestTown()
    {
        // Find the nearest town. TODO: add support for >1 nearest town.
        float minDist = 1000f;
        MapGrid nearestGrid = null;
        foreach (MapGrid townGrid in GridManager.Instance.townGrids)
        {
            float distance = Vector2.Distance(townGrid.IndexToVect(), currentGrid.IndexToVect());
            if (distance < minDist)
            {
                minDist = distance;
                nearestGrid = townGrid;
            }
        }

        MoveTowardsGrid(nearestGrid);
    }

    public override void DecideMovement()
    {
        Debug.Log($"{unitName} turn to move");
        //TODO: loop to move twice
        if (isBaited)
        {
            MoveTowardsBait();
        }
        else if (moveTowardsTown)
        {
            MoveTowardsNearestTown();
        }
        else // move freely
        {
            var adjacentGrids = GridManager.Instance.GetAdjacentGrids(currentGrid, true, true, false, true); // Moves onto monsters. assume only 1 GiantEnemy.
            RandomMovement(adjacentGrids);
        }
    }

    protected override IEnumerator ActivateRage()
    {
        if (rageLevel == 1)
        {
            UnitManager.Instance.SpawnHeart();
            moveTowardsTown = false;
            movesTwice = true;
        }
        else if (rageLevel == 2)
        {
            CalamityManager.Instance.SpeedUp();
        }
        yield break;
    }
}
