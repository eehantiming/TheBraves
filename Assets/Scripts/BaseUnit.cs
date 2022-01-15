using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUnit : MonoBehaviour
{
    public MapGrid currentGrid = null;
    public bool isUnitSelected = false;
    public Vector2 currPosition;
    public string unitName;

    private bool isMoving = false;

    // Update is called once per frame
    void Update()
    {
        // Moves unit to currentGrid set through Move()
        isMoving = transform.position != currentGrid.transform.position;
        if (isUnitSelected || isMoving)
        {
            //transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, 1), 5 * Time.deltaTime);
            transform.position = Vector2.MoveTowards(transform.position, currentGrid.transform.position, 5 * Time.deltaTime);
        }
    }

    //private void OnMouseDown() 
    //{
    //    Debug.Log(transform.position + "unit");
    //    if(!isUnitSelected)
    //    {
    //        isUnitSelected = true;
    //    }
    //}

    /// <summary>
    /// Set unit's grid to new MapGrid. Moves towards this grid.
    /// </summary>
    /// <param name="grid">MapGrid to move to.</param>
    public virtual void Move(MapGrid grid)
    {
        currentGrid.unitOnGrid = null;
        grid.unitOnGrid = this; // TODO: resolve 2 units on same grid. Monster vs hero, monster vs trap, monster vs monster
        currentGrid = grid;
    }

}
