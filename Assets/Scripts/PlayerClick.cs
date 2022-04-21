using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClick : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            RaycastHit hit = new RaycastHit();      
            Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
 
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == this.gameObject)
                {
                    Debug.Log("Click!");
                }
                else
                {
                    Debug.Log("Click on other colliders");
                }
            }
            else
            {
                Debug.Log("Click on non-collider");
                GridManager.Instance.clickedOutside = true;
            }
        }
    }
}
