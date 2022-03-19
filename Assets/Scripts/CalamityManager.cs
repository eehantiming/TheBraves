using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalamityManager : MonoBehaviour
{
    public static CalamityManager Instance;

    private int calamityCounter = 1;
    private int increaseSpeed = 1; // Number of times to increment counter

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Function to increase calamity counter and activate corresponding events. Number of times to increase is determined by increaseSpeed
    /// </summary>
    public IEnumerator IncreaseCalamity()
    {
        for(int i=1; i<=increaseSpeed; i++)
        {
            calamityCounter++;
            UIManager.Instance.ShowCalamityCount(calamityCounter);
            Debug.Log($"Calamity: {calamityCounter}");
            // Check for calamity events
            if (calamityCounter % 4 == 0) // Spawn small monster
            {
                //UnitManager.Instance.SpawnSmallEnemy();
            }
            //if(calamityCounter == 8) // Spawn Giant Monster
            if(calamityCounter == 5) // DEBUG
            {
                Debug.Log("Calamity: Spawning GiantMonster");
                yield return StartCoroutine(UnitManager.Instance.SpawnGiantEnemy());
                //yield return StartCoroutine(UnitManager.Instance.SpawnHeart()); // DEBUG
            }
            else if(calamityCounter == 15) // Fire Breath
            {
                UIManager.Instance.ShowGameMessageText("Giant Monster Fire Breath!");
                //TODO: add animation for this
                GameManager.Instance.PlayerLose();
                break;
            }
        }
        //if (calamityCounter == 4) SpeedUp(); //DEBUG
        GameManager.Instance.ChangeState(GameManager.GameState.SwordsmanPhase);
    }

    /// <summary>
    /// Function to increase Calamity counter by 1 extra count each turn.
    /// </summary>
    public void SpeedUp()
    {
        increaseSpeed++;
        UIManager.Instance.ShowGameMessageText($"Calamity increase speed: {increaseSpeed}");
    }
}
