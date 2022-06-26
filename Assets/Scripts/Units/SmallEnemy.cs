using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallEnemy : EnemyUnit
{
    static new int rageLevel = 0; // shared among all small enemies. New for masking the same attribute from EnemyUnit
    private int movesTwice = 0; // this is either 0 or 1

    public void Start()
    {
        size = 1;
        extraText = "Info: Spawn a small monster when Calamity Counter counts a multiple of 3.";
        inventory = rageLevel + "\nNo Effect\n\nOn Next Rage: A Big Monster Appears! (works once)";
    }

    public IEnumerator MoveDown() // DEBUG
    {
        Vector2Int targetGridPos = currentGrid.IndexToVect();
        targetGridPos.y--;
        yield return StartCoroutine(MoveTo(GridManager.Instance.GetGridFromPosition(targetGridPos)));
    }

    /// <summary>
    /// Function to move small enemy towards bait, else throw dice and move randomly.   
    /// </summary>
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

            else // move freely
            {
                Debug.Log($"{unitName} move freely");
                var adjacentGrids = GridManager.Instance.GetAdjacentGrids(currentGrid, true, true, false, true);
                //look for grid without monster bigger than itself
                var safeGrids = adjacentGrids.FindAll(grid => grid.unitsOnGrid.TrueForAll(unit => unit.size < this.size));
                yield return StartCoroutine(RandomMovement(safeGrids));
            }
        }
        // Only remove bait after finishing movement
        if(isBaited) LoseBait();
    }

    protected override IEnumerator ActivateRage()
    {
        UIManager.Instance.ShowGameMessageText($"{unitName} RAGE!!");
        if (rageLevel == 1)
        {
            Debug.Log("Rage 1, spawning BigEnemy");
            yield return StartCoroutine(UnitManager.Instance.SpawnBigEnemy());
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
    public override IEnumerator IncreaseRageLevel()
    {
        if (rageLevel == 2)
        {
            Debug.Log("small enemy dies from rage");
            
            UnitManager.Instance.DestroyUnit(this);
        }
        //else yield return StartCoroutine(base.IncreaseRageLevel());
        else
        {
            rageLevel++; // TODO: add visualization
            yield return StartCoroutine(ActivateRage());
        }

        if(rageLevel == 1)
        {
            inventory = rageLevel + "\nA Big Monster Appears! (works once)\n\nOn Next Rage: Small Monsters dies instead of increasing Rage when its Rage increases by 1";
        }

        if(rageLevel == 2)
        {
            inventory = rageLevel + " (max)" + "\nSmall Monsters dies instead of increasing Rage when its Rage increases by 1";
        }

    }
}
