using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color grass1, grass2;
    [SerializeField] private GameObject highlight;
    [SerializeField] private BaseUnit warriorPrefab;
    public BaseUnit unitOnGrid = null;
    
    /// <summary>
    /// Set color to produce a checkboard pattern
    /// </summary>
    /// <param name="isOffset">whether to use different color</param>
    public void Init(bool isOffset)
    {
        spriteRenderer.color = isOffset ? grass1 : grass2;
    }
    private void OnMouseEnter()
    {
        highlight.SetActive(true);
    }
    private void OnMouseExit()
    {
        highlight.SetActive(false);
    }
    //private void OnMouseDown()
    //{
    //    if(GameManager.Instance.currentState == GameManager.GameState.SetupHeroes)
    //    {
    //        UnitManager.Instance.SpawnSwordsman(this);
    //    }
    //}
}
