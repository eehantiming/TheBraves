using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MapGrid : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color grass1, grass2;
    [SerializeField] private GameObject highlight, selectedOverlay;
    [SerializeField] private GameObject trapPrefab;
    private GameObject trap;

    public int index;
    public List<BaseUnit> unitsOnGrid;
    public List<HeroUnit> heroesOnGrid;
    public List<EnemyUnit> enemiesOnGrid;
    public bool isHoldingHeart = false;
    public bool isHoldingTrap = false;
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
    public Vector2Int IndexToVect()
    {
        int x = index % 4;
        int y = index / 4;
        return new Vector2Int(x, y);
    }

    /// <summary>
    /// Resolves all conflicts between heroes, monsters, towns and traps on this grid.
    /// </summary>
    public void Resolve()
    {
        if (unitsOnGrid.Count > 0)
        {
            // Monsters attack heroes
            if(heroesOnGrid.Count > 0 && enemiesOnGrid.Count > 0)
            {
                foreach(HeroUnit hero in heroesOnGrid)
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
            if(isTownGrid && enemiesOnGrid.Count > 0)
            {
                GameManager.Instance.DestroyTown(this);
            }
            // TODO: Monsters attack smaller monsters
            if(enemiesOnGrid.Count > 1)
            {

            }
            // Monsters on trap
            if(trap != null)
            {
                enemiesOnGrid[enemiesOnGrid.Count-1].GetStunned(); // Assume only monster moving in gets stunned
                enemiesOnGrid[enemiesOnGrid.Count-1].IncreaseRageLevel();
                RemoveTrapFromGrid();
            }
        }
    }

    public void AddUnitToGrid(BaseUnit unit)
    {
        unitsOnGrid.Add(unit);
        if (unit.faction == Faction.Hero) heroesOnGrid.Add((HeroUnit)unit);
        else if (unit.faction == Faction.Enemy) enemiesOnGrid.Add((EnemyUnit)unit);
    }

    public void RemoveUnitFromGrid(BaseUnit unit)
    {
        unitsOnGrid.Remove(unit);
        if (unit.faction == Faction.Hero) heroesOnGrid.Remove((HeroUnit)unit);
        else if (unit.faction == Faction.Enemy) enemiesOnGrid.Remove((EnemyUnit)unit);
    }

    public void AddTrapToGrid()
    {
        trap = Instantiate(trapPrefab, transform.position, Quaternion.identity); // TODO: add animation
        isHoldingTrap = true;
        UIManager.Instance.ShowGameMessageText("Trap set on grid!");
    }

    public void RemoveTrapFromGrid()
    {
        if (trap==null) return;
        Destroy(trap);
        isHoldingTrap = false;
        UnitManager.Instance.trapper.NumOfTrapsLeft++;
    }
}
