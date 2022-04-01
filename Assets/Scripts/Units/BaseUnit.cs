using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUnit : MonoBehaviour
{
    public MapGrid currentGrid = null;
    public string unitName;
    public Faction faction;

    public int size = 0;

    /// <summary>
    /// Set unit's grid to new MapGrid. Moves towards this grid. Resolve conflicts on grid
    /// </summary>
    /// <param name="grid">MapGrid to move to.</param>
    public IEnumerator MoveTo(MapGrid grid)
    {
        currentGrid.RemoveUnitFromGrid(this);
        grid.AddUnitToGrid(this);
        currentGrid = grid;
        UIManager.Instance.ShowGameMessageText($"{unitName} moving to {currentGrid.IndexToVect()}");
        Vector3 finalPos = grid.transform.position;
        while (transform.position != finalPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, finalPos, 3 * Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSeconds(0.5f); // pause briefly after moving
        if (faction == Faction.Enemy) currentGrid.Resolve();
    }
}

public enum Faction
{
    Hero = 1,
    Enemy = 2,
}
