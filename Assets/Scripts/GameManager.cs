using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        SetUpGridmap = 0,
        SetupEnemies = 1,          
        SetupSwordsman = 2,
        SetupTrapper = 3,
        SetupMagician = 4,
        SwordsmanPhase = 5,
        TrapperPhase = 6,
        MagicianPhase = 7,
        EnemyPhase = 8,
        CalamityPhase = 9,
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
        ChangeState(GameState.SetUpGridmap);
    }
    /// <summary>
    /// Main function to control turn and events.
    /// </summary>
    /// <param name="newState">The GameState to change to.</param>
    public void ChangeState(GameState newState)
    {
        currentState = newState;
        switch (newState)
        {
            case GameState.SetUpGridmap:
                Debug.Log("State: SetUpGridmap");
                GridManager.Instance.SetUpGridmap();
                break;
            case GameState.SetupEnemies:
                Debug.Log("State: SetupEnemies");
                UnitManager.Instance.SpawnSmallEnemy();
                ChangeState(GameState.SetupSwordsman);
                break;
            case GameState.SetupSwordsman:
                Debug.Log("State: SetupSwordsman");
                // TODO: display spawnable grids during this phase
                break;
            case GameState.SetupTrapper:
                break;
            case GameState.SetupMagician:
                break;
            case GameState.SwordsmanPhase:
                Debug.Log("State: SwordsmanPhase");
                if (!UnitManager.Instance.swordsman.isConscious)
                {
                    Debug.Log("Skipping Swordsman!");
                    ChangeState(GameState.TrapperPhase);
                }
                UnitManager.Instance.SetActiveUnit(UnitManager.Instance.swordsman);
                //UnitManager.Instance.swordsman.Move(GridManager.Instance.GridsDict[new Vector2(0, 0)]);
                break;
            case GameState.TrapperPhase:
                break;
            case GameState.MagicianPhase:
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