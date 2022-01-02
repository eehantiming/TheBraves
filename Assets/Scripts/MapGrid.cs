using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    //Highlight when hover over MapGrid
    private void OnMouseEnter()
    {
        highlight.SetActive(true);
    }
    private void OnMouseExit()
    {
        highlight.SetActive(false);
    }

    //Make MapGrid clickable - first click to select then followed by confirmation to move on second click
    private void OnMouseDown() 
    {
        Debug.Log(transform.position);
        if(!isGridSelected)
        {
            isGridSelected = true;
        }
        else
        {
            Debug.Log("moving");
            isGridSelected = false;
        }
        if(GameManager.Instance.currentState == GameManager.GameState.SetupHeroes) // TODO: move this to GridManager, use raycast to get grid
        {
            UnitManager.Instance.SpawnSwordsman(this);
            GameManager.Instance.ChangeState(GameManager.GameState.HeroPhase);
        }
    }

}
