using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    [SerializeField] private MapGrid gridPrefab;
    [SerializeField] private List<MapGrid> grids;
    
    //[SerializeField] private int width, height;
    //[SerializeField] private Camera maincam;
    
    private List<Vector2> enemyGridsPositions = new List<Vector2>();
    private int countlimit = 20;
    private int counter;
    public Dictionary<Vector2, MapGrid> GridsDict = new Dictionary<Vector2, MapGrid>();

    private void Awake()
    {
        Instance = this;
        //enemyGridsPositions.Add(new Vector2(0, 5));
        //enemyGridsPositions.Add(new Vector2(1, 5));
        //enemyGridsPositions.Add(new Vector2(2, 5));
        //enemyGridsPositions.Add(new Vector2(3, 5));
        //enemyGridsPositions.Add(new Vector2(0, 4));
        //enemyGridsPositions.Add(new Vector2(3, 4));
    }

    // Start is called before the first frame update
    void Start()
    {
        //maincam.transform.position = new Vector3(width / 2, height / 2, -10);
    }

    /// <summary>
    /// Function to generate the gameboard, which consists of grids
    /// </summary>
    public void SetUpGridmap()
    {
        //for (int x = 0; x < width; x++)
        //{
        //    for (int y = 0; y < height; y++)
        //    {
        //        Grid thisGrid = Instantiate(gridPrefab, new Vector2(x, y), Quaternion.identity);
        //        bool isOffset = (x + y) % 2 == 1;
        //        thisGrid.Init(isOffset);

        //        GridsDict.Add(new Vector2(x, y), thisGrid);
        //    }
        //}
        foreach (MapGrid grid in grids)
        {
            GridsDict.Add(new Vector2(grid.transform.localPosition.x, grid.transform.localPosition.y), grid);
        }
        GameManager.Instance.ChangeState(GameManager.GameState.SetupEnemies);
    }

    /// <summary>
    /// Function that randomly picks an unoccupied enemy Grid.
    /// </summary>
    /// <returns>Grid object. Use grid.transform.position to get position.</returns>
    //public Grid GetEnemySpawnGrid()
    //{
    //    Vector2 enemyGridPosition = enemyGridsPositions[Random.Range(0, enemyGridsPositions.Count)];
    //    //Debug.Log(enemyGridPosition);
    //    while (GridsDict[enemyGridPosition].unitOnGrid != null && counter < countlimit)
    //    {
    //        enemyGridPosition = enemyGridsPositions[Random.Range(0, enemyGridsPositions.Count)];
    //        //Debug.Log(enemyGridPosition);
    //        counter++; //prevents infinite loop
    //    }
    //    counter = 0; //not sure if required
    //    return GridsDict[enemyGridPosition];
    //}
    public MapGrid GetEnemySpawnGrid()
    {
        int roll = Random.Range(1, 7); // TODO: currently fixed to 6. use this value for dice throw
        Debug.Log(roll);
        MapGrid spawnGrid = grids[roll+17];
        counter = 0;
        while (spawnGrid.unitOnGrid != null && counter < countlimit)
        {
            roll = Random.Range(1, 7); // TODO: currently fixed to 6. use this value for dice throw
            Debug.Log(roll);
            spawnGrid = grids[roll + 17];
            counter++; //prevents infinite loop
        }
        return spawnGrid;
    }
}
