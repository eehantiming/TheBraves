using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    public static GameManager Instance;

    // Update is called once per frame
    public void StartGame()
    {
        Destroy(this.gameObject);
        GameManager.Instance.ChangeState(GameManager.GameState.SetUpGridmap);
        
    }
}
