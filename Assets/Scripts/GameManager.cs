using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // A singleton that manages the game state
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
        SmallEnemyPhase = 8,
        BigEnemyPhase = 9,
        GiantEnemyPhase = 10,
        CalamityPhase = 11,
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
                Debug.Log("\tState: SetUpGridmap");
                StartCoroutine(GridManager.Instance.SetUpGridmap());
                break;
            case GameState.SetupEnemies:
                Debug.Log("\tState: SetupEnemies");
                //Debug.Log(DiceRoll.Instance.Generate());
                StartCoroutine(UnitManager.Instance.SpawnSmallEnemy());
                break;
            case GameState.SetupSwordsman:
                Debug.Log("\tState: SetupSwordsman");
                StartCoroutine(UnitManager.Instance.SpawnSwordsman());
                // TODO: display spawnable grids during this phase
                break;
            case GameState.SetupTrapper:
                Debug.Log("\tState: SetupTrapper");
                StartCoroutine(UnitManager.Instance.SpawnTrapper());
                break;
            case GameState.SetupMagician:
                Debug.Log("\tState: SetupMagician");
                StartCoroutine(UnitManager.Instance.SpawnMagician());
                break;
            case GameState.SwordsmanPhase:
                Debug.Log("\tState: SwordsmanPhase");
                if (!UnitManager.Instance.swordsman.isConscious)
                {
                    UIManager.Instance.ShowGameMessageText("Swordsman is Unconscious!");
                    Debug.Log("Skipping Swordsman!");
                    ChangeState(GameState.TrapperPhase);
                }
                else
                {
                    UnitManager.Instance.SetActiveUnit(UnitManager.Instance.swordsman);
                }
                break;
            case GameState.TrapperPhase:
                Debug.Log("\tState: TrapperPhase");
                if (!UnitManager.Instance.trapper.isConscious)
                {
                    UIManager.Instance.ShowGameMessageText("Trapper is Unconscious!");
                    Debug.Log("Skipping Trapper!");
                    ChangeState(GameState.MagicianPhase);
                }
                else
                {
                    UnitManager.Instance.SetActiveUnit(UnitManager.Instance.trapper);
                }
                break;
            case GameState.MagicianPhase:
                Debug.Log("\tState: MagicianPhase");
                if (!UnitManager.Instance.magician.isConscious)
                {
                    UIManager.Instance.ShowGameMessageText("Magician is Unconscious!");
                    Debug.Log("Skipping Magician!");
                    ChangeState(GameState.SmallEnemyPhase);
                }
                else
                {
                    UnitManager.Instance.SetActiveUnit(UnitManager.Instance.magician);
                }
                break;
            case GameState.SmallEnemyPhase:
                Debug.Log("\tState: SmallEnemyPhase");
                // Check if there are small enemies alive. If so, each takes one action.
                if (UnitManager.Instance.smallEnemies.Count == 0)
                {
                    Debug.Log("No small enemies!");
                    ChangeState(GameState.BigEnemyPhase);
                }
                else
                {
                    foreach(SmallEnemy smallEnemy in UnitManager.Instance.smallEnemies)
                    {
                        if (smallEnemy.isAlive && !smallEnemy.isStunned)
                        {
                            UnitManager.Instance.SetActiveUnit(smallEnemy);
                            smallEnemy.DecideMovement();
                        }
                        else
                        {
                            if(smallEnemy.isStunned) UIManager.Instance.ShowGameMessageText($"{smallEnemy.unitName} is stunned!");
                            if (!smallEnemy.isAlive) Debug.Log($"{smallEnemy.unitName} is Dead");
                        }
                    }
                    ChangeState(GameState.BigEnemyPhase);
                }
                break;
            case GameState.BigEnemyPhase:
                if (UnitManager.Instance.bigEnemy == null)
                {
                    Debug.Log("No Big Enemy!");
                    ChangeState(GameState.GiantEnemyPhase);
                }
                else
                {
                    UnitManager.Instance.SetActiveUnit(UnitManager.Instance.bigEnemy);
                    UnitManager.Instance.bigEnemy.DecideMovement();
                    ChangeState(GameState.GiantEnemyPhase);
                }
                break;
            case GameState.GiantEnemyPhase:
                if (UnitManager.Instance.giantEnemy == null)
                {
                    Debug.Log("No Giant Enemy!");
                    ChangeState(GameState.CalamityPhase);
                }
                else
                {
                    UnitManager.Instance.SetActiveUnit(UnitManager.Instance.giantEnemy);
                    UnitManager.Instance.giantEnemy.DecideMovement();
                    ChangeState(GameState.CalamityPhase);
                }
                break;
            case GameState.CalamityPhase:
                Debug.Log("\tCalamity Phase!");
                StartCoroutine(CalamityManager.Instance.IncreaseCalamity());
                //TODO: check if all heroes are unconscious
                break;
            default:
                Debug.LogError("Invalid state");
                break;
        }

    }

    /// <summary>
    /// Call when any lose condition is met
    /// </summary>
    public void PlayerLose()
    {
        Debug.Log("Player lost");
        UIManager.Instance.ShowLoseText();
    }

    /// <summary>
    /// Call when win condition is met
    /// </summary>
    public void PlayerWin()
    {
        Debug.Log("Player won");
        UIManager.Instance.ShowWinText();
    }
}