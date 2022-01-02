using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUnit : MonoBehaviour
{
    public MapGrid currentGrid = null;
    public bool isUnitSelected = false;
    public Vector2 currPosition;

    private bool isMoving = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isUnitSelected || isMoving)
        {
            //transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, 1), 5 * Time.deltaTime);
            transform.position = Vector2.MoveTowards(transform.position, currentGrid.transform.position, 5 * Time.deltaTime);
        }
    }

    private void OnMouseDown() 
    {
        Debug.Log(transform.position + "unit");
        if(!isUnitSelected)
        {
            isUnitSelected = true;
        }

    }

    /// <summary>
    /// Set unit's grid to new MapGrid. Moves towards this grid.
    /// </summary>
    /// <param name="grid">MapGrid to move to.</param>
    public void Move(MapGrid grid)
    {
        currentGrid.unitOnGrid = null;
        grid.unitOnGrid = this;
        currentGrid = grid;
        isMoving = true;
    }
    
}
