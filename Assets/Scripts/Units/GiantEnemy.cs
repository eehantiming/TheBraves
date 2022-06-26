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
        extraText = "Info: Defeated by players reaching the Monster's heart.";
        inventory = rageLevel + "\nMove towards the Nearest Town\n\nOn Next Rage: Monster's Heart Appears! (works once) and Giant Monster moves twice on its turn";

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

        if(rageLevel == 1)
        {
            inventory = rageLevel + "\nMonster's Heart Appears! (works once) and Giant Monster moves twice on its turn\n\nOn Next Rage: Calamity Counter increases by 1 extra count";
        }

        if(rageLevel == 2)
        {
            inventory = rageLevel + " (max)" + "\nCalamity Counter increases by 1 extra count";
        }
 
        yield break;
    }
}
