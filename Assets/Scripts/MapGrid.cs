using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MapGrid : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color grass1, grass2;
    [SerializeField] private GameObject highlight;
    public BaseUnit unitOnGrid = null;
    public bool isEnemySpawnGrid = false;
    public bool isHeroSpawnGrid = false;
    public bool isGridSelected = false;


    /// <summary>
    /// Set color to produce a checkboard pattern
    /// </summary>
    /// <param name="isOffset">whether to use different color</param>
    public void Init(bool isOffset)
    {
        spriteRenderer.color = isOffset ? grass1 : grass2;
    }

    public void Highlight()
    {
        spriteRenderer.color = grass1;
    }

    private void OnMouseEnter()
    {
        highlight.SetActive(true);
        //if (unitOnGrid != null)
        //{
        //    Debug.Log("setting name!");
        //    UIManager.Instance.SetUnitText(unitOnGrid.unitName);
        //}
    }
    private void OnMouseExit()
    {
        highlight.SetActive(false);
    }

    //Make MapGrid clickable - first click to select then followed by confirmation to move on second click
    private void OnMouseDown() 
    {
        // TODO: Double click logic to move.
        //Debug.Log(transform.position);
        //if(!isGridSelected)
        //{
        //    isGridSelected = true;
        //}
        //else
        //{
        //    Debug.Log("moving");
        //    isGridSelected = false;
        //}
        // Click to spawn Swordsman
        //if(GameManager.Instance.currentState == GameManager.GameState.SetupSwordsman) 
        //{
        //    if (isHeroSpawnGrid)
        //    {
        //        UnitManager.Instance.SpawnSwordsman(this);
        //        GameManager.Instance.ChangeState(GameManager.GameState.SwordsmanPhase);
        //    }
        //    else
        //    {
        //        Debug.Log("Click on hero spawn grids!");
        //    }
        //}

        // Second consecutive click on grid
        if(GridManager.Instance.selectedGrid == this)
        {
            GridManager.Instance.confirmSelectedGrid = this;
        }
        // Clicked on a different tile
        else
        {
            GridManager.Instance.SetSelectedGrid(this);
            GridManager.Instance.confirmSelectedGrid = null;
        }
    }

}
