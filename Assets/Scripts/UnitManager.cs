using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance;
    [SerializeField] private BaseUnit dragonPrefab;
    [SerializeField] private BaseUnit swordsmanPrefab;

    public Swordsman swordsman = null;
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
    BaseUnit SpawnUnit(BaseUnit prefab, MapGrid grid)
    {
        BaseUnit spawnedUnit = Instantiate(prefab, grid.transform.position, Quaternion.identity);
        grid.unitOnGrid = spawnedUnit;
        spawnedUnit.currentGrid = grid;
        return spawnedUnit;
    }

    public void SpawnSmallEnemy()
    {
        Debug.Log("Spawning Small Enemy!");
        SpawnUnit(dragonPrefab, GridManager.Instance.GetEnemySpawnGrid()); // TODO: add smallenemy data structure and add this
    }

    public void SpawnSwordsman(MapGrid spawnGrid)
    {
        Debug.Log("Spawning Warrior!");
        swordsman = (Swordsman)SpawnUnit(swordsmanPrefab, spawnGrid);
    }

    private void Update()
    {
        if (GameManager.Instance.currentState == GameManager.GameState.SetupHeroes && Input.GetMouseButtonDown(0))
        {
            Debug.Log("clicked!");
        }
    }
}
