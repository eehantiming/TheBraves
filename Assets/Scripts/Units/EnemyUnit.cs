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
    //TODO: create line renderer as part of script instead?

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

    /// <summary>
    /// Function for Enemy to move this turn. Throws dice if neccesary.
    /// </summary>
    public virtual void DecideMovement() // Make this abstract?
    {

    }

    protected virtual void ActivateRage() // Make this abstract?
    {

    }

    /// <summary>
    /// Function to make enemy baited and move towards bait source on its next turn.
    /// </summary>
    /// <param name="baitSource">The MapGrid that the baited enemy will move towards.</param>
    public void TakeBait(MapGrid baitSource)
    {
        isBaited = true;
        baitedTo = baitSource;

        LineRenderer lr = GetComponent<LineRenderer>();
        lr.SetPosition(0, currentGrid.transform.position);
        lr.SetPosition(1, baitSource.transform.position);

        GetComponent<SpriteRenderer>().color = new Color(0.7294118f, 0.4941176f, 0.7490196f);
        UIManager.Instance.ShowGameMessageText($"{unitName} is baited!");
        Debug.Log($"{unitName} is baited to {baitedTo.IndexToVect()}");
    }

    public void LoseBait() // TODO: make this protected?
    {
        isBaited = false;
        GetComponent<SpriteRenderer>().color = Color.white;
        LineRenderer lr = GetComponent<LineRenderer>();
        lr.SetPosition(1, transform.position);
        
    }
}
