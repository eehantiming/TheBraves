using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        //Generate the
        GenerateGrid = 0,
        SetupEnemies = 1,          
        SetupHeroes = 2,
        HeroPhase = 3,
        EnemyPhase = 4,
        CalamityPhase = 5,
    }
    public static GameManager Instance;
    public GameState currentState;
    private void Awake()
    {
        //create a global reference
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        ChangeState(GameState.GenerateGrid);
    }
    public void ChangeState(GameState newState)
    {
        currentState = newState;
        switch (newState)
        {
            case GameState.GenerateGrid:
                Debug.Log("State: GenerateGrid");
                GridManager.Instance.SetUpGridmap();
                break;
            case GameState.SetupEnemies:
                Debug.Log("State: SetupEnemies");
                UnitManager.Instance.SpawnSmallEnemy();
                ChangeState(GameState.SetupHeroes);
                break;
            case GameState.SetupHeroes:
                Debug.Log("State: SetupHeroes");
                break;
            case GameState.HeroPhase:
                break;
            case GameState.EnemyPhase:
                break;
            case GameState.CalamityPhase:
                break;
            default:
                Debug.LogError("Invalid state");
                break;
        }

    }

}