using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantEnemy : EnemyUnit
{
    private bool moveTowardsTown = true;
    private bool movesTwice = false;
    public override void DecideMovement()
    {
        if (isBaited)
        {

        }
        else if (moveTowardsTown)
        {
            // if multiple route, roll dice
        }
        else // move freely
        {
            //RandomMovement();
        }
    }

    protected override void ActivateRage()
    {
        if (rageLevel == 1)
        {
            //UnitManager.Instance.SpawnHeart();
            movesTwice = true;
        }
        else if (rageLevel == 2)
        {
            //CalamityManager.Instance.increaseTwice = true;
        }
    }
}
