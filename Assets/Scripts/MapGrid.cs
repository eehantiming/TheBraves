using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MapGrid : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color grass1, grass2;
    [SerializeField] private GameObject highlight, selectedOverlay;
    
    public int index;
    public BaseUnit unitOnGrid = null;
    public bool holdingHeart = false;
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

    public void ToggleOverlay(bool selected)
    {
        if (selected) selectedOverlay.SetActive(true);
        else selectedOverlay.SetActive(false);
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
            ToggleOverlay(false);
            GridManager.Instance.confirmSelectedGrid = this;
        }
        // Clicked on a different tile
        else
        {
            GridManager.Instance.SetSelectedGrid(this);
            GridManager.Instance.confirmSelectedGrid = null;
        }
    }

    /// <summary>
    /// Returns the Coordinates of the Grid. e.g. grid 19 => (3,4). TODO: may not be required
    /// </summary>
    /// <param name="index">Index of the Grid, given by the value in grid to index</param>
    /// <returns>Vector2 of the coordinates of the Grid.</returns>
    public Vector2 IndexToVect()
    {
        int x = index % 4;
        int y = index / 4;
        return new Vector2(x, y);
    }
}
