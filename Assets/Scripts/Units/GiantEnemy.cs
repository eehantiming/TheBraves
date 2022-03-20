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
        //TODO: loop to move twice
        if (isBaited)
        {
            Debug.Log($"{unitName} move to baited");
            MoveTowardsBait();
        }
        else if (moveTowardsTown)
        {
            Debug.Log($"{unitName} move towards town");
            MoveTowardsNearestTown();
        }
        else // move freely
        {
            Debug.Log($"{unitName} move freely");
            var adjacentGrids = GridManager.Instance.GetAdjacentGrids(currentGrid, true, true, false, true); // Moves onto monsters. assume only 1 GiantEnemy.
            RandomMovement(adjacentGrids);
        }
    }

    protected override IEnumerator ActivateRage()
    {
        UIManager.Instance.ShowGameMessageText($"{unitName} RAGE!!");
        if (rageLevel == 1)
        {
            Debug.Log("Rage 1, spawning Heart");
            UnitManager.Instance.SpawnHeart();
            moveTowardsTown = false;
            movesTwice = true;
        }
        else if (rageLevel == 2)
        {
            Debug.Log("Rage 2, speed up calamity");
            CalamityManager.Instance.SpeedUp();
        }
        yield break;
    }
}
