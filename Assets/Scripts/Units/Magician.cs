using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magician : HeroUnit
{
    public override void ActivateSkill()
    {
        base.ActivateSkill();
        Debug.Log("Teleport");
        StartCoroutine(Teleport());
    }

    /// <summary>
    /// Coroutine. Prompt user to select empty grid 2 linear units away to teleport to. Moves Magicial there.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Teleport()
    {
        List<MapGrid> validGrids = GridManager.Instance.GetTeleportGrids(currentGrid);
        if (validGrids.Count == 0)
        {
            UIManager.Instance.ShowGameMessageText("Can't teleport anywhere!");
            Debug.Log("No grids to teleport to");
            yield break;
        }
        // TODO: Highlight valid grids
        UIManager.Instance.ShowGameMessageText("Select Grid to Teleport to");
        yield return StartCoroutine(GridManager.Instance.WaitForGridSelection());
        // Check if selected grid is valid
        while (!validGrids.Contains(GridManager.Instance.confirmSelectedGrid))
        {
            UIManager.Instance.ShowGameMessageText("Select a valid Grid to Teleport to");
            yield return StartCoroutine(GridManager.Instance.WaitForGridSelection());
        }
        StartCoroutine(MoveTo(GridManager.Instance.confirmSelectedGrid));
        //yield return new WaitForSeconds(1);
        EndTurn();
    }
}
