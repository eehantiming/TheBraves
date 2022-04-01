using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantEnemy : EnemyUnit
{
    private bool moveTowardsTown = true;
    private int movesTwice = 0; // this is either 0 or 1

    public void Start()
    {
        size = 3;
    }


    /// <summary>
    /// Function to find and move towards nearest town, which may have 1 or 2 possible paths.
    /// </summary>
    protected IEnumerator MoveTowardsNearestTown()
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

        yield return StartCoroutine(MoveTowardsGrid(nearestGrid));
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
            else if (moveTowardsTown)
            {
                Debug.Log($"{unitName} move towards town");
                yield return StartCoroutine(MoveTowardsNearestTown());
            }
            else // move freely
            {
                Debug.Log($"{unitName} move freely");
                var adjacentGrids = GridManager.Instance.GetAdjacentGrids(currentGrid, true, true, false, true); // Moves onto monsters. assume only 1 GiantEnemy.
                yield return StartCoroutine(RandomMovement(adjacentGrids));
            }
        }
        // Only remove bait after finishing movement
        if(isBaited) LoseBait();
        GameManager.Instance.ChangeState(++GameManager.Instance.currentState);
    }

    protected override IEnumerator ActivateRage()
    {
        UIManager.Instance.ShowGameMessageText($"{unitName} RAGE!!");
        if (rageLevel == 1)
        {
            Debug.Log("Rage 1, spawning Heart");
            StartCoroutine(UnitManager.Instance.SpawnHeart());
            moveTowardsTown = false;
            movesTwice = 1;
        }
        else if (rageLevel == 2)
        {
            Debug.Log("Rage 2, speed up calamity");
            movesTwice = 0;
            CalamityManager.Instance.SpeedUp();
        }
        yield break;
    }
}
