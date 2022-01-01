using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color grass1, grass2;
    [SerializeField] private GameObject highlight;
    public BaseUnit unitOnGrid = null;
    public bool isEnemySpawnGrid = false;
    public bool isHeroSpawnGrid = false;

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

}
