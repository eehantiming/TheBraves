using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance;
    [SerializeField] private BaseUnit dragonPrefab;
    [SerializeField] private BaseUnit swordsmanPrefab;

    public Swordsman swordsman = null;
    public BaseUnit activeUnit = null;
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
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

    /// <summary>
    /// Spawns a small enemy on an unoccupied enemy spawn grid and add it to the small enemies data structure.
    /// </summary>
    public void SpawnSmallEnemy()
    {
        Debug.Log("Spawning Small Enemy!");
        SpawnUnit(dragonPrefab, GridManager.Instance.GetEnemySpawnGrid()); // TODO: add smallenemies data structure and add this
    }

    /// <summary>
    /// Spawns Swordsman at specified grid.
    /// </summary>
    /// <param name="spawnGrid">MapGrid to spawn in</param>
    public void SpawnSwordsman(MapGrid spawnGrid)
    {
        Debug.Log("Spawning Swordsman!");
        swordsman = (Swordsman)SpawnUnit(swordsmanPrefab, spawnGrid);
    }

    /// <summary>
    /// Sets current active unit. Displays this info
    /// </summary>
    /// <param name="unit">BaseUnit to set</param>
    public void SetActiveUnit(BaseUnit unit)
    {
        activeUnit = unit;
        if(activeUnit == null)
        {
            UIManager.Instance.ShowActiveUnitText(null);
        }
        else
        {
            UIManager.Instance.ShowActiveUnitText(activeUnit.unitName);
        }
    }

}
