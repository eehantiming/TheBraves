using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance;
    [SerializeField] private BaseUnit dragonPrefab;
    [SerializeField] private BaseUnit swordsmanPrefab;
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        
    }


    /// <summary>
    /// Spawns unit on Grid.
    /// </summary>
    /// <param name="prefab">Unit prefab to spawn</param>
    /// <param name="grid">Grid to spawn in</param>
    void SpawnUnit(BaseUnit prefab, Grid grid)
    {
        BaseUnit spawnedUnit = Instantiate(prefab, grid.transform.position, Quaternion.identity);
        grid.unitOnGrid = spawnedUnit;
    }

    public void SpawnSmallEnemy()
    {
        Debug.Log("Spawning Small Enemy!");
        SpawnUnit(dragonPrefab, GridManager.Instance.GetEnemySpawnGrid());
    }

    void SpawnSwordsman(Grid spawnGrid)
    {
        Debug.Log("Spawning Warrior!");
        SpawnUnit(swordsmanPrefab, spawnGrid);
    }

    private void Update()
    {
        if (GameManager.Instance.currentState == GameManager.GameState.SetupHeroes && Input.GetMouseButtonDown(0))
        {
            Debug.Log("clicked!");
        }
    }
}
