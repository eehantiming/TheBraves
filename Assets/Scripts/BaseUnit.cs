using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUnit : MonoBehaviour
{
    public bool isUnitSelected = false;
    public Vector2 currPosition;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isUnitSelected)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, 1), 5 * Time.deltaTime);
        }
    }

    private void OnMouseDown () 
    {
        Debug.Log(transform.position + "unit");
        if(!isUnitSelected)
        {
            isUnitSelected = true;
        }

    }
    
}
