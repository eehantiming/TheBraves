using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigEnemy : EnemyUnit
{
    private bool moveTowardsPlayer = false;
    private bool moveTowardsSpawnPoint = false;

    public override void DecideMovement()
    {
        if (isBaited)
        {

        }
        else if (moveTowardsPlayer)
        {
            // if multiple route, roll dice
        }
        else if (moveTowardsSpawnPoint)
        {
            // moves up until. check if reach spawn
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
            moveTowardsPlayer = true;
        }
        else if (rageLevel == 2)
        {
            moveTowardsPlayer = false;
            moveTowardsSpawnPoint = true;
        }
    }
}
