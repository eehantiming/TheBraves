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
    public List<BaseUnit> unitsOnGrid;
    public bool isHoldingHeart = false;
    public bool isEnemySpawnGrid = false;
    public bool isHeroSpawnGrid = false;
    public bool isTownGrid = false;
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

    /// <summary>
    /// Enable/disable visual overlay to show if grid is selected.
    /// </summary>
    /// <param name="selected">whether to enable or disable this overlay.</param>
    public void ToggleOverlay(bool selected)
    {
        if (selected) selectedOverlay.SetActive(true);
        else selectedOverlay.SetActive(false);
    }

    private void OnMouseEnter()
    {
        highlight.SetActive(true);
        //if (unitsOnGrid != null)
        //{
        //    Debug.Log("setting name!");
        //    UIManager.Instance.SetUnitText(unitsOnGrid.unitName);
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
            GridManager.Instance.SetSelectedGrid(null);
            GridManager.Instance.confirmSelectedGrid = this;
            Debug.Log($"Selected grid {IndexToVect()}");
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

    /// <summary>
    /// Resolves all conflicts between heroes, monsters, towns and traps on this grid.
    /// </summary>
    public void Resolve()
    {
        if (unitsOnGrid.Count > 0)
        {
            // Split units by faction
            List<HeroUnit> heroes = new List<HeroUnit>();
            List<EnemyUnit> monsters = new List<EnemyUnit>();
            foreach(BaseUnit unit in unitsOnGrid)
            {
                if (unit.faction == Faction.Hero) heroes.Add((HeroUnit)unit);
                else if (unit.faction == Faction.Enemy) monsters.Add((EnemyUnit)unit);
            }
            // Monsters attack heroes
            if(heroes.Count > 0 && monsters.Count > 0)
            {
                foreach(HeroUnit hero in heroes)
                {
                    if (hero.isConscious)
                    {
                        hero.isConscious = false;
                        UIManager.Instance.ShowGameMessageText($"{hero.unitName} is attacked!");
                        Debug.Log($"{hero.unitName} is attacked");
                    }
                    else 
                    {
                        Debug.Log($"{hero.unitName} was unconscious and attacked again.");
                        GameManager.Instance.PlayerLose();
                    } 
                }
            }
            // Monsters attack town
            if(isTownGrid && monsters.Count > 0)
            {
                GameManager.Instance.DestroyTown(this);
            }
            // Monsters attack smaller monsters
            if(monsters.Count > 1)
            {

            }

        }
    }
}
