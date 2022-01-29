using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : BaseUnit
{
    public bool isStunned = false;
    public bool isAlive = true;

    protected int rageLevel = 0;
    protected bool isBaited = false;
    protected MapGrid baitedTo;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    /// <summary>
    /// Function to increase Rage level by 1. If you do, activates associated ability
    /// </summary>
    public void IncreaseRageLevel()
    {
        if(rageLevel < 2)
        {
            rageLevel++; // TODO: add visualization
            ActivateRage();
        }
    }

    public virtual void DecideMovement() // Make this abstract?
    {

    }

    protected virtual void ActivateRage() // Make this abstract?
    {

    }
}
